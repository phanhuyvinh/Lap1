using Lap1.Data;
using Lap1.Models;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Lap1.Controllers
{
    public class LearnerController : Controller
    {
        private SchoolContext db;

        public LearnerController(SchoolContext context)
        {
            db = context;
        }

        // BƯỚC 8.1: HIỂN THỊ DANH SÁCH
        public IActionResult Index(int? mid)
        {
            if (mid == null)
            {
                var learners = db.Learners.Include(m => m.Major).ToList();
                //**ToList() * *: Kích hoạt câu lệnh `SELECT` nạp toàn bộ
                //danh sách bản ghi từ bảng SQL Server về dưới dạng `List
                return View(learners);
            }
            else
            {
                var learners = db.Learners
                    .Where(l => l.MajorID == mid)
                    .Include(m => m.Major).ToList();
                return View(learners);
            }
            
        }
        public IActionResult LearnerByMajorID(int mid)
        {
            var learners = db.Learners
                .Where(l => l.MajorID == mid)
                .Include(m => m.Major).ToList();
            return PartialView("LearnerTable", learners);
        }
        // BƯỚC 8.2: THÊM MỚI (CREATE)
        public IActionResult Create()
        {
            ViewBag.MajorID = new SelectList(db.Majors, "MajorID", "MajorName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("FirstMidName,LastName,MajorID,EnrollmentDate")] Learner learner)
        {
            if (ModelState.IsValid)
            {
                db.Learners.Add(learner);
                db.SaveChanges();   //Lệnh CỐT LÕI quyết định(Ghi dữ liệu) Từ Ứng dụng xuống SQL Server
                //gọi db.SaveChanges(), EF Core mới kích hoạt cơ chế dịch toàn bộ các thay đổi thành các câu lệnh SQL 
                //thực sự(`INSERT INTO`, `UPDATE`, `DELETE`) và gửi xuống SQL Server để thực thi 
                //ghi dữ liệu vĩnh viễn vào ổ đĩa
                return RedirectToAction(nameof(Index));
            }
            ViewBag.MajorID = new SelectList(db.Majors, "MajorID", "MajorName");//Khởi tạo trực tiếp SelectList từ db.Majors
            return View();
        }

        // BƯỚC 8.3: SỬA THÔNG TIN (EDIT)
        public IActionResult Edit(int id)
        {
            if (id == null || db.Learners == null)
            {
                return NotFound();
            }

            var learner = db.Learners.Find(id);
            if (learner == null)
            {
                return NotFound();
            }

            ViewBag.MajorID = new SelectList(db.Majors, "MajorID", "MajorName", learner.MajorID);
            return View(learner);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("LearnerID,FirstMidName,LastName,MajorID,EnrollmentDate")] Learner learner)
        {
            if (id != learner.LearnerID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(learner);
                    db.SaveChanges();   //Lệnh CỐT LÕI quyết định(Ghi dữ liệu) Từ Ứng dụng xuống SQL Server
                                        //gọi db.SaveChanges(), EF Core mới kích hoạt cơ chế dịch toàn bộ các thay đổi thành các câu lệnh SQL 
                                        //thực sự(`INSERT INTO`, `UPDATE`, `DELETE`) và gửi xuống SQL Server để thực thi 
                                        //ghi dữ liệu vĩnh viễn vào ổ đĩa
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LearnerExists(learner.LearnerID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.MajorID = new SelectList(db.Majors, "MajorID", "MajorName", learner.MajorID);
            return View(learner);
        }

        private bool LearnerExists(int id)
        {
            return (db.Learners?.Any(e => e.LearnerID == id)).GetValueOrDefault();
        }
        // BƯỚC 8.4: XÓA THÔNG TIN (DELETE)
        // GET: Hiển thị trang xác nhận trước khi xóa
        public IActionResult Delete(int id)
        {
            if (id == null || db.Learners == null)
            {
                return NotFound();
            }

            // Lấy thông tin sinh viên kèm theo tên chuyên ngành để hiển thị
            var learner = db.Learners
                .Include(l => l.Major)
                .FirstOrDefault(m => m.LearnerID == id);

            if (learner == null)
            {
                return NotFound();
            }

            return View(learner);
        }

        // POST: Thực thi lệnh xóa khỏi cơ sở dữ liệu
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (db.Learners == null)
            {
                return Problem("Dữ liệu bảng Learners trống.");
            }

            var learner = db.Learners.Find(id);
            if (learner != null)
            {
                db.Learners.Remove(learner);
                db.SaveChanges();   //Lệnh CỐT LÕI quyết định(Ghi dữ liệu) Từ Ứng dụng xuống SQL Server
                //gọi db.SaveChanges(), EF Core mới kích hoạt cơ chế dịch toàn bộ các thay đổi thành các câu lệnh SQL 
                //thực sự(`INSERT INTO`, `UPDATE`, `DELETE`) và gửi xuống SQL Server để thực thi 
                //ghi dữ liệu vĩnh viễn vào ổ đĩa
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
using System.IO; //đường dẫn file
using Lap1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting;

namespace Lap1.Controllers
{
    public class StudentController : Controller
    {
        private static List<Student> listStudents = new List<Student>();
        private readonly IWebHostEnvironment _env;
        public StudentController(IWebHostEnvironment env)
        {
            _env = env;
            if (!listStudents.Any())
            {
                listStudents = new List<Student>() {
                new Student() { Id = 101, Name = "Hải Nam", Branch = Branch.IT, Gender = Gender.Male, IsRegular = true, Address = "A1-2018", Email = "nam@g.com" }, //AvatarUrl = "thêm link ảnh"
                new Student() { Id = 102, Name = "Minh Tú", Branch = Branch.BE, Gender = Gender.Female, IsRegular = true, Address = "A1-2019", Email = "tu@g.com" },
                new Student() { Id = 103, Name = "Hoàng Phong", Branch = Branch.CE, Gender = Gender.Male, IsRegular = false, Address = "A1-2020", Email = "phong@g.com" },
                new Student() { Id = 104, Name = "Xuân Mai", Branch = Branch.EE, Gender = Gender.Female, IsRegular = false, Address = "A1-2021", Email = "mai@g.com" }
                };
            }
        }

        [HttpGet]
        // Cấu hình Route cho Student/Create thành Admin/Student/Add (GET)
        [Route("Admin/Student/Add", Name = "StudentAdd")]
        public IActionResult Create()
        {
            //Lấy danh sach cac gia trị Gender để hiển thị radio button trên form
            ViewBag.AllGenders = Enum.GetValues(typeof(Gender)).Cast<Gender>().ToList();
            //Lấy danh sach cac gia trị Branch đe hien thị select-option trên form
            //Để hien thị select-option trên View cần dung List<SelectListItem>
            ViewBag.AllBranches = new List<SelectListItem>()
            {
                new SelectListItem { Text = "IT", Value = "1" },
                new SelectListItem { Text = "BE", Value = "2" },
                new SelectListItem { Text = "CE", Value = "3" },
                new SelectListItem { Text = "EE", Value = "4" }
            };
            return View();
        }

        [HttpPost]
        // Cấu hình Route cho POST Create (Bỏ Name = "StudentAdd" để tránh lỗi trùng tên Route với hàm GET)
        [Route("Admin/Student/Add")]
        public IActionResult Create(Student s)
        {
            if (ModelState.IsValid)
            {
                // --- BẮT ĐẦU ĐOẠN BỔ SUNG XỬ LÝ UPLOAD ẢNH ---
                if (s.UploadImage != null && s.UploadImage.Length > 0)
                {
                    // Đường dẫn tới thư mục wwwroot/images
                    string uploadsFolder = Path.Combine(_env.WebRootPath, "images");

                    // Tạo thư mục nếu chưa có
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }
                    // Tạo tên file độc nhất để không bị đè ảnh
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + s.UploadImage.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        s.UploadImage.CopyTo(fileStream);
                    }
                    // Lưu đường dẫn vào thuộc tính AvatarUrl (cần chắc chắn trong class Student đã có thuộc tính này)
                    s.AvatarUrl = "/images/" + uniqueFileName;
                }

                s.Id = listStudents.Last<Student>().Id + 1;
                listStudents.Add(s);
                return View("Index", listStudents);
            }

            ViewBag.AllGenders = Enum.GetValues(typeof(Gender)).Cast<Gender>().ToList();
            ViewBag.AllBranches = new List<SelectListItem>()
            {
                new SelectListItem { Text = "IT", Value = "1" },
                new SelectListItem { Text = "BE", Value = "2" },
                new SelectListItem { Text = "CE", Value = "3" },
                new SelectListItem { Text = "EE", Value = "4" }
            };
            return View();
        }

        // Cấu hình Route cho Student/Index thành Admin/Student/List
        [Route("Admin/Student/List", Name = "StudentList")]
        public IActionResult Index()
        {
            return View(listStudents);
        }
    }
}
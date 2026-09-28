using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lap1.Models
{
    public class Student
    {
        public int Id { get; set; }//Mã sinh viên

        [Required(ErrorMessage = "Họ tên bắt buộc phải nhập")]
        [StringLength(100, MinimumLength = 4, ErrorMessage = "Name tối thiểu 4 ký tự, tối đa 100 ký tự")]
        public string? Name { get; set; } //Họ tên

        [Required(ErrorMessage = "Email bắt buộc phải được nhập")]
        [RegularExpression(@"^[A-Za-z0-9._%+-]+@gmail\.com$", ErrorMessage = "Địa chỉ email phải có đuôi gmail.com")]
        public string? Email { get; set; } //Email

        [Required(ErrorMessage = "Mật khẩu bắt buộc phải nhập")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Mật khẩu từ 8 ký tự trở lên")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$", ErrorMessage = "Mật khẩu có ký tự viết hoa, viết thường, chữ số và ký tự đặc biệt")]
        public string? Password { get; set; }//Mật khẩu

        [Required(ErrorMessage = "Ngành học bắt buộc phải chọn")]
        public Branch? Branch { get; set; }//Ngành học

        [Required(ErrorMessage = "Giới tính bắt buộc phải chọn")]
        public Gender? Gender { get; set; }//Giới tính

        public bool IsRegular { get; set; }//Hệ: true-chính qui, false-phi cq

        [Required(ErrorMessage = "Địa chỉ bắt buộc phải nhập")]
        [DataType(DataType.MultilineText)]
        public string? Address { get; set; }//Địa chi

        [Required(ErrorMessage = "Ngày sinh bắt buộc phải nhập")]
        [Range(typeof(DateTime), "1/1/1963", "12/31/2005", ErrorMessage = "Ngày sinh phải nằm trong khoảng 1/1/1963 đến 12/31/2005")]
        [DataType(DataType.Date)]
        public DateTime DateOfBorth { get; set; }//Ngày sinh

        // Bổ sung thuộc tính lưu đường dẫn ảnh
        public string? AvatarUrl { get; set; }

        // Bổ sung thuộc tính xử lý file upload (Không lưu trực tiếp vào Database)
        [NotMapped]
        public IFormFile? UploadImage { get; set; }

        // Bổ sung trường Điểm theo yêu cầu mục 4
        [Required(ErrorMessage = "Điểm bắt buộc phải nhập")]
        [Range(0.0, 10.0, ErrorMessage = "Điểm phải nằm trong miền giá trị từ 0.0 đến 10.0")]
        public double Score { get; set; }
    }
}
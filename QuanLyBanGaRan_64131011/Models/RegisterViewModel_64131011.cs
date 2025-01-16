using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace QuanLyBanGaRan_64131011.Models
{
    public class RegisterViewModel_64131011
    {
        public string UserID { get; set; }
        [DisplayName("Họ và tên")]
        public string FullName { get; set; }
        [DisplayName("Email")]
        public string Email { get; set; }
        [DisplayName("Địa chỉ")]
        public string Address { get; set; }
        [DisplayName("Số điện thoại")]
        public string Phone { get; set; }
        [DisplayName("CCCD")]
        public string CCCD { get; set; }
        [DisplayName("Tên tài khoản")]
        public string Username { get; set; }
        [DisplayName("Mật khẩu")]
        public string Password { get; set; }
        [DisplayName("Nhập lại mật khẩu")]
        public string PasswordRepeat { get; set; }
    }
}
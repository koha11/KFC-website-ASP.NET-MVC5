using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QuanLyBanGaRan_64131011.Models
{
    public class LoginViewModel_64131011
    {
        [DisplayName("Tên tài khoản")]
        [Required(ErrorMessage = "Không được để trống tên tài khoản")]
        public string Username { get; set; }
        [DisplayName("Mật khẩu")]
        [Required(ErrorMessage = "Không được để trống mật khẩu")]
        public string Password { get; set; }
        [DisplayName("Nhớ mật khẩu")]
        public bool RememberMe { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.DTO
{
    public class RequestResetPasswordDTO
    {
        [Required]
        [MaxLength(64)]
        public string UserID { get; set; }
       
        [MaxLength(64)]
        public string Password { get; set; }
        
        [MaxLength(64)]
        public string ConfirmPassword { get; set; }
    }
}

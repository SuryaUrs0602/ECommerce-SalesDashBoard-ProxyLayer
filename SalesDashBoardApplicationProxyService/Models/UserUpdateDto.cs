using System.ComponentModel.DataAnnotations;

namespace SalesDashBoardApplicationProxyService.Models
{
    public class UserUpdateDto
    {
        [Required(ErrorMessage = "This Field cannot be Empty")]
        [StringLength(50)]
        public string UserName { get; set; } = string.Empty;


        [Required(ErrorMessage = "This Field cannot be Empty")]
        [StringLength(50)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string UserEmail { get; set; } = string.Empty;
    }
}

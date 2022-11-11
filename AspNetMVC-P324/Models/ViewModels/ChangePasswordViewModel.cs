using System.ComponentModel.DataAnnotations;

namespace AspNetMVC_P324.Models.ViewModels
{
    public class ChangePasswordViewModel
    {
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password),Compare(nameof(NewPassword))]
        public string ConfirmPassword { get; set; }
    }
}

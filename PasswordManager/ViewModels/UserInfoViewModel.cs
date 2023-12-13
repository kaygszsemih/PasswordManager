namespace PasswordManager.ViewModels
{
    public class UserInfoViewModel
    {
        public string Id { get; set; }

        public string UserName { get; set;}

        public string Email { get; set;}

        public string PhoneNumber { get; set;}

        public string OldPassword { get; set;}

        public string NewPassword { get; set;}

        public string ConfirmPassword { get; set;}

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}

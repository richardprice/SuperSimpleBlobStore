namespace SuperSimpleBlobStore.Api.ViewModel
{
    public class ChangePassword
    {
        public string email { get; set; }
        public string currentPassword { get; set; }
        public string newPassword { get; set; }
        public string confirmNewPassword { get; set; }
    }
}

namespace SBSC.Wallet.CoreObject.ViewModels
{
    public class AddUserRequest
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public byte[]? ProfilePicture { get; set; }
    }
}
namespace SBSC.Wallet.CoreObject.ViewModels
{
    public class EditUserRequest
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public byte[]? ProfilePicture { get; set; }
    }
}
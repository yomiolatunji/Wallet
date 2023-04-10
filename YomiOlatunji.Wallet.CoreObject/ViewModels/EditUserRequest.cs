namespace YomiOlatunji.Wallet.CoreObject.ViewModels
{
    public class EditUserRequest
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? ProfilePictureBase64 { get; set; }
    }
}
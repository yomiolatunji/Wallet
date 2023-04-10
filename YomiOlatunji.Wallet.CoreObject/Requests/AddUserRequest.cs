namespace YomiOlatunji.Wallet.CoreObject.Requests
{
    public class AddUserRequest
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string? ProfilePictureBase64 { get; set; }
    }
}
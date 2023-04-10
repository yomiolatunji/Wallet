namespace YomiOlatunji.Wallet.CoreObject.ViewModels
{
    public class NotificationDto
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        public string UserType { get; set; } = null!;

        public string? Subject { get; set; }

        public string? Message { get; set; }

        public bool IsRead { get; set; }
    }
}
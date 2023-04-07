using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBSC.Wallet.CoreObject.ViewModels
{
    public class UserDto
    {
        public long Id { get; set; }

        public string Email { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string? ProfilePictureUrl { get; set; }
        public byte[]? ProfilePicture { get; set; }
    }
}

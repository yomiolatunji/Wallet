using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YomiOlatunji.Wallet.BusinessCore.Integrations.Interfaces
{
    public interface ICloudinaryIntegration
    {
        string UploadImage(string base64);
    }
}

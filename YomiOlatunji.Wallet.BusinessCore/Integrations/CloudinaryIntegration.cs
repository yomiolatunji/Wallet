using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Configuration;
using YomiOlatunji.Wallet.BusinessCore.Integrations.Interfaces;
using YomiOlatunji.Wallet.BusinessCore.Services;

namespace YomiOlatunji.Wallet.BusinessCore.Integrations
{
    public class CloudinaryIntegration : BaseService, ICloudinaryIntegration
    {
        public CloudinaryIntegration(IConfiguration configuration) : base(configuration)
        {
        }

        public string UploadImage(string base64)
        {
            var cloudName = GetAppSetting("Cloudinary:CloudName");
            var apiKey = GetAppSetting("Cloudinary:ApiKey");
            var apiSecret = GetAppSetting("Cloudinary:ApiSecret");

            var cloudinary = new Cloudinary(new Account(cloudName, apiKey, apiSecret));
            var bytes = Convert.FromBase64String(base64);
            Stream stream = new MemoryStream(bytes);
            var id = Guid.NewGuid().ToString();
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(id, stream),
                PublicId = id,
            };

            var uploadResult = cloudinary.Upload(uploadParams);

            return uploadResult.SecureUri.AbsoluteUri;
        }
    }
}
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonSocialNetwork.Service.Services
{
    public interface ICloudinaryService
    {
        Task<ImageUploadResult> UploadImageToCloudinaryAsync(IFormFile formFile);
    }
    public class CloudinaryService : ICloudinaryService
    {
        public async Task<ImageUploadResult> UploadImageToCloudinaryAsync(IFormFile formFile)
        {
            try
            {
                DotNetEnv.Env.Load();
                Cloudinary cloudinary = new Cloudinary(Environment.GetEnvironmentVariable("CLOUDINARY_URL"));
                cloudinary.Api.Secure = true;
                var uploadResult = new ImageUploadResult();
                if (formFile.Length > 0)
                {
                    using (var stream = formFile.OpenReadStream())
                    {
                        var uploadParams = new ImageUploadParams
                        {
                            File = new FileDescription(formFile.FileName, stream),
                            UseFilename = true,
                            UniqueFilename = false,
                            Overwrite = true,
                        };
                        uploadResult = await cloudinary.UploadAsync(uploadParams);
                    }
                }
                return uploadResult;
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}

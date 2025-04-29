using Epic_Bid.Core.Domain.Entities.Products;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Epic_Bid.Apis.Controllers.UploadImageHandlerExtension
{
    public static class UploadImageHandler
    {
        public static string UploadImage(IFormFile Image)
        {
            // Check if the image is null
            if (Image == null || Image.Length == 0)
            {
                throw new ArgumentException("Image cannot be null or empty.");
            }
            // Get The Extension and check it
            List<string> ValidExtensions = new List<string> { ".jpg", ".jpeg", ".png" };
            var extention = Path.GetExtension(Image.FileName);
            if( !ValidExtensions.Contains(extention))
            {
                throw new ArgumentException("Invalid image format. Only .jpg, .jpeg, .png are allowed.");
            }
            // Size
            if (Image.Length > 5 * 1024 * 1024) // 2 MB
            {
                throw new ArgumentException("Image size exceeds the limit of 5 MB.");
            }
            // Generate a unique file name
            var fileName = Guid.NewGuid().ToString() + extention;
            // Set the path to save the image
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images","Products");
            // save 
            using FileStream stream = new FileStream(Path.Combine(path,fileName), FileMode.Create);
            Image.CopyTo(stream);
            return Path.Combine("Images","Products",fileName);
        


        }
    }
}

 

using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Application.Dtos.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Application.Hepler.ExtensionsMethod{
    public static  class SaveImagesDisk
    {
         public static async Task <List<ImagesSave> > SaveImagesOnDisk(this IFormFileCollection photos, IWebHostEnvironment _ihostingEnvironment, string folder)
        {
             List<ImagesSave> newPaths = new List<ImagesSave>();

            if (photos != null && photos.Count > 0)
            {
                foreach (IFormFile photo in photos)
                {
                     var newPath = _ihostingEnvironment.WebRootPath + "\\imageUpload\\" + folder;
                    if (!(Directory.Exists(newPath)))
                        Directory.CreateDirectory(newPath);
                    var name = Path.GetRandomFileName() + Path.GetExtension(photo.FileName);
                    var path = Path.Combine(newPath, name).ToLower();
                    var stream = new FileStream(path, FileMode.Create);
                     await photo.CopyToAsync(stream);
                   await stream.DisposeAsync();
                     newPaths.Add(new ImagesSave{path=("imageUpload/" + folder + "/" + name),name=photo.FileName,size=photo.Length.ToString()});
                }
            }
            return  await Task.FromResult( newPaths);
        }

             public static async Task <ImagesSave> SaveImageOnDisk(this IFormFile photo, string physicalPath,string serverPath)
        {
             ImagesSave  newPaths = new  ImagesSave ();

            if (photo != null && photo.Length > 0)
            {
                
                     if (!(Directory.Exists(physicalPath)))
                        Directory.CreateDirectory(physicalPath);
                    var name = Path.GetRandomFileName() + Path.GetExtension(photo.FileName);
                    var path = Path.Combine(physicalPath, name).ToLower();
                    var stream = new FileStream(path, FileMode.Create);
                     await photo.CopyToAsync(stream);
                   await stream.DisposeAsync();
                     newPaths.path=(serverPath + "/" + name);
                     newPaths.name=photo.FileName;
                     newPaths.size=photo.Length.ToString();
                     newPaths.contentType=photo.ContentType.Split('/')[0];
                     };
                 
             
            return  await Task.FromResult( newPaths);
        }
    }
}
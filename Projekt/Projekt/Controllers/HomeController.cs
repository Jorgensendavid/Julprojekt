using Logic;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Projekt.Controllers
{
    public class HomeController : BaseController
    {
        UserRepository userRepository = new UserRepository();
        public ActionResult Index()
        { 
                return View(userRepository.StartUsers());
        }

   

        public FileContentResult UserPhotos()
        {
            if (User.Identity.IsAuthenticated)
            {
                String userId = User.Identity.GetUserId();
                var user = userRepository.getUserId(userId);
                byte[] noImg = new byte[0];

                if (user.UserPhoto.Length == noImg.Length)
                {
                    string fileName = HttpContext.Server.MapPath(@"~/Images/noImg.png");

                    byte[] imageData = null;
                    FileInfo fileInfo = new FileInfo(fileName);
                    long imageFileLength = fileInfo.Length;
                    FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fs);
                    imageData = br.ReadBytes((int)imageFileLength);

                    return File(imageData, "image/png");

                }
                // Hämtar användaren så uppladning av bild kan ske
                var bdUsers = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
                var userImage = bdUsers.Users.Where(x => x.Id == userId).FirstOrDefault();
              
                return new FileContentResult(userImage.UserPhoto, "image/jpeg");
            }
            else
            {
                return null;
            }
        }

        //Hämtar andra användares bilder
        public FileContentResult UserPhotoOthers(string id)
        {

            var user = userRepository.getUserId(id);
            byte[] noImg = new byte[0];

            if (user.UserPhoto.Length == noImg.Length)
            {
                string fileName = HttpContext.Server.MapPath(@"~/Images/noImg.png");

                byte[] imageData = null;
                FileInfo fileInfo = new FileInfo(fileName);
                long imageFileLength = fileInfo.Length;
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                imageData = br.ReadBytes((int)imageFileLength);

                return File(imageData, "image/png");

            }

            return new FileContentResult(user.UserPhoto, "image/jpeg");
        }

    }
}
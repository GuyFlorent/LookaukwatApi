//using LookaukwatApi.Models;
//using LookaukwatApi.ViewModel;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Web;
//using System.Net;
//using System.Net.Http;
//namespace LookaukwatApi.Services
//{
//    public static class ImageService
//    {

//        public static List<ImageProcductModel> ImageAdd(ImageModelView userImage)
//        {

//            List<ImageProcductModel> liste = new List<ImageProcductModel>();
//            if (userImage.ImageFile != null)
//            {


//                foreach (var image in userImage.ImageFile)
//                {

//                    if (image != null && image.ContentLength > 0)
//                    {


//                        //Save image name path
//                        string FileName = Path.GetFileNameWithoutExtension(image.FileName);

//                        // save extension of image
//                        string FileExtension = Path.GetExtension(image.FileName);

//                        //Add a curent date to attached file name
//                        FileName = DateTime.Now.ToString("yyyyMMdd") + "-" + FileName + FileExtension;

//                        //Create complete path to store in server
//                        var path =Server.MapPath("~/UserImage/");
//                        if (!Directory.Exists(path))
//                        {
//                            Directory.CreateDirectory(path);
//                        }
//                        userImage.Image = $"/UserImage/{FileName}";
//                        ImageProcductModel picture = new ImageProcductModel
//                        {
//                            Image = userImage.Image,
//                            id = Guid.NewGuid(),

//                        };
//                        liste.Add(picture);
//                        FileName = Path.Combine(path, FileName);

//                        image.SaveAs(FileName);

//                    }
//                    else
//                    {
//                        ImageProcductModel picture = new ImageProcductModel
//                        {
//                            id = Guid.NewGuid(),
//                            Image = "https://particulier-employeur.fr/wp-content/themes/fepem/img/general/avatar.png"
//                        };
//                        liste.Add(picture);
//                    }

//                }
//                return liste;
//            }
//            else
//            {
//                ImageProcductModel picture = new ImageProcductModel
//                {
//                    id = Guid.NewGuid(),
//                    Image = "https://particulier-employeur.fr/wp-content/themes/fepem/img/general/avatar.png"
//                };
//                liste.Add(picture);

//            }
//            return liste;
//        }
//    }
//}
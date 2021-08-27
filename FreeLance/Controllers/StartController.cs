using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FreeLance.Models;
namespace FreeLance.Controllers { 


    public class StartController : Controller
    {

       


       
        public ActionResult ErrorNotFound()
        {
            return View();
        }

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult ModalPopUp()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ModalPopUp(User userModel)
        {


            using (FreeLanceSystemEntities db = new FreeLanceSystemEntities())
            {
                var userDetails = db.Users.Where(x => x.UserName == userModel.UserName && x.Password == userModel.Password).FirstOrDefault();
                if (userDetails == null)
                {
                    userModel.LoginErrorMessage = "Wrong UserName OR Password";
                    return View("_login", userModel);
                }
                else
                {
                   
                    User u = new User();

                    u.ID = userDetails.ID;
                    u.LastName = userDetails.LastName;
                    u.UserName = userDetails.UserName;
                    u.Photo = userDetails.Photo;
                    u.Phone = userDetails.Phone;
                    u.FirstName = userDetails.FirstName;
                    u.Email = userDetails.Email;
                    u.Password = userDetails.Password;
                    u.Role = userDetails.Role;
                    String LN = u.LastName.ToString();


                    Session["UserID"] = u.ID;
                    var UserID = Convert.ToInt32(Session["UserID"]);
                    int Idnum = int.Parse((u.ID).ToString());
                    if(u.Role == "Client") { 
                    return RedirectToAction("AfterLogin",new {@ID = Idnum});
                    }

                    if (u.Role == "admin")
                    {
                        return RedirectToAction("Index", "Profile");
                    }
                    if (u.Role == "FreeLancer")
                    {
                        return RedirectToAction("search","FreeLancer", new { @ID = Idnum });
                    }

                }
            }
            return View("ModalPopUp", userModel);
        }



        public ActionResult ReturnToAfterLogin()
        {
            int Idnum = int.Parse(((int)Session["UserID"]).ToString());
            return RedirectToAction("AfterLogin", new { @ID = Idnum });
           
        }



        [HttpGet]
 
        public ActionResult AfterLogin(int id)
        {
            if (ModelState.IsValid)
            {
                using (FreeLanceSystemEntities db = new FreeLanceSystemEntities())
                {
                    var userDetails = db.Users.Where(x => x.ID == id).FirstOrDefault();
                    if (userDetails != null)
                    {
                        User u = new User();// select your user's data using id and assign to a user object 
                        u.ID = userDetails.ID;
                        u.Photo = userDetails.Photo;
                        u.LastName = userDetails.LastName;
                        u.Password = userDetails.Password;
                        u.Phone = userDetails.Phone;
                        u.Role = userDetails.Role;
                        u.UserName = userDetails.UserName;
                        u.FirstName = userDetails.FirstName;
                        u.Email = userDetails.Email;
                        
                        return View("AfterLogin", u);

                    }
                }
            }
                        return RedirectToAction("ErrorNotFound");// and pass the user object to view
            }
        


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile(User userModel)
        {
            
            
                int userID = int.Parse(((int)Session["UserID"]).ToString());
                String fileName = Path.GetFileNameWithoutExtension(userModel.ImageFile.FileName);
                String extension = Path.GetExtension(userModel.ImageFile.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                userModel.Photo = "~/Content/pics/" + fileName;
                fileName = Path.Combine(Server.MapPath("~/Content/pics"), fileName);
                userModel.ImageFile.SaveAs(fileName);
            if (ModelState.IsValid)
            {
                using (FreeLanceSystemEntities db = new FreeLanceSystemEntities())
                {
                    Console.WriteLine("before");
                    var userDetails = db.Users.Where(x => x.ID == userID).FirstOrDefault();
                    if (userDetails != null)
                    {
                       

                        userDetails.UserName = userModel.UserName;
                        userDetails.FirstName = userModel.FirstName;
                        userDetails.Email = userModel.Email;
                        userDetails.Password = userModel.Password;
                        userDetails.Photo = userModel.Photo;
                        userDetails.Phone = userModel.Phone;
                        db.SaveChanges();
                        return RedirectToAction("AfterLogin", new { @ID = userID });


                    }



                }
            }
            return RedirectToAction("AfterLogin", new { @ID = userID });
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register (User userModel)
        {
            if (ModelState.IsValid)
            {
                String fileName = Path.GetFileNameWithoutExtension(userModel.ImageFile.FileName);
                String extension = Path.GetExtension(userModel.ImageFile.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                userModel.Photo = "~/Content/pics/" + fileName;
                fileName = Path.Combine(Server.MapPath("~/Content/pics"), fileName);
                userModel.ImageFile.SaveAs(fileName);
                using (FreeLanceSystemEntities db = new FreeLanceSystemEntities())
                {
                   
                       
                        User u = new User();
                        u.ID = userModel.ID;
                        u.LastName = userModel.LastName;
                        u.UserName = userModel.UserName;
                        u.Photo = userModel.Photo;
                        u.Phone = userModel.Phone;
                        u.FirstName = userModel.FirstName;
                        u.Email = userModel.Email;
                        u.Password = userModel.Password;
                        u.Role = userModel.Role;


                        String LN = u.LastName.ToString();
                        int Idnum = int.Parse((u.ID).ToString());

                        db.Users.Add(u);    
                        db.SaveChanges();
                        ViewBag.Success = "Inserted";
                        return RedirectToAction("ModalPopUp");
                    
                }
              

            }
            return View("ModalPopUp");
        }
    }





   

}
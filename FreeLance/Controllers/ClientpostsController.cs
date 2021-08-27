using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FreeLance.Models;

namespace FreeLance.Controllers
{
    public class ClientpostsController : Controller
    {
        private FreeLanceSystemEntities db = new FreeLanceSystemEntities();

        public ActionResult ApproveProp(int postid)
        {
            Session["pid"] = postid;
            return View("ApproveProp");
        }


        [HttpPost]
        public ActionResult ApprovePropFreeLancer()
        {
            int postid = Convert.ToInt32(Session["pid"]);
            Post post = db.Posts.Find(postid);
            var data = db.Posts.FirstOrDefault(x => x.ID == postid);
            if (data != null)
            {
                Post p = (from x in db.Posts
                          where x.ID == postid
                          select x).First();
                p.Remove = true;
                p.Accept = false;
                int userID = int.Parse(((int)Session["UserID"]).ToString());
          
                db.SaveChanges();
                return RedirectToAction("AfterLogin","Start", new {@ID = userID });
            }
            return View("ApproveProp",new { @ID = postid});

        }

        [HttpGet]
        public ActionResult Recieved_Proposals(int id)
        {
            List<Proposal> PostProposals = new List<Proposal>();
            List<User> Freelancerspropose = new List<User>();
            User u = new User();
            User p = new User();
            PostProposals = db.Proposals.Where(x => x.PostID == id).ToList();
            foreach (Proposal item in PostProposals)
            {
               u.ID = item.UserID;
               p = db.Users.Where(x => x.ID == u.ID).FirstOrDefault();
               Freelancerspropose.Add(p);
            }
            var model = new listofproposals
            {
                Result = Freelancerspropose,
                PostID = id
                
            };

            return View("Recieved_Proposals",model);
        }

        public ActionResult Recieved_Proposals()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Post post)
        {

            if (ModelState.IsValid)
            {
                using (FreeLanceSystemEntities db = new FreeLanceSystemEntities())
                {
                    Post p = new Post();

                    p.ClientID = (int)Session["UserID"];
                    Session["PostID"] = p.ID;
                    p.Budget = post.Budget;
                    p.Type = post.Type;
                    p.Description = post.Description;
                    p.Date = DateTime.Now;
                    p.Rate = 1;
                    p.NumbrOfProposals = 0;
                    p.Remove = false;
                    p.Accept = false;
                    db.Posts.Add(p);
                    db.SaveChanges();

                    int Idnum = int.Parse((p.ClientID).ToString());

                    return RedirectToAction("AfterLogin","Start", new { @ID = Idnum });
                }
            }
            return View("CreateNewPost");
        }


        // GET: Posts
        public ActionResult CreateNewPost()
        {
            return View();
        }
      

        [HttpGet]
        public ActionResult MyPosts()
        {
            
            List<Post> MyPosts = new List<Post>();
            using (FreeLanceSystemEntities db = new FreeLanceSystemEntities())
            {
                int Idnum = int.Parse(((int)Session["UserID"]).ToString());
                MyPosts = db.Posts.Where(x => x.ClientID == Idnum && x.Remove == false).ToList();
                if (MyPosts.Count != 0)
                {
                   

                    ViewBag.myposts = MyPosts;
                    return View("MyPosts",ViewBag.myposts);
                }

            }
            return View();
        }



        public ActionResult EditPost(int Postid)
        {
            using (FreeLanceSystemEntities db = new FreeLanceSystemEntities())
            {
                var data = db.Posts.Where(x => x.ID == Postid).SingleOrDefault();
                return View(data);
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int id,Post model)
        {
            int postid = Convert.ToInt32(Session["PostID"]);
            if (ModelState.IsValid) { 
                ViewBag.id = id;
           
                using (FreeLanceSystemEntities db = new FreeLanceSystemEntities())
                {
                    var postDetails = db.Posts.FirstOrDefault(x => x.ID == id); 
                    //Post postDetails = db.Posts.Where(x => x.ID == id).FirstOrDefault();
                    if (postDetails != null)
                    {

                    postDetails.Description = model.Description;
                    postDetails.Type = model.Type;
                    postDetails.Budget = model.Budget; 
                    db.SaveChanges();
                    return View("EditPost",postDetails);
                    }
                }
            }

            return RedirectToAction("EditPost", new { @Postid = id });
        }


        public ActionResult DeletePost(int postid)
        {
             
            using (FreeLanceSystemEntities db = new FreeLanceSystemEntities())
            {
                var data = db.Posts.Where(x => x.ID == postid).SingleOrDefault();
                Session["PostID"] = data.ID;
                return View(data);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult
       DeletePost()
        {
            using (FreeLanceSystemEntities db = new FreeLanceSystemEntities())
            {
                int postid = Convert.ToInt32(Session["PostID"]);

                var data = db.Posts.FirstOrDefault(x => x.ID == postid);
                if (data != null)
                {
                    db.Posts.Remove(data);
                    db.SaveChanges();
                    return RedirectToAction("MyPosts");
                }
                else
                    return View();
            }
        }






    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FreeLance.Models;

namespace FreeLance.Controllers
{
    public class FreeLancerController : Controller
    {
        private FreeLanceSystemEntities db = new FreeLanceSystemEntities();
        // GET: Customer

        public ActionResult search()
        {
            return View();

        }
        public ActionResult proposal(int id )
        {
            Session["postid"] = id;

            
            return View();

        }
        [HttpPost]
        public ActionResult proposal()
        {

           
                int postid = Convert.ToInt32(Session["postid"]);
                var UserID = Convert.ToInt32(Session["UserID"]);
                var post = db.Proposals.FirstOrDefault(x => x.PostID == postid && x.UserID == UserID);
                if (post==null)
                {
                    Proposal p = new Proposal();
                    p.PostID = Convert.ToInt32(Session["postid"]);
                    p.UserID = UserID;
                    db.Proposals.Add(p);
                    var postN_of_Prop = db.Posts.FirstOrDefault(x => x.ID == postid);
                    postN_of_Prop.NumbrOfProposals++;
                    db.SaveChanges();
                    List<FreeLance.Models.Post> dis = new List<FreeLance.Models.Post>();
                    dis = db.Posts.Where(x => x.Accept == true && x.Remove == false).ToList();

                    List<Post> result = new List<Post>();
                    result = dis;


                    var model = new listofposts
                    {
                        Result = result
                    };
                    return View("search",model);
                }
               
           

            return View("PostProposed");

                

        }

        public ActionResult PostProposed()
        {
            return View();
        }

        public ActionResult show()
        {
            return View();

        }
        [HttpPost]

        public ActionResult search(string Name)

        {
            if (Name!="")
            {
                    List<FreeLance.Models.Post> dis = new List<FreeLance.Models.Post>();
                    dis = db.Posts.Where(x => (x.Description).ToString().Contains(Name) && x.Accept == true && x.Remove == false).ToList();
                    if (dis.Count != 0)
                    {
                        ViewBag.discription = dis;
                        return View("show");
                    }
                     else
                     {
                         dis = db.Posts.Where(x => (x.Date).ToString().Contains(Name) && x.Accept == true && x.Remove == false).ToList();
                         if (dis != null)
                         {


                            
                             ViewBag.discription = dis;

                             return View("show");
                         }
                     }
                

            }

            List<FreeLance.Models.Post> posts = new List<FreeLance.Models.Post>();
            posts = db.Posts.Where(x => x.Accept == true && x.Remove == false).ToList();

            List<Post> result = new List<Post>();
            result = posts;


            var model = new listofposts
            {
                Result = result
            };
            return View("search", model);
        }
       
        [HttpGet]
        public ActionResult search(int id,listofposts listPosts)
        {
                List<FreeLance.Models.Post> dis = new List<FreeLance.Models.Post>();
                dis = db.Posts.Where(x=>x.Accept == true && x.Remove==false).ToList();
                
                List<Post> result = new List<Post>();
                result = dis;
                var model = new listofposts
                {
                     Result = result
                };
                return View(model);
        }
    }
}


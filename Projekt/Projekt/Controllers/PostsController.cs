using Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Web;
using System.Web.Http;
using System.Net.Http;
using System.Net;
using System.Web.Http.Description;
using Projekt.Models;

namespace Projekt.Controllers
{
    public class PostsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public IQueryable<Post> GetPosts()
        {
            return db.Posts;
        }

        //Hämtar alla posts som ska visas på användarens profil (via ID).
        public List<Post> GetRelevantPosts(string id)
        {
            List<Post> Posts = db.Posts.Where(x => x.To.Id == id).ToList();
            
            return Posts;
        }
        //Lägger till en post i databasen.
        [HttpPost, ActionName("addPost")]
        public void addPost([FromBody] PostViewModel model)
        {
            Post newPost = new Post();
            var userTo = db.Users.Single(x => x.Id == model.ToID);
            newPost.To = userTo;

            var userFrom = db.Users.Single(x => x.Id == model.FromID);
            newPost.From = userFrom;

            newPost.Text = model.Text;

            db.Posts.Add(newPost);
            db.SaveChanges();
        }
    }
}
using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {
        private readonly PostContext db;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly Post post;

        public HomeController(PostContext context, IWebHostEnvironment hostEnvironment)
        {
            this._hostEnvironment = hostEnvironment;
            db = context;
            post = GetRandomPost();
        }

        public IActionResult Index()
            => View(post);
        
        private Post GetRandomPost()
        {
            var PostIds = db.Posts.Select(post => post.Id).ToArray();
            Random random = new Random();
            return db.Posts.Find(PostIds[random.Next(0, PostIds.Count())]);
        }
    }
}

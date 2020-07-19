using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class PostsController : Controller
    {
        private readonly PostContext dataBase;
        private readonly IWebHostEnvironment hostEnvironment;

        public PostsController(PostContext context, IWebHostEnvironment hostEnvironment)
        {
            this.hostEnvironment = hostEnvironment;
            dataBase = context;
        }

        public async Task<IActionResult> Index()
            => View(await dataBase.Posts.ToListAsync());

        public IActionResult Create()
            => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Picture")] Post post)
        {
            if (ModelState.IsValid)
            {
                using (var fileStream = new FileStream(CreatePostPath(post), FileMode.Create)) // files are needed to be saved in big projects
                    await post.Picture.CopyToAsync(fileStream);

                dataBase.Add(post);
                await dataBase.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return View(post);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await dataBase.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,PictureName,Descritpion,Likes")] Post post)
        {
            if (id != post.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    dataBase.Update(post);
                    await dataBase.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(post);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var post = await dataBase.Posts
                .FirstOrDefaultAsync(m => m.Id == id);

            if (post == null)
                return NotFound();

            return View(post);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await dataBase.Posts.FindAsync(id);
            dataBase.Posts.Remove(post);
            await dataBase.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
            => dataBase.Posts.Any(e => e.Id == id);

        private string CreatePostPath([Bind("Id,Title,Description,Picture")] Post post)
            => Path.Combine(hostEnvironment.WebRootPath + "\\Images\\", CreatePictureName(post));

        private string CreatePictureName([Bind("Id,Title,Description,Picture")] Post post)
        {
            string fileName = Path.GetFileNameWithoutExtension(post.Picture.FileName);
            post.PictureName = fileName = fileName + DateTime.Now.ToString("yymmssfff")
                                + Path.GetExtension(post.Picture.FileName);
            return fileName;
        }
    }
}

using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; } 
        public string Title { get; set; }
        public string PictureName { get; set; }
        public string Description { get; set; }
        public int Likes { get; set; }
        public virtual User Author { get; set; }
        [NotMapped]
        public IFormFile Picture { get; set; }
    }
}

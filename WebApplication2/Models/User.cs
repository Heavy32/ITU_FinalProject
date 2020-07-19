using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public virtual IList<Post> Posts { get; set; }
    }
}

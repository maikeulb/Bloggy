using System;
using System.Collections.Generic;
using System.Linq;

namespace Bloggy.API.Entities
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public ApplicationUser Author { get; set; }
        public DateTime CreationDate { get; set; }

        public ICollection<Comment> Comments { get; set; } = new List<Comment> ();
        public ICollection<Tag> Tags { get; set; } = new List<Tag> ();
    }
}

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

        public List<Comment> Comments { get; set; }
        public List<Tag> Tags { get; set; }
    }
}

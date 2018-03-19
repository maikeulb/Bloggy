using System;
using System.Collections.Generic;
using System.Linq;

namespace Bloggy.API.Entities
{
    public class Post
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int AuthorId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime CreatedDate { get; set; }

        public ApplicationUser Author { get; set; }
        public ICollection<Comment> Comments { get; set; } = new List<Comment> ();
        public ICollection<PostTag> PostTags { get; set; } = new List<PostTag> ();
    }
}

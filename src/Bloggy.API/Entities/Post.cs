using System;
using System.Collections.Generic;
using System.Linq;

namespace Bloggy.API.Entities
{
    public class Post
    {
        private readonly List<Comment> _comments = new List<Comment> ();
        private readonly List<Tag> _tags = new List<Tag> ();

        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public ApplicationUser Author { get; set; }
        public DateTime CreationDate { get; set; }

        public IEnumerable<Comment> Comments => _comments.AsReadOnly ();
        public IEnumerable<Tag> Tags => _tags.AsReadOnly ();
    }
}

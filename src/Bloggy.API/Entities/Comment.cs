using System;

namespace Bloggy.API.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public DateTime CreationDate { get; set; }

        public ApplicationUser Author { get; set; }
        public Post Post { get; set; }
    }
}

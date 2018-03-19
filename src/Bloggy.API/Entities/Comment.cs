using System;

namespace Bloggy.API.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public int AuthorId { get; set; }
        public string Body { get; set; }
        public DateTime CreatedDate { get; set; }

        public ApplicationUser Author { get; set; }
    }
}

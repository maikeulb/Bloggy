using System;

namespace Bloggy.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public ApplicationUser Author { get; set; }
        public DateTime CreationDate { get; set; }

        public Blog Blog { get; set; }
    }
}

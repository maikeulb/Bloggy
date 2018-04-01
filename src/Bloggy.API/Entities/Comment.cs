using System;
using Bloggy.API.Entities.Interfaces;

namespace Bloggy.API.Entities
{
    public class Comment: IEntity
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public int AuthorId { get; set; }
        public string Body { get; set; }
        public DateTime CreatedDate { get; set; }

        public ApplicationUser Author { get; set; }
    }
}

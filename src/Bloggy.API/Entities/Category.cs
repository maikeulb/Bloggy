using System.Collections.Generic;
using Bloggy.API.Entities.Interfaces;

namespace Bloggy.API.Entities
{
    public class Category: IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Post> Posts { get; set; } = new List<Post> ();
    }
}

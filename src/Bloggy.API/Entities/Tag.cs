using System.Collections.Generic;
using Bloggy.API.Entities.Interfaces;

namespace Bloggy.API.Entities
{
    public class Tag: IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<PostTag> PostTags { get; set; } = new List<PostTag> ();
    }
}

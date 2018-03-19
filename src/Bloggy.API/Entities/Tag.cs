using System.Collections.Generic;

namespace Bloggy.API.Entities
{
    public class Tag
    {
        public string Id { get; set; }

        public ICollection<PostTag> PostTags { get; set; } = new List<PostTag> ();
    }
}

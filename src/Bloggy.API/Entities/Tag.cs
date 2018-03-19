using System.Collections.Generic;

namespace Bloggy.API.Entities
{
    public class Tag
    {
        public string Id { get; set; }

        public List<PostTag> PostTags { get; set; }
    }
}

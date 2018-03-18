using System.Collections.Generic;

namespace Bloggy.API.Entities
{
    public class Tag
    {
        private readonly List<PostTag> _postTags = new List<PostTag> ();

        public string Id { get; set; }

        public IEnumerable<PostTag> PostTags => _postTags.AsReadOnly();
    }
}

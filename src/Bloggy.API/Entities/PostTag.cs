using Bloggy.API.Entities.Interfaces;

namespace Bloggy.API.Entities
{
    public class PostTag: IEntity
    {
        public int PostId { get; set; }
        public int TagId { get; set; }

        public Post Post { get; set; }
        public Tag Tag { get; set; }
    }
}

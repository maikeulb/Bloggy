using Bloggy.API.Entities.Interfaces;

namespace Bloggy.API.Entities
{
    public class ApplicationUser: IEntity
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public byte[] HashedPassword { get; set; }
        public byte[] Salt { get; set; }
    }
}

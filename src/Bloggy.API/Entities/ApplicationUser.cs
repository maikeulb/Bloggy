using System.Collections.Generic;

namespace Bloggy.Entities
{
    public class ApplicationUser
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public byte[] HashedPassword { get; set; }
        public byte[] Salt { get; set; }
    }
}

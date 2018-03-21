using System;
using System.Security.Cryptography;
using System.Text;

namespace Bloggy.API.Services.Interfaces
{
    public interface IPasswordHasher
    {
        byte[] Hash(string password, byte[] salt);
    }
}

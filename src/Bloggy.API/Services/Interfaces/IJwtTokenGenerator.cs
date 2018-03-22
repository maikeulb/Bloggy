using System.Threading.Tasks;

namespace Bloggy.API.Services.Interfaces
{
    public interface IJwtTokenGenerator
    {
        Task<string> CreateToken (string username);
    }
}
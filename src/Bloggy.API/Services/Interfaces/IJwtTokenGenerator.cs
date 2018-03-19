using System.Threading.Tasks;

namespace Bloggy.API.Infrastructure.Interfaces
{
    public interface IJwtTokenGenerator
    {
        Task<string> CreateToken(string username);
    }
}

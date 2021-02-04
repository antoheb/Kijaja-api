using System.Threading.Tasks;
using Domain;

namespace Application.Interface
{
    public interface IJwtGenerator
    {
        string CreateToken(AppUser user);
    }
}
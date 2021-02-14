using System.Threading.Tasks;

namespace Application.Interface
{
    public interface ICaptchaVerificator
    {
        Task<bool> VerifyToken(string token);
    }
}
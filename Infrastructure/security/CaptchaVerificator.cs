using System.Net.Http;
using System.Threading.Tasks;
using Application.Interface;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Infrastructure.security
{
    class CaptchaVerificationResponse
    {
        public bool Success { get; set; }
    }
    public class CaptchaVerificator : ICaptchaVerificator
    {
        private readonly string _key;
        public CaptchaVerificator(IConfiguration configuration)
        {
            _key = configuration["Captcha:key"];
        }
        public async Task<bool> VerifyToken(string token)
        {
            var googleUrl = "https://www.google.com/recaptcha/api/siteverify";

            var client = new HttpClient();

            var response = await client.PostAsync($"{googleUrl}?secret={_key}&response={token}", null);
            var jsonString = await response.Content.ReadAsStringAsync();
            var captchaVerfication = JsonConvert.DeserializeObject<CaptchaVerificationResponse>(jsonString);

            return captchaVerfication.Success;
        }
    }
}
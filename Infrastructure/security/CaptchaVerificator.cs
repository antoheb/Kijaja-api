using System.Net.Http;
using System.Threading.Tasks;
using Application.Interface;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Infrastructure.security
{
    // This class is simply used to serve as a template when converting the JSON string into an object.
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
            // The URL we want to make a request to.
            var googleUrl = "https://www.google.com/recaptcha/api/siteverify";

            var client = new HttpClient();

            // We stored the private key in an environment variable. 
            var response = await client.PostAsync($"{googleUrl}?secret={_key}&response={token}", null);
            var jsonString = await response.Content.ReadAsStringAsync();

            // Get the response and convert it back into an object.
            var captchaVerfication = JsonConvert.DeserializeObject<CaptchaVerificationResponse>(jsonString);

            // Return true or false depending on if the token has been verified or not.
            return captchaVerfication.Success;
        }
    }
}
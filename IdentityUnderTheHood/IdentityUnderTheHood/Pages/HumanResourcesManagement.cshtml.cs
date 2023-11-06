using IdentityUnderTheHood.DTO;
using IdentityUnderTheHood.Pages.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using System.Net.Http.Headers;

namespace IdentityUnderTheHood.Pages
{
    [Authorize("MustBelongToHRManagent")]
    public class HumanResourcesManagementModel : PageModel
    {
        private IHttpClientFactory httpClientFactory;

        [BindProperty]
        public List<WeatherForecastDTO> weatherForecastItems { get; set; } = new List<WeatherForecastDTO>();


        public HumanResourcesManagementModel(IHttpClientFactory httpClientFactory)        
        { 
            this.httpClientFactory = httpClientFactory;
        }


        public async Task OnGetAsync()
        {
            //get session token
            JwtToken token = null;
            var tokenText = HttpContext.Session.GetString("access_key");


            if (string.IsNullOrEmpty(tokenText))
            {
                tokenText = await Authenticate();
            }
            else
            {
                token = JsonConvert.DeserializeObject<JwtToken>(tokenText) ?? new JwtToken();
            }

            if (token == null || string.IsNullOrWhiteSpace(token.AccessToken) || token.ExpiresAt <= DateTime.UtcNow) 
            {
                tokenText = await Authenticate();
                token = JsonConvert.DeserializeObject<JwtToken>(tokenText) ?? new JwtToken();
            }


            //get data
            var httpClient = httpClientFactory.CreateClient("OurWebAPI");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token?.AccessToken??string.Empty);
            weatherForecastItems = await httpClient.GetFromJsonAsync<List<WeatherForecastDTO>>("WeatherForecast") ?? new List<WeatherForecastDTO>();
        }

        private async Task<string> Authenticate()
        {

            var httpClient = httpClientFactory.CreateClient("OurWebAPI");
            
            var tokenResult = await httpClient.PostAsJsonAsync("auth", new Credential { Username = "admin", Password = "password123" });
            tokenResult.EnsureSuccessStatusCode();
            
            string tokenText = await tokenResult.Content.ReadAsStringAsync();
            return tokenText;
        }
    }
}

using DemoASPApp.model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using UserManagement.Model.User;

namespace DemoASPApp.Pages
{
    public class Index1Model : BaseModel
    {
        private readonly ILogger<Index1Model> _logger;
        private readonly HttpClientConnector _httpClientConnector;

        public Index1Model(ILogger<Index1Model> logger, HttpClientConnector httpClientConnector)
        {
            _logger = logger;
            _httpClientConnector = httpClientConnector;
        }

        public async Task OnGetAsync()
        {
            string uname = Request.Query["un"];
            

            if (TempData.ContainsKey("sessionUser"))
            {
                TempData.Keep("sessionUser");
            }

            var sessionUser = CurrentUser;
            ViewData["UserModel"] = new { user = sessionUser };

            if (CurrentUser != null)
            {
                ViewData["uname"] = CurrentUser.userName;

                await LoadPromotionStatus(_httpClientConnector);
            }
        }

        public IActionResult OnGetLogout()
        {
            TempData.Remove("sessionUser");
            CurrentUser = null; 

            return RedirectToPage("/Index");
        }
    }
}

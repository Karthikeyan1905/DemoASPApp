using DemoASPApp.model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UserManagement.Model.Department;
using UserManagement.Model.User;

namespace DemoASPApp.Pages
{
    public class UserExperienceModel : BaseModel
    {
        [BindProperty]
        public UserInfo user { get; set; }

        public List<Experience> experience { get; set; }
        private readonly HttpClientConnector _httpClientConnector;

        public UserExperienceModel(HttpClientConnector httpClientConnector)
        {
            _httpClientConnector = httpClientConnector;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            string id = Request.Query["id"];
            if (!string.IsNullOrEmpty(id))
            {
                var queryParams = new Dictionary<string, string> { { "userID", id } };
                var users = await _httpClientConnector.GetAsync<List<UserInfo>>("http://localhost:5112/UserInfo/Select", queryParams);
                user = users?.FirstOrDefault();
                experience = await _httpClientConnector.GetAsync<List<Experience>>("http://localhost:5112/Experience/Select", queryParams);
                var employees = await _httpClientConnector.GetAsync<List<Employee>>("http://localhost:5112/Employee/Select", queryParams);
                var isPromotedUser = employees.Any();

                ViewData["UserIdForMenu"] = user.userID.ToString();
                ViewData["IsPromotedUser"] = isPromotedUser;

                IsPromoted = isPromotedUser;

            }
            else
            {
                ViewData["EMsg"] = "User ID is missing.";
            }
            await LoadPromotionStatus(_httpClientConnector);
            return Page();
        }

    }
}

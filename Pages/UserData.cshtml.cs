using DemoASPApp.model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UserManagement.Model.Department;
using UserManagement.Model.User;

namespace DemoASPApp.Pages
{
    public class UserDataModel : BaseModel
    {
        [BindProperty]
        public UserInfo user { get; set; }
       public List<Phone> userPhones { get; set; } = new List<Phone>();
       
        private readonly HttpClientConnector _httpClientConnector;

        public UserDataModel(HttpClientConnector httpClientConnector)
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

                var phones = await _httpClientConnector.GetAsync<List<Phone>>("http://localhost:5112/Phone/Select", queryParams);
                userPhones = phones?.Where(p => p.userID.ToString() == id).ToList() ?? new List<Phone>();

                ViewData["UserModel"] = new { user = user };

                if (user != null)
                {
                    var empParams = new Dictionary<string, string>
                    {
                        { "userID", user.userID.ToString() },
                        { "status", "A" }
                    };

                    var employees = await _httpClientConnector.GetAsync<List<Employee>>("http://localhost:5112/Employee/Select", empParams);
                    var isPromotedUser = employees.Any();

                    ViewData["UserIdForMenu"] = user.userID.ToString();
                    ViewData["IsPromotedUser"] = isPromotedUser;

                    IsPromoted = isPromotedUser;
                }

                if (user == null)
                {
                    ViewData["EMsg"] = "User not found.";
                    return Page();
                }
            }
            else if (CurrentUser != null)
            {
                var queryParams = new Dictionary<string, string> { { "userID", CurrentUser.userID.ToString() } };

                var users = await _httpClientConnector.GetAsync<List<UserInfo>>("http://localhost:5112/UserInfo/Select", queryParams);
                user = users?.FirstOrDefault();

                var phones = await _httpClientConnector.GetAsync<List<Phone>>("http://localhost:5112/Phone/Select", queryParams);
                userPhones = phones?.Where(p => p.userID == CurrentUser.userID).ToList() ?? new List<Phone>();
            }
            await LoadPromotionStatus(_httpClientConnector);

            return Page();
        }

    }
}

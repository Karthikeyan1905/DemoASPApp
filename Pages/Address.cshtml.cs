using DemoASPApp.model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UserManagement.Model.Department;
using UserManagement.Model.User;

namespace DemoASPApp.Pages
{
    public class UserAddressModel : BaseModel
    {
        [BindProperty]
        public UserInfo user { get; set; }
        public List<Address> addresses { get; set; }


        private readonly HttpClientConnector _httpClientConnector;

        public UserAddressModel(HttpClientConnector httpClientConnector)
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
                addresses = await _httpClientConnector.GetAsync<List<Address>>("http://localhost:5112/Address/Select", queryParams);

                var employees = await _httpClientConnector.GetAsync<List<Employee>>("http://localhost:5112/Employee/Select", queryParams);
                var isPromotedUser = employees.Any();

                ViewData["UserIdForMenu"] = user.userID.ToString();
                ViewData["IsPromotedUser"] = isPromotedUser;

                IsPromoted = isPromotedUser;


                if (addresses == null)
                {
                    ViewData["EMsg"] = "User not found.";
                    return Page();
                }
            }
            await LoadPromotionStatus(_httpClientConnector);
            return Page();
        }
    }
}

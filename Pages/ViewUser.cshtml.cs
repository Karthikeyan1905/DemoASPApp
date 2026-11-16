using DemoASPApp.model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UserManagement.Model.Department;
using UserManagement.Model.User;

namespace DemoASPApp.Pages
{
    public class ViewUserModel : BaseModel
    {

        [BindProperty]
        public UserInfo user { get; set; }
        public List<UserInfo> users { get; set; } = new List<UserInfo>();
        public List<long> PromotedUserIds { get; set; } = new List<long>();

        private readonly HttpClientConnector _httpClientConnector;

        public ViewUserModel(HttpClientConnector httpClientConnector)
        {
            _httpClientConnector = httpClientConnector;
        }
        public async Task OnGetAsync()
        {
            string uname = Request.Query["un"];

            users = await _httpClientConnector.GetAsync<List<UserInfo>>("http://localhost:5112/UserInfo/Select");
            var employeeParams = new Dictionary<string, string>
                {
                    { "status", "A" }
                };

            var employees = await _httpClientConnector.GetAsync<List<Employee>>("http://localhost:5112/Employee/Select", employeeParams);
            PromotedUserIds = employees.Select(e => e.userID).Distinct().ToList();
            //ViewData["UserIdForMenu"] = user.userID.ToString();


            if (TempData.ContainsKey("sessionUser"))
            {
                TempData.Keep("sessionUser");
            }

            var sessionUser = CurrentUser;
          
            ViewData["UserModel"] = new { user = sessionUser };

            if (CurrentUser != null)
            {
                ViewData["uname"] = CurrentUser.userName;
            }

        }
        public async Task<IActionResult> OnPostChangeStatusAsync(int userID, string newStatus)
        {
            var queryParams = new Dictionary<string, string>
            {
                { "userID", userID.ToString() },
                { "status", newStatus },
                { "updatedBy", CurrentUser?.userID.ToString() ?? "1" }
            };

            await _httpClientConnector.PostAsync<bool>("http://localhost:5112/UserInfo/ChangeStatus", queryParams);

            return RedirectToPage();
        }


    }
}

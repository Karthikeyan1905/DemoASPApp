using DemoASPApp.model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using UserManagement.Model.Department;
using UserManagement.Model.User;

namespace DemoASPApp.Pages
{
    public class DepartmentModel : BaseModel
    {

        public Employee Employee { get; set; }
        [BindProperty]
        public UserInfo user { get; set; }
        public List<Department> departmentList { get; set; } = new List<Department>();
        public List<Employee> EmployeeList { get; set; } = new List<Employee>();
        private readonly HttpClientConnector _httpClientConnector;

        public DepartmentModel(HttpClientConnector httpClientConnector)
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
                departmentList = await _httpClientConnector.GetAsync<List<Department>>("http://localhost:5112/Department/Select", queryParams);
                EmployeeList = await _httpClientConnector.GetAsync<List<Employee>>("http://localhost:5112/Employee/Select", queryParams);
                
                var isPromotedUser = EmployeeList.Any();

                ViewData["UserIdForMenu"] = user.userID.ToString();
                ViewData["IsPromotedUser"] = isPromotedUser;

                IsPromoted = isPromotedUser;

            }
            await LoadPromotionStatus(_httpClientConnector);
            return Page();
        }
    }
}

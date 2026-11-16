using DemoASPApp.model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using UserManagement.Model.Department;
using UserManagement.Model.User;

namespace DemoASPApp.Pages
{
    public class DesignationModel : BaseModel
    {
        public Employee Employee { get; set; }

        [BindProperty]
        public UserInfo user { get; set; }

        public List<Designation> designationList { get; set; } = new List<Designation>();
        public List<Employee> EmployeeList { get; set; } = new List<Employee>();

        private readonly HttpClientConnector _httpClientConnector;

        public DesignationModel(HttpClientConnector httpClientConnector)
        {
            _httpClientConnector = httpClientConnector;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var queryParams = new Dictionary<string, string> { { "userID", id.ToString() } };
            var users = await _httpClientConnector.GetAsync<List<UserInfo>>("http://localhost:5112/UserInfo/Select", queryParams);
            user = users?.FirstOrDefault();

            EmployeeList = await _httpClientConnector.GetAsync<List<Employee>>("http://localhost:5112/Employee/Select", queryParams);
            Employee = EmployeeList?.FirstOrDefault();

            designationList = await _httpClientConnector.GetAsync<List<Designation>>("http://localhost:5112/Designation/Select", null);
            var isPromotedUser = EmployeeList.Any();

            ViewData["UserIdForMenu"] = user.userID.ToString();
            ViewData["IsPromotedUser"] = isPromotedUser;

            IsPromoted = isPromotedUser;
            await LoadPromotionStatus(_httpClientConnector);
            return Page();
        }
    }
}

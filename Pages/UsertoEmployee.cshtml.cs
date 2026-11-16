using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DemoASPApp.model;
using UserManagement.Model.Department;
using UserManagement.Model.User;
using UserManagement.Model.Team;

namespace DemoASPApp.Pages
{
    public class UsertoEmployeeModel : BaseModel
    {
        [BindProperty] public UserInfo user { get; set; }
        public List<Department> DepartmentList { get; set; } = new List<Department>();
        public List<Designation> DesignationList { get; set; } = new List<Designation>();
        public List<Team> TeamList { get; set; } = new List<Team>();


        private readonly HttpClientConnector _httpClientConnector;

        public UsertoEmployeeModel(HttpClientConnector httpClientConnector)
        {
            _httpClientConnector = httpClientConnector;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            string id = Request.Query["id"];
            var queryParams = new Dictionary<string, string> { { "userID", id } };
            var users = await _httpClientConnector.GetAsync<List<UserInfo>>("http://localhost:5112/UserInfo/Select", queryParams);
            user = users?.FirstOrDefault();

            var departments = await _httpClientConnector.GetAsync<List<Department>>("http://localhost:5112/Department/Select", null);
            DepartmentList = departments ?? new List<Department>();

            var designations = await _httpClientConnector.GetAsync<List<Designation>>("http://localhost:5112/Designation/Select", null);
            DesignationList = designations ?? new List<Designation>();
            var teams = await _httpClientConnector.GetAsync<List<Team>>("http://localhost:5112/Team/Select", null);
            TeamList = teams ?? new List<Team>();


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
            else
            {
                ViewData["EMsg"] = "User not found.";
                return Page();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(long userID, long departmentID, long designationID, long teamID)
        {
            await InsertEmployee(userID, departmentID, designationID);
            await AssignTeamUser(userID, teamID);
            return RedirectToPage("Index1");
        }
       

        private async Task InsertEmployee(long userID, long deptID, long designationID)
        {
            long createdBy = CurrentUser?.userID ?? 0;
            var queryParams = new Dictionary<string, string>
    {
        { "userID", userID.ToString() },
        { "deptID", deptID.ToString() },
        { "designationID", designationID.ToString() },
        { "status", "A" },
        { "createdBy", createdBy.ToString() }
    };

            await _httpClientConnector.PostAsync<bool>("http://localhost:5112/Employee/Insert", queryParams);
        }
        private async Task AssignTeamUser(long userID, long teamID)
        {
            var empParams = new Dictionary<string, string>
    {
        { "userID", userID.ToString() },
        { "status", "A" }
    };

            var employees = await _httpClientConnector.GetAsync<List<Employee>>(
                "http://localhost:5112/Employee/Select", empParams);

            var employee = employees?.FirstOrDefault();

            if (employee == null)
            {
                throw new Exception("Employee record not found for userID: " + userID);
            }

            var queryParams = new Dictionary<string, string>
    {
        { "employeeID", employee.employeeID.ToString() },
        { "userID", userID.ToString() }, 
        { "teamID", teamID.ToString() },
        { "status", "A" },
        { "createdBy", CurrentUser?.userID.ToString() ?? "0" }
    };

            await _httpClientConnector.PostAsync<bool>("http://localhost:5112/TeamUser/Insert", queryParams);
        }
    }
}


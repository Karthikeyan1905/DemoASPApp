using DemoASPApp.model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UserManagement.Model.Department;
using UserManagement.Model.Team;
using UserManagement.Model.User;

namespace DemoASPApp.Pages
{
    public class TeamModel : BaseModel
    {
        [BindProperty]
        public UserInfo user { get; set; }
        public Team team { get; set; }

        private readonly HttpClientConnector _httpClientConnector;

        public TeamModel(HttpClientConnector httpClientConnector)
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

                var teamUsers = await _httpClientConnector.GetAsync<List<TeamUser>>("http://localhost:5112/TeamUser/Select", queryParams);
                var teamUser = teamUsers?.FirstOrDefault();
                var employees = await _httpClientConnector.GetAsync<List<Employee>>("http://localhost:5112/Employee/Select", queryParams);
                var isPromotedUser = employees.Any();

                ViewData["UserIdForMenu"] = user.userID.ToString();
                ViewData["IsPromotedUser"] = isPromotedUser;

                IsPromoted = isPromotedUser;

                if (teamUser != null)
                {
                    var teamQuery = new Dictionary<string, string> { { "teamID", teamUser.teamID.ToString() } };
                    var teams = await _httpClientConnector.GetAsync<List<Team>>("http://localhost:5112/Team/Select", teamQuery);
                    team = teams?.FirstOrDefault();
                }
            }
            await LoadPromotionStatus(_httpClientConnector);
            return Page();
        }
    }
}

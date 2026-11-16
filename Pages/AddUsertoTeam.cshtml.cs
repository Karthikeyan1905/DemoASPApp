using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UserManagement.Model.Department;
using UserManagement.Model.Team;
using UserManagement.Model.User;

public class AddUsertoTeamModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public int TeamID { get; set; }

    [BindProperty]
    public int SelectedUserID { get; set; }

    public string TeamName { get; set; }
    public List<UserInfo> Users { get; set; } = new();
    public List<TeamUser> TeamUsers { get; set; } = new();

    private readonly HttpClientConnector _httpClientConnector;

    public AddUsertoTeamModel(HttpClientConnector httpClientConnector)
    {
        _httpClientConnector = httpClientConnector;
    }
    public async Task OnGetAsync()
    {
        await LoadTeamDetails();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        long employeeID = 0;

        var employeeParams = new Dictionary<string, string>
        {
            { "userid", SelectedUserID.ToString() },
            { "status", "A" }
        };

        var employees = await _httpClientConnector.GetAsync<List<Employee>>(
            "http://192.168.7.233:5112/Employee/Select", employeeParams
        );

        employeeID = employees?.FirstOrDefault()?.employeeID ?? 0;

        var insertParams = new Dictionary<string, string>
        {
            { "userid", SelectedUserID.ToString() },
            { "teamID", TeamID.ToString() },
            { "employeeID", employeeID.ToString() },
            { "status", "A" },
            { "createdBy", "1" }
        };

        var result = await _httpClientConnector.PostAsync<bool>(
            "http://192.168.7.233:5112/TeamUser/Insert", insertParams
        );

        if (result)
        {
            TempData["Success"] = "User assigned successfully!";
            return RedirectToPage("AddUsertoTeam", new { teamID = TeamID });
        }

        TempData["Error"] = "Failed to assign user.";
        return RedirectToPage("AddUsertoTeam", new { teamID = TeamID });
    }

    private async Task LoadTeamDetails()
    {
        var teamParams = new Dictionary<string, string>
    {
        { "teamID", TeamID.ToString() }
    };

        var teams = await _httpClientConnector.GetAsync<List<Team>>(
            "http://192.168.7.233:5112/Team/Select", teamParams
        );

        TeamName = teams?.FirstOrDefault()?.teamName ?? "Unknown";

        var teamUserParams = new Dictionary<string, string>
    {
        { "status", "A" }
    };

        var allTeamUsers = await _httpClientConnector.GetAsync<List<TeamUser>>(
            "http://192.168.7.233:5112/TeamUser/Select", teamUserParams
        );

        TeamUsers = allTeamUsers?.Where(tu => tu.teamID == TeamID).ToList() ?? new List<TeamUser>();

        var userParams = new Dictionary<string, string>
    {
        { "status", "A" }
    };

        Users = await _httpClientConnector.GetAsync<List<UserInfo>>(
            "http://192.168.7.233:5112/UserInfo/Select", userParams
        ) ?? new List<UserInfo>();
    }

}

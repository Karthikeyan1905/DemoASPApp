using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using UserManagement.Model.Team;
using UserManagementModel.Model.Team;

public class CreateTeamModel : PageModel
{
    [BindProperty]
    public string TeamName { get; set; }

    [BindProperty]
    public string TeamCode { get; set; }

    public List<Team> Teams { get; set; } = new();
    public List<TeamStage> TeamStages { get; set; } = new();


    private readonly HttpClientConnector _httpClientConnector;

    public CreateTeamModel(HttpClientConnector httpClientConnector)
    {
        _httpClientConnector = httpClientConnector;
    }

    public async Task OnGetAsync()
    {
        await LoadTeams();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (string.IsNullOrWhiteSpace(TeamName) || string.IsNullOrWhiteSpace(TeamCode))
        {
            await LoadTeams();
            return Page();
        }

        var queryParams = new Dictionary<string, string>
        {
            { "teamname", TeamName },
            { "teamcode", TeamCode },
            { "status", "A" },
            { "createdby", "2" }
        };

        var result = await _httpClientConnector.PostAsync<bool>(
            "http://localhost:5112/Team/Insert", queryParams
        );

        if (result)
        {
            TempData["Success"] = "Team created successfully!";
            return RedirectToPage("CreateTeam");
        }

        await LoadTeams();
        return Page();
    }

    private async Task LoadTeams()
    {
        var queryParams = new Dictionary<string, string>
        {
            { "status", "A" }
        };

        var teams = await _httpClientConnector.GetAsync<List<Team>>(
            "http://localhost:5112/Team/Select", queryParams
        );

        Teams = teams ?? new List<Team>();

        TeamStages = await _httpClientConnector.GetAsync<List<TeamStage>>(
        "http://localhost:5112/TeamStage/Select", queryParams
    ) ?? new List<TeamStage>();
    }
}

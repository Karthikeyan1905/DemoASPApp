using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UserManagement.Model.Team;
using UserManagementModel.Model.Team;

public class AddStageModel : PageModel
{
    private readonly HttpClientConnector _httpClientConnector;

    public AddStageModel(HttpClientConnector httpClientConnector)
    {
        _httpClientConnector = httpClientConnector;
    }

    [BindProperty(SupportsGet = true)]
    public int TeamID { get; set; }

    [BindProperty]
    public int SelectedStageID { get; set; }
    public string TeamName { get; set; }
    public string Message { get; set; }

    public List<Stage> stages { get; set; } = new();

    public async Task OnGetAsync()
    {
        await LoadTeamAndStages();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var existingAssignments = await _httpClientConnector.GetAsync<List<TeamStage>>(
    "http://localhost:5112/TeamStage/Select", new Dictionary<string, string> { { "status", "A" } });

        var insertParams = new Dictionary<string, string>
        {
            { "stageid", SelectedStageID.ToString() },
            { "teamid", TeamID.ToString() },
            { "status", "A" },
            { "createdby", "1" }
        };

        var result = await _httpClientConnector.PostAsync<bool>(
            "http://localhost:5112/TeamStage/Insert", insertParams
        );
        if (result)
        {
            return RedirectToPage("/CreateTeam");
        }
        //bool teamHasStage = existingAssignments.Any(ts => ts.teamid == TeamID);
        //bool stageAlreadyUsed = existingAssignments.Any(ts => ts.stageid == SelectedStageID);
        //if (teamHasStage || stageAlreadyUsed)
        //{

        //    return Page();
        //}
       

        
        await LoadTeamAndStages();
        return Page();
    }

    private async Task LoadTeamAndStages()
    {
        var teamParams = new Dictionary<string, string>
        {
            { "teamID", TeamID.ToString() }
        };

        var teams = await _httpClientConnector.GetAsync<List<Team>>(
            "http://localhost:5112/Team/Select", teamParams
        );

        TeamName = teams?.FirstOrDefault()?.teamName ?? "Unknown";

        var stageParams = new Dictionary<string, string>
        {
            { "status", "A" }
        };

        stages = await _httpClientConnector.GetAsync<List<Stage>>(
            "http://localhost:5112/Stage/Select", stageParams
        ) ?? new List<Stage>();
    }
}

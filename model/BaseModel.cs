using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using UserManagement.Model.Department;
using UserManagement.Model.User;

namespace DemoASPApp.model
{
    public class BaseModel : PageModel
    {
        private UserInfo _currentUser;

        public UserInfo CurrentUser
        {
            get
            {
                if (_currentUser != null) return _currentUser;

                if (TempData["sessionUser"] is string json && !string.IsNullOrEmpty(json))
                {
                    TempData.Keep("sessionUser");
                    _currentUser = JsonSerializer.Deserialize<UserInfo>(json);
                }

                return _currentUser;
            }
            set
            {
                _currentUser = value;
                TempData["sessionUser"] = JsonSerializer.Serialize(value);
                TempData.Keep("sessionUser");
            }
        }
        public async Task LoadPromotionStatus(HttpClientConnector connector)
        {
            if (CurrentUser != null)
            {
                var empParams = new Dictionary<string, string>
        {
            { "userID", CurrentUser.userID.ToString() },
            { "status", "A" }
        };

                var employees = await connector.GetAsync<List<Employee>>("http://localhost:5112/Employee/Select", empParams);
                IsPromoted = employees.Any();
            }
        }

        public bool IsPromoted { get; set; } = false;
        public bool IsAdmin => CurrentUser?.userRoleID == 1;
        public void SetPromotionStatus(bool promoted)
        {
            IsPromoted = promoted;
        }
    }
}

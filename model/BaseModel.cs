using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using UserManagement.User;

namespace DemoASPApp.model
{
    public class BaseModel:PageModel
    {
        [BindProperty]
        
        public UserInfo CurrentUser
        {
            get
            {
                if (TempData["sessionUser"] != null)
                {
                    TempData.Keep("sessionUser");
                    string json = TempData["sessionUser"] as string;
                    return JsonSerializer.Deserialize<UserInfo>(json);
                }
                return null;
            }
        }
        public bool IsAdmin => CurrentUser?.userRoleID == 1;

        public void LoadSession()
        {
            TempData["sessionUser"] = JsonSerializer.Serialize(CurrentUser);
            TempData.Keep("sessionUser");
        }

        
    }
}

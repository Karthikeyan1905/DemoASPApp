using Microsoft.AspNetCore.Mvc;
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
                    string json = TempData["sessionUser"] as string;
                    return JsonSerializer.Deserialize<UserInfo>(json);
                }
                return null;
            }
        }

        public void LoadSession()
        {
            TempData["sessionUser"] = JsonSerializer.Serialize(CurrentUser);
            TempData.Keep("sessionUser");
        }
    }
}

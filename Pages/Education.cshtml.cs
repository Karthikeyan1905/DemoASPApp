using DemoASPApp.model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UserManagement.User;

namespace DemoASPApp.Pages
{
    public class UserEducationModel : BaseModel
    {
        [BindProperty]
        public UserInfo user { get; set; }

        public void OnGet()
        {
            string id = Request.Query["id"];
            user = Common.users.FirstOrDefault(u => u.userID.ToString() == id);
        }
    }
}

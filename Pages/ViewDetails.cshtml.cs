using DemoASPApp.model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UserManagement.User;

namespace DemoASPApp.Pages
{
    public class ViewDetailsModel : BaseModel
    {

        [BindProperty]
        public UserInfo user { get; set; }
        public void OnGet()
        {
            string id = Request.Query["id"];

            if (!string.IsNullOrEmpty(id))
            {
                user = Common.users.FirstOrDefault(u => u.userID.ToString().Equals(id));
            }
            else if (CurrentUser != null)
            {
                user = Common.users.FirstOrDefault(u => u.userID == CurrentUser.userID);
            }
        }
    }
    
}

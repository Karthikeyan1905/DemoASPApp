using DemoASPApp.model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UserManagement.User;

namespace DemoASPApp.Pages
{
    public class ViewUserModel : BaseModel
    {
        

        public void OnGet(string un)
        {
            Common.LoadRegisterUsers(); 
            
        }

        public IActionResult OnGetLogout(string un)
        {
            Common.LoadRegisterUsers();
            Common.LoadLoginHistory();

            var user = Common.users.FirstOrDefault(u => u.userName == un);
            if (user != null && user.userID.HasValue)
            {
                Common.MarkLogout((long)user.userID);
            }

            return RedirectToPage("Index");
        }
    }
}

using DemoASPApp.model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UserManagement.User;

namespace DemoASPApp.Pages
{
    public class InfoModel : PageModel
    {

      
        public void OnGet()
        {



        }

        public ActionResult OnPost(UserInfo user)
        {

            user.userID = Common.users.Count > 0
        ? Common.users.Max(u => u.userID ?? 0) + 1
        : 1;

            Common.users.Add(user);
            return RedirectToPage("Index");
        }

    }


}
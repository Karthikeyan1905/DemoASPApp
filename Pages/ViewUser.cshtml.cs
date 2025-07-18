using DemoASPApp.model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UserManagement.User;

namespace DemoASPApp.Pages
{
    public class ViewUserModel : BaseModel
    {

        [BindProperty]
        public UserInfo user { get; set; }
        public void OnGet()
        {
            string uname = Request.Query["un"];
            Common.LoadRegisterUsers();

            if (TempData.ContainsKey("sessionUser"))
            {

                TempData.Keep("sessionUser");
            }

            var sessionUser = CurrentUser;
            LoadSession();
            ViewData["UserModel"] = new { user = sessionUser };

            if (CurrentUser != null)
            {
                ViewData["uname"] = CurrentUser.userName;
            }
        }

    }
}

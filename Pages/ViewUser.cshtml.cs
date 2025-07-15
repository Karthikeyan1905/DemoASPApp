using DemoASPApp.model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UserManagement.User;
namespace DemoASPApp.Pages
{
    public class ViewUserModel : PageModel
    {
        [BindProperty]
        public UserInfo CurrentUser { get; set; }

        public void OnGet(string id)
        {
            Common.LoadRegisterUsers();
            CurrentUser = Common.users.FirstOrDefault(u => u.userID.ToString() == id);
            ViewData["UserModel"] = this;
        }
    }
}


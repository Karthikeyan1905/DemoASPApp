using DemoASPApp.model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UserManagement.Department;
using UserManagement.User;

namespace DemoASPApp.Pages
{
    
    public class LoginModel : PageModel
        {
        public bool isEmployee { get { return Request.Form["isEmployee"].Equals("on"); } }
        public void OnGet()
            {
            Common.LoadRegisterUsers();
            Common.LoadRegisteredEmployees();
        }

        public IActionResult OnPost(string uname, string psw)
        {
            if (!string.IsNullOrEmpty(uname))
            {
                UserInfo user = null;

                if (!isEmployee)
                {
                    user = Common.users.FirstOrDefault(u => u.userName == uname);
                }
                else
                {
                    var emp = Common.employees?.FirstOrDefault(e => e.user.userName == uname);
                    user = emp?.user;
                }

                if (user != null)
                {
                    return RedirectToPage("Index1", new { un = user.userName });
                }
                    
                ViewData["EMsg"] = "Username not found. Please register first.";
                return Page();
            }

            ViewData["EMsg"] = "Username is required.";
            return Page();
        }

    }
}

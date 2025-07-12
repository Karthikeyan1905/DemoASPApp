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
            if (!string.IsNullOrWhiteSpace(uname))
            {
                UserInfo user = null;

                if (!isEmployee)
                {
                    foreach (var u in Common.users)
                    {
                        if (u.loginCredential != null &&
                            u.loginCredential.loginUsername == uname && u.loginCredential.loginPassword == psw)
                        {
                            user = u;
                            break;
                        }
                    }
                }
                else
                {
                    if (Common.employees != null)
                    {
                        foreach (var e in Common.employees)
                        {
                            if (e.user != null &&
                                e.user.loginCredential != null &&
                                e.user.loginCredential.loginUsername == uname )
                            {
                                user = e.user;
                                break;
                            }
                        }
                    }
                }

                if (user != null)
                {

                    return RedirectToPage("Index1", new { un = user.userName, isEmployee = isEmployee });
                }

                ViewData["EMsg"] = "Username not found. Please register first.";
                return Page();
            }

            ViewData["EMsg"] = "Username is required.";
            return Page();
        }

    }
}
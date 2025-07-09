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
                    if(Common.employees!= null &&  Common.employees.Count > 0)  
                    user = Common.employees.FirstOrDefault(u => u.user.userName == uname).user;
                }
                if (user != null)
                {
                    return RedirectToPage("Index1");
                }
                else
                {
                    ViewData["EMsg"] = "Username not found. Please register first.";
                    return Page();
                }
            }
            ViewData["EMsg"] = "Username is required.";

            

            return Page();
        }
    }
}

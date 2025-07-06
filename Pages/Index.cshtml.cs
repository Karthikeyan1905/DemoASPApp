using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UserManagement.User;
using DemoASPApp.model;

namespace DemoASPApp.Pages
{
        public class LoginModel : PageModel
        {
            public void OnGet()
            {
            }

        public IActionResult OnPost(string uname, string psw)
        {
            if (!string.IsNullOrEmpty(uname))
            {
                var user = Common.users.FirstOrDefault(u => u.userName == uname );
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

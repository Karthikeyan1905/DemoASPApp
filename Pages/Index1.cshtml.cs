using DemoASPApp.model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using UserManagement.User;

namespace DemoASPApp.Pages
{
    public class Index1Model : BaseModel
    {
        private readonly ILogger<Index1Model> _logger;

        public Index1Model(ILogger<Index1Model> logger)
        {
            _logger = logger;
        }



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

        public IActionResult OnGetLogout()
        {
            Common.LoadRegisterUsers();
            Common.LoadLoginHistory();
            LoadSession();

            if (CurrentUser != null)
            {
                
                Common.MarkLogout((long)CurrentUser.userID);
            }
            return RedirectToPage("/Index");
        }
    }
}

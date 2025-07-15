using DemoASPApp.model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UserManagement.User;

namespace DemoASPApp.Pages
{
    public class Index1Model : PageModel
    {
        private readonly ILogger<Index1Model> _logger;

        public Index1Model(ILogger<Index1Model> logger)
        {
            _logger = logger;
        }
        [BindProperty]
        public UserInfo CurrentUser { get; set; }

        public void OnGet(string un)
        {
            Common.LoadRegisterUsers();
            CurrentUser = Common.users.FirstOrDefault(u => u.userName == un);
            ViewData["UserModel"] = this;
        }
        public IActionResult OnGetLogout(string un)
        {
            Common.LoadRegisterUsers();
            Common.LoadLoginHistory(); 
            var user = Common.users.FirstOrDefault(u => u.userName == un);

            if (user != null && user.userID != null)
            {
                Common.MarkLogout((long)user.userID);
            }

            return RedirectToPage("Index");
        }
    }
}
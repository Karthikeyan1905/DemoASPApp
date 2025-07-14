using DemoASPApp.model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DemoASPApp.Pages
{
    public class Index1Model : PageModel
    {
        private readonly ILogger<Index1Model> _logger;

        public Index1Model(ILogger<Index1Model> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

            ViewData["uname"] = Request.Query["un"].ToString();
        }
        public IActionResult OnGetLogout(string un)
        {
            Common.LoadRegisterUsers();
            Common.LoadLoginHistory(); 
            var user = Common.users.FirstOrDefault(u => u.userName == un);

            if (user != null)
            {
                Common.MarkLogout(user.userID ?? 0); 
            }

            return RedirectToPage("Index");
        }
    }
}
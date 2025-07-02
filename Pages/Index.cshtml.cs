using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DemoASPApp.Pages
{
    public class LoginModel : PageModel
    {
        public void OnGet()
        {
        }

        public ActionResult OnPost(string uname, string psw)
        {
            if (!string.IsNullOrEmpty(uname) && !string.IsNullOrEmpty(psw))
            {
                
               
                return RedirectToPage("Index1", new { un = uname,pwd= psw,id=2 });

            }
            else
            {

                ViewData["EMsg"] = "Invalid User Name/ Password";
            
            }
            return Page();
        }
    }
}

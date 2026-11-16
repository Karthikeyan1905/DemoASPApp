using DemoASPApp.model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using UserManagement.Model.User;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace DemoASPApp.Pages
{

    public class IndexModel : PageModel
    {
        //public bool isEmployee { get { return Request.Form["isEmployee"].Equals("on"); } }
        public void OnGet()
        {
            
        }

        public async Task<IActionResult> OnPostAsync(string uname, string psw)
        {
            if (!string.IsNullOrWhiteSpace(uname) && !string.IsNullOrWhiteSpace(psw))
            {
                UserInfo user = null;
                var httpClient = new HttpClient();
                var connector = new HttpClientConnector(httpClient);

                var queryParams = new Dictionary<string, string>
                {
                    { "status", "A" } 
                };

                var credentials = await connector.GetAsync<List<LoginCredential>>("http://localhost:5112/LoginCredential/Select", queryParams);

                var matchedCredential = credentials?.FirstOrDefault(u =>
                    u.loginUsername == uname && u.login_Password == psw);

                if (matchedCredential != null)
                {
                    if (matchedCredential.status == "A")
                    {
                        user = matchedCredential.user;
                    }
                    else if (matchedCredential.status == "IA")
                    {
                        ViewData["EMsg"] = "User is InActive. Please contact admin.";
                        return Page();
                    }
                    else
                    {
                        ViewData["EMsg"] = "User is Pending. Please wait for Approve.";
                        return Page();
                    }
                }

                if (user != null)
                {
                    TempData["sessionUser"] = JsonSerializer.Serialize(user);
                    return RedirectToPage("Index1", new { un = user.userName });
                }

                ViewData["EMsg"] = "Username not found. Please register first.";
                return Page();
            }

            ViewData["EMsg"] = "Username and Password is required.";
            return Page();
        }
    }
    }

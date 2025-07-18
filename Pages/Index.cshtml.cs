    using DemoASPApp.model;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using System.Text.Json;
    using UserManagement.Department;
    using UserManagement.User;

    namespace DemoASPApp.Pages
    {

        public class IndexModel : PageModel
        {
            public bool isEmployee { get { return Request.Form["isEmployee"].Equals("on"); } }
            public void OnGet()
            {
                Common.LoadRegisterUsers();
                Common.LoadRegisteredEmployees();

            }

            public IActionResult OnPost(string uname, string psw)
            {
                if (!string.IsNullOrWhiteSpace(uname) && !string.IsNullOrWhiteSpace(psw))
                {
                    UserInfo user = null;

                        foreach (var u in Common.users)
                        {
                        if (u.loginCredential != null &&
                        u.loginCredential.loginUsername == uname && u.loginCredential.loginPassword == psw)
                            {
                                if (u.Status == "A") { 
                                user = u;
                                break;
                            }
                                else
                                {
                                    ViewData["EMsg"] = "User is Pending Please Wait for Approval.";
                                    return Page();
                                }  
                        }
                        }

                    if (user != null )
                    {
                    
                        Common.AddLoginRecord(user.userID ?? 0);
                        Common.CurrentUser = user;
                        TempData["sessionUser"] = JsonSerializer.Serialize(user);
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
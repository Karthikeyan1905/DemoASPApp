using DemoASPApp.model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Dynamic;

namespace DemoASPApp.Pages
{
    public class InfoModel : PageModel
    {
       

        public void OnGet()
        {

           

        }
      
        public ActionResult OnPost(User user)
        {
            Common.users.Add(user);
            return RedirectToPage("Privacy");
        }

    }

    public class User
    {

        public int id { get; set; }
        public string? email { get; set; }
        public string? userName { get; set; }
        public string? fatherName { get; set; }
        public string? Desgination { get; set; }

    }
}

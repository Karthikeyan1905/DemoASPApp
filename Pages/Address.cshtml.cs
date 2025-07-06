using DemoASPApp.model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UserManagement.User;

namespace DemoASPApp.Pages
{
    public class UserAddressModel : PageModel
    {
        [BindProperty]
        public List<Address> Addresses { get; set; }

        public void OnGet()
        {
            string id = Request.Query["id"];
            var user = Common.users.FirstOrDefault(u => u.userID.ToString() == id);

            if (user != null && user.addresses != null)
            {
                Addresses = user.addresses;
            }
            else
            {
                Addresses = new List<Address>();
            }
        }
    }
}

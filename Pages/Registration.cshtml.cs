using DemoASPApp.model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UserManagement.User;

namespace DemoASPApp.Pages
{
    public class InfoModel : PageModel
    {

        public void OnGet()
        {


        }

        public ActionResult OnPost(UserInfo user)
        {

            user.userID = Common.users.Count > 0
        ? Common.users.Max(u => u.userID ?? 0) + 1
        : 1;
            if (user != null)
            {
                user.phone = GetPhoneFromCollection();
               
            }
            Common.users.Add(user);
            return RedirectToPage("Index");
        }



        private List<Phone> GetPhoneFromCollection()
        {

            List<Phone> phones = new List<Phone>();
            var form = Request.Form;

            

            for (int i = 1; i <= 2; i++)
            {
                long phone = long.TryParse(form[("phoneNumber_"+i.ToString())], out var parsedPhone) ? parsedPhone : 0;
               
                phones.Add(new Phone() { phoneNumber = phone, phoneType = ("Phone" + i.ToString()).ToString() });
                
            }
            return phones;

        }

    }
}
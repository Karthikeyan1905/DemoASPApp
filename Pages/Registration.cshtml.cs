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
                user.addresses = GetAddressesFromCollection();
            }
            Common.users.Add(user);
            return RedirectToPage("Index", new { id = user.userID });
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
        private List<Address> GetAddressesFromCollection()
        {
            var form = Request.Form;
            List<Address> addresses = new List<Address>();

            addresses.Add(new Address
            {
                addressType = "Permanent",
                permDoorNo = form["permDoorNo"],
                street = form["permStreet"],
                town = form["permCity"],
                district = form["permDistrict"],
                state = form["permState"],
                country = form["permCountry"],
                pincode = long.TryParse(form["permPincode"], out var permPin) ? permPin : 0
            });

            
            addresses.Add(new Address
            {
                addressType = "Current",
                permDoorNo = form["currDoorNo"],
                street = form["currStreet"],
                town = form["currCity"],
                district = form["currDistrict"],
                state = form["currState"],
                country = form["currCountry"],
                pincode = long.TryParse(form["currPincode"], out var currPin) ? currPin : 0
            });

            return addresses;
        }
    }

}

using DemoASPApp.model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
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
                user.experiance = GetExperienceFromCollection();
                user.educations = GetEducationFromCollection();
            }

            
            Common.users.Add(user);
            Common.SaveToFile();

            

            return RedirectToPage("Index", new { id = user.userID });
        }

        


        private List<Education> GetEducationFromCollection()
        {
            var form = Request.Form;
            List<Education> educations = new List<Education>();

            
            educations.Add(new Education
            {
                instituteName = form["tenthCollege"],
                board = form["tenthBoard"],
                percentage = float.TryParse(form["tenthPercentage"], out var tenthPct) ? tenthPct : 0,
                courseName = "10th Standard",
                startYear = 0
            });

            
            educations.Add(new Education
            {
                instituteName = form["twelfthCollege"],
                board = form["twelfthBoard"],
                percentage = float.TryParse(form["twelfthPercentage"], out var twelfthPct) ? twelfthPct : 0,
                courseName = "12th Standard",
                startYear = 0
            });

           
            educations.Add(new Education
            {
                instituteName = form["bachelorCollege"],
                instituteAddress = form["bachelorUniversity"],
                courseName = form["bachelorCourse"],
                percentage = float.TryParse(form["bachelorPercentage"], out var bachelorPct) ? bachelorPct : 0,
                startYear = 0
            });

            
            educations.Add(new Education
            {
                instituteName = form["masterCollege"],
                instituteAddress = form["masterUniversity"],
                courseName = form["masterCourse"],
                percentage = float.TryParse(form["masterPercentage"], out var masterPct) ? masterPct : 0,
                startYear = 0
            });

            return educations;
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
        private List<Company> GetExperienceFromCollection()
        {
            var form = Request.Form;
            List<Company> companies = new List<Company>();

            for (int i = 1; i <= 2; i++)
            {
                companies.Add(new Company
                {
                    CompanyName = form[$"company{i}Name"],
                    CompanyAddress = form[$"company{i}Address"],
                    WorkingExperienceInMonths = int.TryParse(form[$"company{i}Duration"], out var months) ? months : 0,
                    ContactPersonName = form[$"company{i}ContactName"],
                    ContactPersonDesignation = form[$"company{i}Designation"],
                    ContactPersonPhone = form[$"company{i}Phone"],
                    ContactPersonEmail = form[$"company{i}Email"]
                });
            }

            return companies;
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
//string jsonString = JsonSerializer.Serialize(Common.users, new JsonSerializerOptions
//{
//    WriteIndented = true
//});

//string filePath = Path.Combine(Environment.CurrentDirectory, "Users.json");
//System.IO.File.WriteAllText("Users.json", jsonString);
//string json = System.IO.File.ReadAllText(filePath);
//var users = JsonSerializer.Deserialize<List<UserInfo>>(json);
//Common.users = users ?? new List<UserInfo>();
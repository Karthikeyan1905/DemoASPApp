using DemoASPApp.model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UserManagement.Model.User;

namespace DemoASPApp.Pages
{
    public class UserExperienceModel : BaseModel
    {
        [BindProperty]
        public UserInfo user { get; set; }

        public void OnGet()
        {
            string id = Request.Query["id"];
            user = Common.users.FirstOrDefault(u => u.userID.ToString() == id);
        }
        public bool HasValidExperience(UserInfo user)
        {
            return user?.experiance != null &&
                   user.experiance.Any(c =>
                       !string.IsNullOrWhiteSpace(c.CompanyName) ||
                       !string.IsNullOrWhiteSpace(c.CompanyAddress) ||
                       c.WorkingExperienceInMonths > 0 ||
                       !string.IsNullOrWhiteSpace(c.ContactPersonDesignation));
        }

    }
}

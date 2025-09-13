
using DemoASPApp.model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UserManagement.Model.User;

namespace DemoASPApp.Pages
{
    public class UserDataModel : BaseModel
    {
        [BindProperty]
        public UserInfo user { get; set; }
        public void OnGet()
        {
            string id = Request.Query["id"];

            if (!string.IsNullOrEmpty(id))
            {
                user = Common.users.FirstOrDefault(u => u.userID.ToString().Equals(id));
            }
            else if (CurrentUser != null)
            {
                user = Common.users.FirstOrDefault(u => u.userID == CurrentUser.userID);
            }
        }
    }
}
//foreach(UserInfo currentuser  in Common.users)
//{
//    if(currentuser.userID.ToString() == id)
//    {
//        user = currentuser;
//        break;

//    }
//}

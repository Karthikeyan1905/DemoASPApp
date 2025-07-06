using DemoASPApp.model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UserManagement.User;

namespace DemoASPApp.Pages
{
    public class UserDataModel : PageModel
    {
        [BindProperty]
        public UserInfo user { get; set; }
        public void OnGet()
        {
            string id = Request.Query["id"];
            

             user = Common.users.FirstOrDefault(u => u.userID.ToString().Equals(id));
            
            
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

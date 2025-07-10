using DemoASPApp.model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using UserManagement.Department;
using UserManagement.User;

namespace DemoASPApp.Pages
{
    public class DepartmentModel : PageModel
    {

        public Employee Employee { get; set; }
        [BindProperty]
        public UserInfo user { get; set; }
        //public UserInfo user { get { if (Employee != null) { return Employee.user; } return null; } }


        public void OnGet(int id)
        {
            if (Common.employees != null)
                Employee = Common.employees.FirstOrDefault(e => e.UserID == id);

            if (Common.users != null)
                user = Common.users.FirstOrDefault(u => u.userID == id);
        }
    }
}

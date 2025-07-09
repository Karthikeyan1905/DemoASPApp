using DemoASPApp.model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using UserManagement.Department;
using UserManagement.User;

namespace DemoASPApp.Pages
{
    public class DesignationModel : PageModel
    {
        
        public Employee Employee { get; set; }

        public UserInfo user => Employee?.user;
        public void OnGet(int id)
        {
            Employee = Common.employees.FirstOrDefault(e => e.UserID == id);
        }
    }
}

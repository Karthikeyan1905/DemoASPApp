using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UserManagement.User;
using UserManagement.Department;
using DemoASPApp.model;

namespace DemoASPApp.Pages
{
    public class UsertoEmployeeModel : PageModel
    {
        [BindProperty] public UserInfo user { get; set; }

        public void OnGet(int id)
        {
            user = Common.users.FirstOrDefault(u => u.userID == id);
        }

        public IActionResult OnPost(int userID, string department, string designation)
        {
            var user = Common.users.FirstOrDefault(u => u.userID == userID);
            if (user == null)
            {
                return Page();
            }

            if (Common.employees == null)
                Common.employees = new List<Employee>();

            var dept = new Department { Name = department, ShortName = department };
            var desig = new Designation { Name = designation, ShortName = designation };

            var employee = new Employee
            {
                EmployeeID = Common.employees.Count + 1,
                UserID = user.userID ?? 0,
                user = user,
                Department = dept,
                Designation = desig
            };

            Common.employees.Add(employee);
            Common.SaveEmployeesToFile();
            Common.SaveToFile();
            return RedirectToPage("Index", new { id = employee.EmployeeID });
        }

    }
}

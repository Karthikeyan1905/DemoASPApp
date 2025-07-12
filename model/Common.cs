using System.Text.Json;
using UserManagement.Department;
using UserManagement.History;
using UserManagement.User;

namespace DemoASPApp.model
{
    public class Common
    {
        public static List<UserInfo> users = new List<UserInfo>();
        public static List<Employee> employees { get; set; } = new List<Employee>();
        public static List<LoginHistory> loginRecords = new List<LoginHistory>();
        public static string loginHistoryFile = "loginHistory.json";

        public static void SaveLoginHistory(LoginHistory history)
        {
            loginRecords.Add(history);

            var json = JsonSerializer.Serialize(loginRecords, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(loginHistoryFile, json);
        }

        public static void LoadLoginHistory()
        {
            if (File.Exists(loginHistoryFile))
            {
                string json = File.ReadAllText(loginHistoryFile);
                loginRecords = JsonSerializer.Deserialize<List<LoginHistory>>(json) ?? new List<LoginHistory>();
            }
        }
        public static List<UserInfo> RegistedUsers
        {

            get { if (users != null) if (users.Count > 0) return users; return null; }
        }

        public static bool hasRegistedUsers
        {

            get { if (RegistedUsers == null || RegistedUsers.Count == 0) return false; return true; }
        }

        public static string RegistedUsersAsJson
        {
            get
            {
                if (hasRegistedUsers)
                    return JsonSerializer.Serialize(RegistedUsers, new JsonSerializerOptions
                    {
                        WriteIndented = true
                    });
                return "";
            }

        }
        public static List<UserInfo> GetJsonToRegistedUsers(string jsonInput)
        {
            if (!string.IsNullOrEmpty(jsonInput))
                return JsonSerializer.Deserialize<List<UserInfo>>(jsonInput);
            return null;

        }
        public static string filePath
        {
            get
            {

                return Path.Combine(Environment.CurrentDirectory, "Users.json");
            }
        }
        public static bool SaveToFile()
        {
            if (hasRegistedUsers)
            {

                System.IO.File.WriteAllText(filePath, RegistedUsersAsJson);
                return true;
            }
            return false;

        }

        public static string FetchFromFile()
        {

            return System.IO.File.ReadAllText(filePath);


        }
        public static bool LoadRegisterUsers()
        {
            users = GetJsonToRegistedUsers(FetchFromFile());
            return true;

        }

        public static List<Employee> RegisteredEmployees
        {
            get
            {
                if (employees != null && employees.Count > 0)
                    return employees;
                return null;
            }
        }


        public static bool hasRegisteredEmployees
        {
            get
            {
                if (RegisteredEmployees == null || RegisteredEmployees.Count == 0)
                    return false;
                return true;
            }
        }


        public static string RegisteredEmployeesAsJson
        {
            get
            {
                if (hasRegisteredEmployees)
                    return JsonSerializer.Serialize(RegisteredEmployees, new JsonSerializerOptions
                    {
                        WriteIndented = true
                    });
                return "";
            }
        }


        public static List<Employee> GetJsonToRegisteredEmployees(string jsonInput)
        {
            if (!string.IsNullOrEmpty(jsonInput))
                return JsonSerializer.Deserialize<List<Employee>>(jsonInput);
            return null;
        }


        public static string employeeFilePath
        {
            get
            {
                return Path.Combine(Environment.CurrentDirectory, "Employees.json");
            }
        }


        public static bool SaveEmployeesToFile()
        {
            if (hasRegisteredEmployees)
            {
                System.IO.File.WriteAllText(employeeFilePath, RegisteredEmployeesAsJson);
                return true;
            }
            return false;
        }


        public static bool LoadRegisteredEmployees()
        {
            string json = FetchEmployeesFromFile();
            employees = string.IsNullOrEmpty(json)
                ? new List<Employee>()
                : GetJsonToRegisteredEmployees(json);
            return true;
        }

        public static string FetchEmployeesFromFile()
        {
            if (File.Exists(employeeFilePath))
            {
                return System.IO.File.ReadAllText(employeeFilePath);
            }

            return "";
        }
    }
}

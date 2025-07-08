using System.Text.Json;
using UserManagement.User;

namespace DemoASPApp.model
{
    public class Common
    {
        public static List<UserInfo> users = new List<UserInfo>();

        public static List<UserInfo> RegistedUsers
        {

            get { if (users != null) if (users.Count > 0) return users; return null; }
        }

        public static bool hasRegistedUsers
        {

            get { if (RegistedUsers == null || RegistedUsers.Count == 0) return false; return true; }
        }

        public static  string RegistedUsersAsJson
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
    }
}

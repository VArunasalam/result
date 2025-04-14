using Assessment.Controllers;

namespace Assessment
{
    public class UserService
    {
       
        public static Dictionary<string, string> logintoken = new Dictionary<string, string>();
        public static Dictionary<string, string> loginrefreshtoken = new Dictionary<string, string>();
        public static string  logouttoken=string.Empty;
    }

    
}

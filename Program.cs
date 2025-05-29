namespace SettingUpSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Random r = new Random();

            string username = $"Te$t{r.Next(666)}";
            string password = username;

            string startProgramPath = "\"C:\\Program Files\\BAF\\common\\1cestart.exe\" enterprise /F\"D:\\B\"";
            string starInPath = "C:\\Program Files\\BAF\\common\\";

            UserCreation.CreateUser_Kasa(username, password, startProgramPath, starInPath);

            //UserCreation.CreateUser(username, password);


            Console.ReadLine();
        }

    }
}


using App.DataBase;
using App.Service;

namespace App
{
    public class App
    {
       public static async Task Main(string[] args)
        {
            EmployeeDB.Make();
            var start = new StartConsoleMenu();
            await start.ViewMenu();
        }
    }
}


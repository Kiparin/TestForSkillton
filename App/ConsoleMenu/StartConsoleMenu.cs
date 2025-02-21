using App.ConsoleMenu;
using App.Interface;

namespace App.Service
{
    internal class StartConsoleMenu : IConsoleMenu
    {
        public async Task ViewMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine(new string('=', 30));
                Console.WriteLine("----- Меню -----");
                Console.WriteLine("Добавить пользователя (1)");
                Console.WriteLine("Редактировать пользователя (2)");
                Console.WriteLine("Показать всех пользователей (3)");
                Console.WriteLine("Удалить пользователя (4)");
                Console.WriteLine("Выйти из приложения (5)");
                Console.WriteLine(new string('=', 30));

                string choice = Console.ReadLine();
                await MenuSelection(choice);
            }
        }

        private async Task MenuSelection(string choice)
        {
            switch (choice)
            {
                case "1":
                    //Меню Создания нового пользователя
                    await new AddNewEmployeeMenu().ViewMenu();
                    Console.ReadKey();
                    break;

                case "2":
                    //Меню редактирования пользователя
                    await new EditEmployeeMenu().ViewMenu();
                    break;

                case "3":
                    //Меню показа всех пользователей
                    await new ReadAllEmployeeMenu().ViewMenu();
                    Console.ReadKey();
                    break;

                case "4":
                    //Меню удаления пользователя
                    await new DeleteEmployeeMenu().ViewMenu();
                    break;

                case "5":
                    Console.WriteLine("Выход из программы.");
                    Environment.Exit(0);
                    break;

                default:
                    Console.WriteLine("Неверный ввод, попробуйте снова. Для повтора надмите любую кнопку");
                    Console.ReadLine();
                    break;
            }
        }
    }
}
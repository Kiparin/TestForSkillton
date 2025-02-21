using App.DataBase;
using App.Interface;

namespace App.ConsoleMenu
{
    internal class DeleteEmployeeMenu : IConsoleMenu
    {
        public async Task ViewMenu()
        {
            await new ReadAllEmployeeMenu().ViewMenu();
            Console.WriteLine(new string('=', 30));
            Console.WriteLine("----- Удаление пользователя -----");
            Console.WriteLine(new string('=', 30));
            try
            {
                while (true)
                {
                    Console.Write("Введите ID сотрудника для удаления: ");
                    Console.WriteLine("Для выхода оставьте строку пустой и нажмите Enter");

                    await DeleteEmployee();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
            }
            finally
            {
                Console.WriteLine("Для продолжения нажмите любую кнопку ");
                Console.ReadKey();
            }
        }

        private async Task DeleteEmployee()
        {
            var input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Выход...");
                return;
            }

            if (int.TryParse(input, out int employeeId))
            {
                var employeeToDelete = await EmployeeDB.GetEmployeeByIdAsync(employeeId);
                if (employeeToDelete != null)
                {
                    Console.WriteLine($"Вы уверены, что хотите удалить сотрудника: {employeeToDelete.FirstName} {employeeToDelete.LastName}? (Y/N)");
                    string confirmation = Console.ReadLine();
                    if (confirmation?.ToUpper() == "Y")
                    {
                        await EmployeeDB.DeleteEmployeeAsync(employeeId);
                        Console.WriteLine("Сотрудник успешно удалён.");
                    }
                    else
                    {
                        Console.WriteLine("Удаление отменено.");
                    }
                }
                else
                {
                    Console.WriteLine("Сотрудник не найден.");
                }
            }
            else
            {
                Console.WriteLine("Неверный ввод. Пожалуйста, введите числовое значение.");
            }
        }
    }
}
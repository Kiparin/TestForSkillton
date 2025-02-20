using App.DataBase;
using App.Interface;

using System;
using System.Threading.Tasks;

namespace App.ConsoleMenu
{
    class DeleteEmployeeMenu : IConsoleMenu
    {
        public void Display()
        {
            Console.WriteLine(new string('=', 30));
            Console.WriteLine("----- Удаление пользователя -----");
            Console.WriteLine(new string('=', 30));
        }

        public async Task ViewMenu()
        {
            await new ReadAllEmployeeMenu().ViewMenu();
            Display();
            try
            {
                while (true)
                {
                    Console.Write("Введите ID сотрудника для удаления: ");
                    Console.WriteLine("Для выхода оставьте строку пустой и нажмите Enter");

                    var input = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(input))
                    {
                        Console.WriteLine("Выход...");
                        return;
                    }

                    if (int.TryParse(input, out int employeeId))
                    {
                        // Получаем сотрудника по ID
                        var employeeToDelete = await EmployeeDB.GetEmployeeByIdAsync(employeeId);
                        if (employeeToDelete != null)
                        {
                            // Подтверждаем удаление
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
    }
}
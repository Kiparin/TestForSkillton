using App.DataBase;
using App.Interface;
using App.Model;

namespace App.ConsoleMenu
{
    class EditEmployeeMenu : IConsoleMenu
    {
        public void Display()
        {
            Console.WriteLine(new string('=', 30));
            Console.WriteLine("----- Редактирование пользователя -----");
            Console.WriteLine(new string('=', 30));
        }

        public async Task ViewMenu() // Изменяем метод на асинхронный
        {
            await new ReadAllEmployeeMenu().ViewMenu();
            Display();
            try
            {
                while (true)
                {
                    Console.Write("Введите ID сотрудника для редактирования: ");
                    Console.WriteLine("Для выхода оставьте строку пустой и нажмите Enter");

                    var input = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(input))
                    {
                        Console.WriteLine("Выход...");
                        return;
                    }

                    if (int.TryParse(input, out int employeeId))
                    {
                        var employeeToEdit = await EmployeeDB.GetEmployeeByIdAsync(employeeId);
                        if (employeeToEdit != null)
                        {
                            await UpdateEmployeeInfo(employeeToEdit);
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

        public async Task UpdateEmployeeInfo(Employee employee) // Изменяем метод на асинхронный
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Текущая информация о сотруднике:");
                Console.WriteLine($"ID: {employee.EmployeeID}, Имя: {employee.FirstName}, Фамилия: {employee.LastName}, Email: {employee.Email}, Дата рождения: {employee.DateOfBirth.ToShortDateString()}, Зарплата: {employee.Salary}");

                Console.WriteLine("Что вы хотите изменить?");
                Console.WriteLine("1. Имя");
                Console.WriteLine("2. Фамилия");
                Console.WriteLine("3. Email");
                Console.WriteLine("4. Дата рождения");
                Console.WriteLine("5. Зарплата");
                Console.WriteLine("6. Выход");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("Введите новое имя: ");
                        string newFirstName = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(newFirstName))
                        {
                            employee.FirstName = newFirstName;
                        }
                        break;

                    case "2":
                        Console.Write("Введите новую фамилию: ");
                        string newLastName = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(newLastName))
                        {
                            employee.LastName = newLastName;
                        }
                        break;

                    case "3":
                        Console.Write("Введите новый Email: ");
                        string newEmail = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(newEmail))
                        {
                            employee.Email = newEmail;
                        }
                        break;

                    case "4":
                        Console.Write("Введите новую дату рождения (ГГГГ-ММ-ДД): ");
                        if (DateTime.TryParse(Console.ReadLine(), out DateTime newDateOfBirth))
                        {
                            employee.DateOfBirth = newDateOfBirth;
                        }
                        break;

                    case "5":
                        Console.Write("Введите новую зарплату: ");
                        if (decimal.TryParse(Console.ReadLine(), out decimal newSalary))
                        {
                            employee.Salary = newSalary;
                        }
                        break;

                    case "6":
                        Console.WriteLine("Выход...");
                        return;

                    default:
                        Console.WriteLine("Неверный выбор.");
                        continue; 
                }

                await EmployeeDB.UpdateEmployeeAsync(employee);
                Console.WriteLine("Данные сотрудника успешно обновлены.");
                await Task.Delay(2000);
            }
        }
    }
}
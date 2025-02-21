using App.DataBase;
using App.Interface;
using App.Model;

namespace App.ConsoleMenu
{
    internal class AddNewEmployeeMenu : IConsoleMenu
    {
        public async Task ViewMenu()
        {
            Console.Clear();
            Console.WriteLine(new string('=', 30));
            Console.WriteLine("-----Добавить нового пользователя-----");
            Console.WriteLine(new string('=', 30));

            Employee newEmployee = CreateEmployeeFromConsole();
            try
            {
                Console.WriteLine("Дабавляю нового пользователя");

                await EmployeeDB.AddEmployeeAsync(newEmployee);
                Console.WriteLine(new string('=', 30));
                Console.WriteLine("Пользователь добавлен (Для продолжнение нажмите любую кнопку)");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
                Console.WriteLine("Для выхода нажмите любую кнопку и попробуйте снова");
            }
        }

        private static Employee CreateEmployeeFromConsole()
        {
            Employee employee = new Employee();

            Console.WriteLine("Введите данные сотрудника:");

            Console.Write("Имя: ");
            employee.FirstName = Console.ReadLine();

            Console.Write("Фамилия: ");
            employee.LastName = Console.ReadLine();

            Console.Write("Email: ");
            employee.Email = Console.ReadLine();

            Console.Write("Дата рождения (в формате ГГГГ-ММ-ДД): ");

            DateTime dateOfBirth;
            while (!DateTime.TryParse(Console.ReadLine(), out dateOfBirth))
            {
                Console.WriteLine("Неверный формат. Пожалуйста, введите дату в формате ГГГГ-ММ-ДД:");
            }
            employee.DateOfBirth = dateOfBirth;

            decimal salary;
            Console.Write("Зарплата: ");
            while (!decimal.TryParse(Console.ReadLine(), out salary))
            {
                Console.WriteLine("Неверный формат. Пожалуйста, введите зарплату:");
            }
            employee.Salary = salary;

            return employee;
        }
    }
}
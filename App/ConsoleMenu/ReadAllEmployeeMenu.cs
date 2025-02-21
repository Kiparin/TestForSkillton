using App.DataBase;
using App.Interface;
using App.Model;

namespace App.ConsoleMenu
{
    internal class ReadAllEmployeeMenu : IConsoleMenu
    {
        public async Task ViewMenu()
        {
            Console.Clear();
            Console.WriteLine(new string('=', 30));
            Console.WriteLine("----- Все пользователи -----");
            Console.WriteLine(new string('=', 30));
            try
            {
                Console.WriteLine("Пользователи Системы");
                var result = await EmployeeDB.GetAllEmployeesAsync();
                await ViewEmployees(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
            }
        }

        private async Task ViewEmployees(List<Employee> employees)
        {
            foreach (var employee in employees)
            {
                Console.WriteLine($"Идентификатор: {employee.EmployeeID}, Имя: {employee.FirstName}, Фамилия: {employee.LastName}, Email: {employee.Email}, Дата рождения: {employee.DateOfBirth.ToShortDateString()}, Зарплата: {employee.Salary}");
            }
        }
    }
}
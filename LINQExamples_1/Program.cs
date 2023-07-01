using LINQExamples_1;
using System;

internal class Program
{
    private static void Main(string[] args)
    {
        List<Employee> employeeList = Data.GetEmployees();
        List<Department> departmentList = Data.GetDepartments();

        Console.WriteLine("-------------------------------------------------");
        Console.WriteLine(" Select                                          ");
        Console.WriteLine("-------------------------------------------------");
        var results = employeeList.Select(e => new
        {
            FullName = e.FirstName + " " + e.LastName,
            AnnualSalary = e.AnnualSalary
        }).Where(e => e.AnnualSalary >= 50000);

        foreach (var item in results)
        {
            Console.WriteLine($"{item.FullName,-20} {item.AnnualSalary,10}");
        }
        Console.WriteLine("-------------------------------------------------");
        Console.WriteLine(" sql statement                                   ");
        Console.WriteLine("-------------------------------------------------");
        var results2 = from emp in employeeList
                       where emp.AnnualSalary >= 50000
                       select new
                       {
                           FullName = emp.FirstName + " " + emp.LastName,
                           AnnualSalary = emp.AnnualSalary
                       };

        foreach (var item in results2)
        {
            Console.WriteLine($"{item.FullName,-20} {item.AnnualSalary,10}");
        }

        Console.WriteLine("-------------------------------------------------");
        Console.WriteLine(" Use custom filter                               ");
        Console.WriteLine("-------------------------------------------------");

        var results3 = from emp in employeeList.GetHighSalariedEmployees()
                       select new
                       {
                           FullName = emp.FirstName + " " + emp.LastName,
                           AnnualSalary = emp.AnnualSalary
                       };


        employeeList.Add(new Employee
        {
            Id = 5,
            FirstName = "Saena",
            LastName = "Jeong",
            AnnualSalary = 100000.20m,
            IsManager = true,
            DepartmentId = 2
        });


        foreach (var item in results3)
        {
            Console.WriteLine($"{item.FullName,-20} {item.AnnualSalary,10}");
        }


        Console.WriteLine("-------------------------------------------------");
        Console.WriteLine(" employee Join department                        ");
        Console.WriteLine("-------------------------------------------------");
        var results4 = departmentList.Join(
                        employeeList,
                        department => department.Id,
                        employee => employee.DepartmentId,
                        (department, employee) =>
                            new
                            {
                                FullName = employee.FirstName + " " + employee.LastName,
                                AnnualSalary = employee.AnnualSalary,
                                DepartmentName = department.LongName
                            }
                        );
        foreach (var item in results4)
        {
            Console.WriteLine($"{item.FullName,-20} {item.AnnualSalary,10}\t{item.DepartmentName}");
        }

        Console.WriteLine("-------------------------------------------------");
        Console.WriteLine(" Join Operation Example - Query Syntax           ");
        Console.WriteLine("  (Inner Join)                                   ");
        Console.WriteLine("-------------------------------------------------");
        var results5 = from dept in departmentList
                       join emp in employeeList // join performs an inner join
                       on dept.Id equals emp.DepartmentId
                       select new
                       {
                           FullName = emp.FirstName + " " + emp.LastName,
                           AnnualSalary = emp.AnnualSalary,
                           DepartmentName = dept.LongName
                       };
        foreach (var item in results5)
        {
            Console.WriteLine($"{item.FullName,-20} {item.AnnualSalary,10}\t{item.DepartmentName}");
        }

        Console.WriteLine("-------------------------------------------------");
        Console.WriteLine(" Group Join Operator Example: Method Syntax      ");
        Console.WriteLine("  (Left Outer Join)                              "); 
        Console.WriteLine("-------------------------------------------------");
        var results6 = departmentList.GroupJoin(  // group join performs left outer join
                       employeeList,
                       dept => dept.Id,
                       emp => emp.DepartmentId,
                       (dept, employeesGroup) =>
                           new
                           {
                               Employees = employeesGroup,
                               DepartmentName = dept.LongName
                           }
                        );


        foreach(var item in results6)
        {
            Console.WriteLine($"Department Name: {item.DepartmentName}");
            foreach(var emp in item.Employees)
            {
                Console.WriteLine($"\t{emp.FirstName} {emp.LastName}");
            }
        }

        Console.WriteLine("-------------------------------------------------");
        Console.WriteLine(" Group Join Operator Example: Query Syntax       ");
        Console.WriteLine("  (Left Outer Join)                              ");
        Console.WriteLine("-------------------------------------------------");

        var result6 = from dept in departmentList
                      join emp in employeeList
                      on dept.Id equals emp.DepartmentId
                      into employeeGroup //into create a group for each employee
                      select new
                      {
                          Employees = employeeGroup,
                          DepartmentName = dept.LongName
                      };

        foreach(var item in results6)
        {
            Console.WriteLine($"Department Name: {item.DepartmentName}");
            foreach(var emp in item.Employees)
            {
                Console.WriteLine($"\t{emp.FirstName} {emp.LastName}");
            }
        }

        //1:16:29
        Console.ReadKey();
    }

}

public static class EnumerableCollectionExtensionMethods
{
    public static IEnumerable<Employee> GetHighSalariedEmployees(this IEnumerable<Employee> employees)
    {
        foreach (Employee emp in employees)
        {
            Console.WriteLine($"Accessing employee: {emp.FirstName + " " + emp.LastName}");
            if (emp.AnnualSalary >= 50000)
            {
                yield return emp;
            }
        }
    }
}
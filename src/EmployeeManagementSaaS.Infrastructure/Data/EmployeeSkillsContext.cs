using EmployeeManagementSaaS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementSaaS.Infrastructure.Data
{
    public static class EmployeeSkillsContext
    {
        public static List<Employee> Employees {  get; set; }

        public static List<Skill> Skills { get; set; }

        static EmployeeSkillsContext()
        {
            var cSharp = new Skill
            {
                Id = Guid.NewGuid(),
                Name = "C#",
                Description = "Ability to write C# applications",
                CreationAt = DateTime.Now
            };
            var wpf = new Skill
            {
                Id = Guid.NewGuid(),
                Name = "WPF",
                Description = "Ability to write WPF applications",
                CreationAt = DateTime.Now
            };
            var vb = new Skill
            {
                Id = Guid.NewGuid(),
                Name = "VB",
                Description = "Ability to write VB applications",
                CreationAt = DateTime.Now
            };
            var winforms = new Skill
            {
                Id = Guid.NewGuid(),
                Name = "WinForms",
                Description = "Ability to write WinForms applications",
                CreationAt = DateTime.Now
            };
            var aspNET = new Skill
            {
                Id = Guid.NewGuid(),
                Name = "ASP.NET",
                Description = "Ability to write ASP.NET applications",
                CreationAt = DateTime.Now
            };
            var angular = new Skill
            {
                Id = Guid.NewGuid(),
                Name = "Angular",
                Description = "Ability to write Angular applications",
                CreationAt = DateTime.Now
            };
            var react = new Skill
            {
                Id = Guid.NewGuid(),
                Name = "REACT",
                Description = "Ability to write REACT applications",
                CreationAt = DateTime.Now
            };
            Skills = new List<Skill>() { cSharp, wpf, vb, winforms, aspNET, angular, react };
           
            Employees = new List<Employee>()
            {
                new Employee
                {
                    Name = "Christina",
                    Surname = "Mavridi",
                    Skills = new List<Skill>(){ wpf, cSharp },
                    CreationAt = DateTime.Now
                },
                new Employee
                {
                    Id = Guid.NewGuid(),
                    Name = "Maria",
                    Surname = "Mavridi",
                    Skills = new List<Skill>(){ vb, winforms },
                    CreationAt = DateTime.Now
                },
                new Employee
                {
                    Id = Guid.NewGuid(),
                    Name = "Sofia",
                    Surname = "Mavridi",
                    Skills = new List<Skill>(){ wpf, cSharp, wpf, cSharp },
                    CreationAt = DateTime.Now
                },
                new Employee
                {
                    Id = Guid.NewGuid(),
                    Name = "Eleni",
                    Surname = "Mavridi",
                    Skills = new List<Skill>(){ react, angular, cSharp },
                    CreationAt = DateTime.Now
                },
                new Employee
                {
                    Id = Guid.NewGuid(),
                    Name = "Giannis",
                    Surname = "Mavridis",
                    Skills = new List<Skill>(){ wpf, angular, cSharp },
                    CreationAt = DateTime.Now
                },
                new Employee
                {
                    Id = Guid.NewGuid(),
                    Name = "Kostas",
                    Surname = "Mavridis",
                    Skills = new List<Skill>(){ wpf, aspNET, cSharp },
                    CreationAt = DateTime.Now
                },
                new Employee
                {
                    Id = Guid.NewGuid(),
                    Name = "Nikos",
                    Surname = "Mavridis",
                    Skills = new List<Skill>(){ wpf, react, angular, cSharp, vb, winforms },
                    CreationAt = DateTime.Now
                }
            };
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectumToJira.libraries
{
    class Employee
    {
        private readonly User _user;

        public Employee()
        {
            //_user = user;
        }

        public string Department => _user.Department;
        public string DepartmentShortName => _user.DepartmentShortName;

        public string DepartmentJiraId => _user.DepartmentJiraId;
        public string LoginJira { get; set; }
        public int DepartmentId => _user.DepartmentId;
        public string Email => _user.Email;
        public string FullName => _user.Fio;
        public int Id => _user.Id;
        public string LoginAd { get; set; }
        public string Sector => _user.Sector ?? $"Вне сектора\\ Отдел {_user.DepartmentShortName}";
        public int SectorId => _user.SectorId;

        public bool NeedCreatingTicketInIt { get; set; } = true;
    }

    internal class User
    {
        public string Department { get; internal set; }
        public string DepartmentShortName { get; internal set; }
        public string DepartmentJiraId { get; internal set; }
        public int DepartmentId { get; internal set; }
        public string Email { get; internal set; }
        public string Fio { get; internal set; }
        public int Id { get; internal set; }
        public string Sector { get; internal set; }
        public int SectorId { get; internal set; }
    }
}

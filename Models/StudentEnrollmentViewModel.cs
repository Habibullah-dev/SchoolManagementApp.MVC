using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolManagementApp.MVC.Models
{
    public class StudentEnrollmentViewModel
    {
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool IsEnrolled { get; set; }
    }
}
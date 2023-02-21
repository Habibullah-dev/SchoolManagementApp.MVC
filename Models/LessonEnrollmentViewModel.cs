using System.Collections.Generic;
using SchoolManagementApp.MVC.Data;

namespace SchoolManagementApp.MVC.Models;

public class LessonEnrollmentViewModel
{
    public LessonViewModel? Lesson { get; set; }
    public List<StudentEnrollmentViewModel> Students { get; set; } = new List<StudentEnrollmentViewModel>();
}
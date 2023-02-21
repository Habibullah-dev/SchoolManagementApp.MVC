using System;
using System.Collections.Generic;

namespace SchoolManagementApp.MVC.Data;

public partial class Enrollment
{
    public int Id { get; set; }

    public int? StudentId { get; set; }

    public int? LessonId { get; set; }

    public string? Grade { get; set; }

    public virtual Lesson? Lesson { get; set; }

    public virtual Student? Student { get; set; }
}

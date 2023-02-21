using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SchoolManagementApp.MVC.Models
{

public class LessonMetadata
{
    [Display(Name="Lecturer")]
    public int LecturerId { get; set; }

    [Display(Name="Course")]
    public int CourseId { get; set; }
    [DataType(DataType.Time)]
    public string Time { get; set; }
}

    // [ModelMetadataType(typeof(LessonMetadata))]
    // public partial class Lesson{}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolManagementApp.MVC.Models
{
    public class LessonViewModel
    {
        public int Id { get; set; }
        public string CourseName {get;set;}
        public string Time { get; set; }
        public string LecturerName {get;set;}     
    }
}



using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SchoolManagementApp.MVC.Data;

public class StudentMetadata
{
    [StringLength(50)]
    [Display(Name="First Name")]
    public String FirstName {get;set;} = null!;

    [Display(Name = "Last Name")]
    public String LastName { get; set; } = null!;

    [Required]
    [Display(Name = "Date Of Birth")]
    public DateTime? DateOfBirth{get ; set; }
}

[ModelMetadataType(typeof(StudentMetadata))]
public partial class Student{}
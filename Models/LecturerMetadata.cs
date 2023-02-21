using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SchoolManagementApp.MVC.Models;

public class LecturerMetadata
{
    [Display(Name="First Name")]
    public string FirstName { get; set; } = null!;

    [Display(Name="Last Name")]
    public string LastName { get; set; } = null!;
}

[ModelMetadataType(typeof(LecturerMetadata))]
public partial class Lecturer{}

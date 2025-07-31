using Backend.Models;
using Backend.Models.Users;

namespace Backend.DTOs.Job;

public record EditJobDto(
 string Title,
 
 string Description ,
 string TermsAndConditions ,
 string Responsibilities ,
 int RequiredWorkExperience ,
    
 decimal MinSalary ,
 decimal MaxSalary ,
    
// Post Details
 string Status ,
 string WorkMode ,
 string Type ,
 int ApplicationsLimit

    );
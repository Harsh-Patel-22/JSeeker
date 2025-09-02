using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Backend.DTOs;
using Backend.Models.Mapping;
using Microsoft.Playwright;

namespace Backend.Services;

public class PdfService {
    private static bool _playwrightInstalled = false;
    public async Task<byte[]> GeneratePdfAsync(string resumeJsonString, int? templateNumber) {
        // if (!_playwrightInstalled) {
        //     await Playwright;
        // }
        
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions() {
            Headless = true
        });
        
        var files = System.IO.Directory.EnumerateFiles("./../Backend/PdfHtmlTemplates").ToArray();
        var templateContent = await File.ReadAllTextAsync(files[templateNumber ?? 0]);
        JsonElement resumeContent = JsonSerializer.Deserialize<JsonElement>(resumeJsonString);
        
        JsonElement BasicDetails = resumeContent.GetProperty("BasicDetails");
        JsonElement ContactDetials = resumeContent.GetProperty("ContactDetails");
        JsonElement ProjectDetails = resumeContent.GetProperty("ProjectDetails");
        JsonElement WorkExperienceDetails = resumeContent.GetProperty("WorkExperienceDetails");
        JsonElement EducationDetails =  resumeContent.GetProperty("EducationDetails");
        JsonElement LanguageDetails = resumeContent.GetProperty("LanguageDetails");
        
        
        // string htmlContent = templateContent.Replace("{{FirstName}}", BasicDetials.GetProperty("FirstName").GetString()).Replace("{{LastName}}",  BasicDetials.GetProperty("LastName").GetString()).Replace("{{State}}",  BasicDetials.GetProperty("State").GetString()).Replace("{{Country}}", BasicDetials.GetProperty("Country").GetString()).Replace("{{AboutLine}}", BasicDetials.GetProperty("AboutLine").GetString()).Replace("{{Email}}", ContactDetials.GetProperty("Email").GetString()).Replace("PhoneNumber", ContactDetials.GetProperty("PhoneNumber").GetString()).Replace();

        // Adding the projects in the template
        string projectTemplate = @"
        <div class='mb-3'>
          <h5>{{ProjectName}}</h5>
          <p>{{ProjectDescription}}</p>
          <p><strong>Technologies:</strong> {{Technologies}}</p>
          <p><small>{{StartDate}} – {{LastUpdatedDate}}</small></p>
          <a href='{{GithubRepoLink}}'>GitHub Repo</a>
        </div>";

        var projectsStringBuilder = new StringBuilder();
        foreach (var projectProperty in ProjectDetails.EnumerateObject()) {
          var project =  projectProperty.Value;
          var projectHtml = projectTemplate.Replace("{{ProjectName}}", project.GetProperty("Name").GetString())
            .Replace("{{ProjectDescription}}", project.GetProperty("Description").GetString()).Replace("{{StartDate}}", project.GetProperty("StartDate").GetString()).Replace("{{LastUpdateDate}}",  project.GetProperty("LastUpdatedDate").GetString()).Replace("{{GithubRepoLink}}", project.GetProperty("GithubRepoLink").GetString());
          
          var technologyUsages = project.GetProperty("TechnologiesUsages").EnumerateArray()
            .Select(element => element.GetProperty("Name")).ToArray();
          string technologyUsagesString = string.Join(", ", technologyUsages);
          projectHtml = projectHtml.Replace("{{TechnologyUsages}}", technologyUsagesString);
          
          projectsStringBuilder.AppendLine(projectHtml);
        }
        
        // Adding the Work Experiences in the template
        string workExperienceTemplate = @"
        <div class=""mb-3"">
            <h5>{{Role}} @ {{CompanyName}}</h5>
            <p>{{Description}}</p>
            <p><small>{{StartDate}} – {{EndDate}}</small></p>
        </div>";

        var workExperienceStringBuilder = new StringBuilder();
        foreach (var workExperience in WorkExperienceDetails.EnumerateArray()) {
          var workExperienceHtml = workExperienceTemplate.Replace("{{Role}}", workExperience.GetProperty("Role").GetString())
            .Replace("{{CompanyName}}", workExperience.GetProperty("CompanyName").GetString()).Replace("{{StartDate}}", workExperience.GetProperty("StartDate").GetString()).Replace("{{EndDate}}",  workExperience.GetProperty("EndDate").GetString()).Replace("{{Description}}", workExperience.GetProperty("Description").GetString());
          
          workExperienceStringBuilder.AppendLine(workExperienceHtml);
        }
        
        // Adding the Education Details in the template
        string educationTemplate = @"
        <div class=""mb-3"">
            <h5>{{Study}}</h5>
            <p>{{InstituteName}}, {{State}}, {{Country}}</p>
            <p><small>{{StartDate}} – {{EndDate}}</small></p>
        </div>";

        var educationStringBuilder = new StringBuilder();
        foreach (var education in EducationDetails.EnumerateArray()) {
          var educationHtml = educationTemplate.Replace("{{Study}}", education.GetProperty("Study").GetString())
            .Replace("{{InstituteName}}", education.GetProperty("InstituteName").GetString()).Replace("{{StartDate}}", education.GetProperty("StartDate").GetString()).Replace("{{EndDate}}",  education.GetProperty("EndDate").GetString()).Replace("{{State}}", education.GetProperty("State").GetString()).Replace("{{Country}}", education.GetProperty("Country").GetString());
          
          educationStringBuilder.AppendLine(educationHtml);
        }
        // Adding the Education Details in the template
        string languageTemplate = @"
        <li>{{LanguageName}} – {{LanguageLevel}}</li>";

        var languageStringBuilder = new StringBuilder();
        foreach (var language in LanguageDetails.EnumerateArray()) {
          Enum.TryParse(language.GetProperty("Level").GetInt16().ToString(), out LanguageLevel level);
          var languageHtml = languageTemplate.Replace("{{LanguageName}}", language.GetProperty("Name").GetString())
            .Replace("{{LanguageLevel}}", level.ToString());
          
          languageStringBuilder.AppendLine(languageHtml);
        }
        
        // TODO - Make it nullable, if work experiences are not there then what? Have a check on that and handle that possible case.
        
        Dictionary<string, string?> replacement = new Dictionary<string, string?> {
          {"FirstName", BasicDetails.GetProperty("FirstName").GetString()}, 
          {"LastName", BasicDetails.GetProperty("LastName").GetString()}, 
          {"State", BasicDetails.GetProperty("State").GetString()}, 
          {"Country", BasicDetails.GetProperty("Country").GetString()}, 
          {"AboutLine", BasicDetails.GetProperty("AboutLine").GetString()}, 
          {"Email", ContactDetials.GetProperty("Email").GetString()}, 
          {"PhoneNumber", ContactDetials.GetProperty("PhoneNumber").GetString()}, 
          {"GithubProfileLink", ContactDetials.GetProperty("GithubProfileLink").GetString()}, 
          {"LinkedInProfileLink", ContactDetials.GetProperty("LinkedInProfileLink").GetString()},
          
          {"Projects", projectsStringBuilder.ToString()},
          {"WorkExperiences", workExperienceStringBuilder.ToString()},
          {"Educations", educationStringBuilder.ToString()},
          {"Languages", languageStringBuilder.ToString()},
        };

        Regex re = new Regex(@"\{\{(\w+)\}\}");
        templateContent = re.Replace(templateContent, match => {
          var key = match.Groups[1].Value;
          return (replacement.TryGetValue(key, out var value) ? value : match.Value) ?? string.Empty;
        });
        /*
         {
  "BasicDetails": {
    "FirstName": "test",
    "LastName": "ing",
    "State": "Gujarat",
    "Country": "India",
    "AboutLine": "Passionate Software Engineer"
  },
  "ContactDetails": {
    "Email": "testing@testing.com",
    "GithubProfileLink": "Harsh-Patel-22",
    "LinkedInProfileLink": "https://www.linkedin.com/in/johndoe",
    "PhoneNumber": "1823482934"
  },
  "ProjectDetails": {
    "Tournament_Checker": {
      "Name": "Tournament_Checker",
      "Description": "",
      "TechnologiesUsages": [
        {
          "Name": "C#",
          "Usage": 100
        }
      ],
      "StartDate": "2025-05-03",
      "LastUpdatedDate": "2025-05-13",
      "GithubRepoLink": "https://github.com/Harsh-Patel-22/Tournament_Checker"
    },
    "SciFi-Snakes-And-Ladders": {
      "Name": "SciFi-Snakes-And-Ladders",
      "Description": "SciFi style Snakes \u0026 Ladders with Crazy Mechanics \u0026 extreme difficulty.",
      "TechnologiesUsages": [
        {
          "Name": "ShaderLab",
          "Usage": 58.21125
        },
        {
          "Name": "C#",
          "Usage": 29.787043
        },
        {
          "Name": "HLSL",
          "Usage": 12.001707
        }
      ],
      "StartDate": "2025-02-05",
      "LastUpdatedDate": "2025-02-20",
      "GithubRepoLink": "https://github.com/Harsh-Patel-22/SciFi-Snakes-And-Ladders"
    },
    "Survival-Game-3D": {
      "Name": "Survival-Game-3D",
      "Description": "An extension of my already created 2D survival game.",
      "TechnologiesUsages": [
        {
          "Name": "C#",
          "Usage": 100
        }
      ],
      "StartDate": "2024-09-02",
      "LastUpdatedDate": "2024-09-02",
      "GithubRepoLink": "https://github.com/Harsh-Patel-22/Survival-Game-3D"
    }
  },
  "WorkExperienceDetails": [
    {
      "Role": "Software Engineer",
      "Description": "Developed scalable web applications using ASP.NET Core, React, and SQL Server.",
      "CompanyName": "TechNova Solutions",
      "StartDate": "2019-06-01",
      "EndDate": "2022-12-31"
    },
    {
      "Role": "Senior Software Engineer",
      "Description": "Led a team of 5 developers to design and implement enterprise SaaS solutions.",
      "CompanyName": "NextGen Systems",
      "StartDate": "2023-01-01",
      "EndDate": "2025-08-01"
    },
    {
      "Role": "Software Engineer",
      "Description": "Developed scalable web applications using ASP.NET Core, React, and SQL Server.",
      "CompanyName": "TechNova Solutions",
      "StartDate": "2019-06-01",
      "EndDate": "2022-12-31"
    },
    {
      "Role": "Senior Software Engineer",
      "Description": "Led a team of 5 developers to design and implement enterprise SaaS solutions.",
      "CompanyName": "NextGen Systems",
      "StartDate": "2023-01-01",
      "EndDate": "2025-08-01"
    }
  ],
  "EducationDetails": [
    {
      "Study": "B.Tech in Computer Science",
      "InstituteName": "Indian Institute of Technology Bombay",
      "State": "Maharashtra",
      "Country": "India",
      "StartDate": "2015-07-01",
      "EndDate": "2019-05-15"
    },
    {
      "Study": "B.Tech in Computer Science",
      "InstituteName": "Indian Institute of Technology Bombay",
      "State": "Maharashtra",
      "Country": "India",
      "StartDate": "2015-07-01",
      "EndDate": "2019-05-15"
    }
  ],
  "LanguageDetails": [
    {
      "Name": "English",
      "Level": 0
    },
    {
      "Name": "Hindi",
      "Level": 1
    },
    {
      "Name": "Gujarati",
      "Level": 1
    },
    {
      "Name": "Spanish",
      "Level": 2
    }
  ]
}

         */
        
        var page = await browser.NewPageAsync();
        await page.SetContentAsync(templateContent, new PageSetContentOptions(){WaitUntil = WaitUntilState.NetworkIdle});
        var pdf = await page.PdfAsync(new PagePdfOptions() {
            Format = "A4",
            PrintBackground = true
        });
        return pdf;
    }
}
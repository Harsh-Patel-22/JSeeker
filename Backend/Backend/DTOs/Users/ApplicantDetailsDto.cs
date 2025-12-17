namespace Backend.DTOs.Users;

public class ApplicantDetailsDto {
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public List<string> Skills { get; set; }

    public List<string> ProjectLinks { get; set; }
    public byte[] ResumePDF { get; set; }
}
namespace Backend.DTOs.Users.Hirer;

public class ApplicationFunnelDto {
    public int Applied { get; set; }
    public int Shortlisted { get; set; }
    public int Interviewed { get; set; }
    public int Hired { get; set; }
}
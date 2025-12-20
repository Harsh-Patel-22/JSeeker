using System.Drawing;

namespace Backend.DTOs.Users.Hirer;

public enum Stages {
    Applied,
    Shortlisted,
    Interviewed,
    Hired
};

public class ApplicationFunnelEntryDto {
    public Stages Stage { get; set; }
    public int Value { get; set; }
    public Color Fill { get; set; }
}
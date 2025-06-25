namespace Backend.DTOs;

public class SearchLocationDto {
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public decimal SearchDistance { get; set; }
}
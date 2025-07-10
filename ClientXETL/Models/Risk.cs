namespace ClientXETL.Models;

public class Risk
{
    public int ID { get; init; }
    public required string RiskName { get; init; }
    public Peril Peril { get; init; }
    public int PolicyID { get; init; }
    public string? Street { get; init; }
    public string? ClientID { get; init; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}

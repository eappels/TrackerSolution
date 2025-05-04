namespace Tracker.Models;

public class CustomLocation
{

    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public DateTime Timestamp { get; set; }

    public CustomLocation()
    {        
    }

    public CustomLocation(double latitude, double longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
        Timestamp = DateTime.Now;
    }

    public override string ToString()
    {
        return $"{Latitude} {Longitude} {Timestamp}";
    }
}
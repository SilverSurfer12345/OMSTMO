using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace OrderManagement.Model
{
    public class PostcodeDistanceCalculator
    {
        private const string DefaultPostcode = "NE16 6LX";
        private const decimal BaseDeliveryCharge = 2.50m;
        private const decimal ChargePerMile = 0.50m;
        private const decimal MinimumCharge = 2.50m;
        private const decimal MaximumCharge = 10.00m;

        // Cache for postcode coordinates to avoid repeated API calls
        private static Dictionary<string, Coordinates> postcodeCache = new Dictionary<string, Coordinates>();

        public static decimal CalculateDeliveryCharge(string customerPostcode)
        {
            if (string.IsNullOrWhiteSpace(customerPostcode))
                return BaseDeliveryCharge;

            try
            {
                // Normalize postcodes by removing spaces
                string normalizedCustomerPostcode = customerPostcode.Replace(" ", "").ToUpper();
                string normalizedDefaultPostcode = DefaultPostcode.Replace(" ", "").ToUpper();

                // If postcodes are the same, return base charge
                if (normalizedCustomerPostcode == normalizedDefaultPostcode)
                    return BaseDeliveryCharge;

                // Get coordinates for both postcodes
                Coordinates defaultCoordinates = GetPostcodeCoordinates(DefaultPostcode).Result;
                Coordinates customerCoordinates = GetPostcodeCoordinates(customerPostcode).Result;

                if (defaultCoordinates == null || customerCoordinates == null)
                    return BaseDeliveryCharge;

                // Calculate distance between the two points
                double distance = CalculateDistance(
                    defaultCoordinates.Latitude, 
                    defaultCoordinates.Longitude,
                    customerCoordinates.Latitude, 
                    customerCoordinates.Longitude);

                // Calculate delivery charge based on distance
                decimal charge = BaseDeliveryCharge + (decimal)Math.Ceiling(distance) * ChargePerMile;

                // Ensure charge is within acceptable range
                charge = Math.Max(MinimumCharge, Math.Min(MaximumCharge, charge));

                return Math.Round(charge, 2);
            }
            catch (Exception)
            {
                // If any error occurs, return the base delivery charge
                return BaseDeliveryCharge;
            }
        }

        private static async Task<Coordinates> GetPostcodeCoordinates(string postcode)
        {
            // Check cache first
            string normalizedPostcode = postcode.Replace(" ", "").ToUpper();
            if (postcodeCache.ContainsKey(normalizedPostcode))
                return postcodeCache[normalizedPostcode];

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Set user agent to avoid being blocked
                    client.DefaultRequestHeaders.Add("User-Agent", "OrderManagementSystem/1.0");

                    // Use Nominatim API to get coordinates
                    string apiUrl = $"https://nominatim.openstreetmap.org/search?postalcode={postcode}&country=UK&format=json";
                    
                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    
                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        dynamic[] results = JsonConvert.DeserializeObject<dynamic[]>(json);
                        
                        if (results != null && results.Length > 0)
                        {
                            double lat = Convert.ToDouble(results[0].lat);
                            double lon = Convert.ToDouble(results[0].lon);
                            
                            Coordinates coords = new Coordinates { Latitude = lat, Longitude = lon };
                            
                            // Cache the result
                            postcodeCache[normalizedPostcode] = coords;
                            
                            return coords;
                        }
                    }
                }
            }
            catch (Exception)
            {
                // Silently fail and return null
            }
            
            return null;
        }

        private static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            // Haversine formula to calculate distance between two points on Earth
            const double EarthRadiusKm = 6371.0;
            
            double dLat = ToRadians(lat2 - lat1);
            double dLon = ToRadians(lon2 - lon1);
            
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
                       
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double distanceKm = EarthRadiusKm * c;
            
            // Convert to miles
            return distanceKm * 0.621371;
        }

        private static double ToRadians(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }
    }

    public class Coordinates
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
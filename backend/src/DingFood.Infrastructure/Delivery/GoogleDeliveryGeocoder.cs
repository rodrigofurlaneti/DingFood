using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace DingFood.Infrastructure.Delivery;

public interface IDeliveryGeocoder
{
    Task<decimal> DistanceKmAsync(string origin, string destination, CancellationToken ct);
}

public sealed class GoogleDeliveryGeocoder(HttpClient client, IConfiguration configuration) : IDeliveryGeocoder
{
    public async Task<decimal> DistanceKmAsync(string origin, string destination, CancellationToken ct)
    {
        var key = configuration["Delivery:GoogleMapsApiKey"];
        if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("Configure a chave de geocodificação para calcular a entrega.");
        var a = await Locate(origin, key, ct);
        var b = await Locate(destination, key, ct);
        return Distance(a.Lat, a.Lng, b.Lat, b.Lng);
    }
    public static decimal Distance(double lat1, double lng1, double lat2, double lng2)
    {
        const double rad = Math.PI / 180;
        var h = Math.Pow(Math.Sin((lat2-lat1)*rad/2),2) + Math.Cos(lat1*rad)*Math.Cos(lat2*rad)*Math.Pow(Math.Sin((lng2-lng1)*rad/2),2);
        return (decimal)(6371.0088 * 2 * Math.Asin(Math.Sqrt(Math.Clamp(h, 0, 1))));
    }
    private async Task<(double Lat, double Lng)> Locate(string address, string key, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(address)) throw new ArgumentException("Endereço completo necessário para calcular a entrega.");
        try
        {
            using var response = await client.GetAsync($"https://maps.googleapis.com/maps/api/geocode/json?address={Uri.EscapeDataString(address)}&components=country:BR&key={Uri.EscapeDataString(key)}", ct);
            if (!response.IsSuccessStatusCode) throw new ArgumentException("Geocodificação indisponível. Tente novamente.");
            using var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync(ct));
            var root = json.RootElement;
            if (root.GetProperty("status").GetString() != "OK" || root.GetProperty("results").GetArrayLength() != 1)
                throw new ArgumentException("Endereço não localizado ou ambíguo. Confira rua, número, cidade e CEP.");
            var result = root.GetProperty("results")[0];
            if (result.TryGetProperty("partial_match", out var partial) && partial.GetBoolean()) throw new ArgumentException("Endereço incompleto. Confira os dados da entrega.");
            var geometry = result.GetProperty("geometry");
            if (geometry.GetProperty("location_type").GetString() is not ("ROOFTOP" or "RANGE_INTERPOLATED")) throw new ArgumentException("Endereço sem precisão suficiente para calcular o raio.");
            var location = geometry.GetProperty("location");
            return (location.GetProperty("lat").GetDouble(), location.GetProperty("lng").GetDouble());
        }
        catch (Exception e) when (e is HttpRequestException or JsonException || e is TaskCanceledException && !ct.IsCancellationRequested)
        { throw new ArgumentException("Não foi possível calcular a distância. Tente novamente."); }
    }
}

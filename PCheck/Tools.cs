using System.Text.Json;

namespace PCheck;

public static class Tools
{
    private static readonly string _path = "dnsList.txt";
    public static void SaveToFile(DNS newDns)
    {
        var data = JsonSerializer.Serialize(newDns);
        File.AppendAllText(_path, data);
    }

    public static List<DNS> GetAllDnsFromFile()
    {
        var data = File.ReadAllText(_path);
        return JsonSerializer.Deserialize<List<DNS>>(data) ?? new List<DNS>();
    }
}

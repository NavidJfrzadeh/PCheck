using System.Text.Json;

namespace PCheck;

public static class Tools
{
    private static readonly string _path = "dnsList.txt";
    public static List<DNS> dnsList = new List<DNS>();
    public static int nextId = dnsList.Any() ? dnsList.Max(d => d.Id) + 1 : 1;

    public static void SaveToFile(DNS newDns)
    {
        var data = JsonSerializer.Serialize(newDns);
        File.AppendAllText(_path, data + "\n");
    }

    public static void GetAllDns()
    {
        dnsList.Clear();

        var dataList = File.ReadAllLines(_path);
        foreach (var data in dataList)
        {
            if (string.IsNullOrWhiteSpace(data))
                continue;
            dnsList.Add(JsonSerializer.Deserialize<DNS>(data) ?? throw new Exception("DNS File is not Correct!"));
        }
    }

    public static void CheckFileExist()
    {
        if (!File.Exists(_path))
            File.Create(_path);

        return;
    }
}
using System.Text.Json;

namespace PCheck;

public static class Tools
{
    private static readonly string _dnsPath = "dnsList.txt";
    public static List<DNS> dnsList = new List<DNS>();

    private static readonly string _domainPath = "domains.txt";
    public static List<Domain> Domains = new List<Domain>();

    public static void SaveDns(DNS newDns)
    {
        var json = JsonSerializer.Serialize(newDns);
        File.AppendAllText(_dnsPath, json + "\n");
    }

    public static void SaveDomain(Domain newDomain)
    {
        var json = JsonSerializer.Serialize(newDomain);
        File.AppendAllText(_domainPath, json + "\n");
    }

    public static void GetAllDns()
    {
        dnsList.Clear();
        var dataList = File.ReadAllLines(_dnsPath);
        foreach (var data in dataList)
        {
            if (string.IsNullOrWhiteSpace(data))
                continue;
            dnsList.Add(JsonSerializer.Deserialize<DNS>(data) ?? throw new Exception("DNS file is not correct!"));
        }
    }

    public static void GetAllDomains()
    {
        Domains.Clear();
        var dataList = File.ReadAllLines(_domainPath);
        foreach (var data in dataList)
        {
            if (string.IsNullOrWhiteSpace(data))
                continue;
            Domains.Add(JsonSerializer.Deserialize<Domain>(data) ?? throw new Exception("Domain file is not correct"));
        }
    }

    public static void CheckFileExist()
    {
        if (!File.Exists(_dnsPath))
            File.Create(_dnsPath);

        if (!File.Exists(_domainPath))
            File.Create(_domainPath);

        return;
    }
}
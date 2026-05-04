using PCheck;
using Spectre.Console;

var dnsList = new List<DNS>();
var nextId = dnsList.Max(d => d.Id) + 1;

try
{
    Main();
}
catch (Exception)
{
    AnsiConsole.WriteLine("Something went wrong returning to Menu...");
    Thread.Sleep(5000);
    Main();
}

void Main()
{
    while (true)
    {
        Console.Clear();
        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("[Yellow] Dns Management Panel[/]")
            .AddChoices(new[]
            {
            "1.Add DNS",
            "2.Show All DNS",
            "3.Edit A DNS",
            "4.Ping DNS"
            }));

        switch (choice[0])
        {
            case '1': AddDns(); break;
            case '2': ShowAllDns(); break;
            case '3': EditDns(); break;
            case '4': PingAllDns(); break;
        }
    }
}


void PingAllDns()
{
    throw new NotImplementedException();
}

void EditDns()
{

}

void ShowAllDns()
{
    var refDnsList = Tools.GetAllDnsFromFile();
    dnsList = refDnsList;

    var table = new Table();
    table.Border = TableBorder.Rounded;
    table.AddColumns("[cyan]ID[/]", "[cyan]Title[/]", "[cyan]Address[/]");

    foreach (var dns in dnsList)
        table.AddRow(dns.Id.ToString(), dns.Title, dns.Address);

    AnsiConsole.Write(table);
}

void AddDns()
{
    Console.Clear();
    var newAddress = AnsiConsole.Ask<string>("[bold green]Add New DNS Address:[/]");
    if (dnsList.Any(d => d.Address == newAddress))
    {
        AnsiConsole.MarkupLine("[yellow]Address Already Exists!");
        Console.ReadKey();
        AddDns();
    }
    var newTitle = AnsiConsole.Ask<string>("[bold green]Enter Title:[/]");
    var newDNs = new DNS(nextId, newAddress, newTitle);
    dnsList.Add(newDNs);
    Tools.SaveToFile(newDNs);
    AnsiConsole.MarkupLine("[yellow]DNS Added Successfully[/]");
}
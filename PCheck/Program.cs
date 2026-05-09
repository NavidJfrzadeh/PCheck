using DnsClient;
using PCheck;
using Spectre.Console;
using System.Net;

try
{
    Tools.CheckFileExist();
    Main();
}
catch (Exception e)
{
    var exception = e.Message;
    Console.WriteLine("Something Went Wrong Returing to menu...");
    Console.ReadKey();
    Main();
}

void Main()
{
    Console.Clear();
    Tools.GetAllDns();
    Tools.GetAllDomains();
    var choice = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
        .Title("[green] DNS Management Panel[/]")
        .AddChoices(
        [
            "1.Add DNS",
            "2.Show All DNS",
            "3.Edit DNS",
            "4.Add Domain",
            "0.Exit",
        ]));

    switch (choice[0])
    {
        case '1': AddDns(); break;
        case '2': ShowAllDns(); break;
        case '3': EditDns(); break;
        case '4': AddDomain(); break;
        case '0': Environment.Exit(0); break;
    }
}

void AddDomain()
{
    Console.Clear();
    var domainName = AnsiConsole.Ask<string>("[bold green]Enter new Domain Name:(return menu press 'N')[/]");
    if (domainName.Trim().ToLower() == "n")
        Main();

    if (Tools.Domains.Any(d => d.Name == domainName))
    {
        AnsiConsole.MarkupLine("[yellow]Domain Name is already exists");
        Console.ReadKey();
        AddDomain();
    }

    var newDomain = new Domain(domainName);

    var newIpAddress = AnsiConsole.Ask<string>("[bold green]Enter Ip Address:(Optional press 'N to skip')[/]");
    if (newIpAddress.Trim().ToLower() != "n")
        newDomain.IpAddress = newIpAddress;

    Tools.SaveDomain(newDomain);
    AnsiConsole.MarkupLine("[yellow]Domain added successfully[/]");
    Console.ReadKey();
    Main();
}

void AddDns()
{
    Console.Clear();
    var newAddress = AnsiConsole.Ask<string>("[bold green]Add New DNS Address:(return Menu Press 'N')[/]");
    if (newAddress.Trim().ToLower() == "n")
        Main();

    if (Tools.dnsList.Any(d => d.Address == newAddress))
    {
        AnsiConsole.MarkupLine("[yellow]Address already exists!");
        Console.ReadKey();
        AddDns();
    }
    var newTitle = AnsiConsole.Ask<string>("[bold green]Enter Title:[/]");
    var newDNs = new DNS(newAddress, newTitle);
    Tools.SaveDns(newDNs);
    AnsiConsole.MarkupLine("[yellow]DNS added successfully[/]");
    Console.ReadKey();
    Main();
}

void EditDns()
{

}

void ShowAllDns()
{
    Console.Clear();
    AnsiConsole.MarkupLine("[bold green]Select DNS To Ping[/]");
    if (!Tools.dnsList.Any())
    {
        AnsiConsole.MarkupLine("[yellow]File is empty add DNS first![/]");
        Console.ReadKey();
        Main();
    }
    var tempList = new List<DNS>(Tools.dnsList);
    var selectAllOption = new DNS("", "0");
    tempList.Insert(0, selectAllOption);
    var backOption = new DNS("", "1");
    tempList.Insert(0, backOption);

    var multiSelect = new MultiSelectionPrompt<DNS>()
        .MoreChoicesText("[grey](move ↑/↓)[/]")
        .InstructionsText("[grey](select:[green][[Space]][/] | Confirm:[green][[Enter]][/] | Select All:[green][[Ctrl+A]][/])[/]")
        .AddChoices(tempList);
    var selectedDns = AnsiConsole.Prompt(multiSelect);

    if (selectedDns.Any(d => d.Title == "1"))
        Main();

    if (selectedDns.Any(d => d.Title == "0"))
    {
        LookupDns(Tools.dnsList);
        Main();
    }

    LookupDns(selectedDns);
    Main();
}

void LookupDns(List<DNS> selectedDnsList)
{
    List<Result> results = [];
    AnsiConsole.Status().StartAsync("Requesting...",
        async ctx =>
        {
            ctx.Spinner(Spinner.Known.Dots);
            ctx.SpinnerStyle(Style.Parse("green"));
            foreach (var dns in selectedDnsList)
            {
                var nameServer = new NameServer(IPAddress.Parse(dns.Address));
                var lookUp = new LookupClient(nameServer);
                List<string> tempDomains = [];

                foreach (var domain in Tools.Domains)
                {
                    try
                    {
                        var result = lookUp.Query(domain.Name, QueryType.A);
                        var aRecord = result.Answers.ARecords().ToList();
                        if (aRecord.Count != 0)
                            tempDomains.Add(domain.Name);
                    }
                    catch (Exception)
                    {

                    }
                }

                results.Add(new Result(dns.Address, tempDomains));
            }
            ctx.Status = "[yellow]operation completed successfully[/]";
            await Task.Delay(5000);
        });

    //Show Result in Table
    Console.WriteLine();
    var table = new Table();
    table.Border = TableBorder.Rounded;
    table.Title = new TableTitle("[bold green]Lookup Results:[/]");

    table.AddColumns("[cyan]DNS Address[/]", "[cyan]Domain[/]");
    foreach (var result in results)
    {
        string domainStr = result.Domains.Count != 0 ? string.Join("\n", result.Domains) : "[red]Inaccessable[/]";
        table.AddRow(result.DnsAddress, domainStr + "\n");
    }
    Console.Clear();
    AnsiConsole.Write(table);
    Console.ReadKey();
}

public record Result(string DnsAddress, List<string> Domains);
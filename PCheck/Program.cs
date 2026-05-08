using PCheck;
using Spectre.Console;

try
{
    Tools.CheckFileExist();
    Main();
}
catch (Exception)
{
    AnsiConsole.MarkupLine("[bold red]Something went wrong press any key to return menu...");
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
        ]));

    switch (choice[0])
    {
        case '1': AddDns(); break;
        case '2': ShowAllDns(); break;
        case '3': EditDns(); break;
        case '4': AddDomain(); break;
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

    var multiSelect = new MultiSelectionPrompt<DNS>()
        .MoreChoicesText("[grey](move ↑/↓)[/]")
        .InstructionsText("[grey](select:[green][[Space]][/] | Confirm:[green][[Enter]][/] | Select All:[green][[Ctrl+A]][/])[/]")
        .AddChoices(tempList);
    var selectedDns = AnsiConsole.Prompt(multiSelect);

    if (selectedDns.Any(d => d.Title == "0"))
    {
        PingDns(Tools.dnsList);
    }

    PingDns(selectedDns);
}

void PingDns(List<DNS> selectedDnsList)
{
    Main();
}
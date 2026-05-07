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
    while (true)
    {
        Console.Clear();
        Tools.GetAllDns();
        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("[Yellow] Dns Management Panel[/]")
            .AddChoices(new[]
            {
            "1.Add DNS",
            "2.Show All DNS",
            "3.Edit DNS",
            }));

        switch (choice[0])
        {
            case '1': AddDns(); break;
            case '2': ShowAllDns(); break;
            case '3': EditDns(); break;
        }
    }
}

void AddDns()
{
    Console.Clear();
    var newAddress = AnsiConsole.Ask<string>("[bold green]Add New DNS Address:(return Menu Press 'N')[/]");
    if (newAddress.Trim().ToLower() == "n")
        Main();
    if (Tools.dnsList.Any(d => d.Address == newAddress))
    {
        AnsiConsole.MarkupLine("[yellow]Address Already Exists!");
        Console.ReadKey();
        AddDns();
    }
    var newTitle = AnsiConsole.Ask<string>("[bold green]Enter Title:[/]");
    var newDNs = new DNS(Tools.nextId, newAddress, newTitle);
    Tools.dnsList.Add(newDNs);
    Tools.SaveToFile(newDNs);
    AnsiConsole.MarkupLine("[yellow]DNS Added Successfully[/]");
    Console.ReadKey();
}

void EditDns()
{

}

void ShowAllDns()
{
    Console.Clear();
    AnsiConsole.MarkupLine("[bold yellow]Select DNS To Ping[/]");
    if (!Tools.dnsList.Any())
    {
        AnsiConsole.MarkupLine("[bold yellow]DNS file is empty add DNS first![/]");
        Console.ReadKey();
        Main();
    }
    var tempList = new List<DNS>(Tools.dnsList);
    var selectAllItem = new DNS(0, "", "Select All");
    tempList.Insert(0, selectAllItem);

    var multiSelect = new MultiSelectionPrompt<DNS>()
        .MoreChoicesText("[grey](move ↑/↓)[/]")
        .InstructionsText("[grey](select:[green][[Space]][/] | Confirm:[green][[Enter]][/] | Select All:[green][[Ctrl+A]][/])[/]")
        .AddChoices(tempList);

    var selectedDns = AnsiConsole.Prompt(multiSelect);
    if (selectedDns.Any(d => d.Id == 0))
    {
        PingDns(Tools.dnsList);
    }

    PingDns(selectedDns);
}

void PingDns(List<DNS> selectedDnsList)
{
    Main();
}
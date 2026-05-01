using Spectre.Console;
using System.Net.NetworkInformation;

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

}

void AddDns()
{

}
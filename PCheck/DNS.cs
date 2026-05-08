namespace PCheck;

public class DNS(string address, string title)
{
    public string Address { get; set; } = address;
    public string Title { get; set; } = title;

    public override string ToString()
    {
        if (Title == "0")
            return $"[bold green]Select All[/]";

        return $"[green]Title:[/] {Title} | [green]Address =>[/] {Address}";
    }
}

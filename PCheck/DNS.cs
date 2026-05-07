namespace PCheck;

public class DNS(int id, string address, string title)
{
    public int Id { get; set; } = id;
    public string Address { get; set; } = address;
    public string Title { get; set; } = title;

    public override string ToString()
    {
        if (Id == 0)
            return $"[bold green]{Title}[/]";

        return $"[green]Id:[/]{Id} | [green]Title:[/]{Title} | [green]Address =>[/] {Address}";
    }
}

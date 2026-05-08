namespace PCheck;

public class Domain(string name)
{
    public string Name { get; set; } = name;
    public string IpAddress { get; set; } = string.Empty;

    //public override string ToString()
    //{
    //    return $"[green]Title:[/] {Name} | [green]IpAddress =>[/] {IpAddress}";
    //}
}

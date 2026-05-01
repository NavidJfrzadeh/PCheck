namespace PCheck;

public class DNS(string address, string title)
{
    public int Id { get; set; }
    public string Address { get; set; } = address;
    public string Title { get; set; } = title;
}

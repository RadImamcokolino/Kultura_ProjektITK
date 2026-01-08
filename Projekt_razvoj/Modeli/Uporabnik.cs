public class Uporabnik
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string? Lokacija { get; set; }
    public List<string> Interesi { get; set; } = new();
    public string? Fotografija { get; set; }
}
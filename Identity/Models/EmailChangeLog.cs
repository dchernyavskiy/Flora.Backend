namespace Flora.Identity.Models;

public class EmailChangeLog
{
    public Guid Id { get; set; }
    public string? OldEmail { get; set; }
    public string? NewEmail { get; set; }
    public string? Token { get; set; }
    public DateTime DateOfRequest { get; set; }

    public string AppUserId { get; set; }
    public AppUser AppUser { get; set; }
}
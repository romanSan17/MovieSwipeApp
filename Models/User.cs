using SQLite;

namespace MovieSwipeApp.Models;

public class User
{
    [PrimaryKey, AutoIncrement] public int Id { get; set; }
    [Unique] public string Login { get; set; } = "";
    public string Password { get; set; } = "";
    public string Role { get; set; } = "user"; // user | admin
}

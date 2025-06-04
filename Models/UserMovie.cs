using SQLite;

namespace MovieSwipeApp.Models;

public class UserMovie
{
    [PrimaryKey, AutoIncrement] public int Id { get; set; }
    public int UserId { get; set; }
    public int MovieId { get; set; }
    public int Rating { get; set; } // 0-5
}

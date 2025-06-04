using SQLite;

namespace MovieSwipeApp.Models;

public class Movie
{
    [PrimaryKey, AutoIncrement] public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Genre { get; set; } = "";
    public string PosterUrl { get; set; } = "";
    public string Description { get; set; } = "";
}

using SQLite;
using MovieSwipeApp.Models;

namespace MovieSwipeApp.Helpers;

public static class SampleDataSeeder
{
    public static async Task SeedAsync(SQLiteAsyncConnection db)
    {
        if (await db.Table<Movie>().CountAsync() > 0) return;

        var movies = new List<Movie>
        {
            new() { Title="Крик", Genre="horror", PosterUrl="", Description="…" },
            /* … ещё 19 фильмов по образцу … */
        };
        await db.InsertAllAsync(movies);
    }
}

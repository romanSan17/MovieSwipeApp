using SQLite;
using MovieSwipeApp.Models;
using MovieSwipeApp.Helpers;

namespace MovieSwipeApp.Services;

public static class DatabaseService
{
    static SQLiteAsyncConnection? _db;
    const string DbName = "movies.db3";

    public static async Task EnsureInitialized()
    {
        if (_db != null) return;
        var path = Path.Combine(FileSystem.AppDataDirectory, DbName);
        _db = new SQLiteAsyncConnection(path);

        await _db.CreateTableAsync<User>();
        await _db.CreateTableAsync<Movie>();
        await _db.CreateTableAsync<UserMovie>();

        await SampleDataSeeder.SeedAsync(_db);
    }

    // ---------- Auth ----------
    public static async Task<User?> LoginAsync(string login, string pass)
    {
        await EnsureInitialized();
        return await _db!.Table<User>()
                         .FirstOrDefaultAsync(u => u.Login == login && u.Password == pass);
    }

    public static async Task<bool> RegisterAsync(string login, string pass)
    {
        await EnsureInitialized();
        if (await _db!.Table<User>().Where(u => u.Login == login).FirstOrDefaultAsync() != null) return false;
        var role = (login == "admin" && pass == "12345") ? "admin" : "user";
        await _db.InsertAsync(new User { Login = login, Password = pass, Role = role });
        return true;
    }

    // ---------- Movies ----------
    public static async Task<List<Movie>> GetMoviesAsync(string? genre = null)
    {
        await EnsureInitialized();
        return string.IsNullOrEmpty(genre)
            ? await _db!.Table<Movie>().ToListAsync()
            : await _db!.Table<Movie>().Where(m => m.Genre == genre).ToListAsync();
    }

    public static async Task LikeMovieAsync(int userId, int movieId)
    {
        await EnsureInitialized();
        var existing = await _db!.Table<UserMovie>()
            .FirstOrDefaultAsync(um => um.UserId == userId && um.MovieId == movieId);
        if (existing == null)
            await _db.InsertAsync(new UserMovie { UserId = userId, MovieId = movieId, Rating = 0 });
    }

    public static async Task<List<(Movie, UserMovie)>> GetLikedMoviesAsync(int userId)
    {
        await EnsureInitialized();

        var liked = await _db!.Table<UserMovie>()
                              .Where(um => um.UserId == userId)
                              .ToListAsync();

        var movies = await _db.Table<Movie>().ToListAsync();

        var result = liked.Join(
            movies,
            um => um.MovieId,
            m => m.Id,
            (um, m) => (m, um)
        ).ToList();

        return result;
    }


    public static Task UpdateRatingAsync(UserMovie um) => _db!.UpdateAsync(um);

    // ---------- Admin ----------
    public static Task AddMovieAsync(Movie m) => _db!.InsertAsync(m);
    public static Task DeleteMovieAsync(Movie m) => _db!.DeleteAsync(m);
}

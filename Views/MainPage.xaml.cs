using CommunityToolkit.Maui.Views;
using MovieSwipeApp.Models;
using MovieSwipeApp.Services;

namespace MovieSwipeApp.Views;

public partial class MainPage : ContentPage
{
    readonly User _user;
    List<Movie> _movies = new();
    public MainPage(User user)
    {
        _user = user;
        InitializeComponent();
        GenrePicker.ItemsSource = new[] { "Все", "horror", "comedy", "drama", "sci‑fi" };
        GenrePicker.SelectedIndex = 0;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadMovies();
    }

    async Task LoadMovies()
    {
        var genre = GenrePicker.SelectedItem?.ToString();
        _movies = await DatabaseService.GetMoviesAsync(genre == "Все" ? null : genre);
        CardView.ItemsSource = _movies;
        CardView.Swiped += async (s, e) =>
        {
            if (e.Direction == SwipeDirection.Right)
            {
                var movie = (Movie)e.Item;
                await DatabaseService.LikeMovieAsync(_user.Id, movie.Id);
            }
        };
    }

    async void OnGenreChanged(object sender, EventArgs e) => await LoadMovies();
    async void OnProfile(object s, EventArgs e) => await Navigation.PushAsync(new ProfilePage(_user));
}
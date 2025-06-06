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
        MoviesCollectionView.ItemsSource = _movies;
    }

    async void OnGenreChanged(object sender, EventArgs e) => await LoadMovies();

    async void OnProfile(object sender, EventArgs e) => await Navigation.PushAsync(new ProfilePage(_user));

    async void OnSwipeLike(object sender, EventArgs e)
    {
        if (sender is SwipeItem swipeItem && swipeItem.BindingContext is Movie movie)
        {
            await DatabaseService.LikeMovieAsync(_user.Id, movie.Id);
            _movies.Remove(movie);
            MoviesCollectionView.ItemsSource = null; // force UI refresh
            MoviesCollectionView.ItemsSource = _movies;
        }
    }
}

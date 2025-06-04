using Microsoft.Extensions.Logging.Abstractions;
using MovieSwipeApp.Services;

namespace MovieSwipeApp.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage() => InitializeComponent();

    async void OnLoginClicked(object sender, EventArgs e)
    {
        var user = await DatabaseService.LoginAsync(LoginEntry.Text!, PassEntry.Text!);
        if (user == null)
            await DisplayAlert("Ошибка", "Неверные данные", "OK");
        else if (user.Role == "admin")
            await Navigation.PushAsync(new AdminPage(user));
        else
            await Navigation.PushAsync(new MainPage(user));
    }
}
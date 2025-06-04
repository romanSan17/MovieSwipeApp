namespace MovieSwipeApp.Views;

public partial class WelcomePage : ContentPage
{
    public WelcomePage() => InitializeComponent();
    void OnLogin(object s, EventArgs e) => Navigation.PushAsync(new LoginPage());
    void OnRegister(object s, EventArgs e) => Navigation.PushAsync(new RegisterPage());
}

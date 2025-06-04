using Microsoft.Extensions.Logging.Abstractions;
using MovieSwipeApp.Services;

namespace MovieSwipeApp.Views;

public partial class RegisterPage : ContentPage
{
    public RegisterPage() => InitializeComponent();

    async void OnRegisterClicked(object sender, EventArgs e)
    {
        // простая валидация
        if (string.IsNullOrWhiteSpace(LoginEntry.Text) ||
            string.IsNullOrWhiteSpace(PassEntry.Text))
        {
            await DisplayAlert("Ошибка", "Введите логин и пароль", "OK");
            return;
        }

        if (PassEntry.Text != ConfirmEntry.Text)
        {
            await DisplayAlert("Ошибка", "Пароли не совпадают", "OK");
            return;
        }

        // пробуем зарегистрировать
        bool success = await DatabaseService.RegisterAsync(LoginEntry.Text!, PassEntry.Text!);
        if (!success)
        {
            await DisplayAlert("Ошибка", "Такой логин уже существует", "OK");
            return;
        }

        // логинимся сразу после успешной регистрации
        var user = await DatabaseService.LoginAsync(LoginEntry.Text!, PassEntry.Text!);

        if (user is null)             // маловероятно, но проверим
        {
            await DisplayAlert("Ошибка", "Не удалось войти после регистрации", "OK");
            return;
        }

        // переходим на нужную страницу
        Page next = user.Role == "admin"
                    ? new AdminPage(user)
                    : new MainPage(user);

        // очищаем стек (чтобы по Back не возвращаться к логину)
        Application.Current!.MainPage = new NavigationPage(next);
    }

    void OnBack(object sender, EventArgs e) => Navigation.PopAsync();
}

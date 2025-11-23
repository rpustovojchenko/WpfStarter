using System.Windows;

namespace WpfStarter.utils;

public static class MessageBoxHelper
{
    public static void ShowInfo(string message, string caption = "Информация") =>
        MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Information);

    public static void ShowError(string message, string caption = "Ошибка") =>
        MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Error);
}

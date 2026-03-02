using System.Windows;

namespace WpfStarter.Utils;

public static class MessageBoxHelper
{
    public static void ShowInfo(string message, string caption = "Информация") =>
        MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Information);

    public static void ShowWarning(string message, string caption = "Предупреждение") =>
        MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Warning);

    public static void ShowError(string message, string caption = "Ошибка") =>
        MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Error);
}

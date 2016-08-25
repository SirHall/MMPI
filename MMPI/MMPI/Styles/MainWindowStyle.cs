using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MMPI
{
  /// <summary>Класс для работы с нестандартным окном</summary>
  public  partial class MainWindowStyle
  {
    #region Методы
    /// <summary>Обработчик события загрузки окна</summary>
    /// <param name="sender">Источник события.</param>
    /// <param name="e"><see cref="RoutedEventArgs"/> Содержит информацию о событии.</param>
    void WindowLoaded(object sender, RoutedEventArgs e)
    {
      ((Window)sender).StateChanged += WindowStateChanged;
    }

    /// <summary>Обработчик события нажатия левой кнопки мыши</summary>
    /// <param name="sender">Источник события.</param>
    /// <param name="e"><see cref="RoutedEventArgs"/> Содержит информацию о событии.</param>
    void IconMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      if (e.ClickCount > 1)
        sender.ForWindowFromTemplate(SystemCommands.CloseWindow);
    }

    /// <summary>Обработчик события щелчка по иконке</summary>
    /// <param name="sender">Источник события.</param>
    /// <param name="e"><see cref="RoutedEventArgs"/> Содержит информацию о событии.</param>
    void IconMouseUp(object sender, MouseButtonEventArgs e)
    {
      var element = sender as FrameworkElement;
      if (element == null)
        return;
      var point = element.PointToScreen(new Point(element.ActualWidth / 2, element.ActualHeight));
      sender.ForWindowFromTemplate(w => SystemCommands.ShowSystemMenu(w, point));
    }

    /// <summary>Обработчик события изменения состояния окна</summary>
    /// <param name="sender">Источник события.</param>
    /// <param name="e"><see cref="RoutedEventArgs"/> Содержит информацию о событии.</param>
    static void WindowStateChanged(object sender, EventArgs e)
    {
      var w = ((Window)sender);
      var handle = w.GetWindowHandle();
      var containerBorder = (Border)w.Template.FindName("PART_Container", w);

      if (w.WindowState == WindowState.Maximized)
      {
        // Make sure window doesn't overlap with the taskbar.
        var screen = System.Windows.Forms.Screen.FromHandle(handle);
        if (screen.Primary)
        {
          containerBorder.Padding = new Thickness(
              SystemParameters.WorkArea.Left + 7,
              SystemParameters.WorkArea.Top + 7,
              (SystemParameters.PrimaryScreenWidth - SystemParameters.WorkArea.Right) + 7,
              (SystemParameters.PrimaryScreenHeight - SystemParameters.WorkArea.Bottom) + 5);
        }
      }
      else
      {
        containerBorder.Padding = new Thickness(7, 7, 7, 5);
      }
    }

    /// <summary>Обработчик события закрытия окна</summary>
    /// <param name="sender">Источник события.</param>
    /// <param name="e"><see cref="RoutedEventArgs"/> Содержит информацию о событии.</param>
    void CloseButtonClick(object sender, RoutedEventArgs e)
    {
      sender.ForWindowFromTemplate(SystemCommands.CloseWindow);
    }

    /// <summary>Обработчик события минимизации окна</summary>
    /// <param name="sender">Источник события.</param>
    /// <param name="e"><see cref="RoutedEventArgs"/> Содержит информацию о событии.</param>
    void MinButtonClick(object sender, RoutedEventArgs e)
    {
      sender.ForWindowFromTemplate(SystemCommands.MinimizeWindow);
    }

    /// <summary>Обработчик события восстановления окна</summary>
    /// <param name="sender">Источник события.</param>
    /// <param name="e"><see cref="RoutedEventArgs"/> Содержит информацию о событии.</param>
    void MaxButtonClick(object sender, RoutedEventArgs e)
    {
      sender.ForWindowFromTemplate(w =>
      {
        if (w.WindowState == WindowState.Maximized) SystemCommands.RestoreWindow(w);
        else SystemCommands.MaximizeWindow(w);
      });
    }
    #endregion
  }
}

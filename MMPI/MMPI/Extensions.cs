using System;
using System.Windows;
using System.Windows.Interop;

namespace MMPI
{
  /// <summary>Класс с расширениями</summary>
  internal static class Extensions
  {
    public static void ForWindowFromTemplate(this object templateFrameworkElement, Action<Window> action)
    {
      var window = ((FrameworkElement)templateFrameworkElement).TemplatedParent as Window;
      if (window != null) action(window);
    }

    public static IntPtr GetWindowHandle(this Window window)
    {
      var  helper = new WindowInteropHelper(window);
      return helper.Handle;
    }
  }
}

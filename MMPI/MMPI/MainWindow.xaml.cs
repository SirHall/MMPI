using System.Windows;

namespace MMPI
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  internal partial class MainWindow :Window
  {
    public MainWindow()
    {
      InitializeComponent();
      var viewModel = new MainWindowViewModel(true);
      DataContext = viewModel;
    }
  }
}

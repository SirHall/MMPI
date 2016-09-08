namespace MMPI
{
  public partial class Chart
  {
    public Chart()
    {
      InitializeComponent();
    }
    private void GraphPanelLoaded(object sender, System.Windows.RoutedEventArgs e)
    {
      var viewModel = DataContext as ChartViewModel;
      if (viewModel == null)
        return;
      viewModel.Width = _GraphPanel.ActualWidth;
      viewModel.Height = _GraphPanel.ActualHeight;
    }

    private void GraphPanelSizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
    {
      var viewModel = DataContext as ChartViewModel;
      if (viewModel == null)
        return;
      viewModel.Width = _GraphPanel.ActualWidth;
      viewModel.Height = _GraphPanel.ActualHeight;
    }
  }
}

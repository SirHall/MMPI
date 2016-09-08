using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MMPI
{
  /// <summary>Класс отображения результатов</summary>
  public class ResultsViewModel:ObservableObject
  {
    #region Поля

    /// <summary>Список шкал с результатами</summary>
    private readonly List<ResultViewModel> _ResultList = new List<ResultViewModel>();

    /// <summary>Видимость результатов</summary>
    private bool _IsVisible;


    #endregion

    #region Свойства


    /// <summary>Возвращает список результатов</summary>
    public List<ResultViewModel> ResultList
    {
      get { return _ResultList; }
    }

    /// <summary>Возвращает или задает видимость элемента</summary>
    public bool IsVisible
    {
      get { return _IsVisible; }
      set
      {
        if( value == _IsVisible )
          return;
        _IsVisible = value;
        Redraw();
        OnPropertyChanged();
      }
    }

    public ChartViewModel Chart { get; private set; }

    #endregion

    #region Методы
    /// <summary>Устанавливает значение шкалы</summary>
    /// <param name="type">Тип шкалы.</param>
    /// <param name="value">Значение.</param>
    public void SetValue(ScaleType type, double value)
    {
      _ResultList.Add(new ResultViewModel(type, value));
    }

    public void Redraw()
    {
      Chart = new ChartViewModel(Globals.CenterPoint,_ResultList.Count-1,GetMaxValue());
      var first = Chart.AddSeries(Brushes.Red);
      var second = Chart.AddSeries(Brushes.RoyalBlue);
      double x = 0;
      foreach( var res in _ResultList )
      {
        if( res.ScaleType == ScaleType.DontKnow )
          continue;
        if (res.ScaleType==ScaleType.Lie||res.ScaleType==ScaleType.True||res.ScaleType==ScaleType.Correction)
          first.AddXy(x,res.Value);
        else
          second.AddXy(x, res.Value);
        x++;
      }
      OnPropertyChanged("Chart");
    }

    public double GetMaxValue()
    {
      return ResultList.Select(result => result.Value).Concat(new[] { 0.0 }).Max();
    }

    #endregion
  }
}

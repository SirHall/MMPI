using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace MMPI
{
  /// <summary>Класс отображения результатов</summary>
  public class Results:ObservableObject
  {
    #region Поля

    /// <summary>Список шкал с результатами</summary>
    private readonly List<Result> _ResultList = new List<Result>();

    private int _Height =300;

    private int _Width = 400;

    private bool _IsVisible;

    #endregion

    #region Свойства
    /// <summary>Возвращает список результатов</summary>
    public List<Result> ResultList
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

    public int Width
    {
      set
      {
        if (value==0|| _Width==value)
          return;
        _Width = value;
        Redraw();
        OnPropertyChanged();
      }
    }

    public int Height 
    {
      set
      {
        if (_Height==value || value==0)
          return;
        _Height = value;
        Redraw();
        OnPropertyChanged();
      }
    }

    public PointCollection Points { get; private set; }

    #endregion

    #region Методы
    /// <summary>Устанавливает значение шкалы</summary>
    /// <param name="type">Тип шкалы.</param>
    /// <param name="value">Значение.</param>
    public void SetValue(ScaleType type, double value)
    {
      _ResultList.Add(new Result(type, value));
    }

    public void Redraw()
    {
      Points = new PointCollection();
      var max = GetMaxValue();
      var scale = _Height / max;
      OnPropertyChanged("Points");
    }

    public double GetMaxValue()
    {
      return ResultList.Select(result => result.Value).Concat(new[] { 0.0 }).Max();
    }

    #endregion
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

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

    /// <summary>Видимость сетки на графике</summary>
    private bool _ShowGrid=true;

    /// <summary>Видимость значений на графике</summary>
    private bool _ShowValues;
    #endregion

    #region Свойства

    /// <summary>Возвращает значение индекса Уэлша</summary>
    public double FkIndex { get; private set; }

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
        SetGraph();
        CaclulateIndex();
        OnPropertyChanged();
      }
    }

    /// <summary>Расчитывает индекс Уэлша</summary>
    private void CaclulateIndex()
    {
      var truthScale = _ResultList.First(item => item.ScaleType == ScaleType.True);
      var correctionScale = _ResultList.First(item => item.ScaleType == ScaleType.Correction);
      FkIndex = Math.Abs(truthScale.Value - correctionScale.Value);
    }

    /// <summary>Модель отображения графика результатов</summary>
    public ChartViewModel Chart { get; private set; }

    /// <summary>Возвращает или задает параметры отображения сетки</summary>
    public bool ShowGrid
    {
      get { return _ShowGrid; }
      set
      {
        if (value==_ShowGrid)
          return;
        _ShowGrid = value;
        Chart.ShowGrid = value;
        OnPropertyChanged("Chart");
      }
    }

    /// <summary>Возвращает или назначает отображение значений на графике</summary>
    public bool ShowValues
    {
      get { return _ShowValues; }
      set
      {
        if (value==_ShowValues)
          return;
        _ShowValues = value;
        Chart.ShowValues = value;
        OnPropertyChanged("Chart");
      }
    }

    #endregion

    #region Методы
    /// <summary>Добавляет шкалу к результатам</summary>
    /// <param name="type">Тип шкалы.</param>
    /// <param name="value">Значение.</param>
    public void SetValue(ScaleType type, double value)
    {
      _ResultList.Add(new ResultViewModel(type, value));
    }

    /// <summary>Задает значения для построения графика</summary>
    public void SetGraph()
    {
      Chart = new ChartViewModel(Globals.CenterPoint,_ResultList.Count-1,GetMaxValue())
                {
                  XAxisLegend =
                    new List<Tuple<string,string>>
                      {
                        new Tuple<string,string>("L","Шкала достоверности"),
                        new Tuple<string,string>("F","Шкала надежности"),
                        new Tuple<string,string>("K","Шкала корректировки"),
                        new Tuple<string,string>("1","Сверхконтроль"),
                        new Tuple<string,string>("2","Пессимистичность"),
                        new Tuple<string,string>("3","Эмоциональная лабильность"),
                        new Tuple<string,string>("4","Импульсивность"),
                        new Tuple<string,string>("5","Мужественность/женственность"),
                        new Tuple<string,string>("6","Ригидность"),
                        new Tuple<string,string>("7","Тревожность"),
                        new Tuple<string,string>("8","Индивидуалистичность"),
                        new Tuple<string,string>("9","Оптимистичность"),
                        new Tuple<string,string>("0","Интроверсия"),
                      },
                      XAxisName = "Шкалы",
                      YAxisName = "T",
                      ShowGrid = true
                };
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
      AnalyzeResults();
    }

    /// <summary>Возвращает максимальное значение T из шкал</summary>
    public double GetMaxValue()
    {
      return ResultList.Select(result => result.Value).Concat(new[] { 0.0 }).Max();
    }

    private void AnalyzeResults()
    {
      var scale = _ResultList.First(item => item.ScaleType == ScaleType.Overcontrol);
      var isPeak = IsScalesInArea(ScaleType.Overcontrol, 45, 54);

    }

    private bool IsScalesInArea(ScaleType excludeScale,double minValue,double maxValue)
    {
      return ResultList.Where(result => result.ScaleType != excludeScale).All(result => !( result.Value < minValue ) && !( result.Value > maxValue ));
    }

    #endregion
  }
}

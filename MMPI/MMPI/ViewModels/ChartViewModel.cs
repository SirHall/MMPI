using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

using Brushes = System.Windows.Media.Brushes;

namespace MMPI
{
  /// <summary>Модель отображения для графика</summary>
  public class ChartViewModel:ObservableObject
  {
    #region Поля
    /// <summary>Отображение сетки графика</summary>
    private bool _ShowGrid;

    /// <summary>Отображать значения на графике</summary>
    private bool _ShowValues;

    /// <summary>Точка отсчета</summary>
    private Point _StartPosition;

    /// <summary>Канва для рисования</summary>
    private Canvas _Parent;

    /// <summary>Максимальное значение по оси X</summary>
    private readonly double _XMax;

    /// <summary>Максимальное значение по оси Y</summary>
    private readonly double _YMax;

    /// <summary>Высота фрейма</summary>
    private double _Height;

    /// <summary>Ширина фрейма</summary>
    private double _Width;

    /// <summary>Цена деления по шкале Y</summary>
    private double _YScale;

    /// <summary>Цена деления по шкале X</summary>
    private double _XScale;

    /// <summary>Точки для отметок по шкале X</summary>
    private Point _YBase;

    /// <summary>Точки для отметок по шкале Y</summary>
    private Point _XBase;

    /// <summary>Входные данные</summary>
    private readonly List<Series> _Data; 
    #endregion

    #region Конструкторы

    /// <summary>Создаёт новый экземпляр класса <see cref="ChartViewModel"/>.</summary>
    /// <param name="startPosition">Начало отсчета.</param>
    /// <param name="xMax">Максимальное значение по оси X.</param>
    /// <param name="yMax">Максимальное значение по оси Y.</param>
    public ChartViewModel(Point startPosition, double xMax = 10, double yMax = 10)
    {
      _StartPosition = startPosition;
      _XMax = xMax;
      _YMax = yMax;
      _XBase = new Point(_StartPosition.X - 5, _StartPosition.X + 5);
      _Data = new List<Series>();
    }
    #endregion

    #region Свойства

    public bool ShowValues
    {
      get { return _ShowValues; }
      set
      {
        if (value==_ShowValues)
          return;
        _ShowValues = value;
        Redraw();
      }
    }

    /// <summary>Возвращает или задает отображение сетки на графике</summary>
    public bool ShowGrid
    {
      get { return _ShowGrid; }
      set
      {
        if (value==ShowGrid)
          return;
        _ShowGrid = value;
        Redraw();
      }
    }

    /// <summary>Возвращает или задает подписи по оси X</summary>
    public List<Tuple<string,string>> XAxisLegend { get; set; }
    
    /// <summary>Возвращает или задает подписи по оси Y</summary>
    public List<string> YAxisLegend { get; set; }

    /// <summary>Возвращает или задает название оси X</summary>
    public string XAxisName { get; set; }
    
    /// <summary>Возвращает или задает название оси Y</summary>
    public string YAxisName { get; set; }

    /// <summary>Осевые линии</summary>
    public List<Line> Axis { get; private set; }

    /// <summary>Графики</summary>
    public List<Polyline> Graphs { get; private set; }

    /// <summary>Задает ширину фрейма</summary>
    public double Width
    {
      set
      {
        if (value.Equals(0) || _Width.Equals(value))
          return;
        _Width = value;
        _XScale = (_Width - 2 * _StartPosition.X) / _XMax;
        _YScale = (_Height - 2 * _StartPosition.Y) / _YMax;
        Redraw();
        OnPropertyChanged();
      }
    }

    /// <summary>Задает высоту фрейма</summary>
    public double Height
    {
      set
      {
        if (_Height.Equals(value) || value.Equals(0))
          return;
        _Height = value;
        _YScale = (_Height - 2 * _StartPosition.Y) / _YMax;
        _XScale = (_Width - 2 * _StartPosition.X) / _XMax;
        _YBase = new Point(_Height-_StartPosition.Y+5,_Height-_StartPosition.Y-5);
        Redraw();
        OnPropertyChanged();
      }
    }

    /// <summary>Родительский объект для рисования графика</summary>
    public Canvas Parent
    {
      get { return _Parent; }
      set
      {
        if( value == null || Equals(value, _Parent) )
          return;
        _Parent = value;
      }
    }

    #endregion

    #region Методы
    /// <summary>Добавляет новую полилинию на график</summary>
    /// <param name="color">Цвет линии.</param>
    public Series AddSeries(SolidColorBrush color)
    {
      _Data.Add(new Series { Color = color });
      return _Data.Last();
    }

    /// <summary>Рекурсивное удаление текстовых элементов</summary>
    private void Clear()
    {
      foreach( TextBlock element in _Parent.Children.OfType<TextBlock>() )
      {
        _Parent.Children.Remove(element);
        Clear();
        break;
      }
    }

    /// <summary>Добавляет текстовый элемент на график</summary>
    /// <param name="position">Левый верхний угол элемента.</param>
    /// <param name="text">Добавляемый текст.</param>
    /// <param name="fontSize">Размер шрифта (по умолчанию 14pt).</param>
    /// <param name="toolTip">Всплывающая подсказка</param>
    private void AddLegendText( Point position, string text, int fontSize = 12,string toolTip="" )
    {
      var legend = new TextBlock { Text = text, FontSize = fontSize, FontWeight = FontWeights.Bold ,ToolTip = toolTip};
      Canvas.SetLeft(legend, position.X);
      Canvas.SetTop(legend, position.Y);
      _Parent.Children.Add(legend);
    }

    /// <summary>Формирует оси</summary>
    private void GenerateAxis()
    {
      Clear();
      AddLegendText(new Point(_Width - 2 * _StartPosition.X, _Height - _StartPosition.Y), XAxisName);
      AddLegendText(new Point(_StartPosition.X - _StartPosition.X / 2, _StartPosition.Y - _StartPosition.Y / 2), YAxisName);
      Axis = new List<Line>
               {
                 new Line
                   {
                     X1 = _StartPosition.X,
                     Y1 =_Height- _StartPosition.Y,
                     X2 = _Width - _StartPosition.X,
                     Y2 = _Height-_StartPosition.Y,
                     Stroke = Brushes.Black,
                   },
                 new Line
                   {
                     X1 = _StartPosition.X,
                     Y1 = _StartPosition.Y,
                     X2 = _StartPosition.X,
                     Y2 = _Height-_StartPosition.Y,
                     Stroke = Brushes.Black,
                   },
                new Line
                   {
                     X1 = _Width - _StartPosition.X,
                     Y1 = _Height- _StartPosition.Y,
                     X2 = _Width - _StartPosition.X-5,
                     Y2 = _Height- _StartPosition.Y-5,
                     Stroke = Brushes.Black,
                   },
                new Line
                   {
                     X1 = _Width - _StartPosition.X,
                     Y1 = _Height- _StartPosition.Y,
                     X2 = _Width - _StartPosition.X-5,
                     Y2 = _Height- _StartPosition.Y+5,
                     Stroke = Brushes.Black,  
                   },
                 new Line
                   {
                     X1 = _StartPosition.X,
                     Y1 = _StartPosition.Y,
                     X2 = _StartPosition.X-5,
                     Y2 = _StartPosition.Y+5,
                     Stroke = Brushes.Black,  
                   },
                  new Line
                   {
                     X1 = _StartPosition.X,
                     Y1 = _StartPosition.Y,
                     X2 = _StartPosition.X+5,
                     Y2 = _StartPosition.Y+5,
                     Stroke = Brushes.Black,  
                   }
               };
      double x = _StartPosition.X;
      for (var i = 0; i < _XMax; i++)
      {
        Axis.Add(ShowGrid
                   ? new Line
                       {
                         X1 = x,
                         Y1 = _YBase.X,
                         X2 = x,
                         Y2 = _StartPosition.Y,
                         Stroke = Brushes.Black,
                         StrokeThickness = 0.3
                       }
                   : new Line
                       {
                         X1 = x,
                         Y1 = _YBase.X,
                         X2 = x,
                         Y2 = _YBase.Y,
                         Stroke = Brushes.Black,
                         StrokeThickness = 0.3
                       });
        AddLegendText(new Point(x - 5, _YBase.X),
                      XAxisLegend == null ? (i + 1).ToString(CultureInfo.InvariantCulture) : XAxisLegend[i].Item1, 14, XAxisLegend == null ? (i + 1).ToString(CultureInfo.InvariantCulture) : XAxisLegend[i].Item2);
        x = x + _XScale;
      }

      double y = _Height - _StartPosition.Y;
      int value = 0;
      while (y >= _StartPosition.Y)
      {
        Axis.Add(ShowGrid
                   ? new Line
                       {
                         X1 = _XBase.X,
                         Y1 = y,
                         X2 = _Width - _StartPosition.X,
                         Y2 = y,
                         Stroke = Brushes.Black,
                         StrokeThickness = 0.3
                       }
                   : new Line
                       {
                         X1 = _XBase.X,
                         Y1 = y,
                         X2 = _XBase.Y,
                         Y2 = y,
                         Stroke = Brushes.Black,
                         StrokeThickness = 0.3
                       });
        AddLegendText(new Point(0, y-10), (value).ToString(CultureInfo.InvariantCulture));
        y = y - 10 * _YScale;
        value = value + 10;
      }
    }

    /// <summary>Перерисовывает график</summary>
    private void Redraw()
    {
      if (_Width.Equals(0) || _Height.Equals(0))
        return;
      GenerateAxis();
      Graphs = new List<Polyline>();
      foreach (var series in _Data)
      {
        Graphs.Add(new Polyline { Stroke = series.Color });
        for (var i = 0; i < series.Count; i++)
        {
          var y = _Height - _StartPosition.Y - series[i].Y * _YScale;
          var x = _StartPosition.X + series[i].X * _XScale;
          Graphs.Last().Points.Add(new Point(x, y));
          if (_ShowValues)
            AddLegendText(new Point(x, y), series[i].Y.ToString("F1"));
        }
      }
      OnPropertyChanged("Graphs");
      OnPropertyChanged("Axis");
    }
    #endregion
  }
}

using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace MMPI
{
  /// <summary>Класс исходных данных для графика</summary>
  public class Series
  {
    #region Поля
    /// <summary>Список реальных значений точек</summary>
    private readonly List<Point> _Points; 
    #endregion

    #region Конструкторы
    /// <summary>Создаёт новый экземпляр класса <see cref="Series"/>.</summary>
    public Series()
    {
      _Points = new List<Point>();
      Color = Brushes.Black;
    }
    #endregion

    #region Свойства

    /// <summary>Возвращает количество точек</summary>
    public int Count
    {
      get { return _Points.Count; }
    }

    /// <summary>Возвращает цвет линии</summary>
    public SolidColorBrush Color { get; set; }
    #endregion

    #region Индексаторы
    public Point this[int index]
    {
      get { return _Points[index]; }
    }
    #endregion

    #region Методы

    /// <summary>Добавляет точку к графику</summary>
    /// <param name="x">Координата X.</param>
    /// <param name="y">Координата Y.</param>
    public void AddXy(double x, double y)
    {
      _Points.Add(new Point(x, y));
    }
    #endregion
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Ink;
using System.Windows.Media;

namespace MMPI
{
  public class Series
  {
    private readonly List<Point> _Points; 
    public Series()
    {
      _Points = new List<Point>();
    }

    public void AddXy( double x, double y )
    {
      _Points.Add(new Point(x,y));
    }

    public SolidColorBrush Color { get; set; }

    public Point this[int index]
    {
      get { return _Points[index]; }
    }

    public int Count
    {
      get { return _Points.Count; }
    }
  }
}

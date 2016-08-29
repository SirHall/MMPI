using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace MMPI
{

  /// <summary>Класс отображения результатов</summary>
  public class Results
  {

    /// <summary>Список шкал с результатами</summary>
    private readonly List<Tuple<ScaleType, double,string,string,string,bool>> _ResultList;
    public Results()
    {
      _ResultList = new List<Tuple<ScaleType, double, string, string,string, bool>>();
    }

    /// <summary>Устанавливает значение шкалы</summary>
    /// <param name="type">Тип шкалы.</param>
    /// <param name="value">Значение.</param>
    public void SetValue( ScaleType type, double value )
    {
      var stringValue = Math.Round(value, 2).ToString(CultureInfo.InvariantCulture) + " T";
      var name = Globals.ScaleNames.First(item => item.Item1 == type).Item2;
      var message = "";
      var isWarning = false;
      //Шкала правдивости
      switch( type )
      {
        case ScaleType.Lie:
          if( value > Globals.MAX_LIE_COUNT )
          {
            message = string.Format(Globals.DataTooMuchLie, value);
            isWarning = true;
          }
          else if( value > Globals.MiddleLieCount.Item1 && value < Globals.MiddleLieCount.Item2 )
            message = Globals.DataMiddleLie;
          else if (value > Globals.LowLieCount.Item1 && value < Globals.LowLieCount.Item2)
            message = Globals.DataLowLie;
          break;
        case ScaleType.True:
          if (value > Globals.MAX_TRUE_COUNT)
          {
            message = string.Format(Globals.DataTooMuchTrue, value);
            isWarning = true;
          }
          else if( value > Globals.MiddleTrueCount.Item1 && value < Globals.MiddleTrueCount.Item2 )
            message = Globals.DataMiddleTrue;
          break;
      }

      _ResultList.Add(new Tuple<ScaleType, double, string, string,string, bool>(type, value, stringValue,name,message,isWarning));
    }
  }
}

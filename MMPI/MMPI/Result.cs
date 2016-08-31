using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MMPI
{
  public class Result
  {
    private readonly ScaleType _ScaleType;

    private readonly double _Value;

    private bool _IsWarning;

    public Result(ScaleType scaleType,double value)
    {
      _ScaleType = scaleType;
      _Value = value;
      InitResult();
    }

    public double Value
    {
      get { return _Value; }
    }

    public string Message { get; private set; }

    public string Name { get; private set; }

    public string StringValue { get; private set; }

    public SolidColorBrush FontColor
    {
    get { return _IsWarning ? Brushes.Red : Brushes.Black; }
    }

    private void InitResult()
    {
      StringValue = Math.Round(_Value, 2).ToString(CultureInfo.InvariantCulture) + " T";
      Name = Globals.ScaleNames.First(item => item.Item1 == _ScaleType).Item2;

      _IsWarning = false;
      //Шкала правдивости
      switch (_ScaleType)
      {
        case ScaleType.Lie:
          if (_Value > Globals.MAX_LIE_COUNT)
          {
            Message = string.Format(Globals.DataTooMuchLie, StringValue);
            _IsWarning = true;
          }
          else if (_Value > Globals.MiddleLieCount.Item1 && _Value < Globals.MiddleLieCount.Item2)
            Message = Globals.DataMiddleLie;
          else if (_Value > Globals.LowLieCount.Item1 && _Value < Globals.LowLieCount.Item2)
            Message = Globals.DataLowLie;
          break;
        case ScaleType.True:
          if (_Value > Globals.MAX_TRUE_COUNT)
          {
            Message = string.Format(Globals.DataTooMuchTrue, StringValue);
            _IsWarning = true;
          }
          else if (_Value > Globals.MiddleTrueCount.Item1 && _Value < Globals.MiddleTrueCount.Item2)
            Message = Globals.DataMiddleTrue;
          break;

        case ScaleType.Correction:
          if (_Value > Globals.MAX_CORRECTION_COUNT)
          {
            Message = Globals.DataMaxCorrection;
            _IsWarning = true;
          }
          else if (_Value > Globals.MiddleCorrectCount.Item1 && _Value < Globals.MiddleCorrectCount.Item2)
            Message = Globals.DataMiddleCorrection;
          break;
      }
    }
  }
}

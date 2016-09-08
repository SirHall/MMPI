using System;
using System.Globalization;
using System.Linq;
using System.Windows.Media;

namespace MMPI
{
  /// <summary>Модель отображения данных для одной из Шкал MMPI</summary>
  public class ResultViewModel
  {
    #region Поля
    /// <summary>Если true, то предупреждение по шкале, иначе false</summary>
    private bool _IsWarning;
    #endregion

    #region Конструкторы

    /// <summary>Создаёт новый экземпляр класса <see cref="ResultViewModel"/>.</summary>
    /// <param name="scaleType">Тип шкалы.</param>
    /// <param name="value">Значение шкалы.</param>
    public ResultViewModel(ScaleType scaleType, double value)
    {
      ScaleType = scaleType;
      Value = value;
      CalculateScale();
    }
    #endregion

    #region Свойства

    /// <summary>Возвращает значение шкалы</summary>
    public double Value { get; private set; }

    /// <summary>Возвращает тип шкалы</summary>
    public ScaleType ScaleType { get; private set; }

    /// <summary>Возвращает сообщение шкалы</summary>
    public string Message { get; private set; }

    /// <summary>Возвращает наименование шкалы</summary>
    public string Name { get; private set; }

    /// <summary>Возвращает строковое значение шкалы</summary>
    public string StringValue { get; private set; }

    /// <summary>Возвращает цвет отображения сообщения</summary>
    public SolidColorBrush FontColor
    {
      get { return _IsWarning ? Brushes.Red : Brushes.Black; }
    }
    #endregion

    #region Методы

    /// <summary>Рассчитывает шкалу</summary>
    private void CalculateScale()
    {
      StringValue = Math.Round(Value, 2).ToString(CultureInfo.InvariantCulture) + " T";
      Name = Globals.ScaleNames.First(item => item.Item1 == ScaleType).Item2;

      _IsWarning = false;
      //Шкала правдивости
      switch (ScaleType)
      {
        case ScaleType.Lie:
          if (Value > Globals.MAX_LIE_COUNT)
          {
            Message = string.Format(Globals.DataTooMuchLie, StringValue);
            _IsWarning = true;
          }
          else if (Value > Globals.MiddleLieCount.Item1 && Value < Globals.MiddleLieCount.Item2)
            Message = Globals.DataMiddleLie;
          else if (Value > Globals.LowLieCount.Item1 && Value < Globals.LowLieCount.Item2)
            Message = Globals.DataLowLie;
          break;
        case ScaleType.True:
          if (Value > Globals.MAX_TRUE_COUNT)
          {
            Message = string.Format(Globals.DataTooMuchTrue, StringValue);
            _IsWarning = true;
          }
          else if (Value > Globals.MiddleTrueCount.Item1 && Value < Globals.MiddleTrueCount.Item2)
            Message = Globals.DataMiddleTrue;
          break;

        case ScaleType.Correction:
          if (Value > Globals.MAX_CORRECTION_COUNT)
          {
            Message = Globals.DataMaxCorrection;
            _IsWarning = true;
          }
          else if (Value > Globals.MiddleCorrectCount.Item1 && Value < Globals.MiddleCorrectCount.Item2)
            Message = Globals.DataMiddleCorrection;
          break;
      }
    }
    #endregion
  }
}

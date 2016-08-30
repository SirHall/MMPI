using System.Collections.Generic;

namespace MMPI
{
  /// <summary>Класс отображения результатов</summary>
  public class Results:ObservableObject
  {
    #region Поля

    /// <summary>Список шкал с результатами</summary>
    private readonly List<Result> _ResultList = new List<Result>();
    #endregion

    #region Свойства
    /// <summary>Возвращает список результатов</summary>
    public List<Result> ResultList
    {
      get { return _ResultList; }
    }

    /// <summary>Возвращает или задает видимость элемента</summary>
    public bool IsVisible { get; set; }
    #endregion

    #region Методы
    /// <summary>Устанавливает значение шкалы</summary>
    /// <param name="type">Тип шкалы.</param>
    /// <param name="value">Значение.</param>
    public void SetValue(ScaleType type, double value)
    {
      _ResultList.Add(new Result(type, value));
    }
    #endregion
  }
}

namespace MMPI
{
  /// <summary>Структура для пола</summary>
  public struct Gender
  {
    /// <summary>Пол</summary>
    public GenderType Type;

    /// <summary>Возвращает или задает наименование пола</summary>
    public string Name { get; set; }
  }
}

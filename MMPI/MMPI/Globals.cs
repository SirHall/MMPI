namespace MMPI
{

  /// <summary>Класс содержит глобальные функции, переменные и константы</summary>
  internal static class Globals
  {
    #region XML
    /// <summary>Раздел xml с вопросами</summary>
    public const string XML_QUESTIONS = "Questions";
    /// <summary>Раздел xml с вопросом</summary>
    public const string XML_QUESTION = "Question";

    public const string XML_SELECT_QUESTIONS = XML_QUESTIONS + "//" + XML_QUESTION;
    /// <summary>Раздел xml с номером вопроса</summary>
    public const string XML_NUMBER = "Number";
    /// <summary>Раздел xml с текстом вопроса</summary>
    public const string XML_TEXT = "Text";

    /// <summary>Xml-файл с мужскими вопросами</summary>
    public const string XML_MALE = "male.xml";

    /// <summary>Xml-файл с женскими вопросами</summary>
    public const string XML_FEMALE = "female.xml";
    #endregion

    /// <summary>Ошибка с расположением файла</summary>
    public const string FILE_NOT_FOUND = "Файл с вопросами {0} не найден. Продолжение невозможно";

    /// <summary>Формат отображения вопроса</summary>
    public const string QUESTION_FORMAT = "{0}. {1}";
  }
}

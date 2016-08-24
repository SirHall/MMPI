namespace MMPI
{

  /// <summary>Класс содержит глобальные функции, переменные и константы</summary>
  internal class Globals
  {
    /// <summary>Раздел xml с вопросами</summary>
    public const string XML_QUESTIONS = "Questions";
    /// <summary>Раздел xml с вопросом</summary>
    public const string XML_QUESTION = "Question";

    public const string XML_SELECT_QUESTIONS = XML_QUESTIONS+"//"+XML_QUESTION;
    /// <summary>Раздел xml с номером вопроса</summary>
    public const string XML_NUMBER = "Number";
    /// <summary>Раздел xml с текстом вопроса</summary>
    public const string XML_TEXT = "Text";

    /// <summary>Xml-файл с мужскими вопросами</summary>
    public const string XML_MALE = "male.xml";
    
    /// <summary>Xml-файл с женскими вопросами</summary>
    public const string XML_FEMALE = "female.xml";

    public const string FILE_NOT_FOUND = "Файл с вопросами {0} не найден. Продолжение невозможно";
  }
}

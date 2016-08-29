using System;
using System.Xml;

namespace MMPI
{
  /// <summary>Класс вопроса для MMPI</summary>
  public class Question
  {
    #region Поля

    /// <summary>Текст вопроса</summary>
    private readonly string _Text;
    #endregion

    #region Конструкторы
    /// <summary>Создаёт новый экземпляр класса <see cref="Question"/>.</summary>
    /// <param name="node">Запись в файле XML.</param>
    /// <exception cref="System.ArgumentNullException">Запись не может быть пустой</exception>
    public Question(XmlNode node)
    {
      if (node == null)
        throw new ArgumentNullException("node");
      var selectSingleNode = node.SelectSingleNode(Globals.XML_NUMBER);
      if (selectSingleNode != null)
        Number = Convert.ToInt32(selectSingleNode.InnerText);
      selectSingleNode = node.SelectSingleNode(Globals.XML_TEXT);
      if (selectSingleNode != null)
        _Text = selectSingleNode.InnerText;
    }
    #endregion

    #region Свойства
    /// <summary>Возвращает номер вопроса</summary>
    public int Number { get; private set; }

    /// <summary>Возвращает текст вопроса</summary>
    public string Text
    {
      get { return string.Format(Globals.QUESTION_FORMAT, Number, _Text); }
    }

    /// <summary>Возвращает ответ на вопрос</summary>
    public AnswerType Answer { get; set; }
    #endregion
  }
}

using System;
using System.Xml;

namespace MMPI
{
  /// <summary>Класс вопроса для MMPI</summary>
  public class Question
  {
    /// <summary>Создаёт новый экземпляр класса <see cref="Question"/>.</summary>
    /// <param name="node">Запись в файле XML.</param>
    /// <exception cref="System.ArgumentNullException">Запись не может быть пустой</exception>
    public Question( XmlNode node )
    {
      if (node==null)
        throw new ArgumentNullException("node");
      var selectSingleNode = node.SelectSingleNode(Globals.XML_NUMBER);
      if( selectSingleNode != null )
        Number =Convert.ToInt32(selectSingleNode.InnerText);
      selectSingleNode = node.SelectSingleNode(Globals.XML_TEXT);
      if( selectSingleNode != null )
        Text = selectSingleNode.InnerText;
    }

    /// <summary>Возвращает номер вопроса</summary>
    public int Number { get; private set; }

    /// <summary>Возвращает текст вопроса</summary>
    public string Text { get; private set; }

    /// <summary>Возвращает ответ на вопрос</summary>
    public bool Answer { get; set; }
  }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using System.Xml;

using Mailer;

namespace MMPI
{

  /// <summary>Модель отображения последовательности вопросов</summary>
  internal class MainWindowViewModel : INotifyPropertyChanging, INotifyPropertyChanged
  {
    #region Поля

    /// <summary>Список вопросов</summary>
    private readonly List<Question> _Questions;

    /// <summary>Текущий вопрос</summary>
    private Question _CurrentQuestion;

    /// <summary>Команда "Да"</summary>
    private ICommand _YesCommand;

    /// <summary>Команда "Нет"</summary>
    private ICommand _NoCommand;

    #endregion

    #region Конструкторы

    /// <summary>Создаёт новый экземпляр класса <see cref="MainWindowViewModel"/>.</summary>
    /// <param name="isMale">Если <c>true</c> мужской тест, иначе женский.</param>
    /// <exception cref="System.Exception">Файл с вопросами не существует</exception>
    public MainWindowViewModel( bool isMale )
    {
      var fileName = isMale ? Globals.XML_MALE : Globals.XML_FEMALE;
      var doc = new XmlDocument();
      if( !File.Exists(fileName) )
        throw new Exception(string.Format(Globals.FILE_NOT_FOUND, fileName));
      doc.Load(fileName);
      var nodeList = doc.SelectNodes(Globals.XML_SELECT_QUESTIONS);
      if( nodeList == null || nodeList.Count == 0 )
        return;
      _Questions = new List<Question>();
      foreach( XmlNode node in nodeList )
        _Questions.Add(new Question(node));
      _CurrentQuestion = _Questions.First();
    }

    #endregion

    #region Свойства

    public ICommand YesCommand
    {
      get { return _YesCommand ?? ( _YesCommand = new RelayCommand(parameter => NextQuestion(true)) ); }
    }
    public ICommand NoCommand
    {
      get { return _NoCommand ?? (_NoCommand = new RelayCommand(parameter => NextQuestion(false))); }
    }

  #endregion
    public Question CurrentQuestion
    {
      get { return _CurrentQuestion; }
      set
      {
        if (value==null||_CurrentQuestion==value)
          return;
        _CurrentQuestion = value;
      }
    }

    private void NextQuestion(bool value)
    {
      _CurrentQuestion.Answer = value;
      CurrentQuestion = _Questions[_CurrentQuestion.Number];
    }

    #region Implementation of INotifyPropertyChanging

    public event PropertyChangingEventHandler PropertyChanging;

    #endregion

    #region Implementation of INotifyPropertyChanged

    public event PropertyChangedEventHandler PropertyChanged;

    #endregion
  }

}

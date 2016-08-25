using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Xml;


namespace MMPI
{
  /// <summary>Модель отображения последовательности вопросов</summary>
  internal class MainWindowViewModel : ObservableObject
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

    /// <summary>Команда начала теста</summary>
    private ICommand _StartTest;

    private bool _IsStarted;

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
      User = new UserInfo();
      doc.Load(fileName);
      var nodeList = doc.SelectNodes(Globals.XML_SELECT_QUESTIONS);
      if( nodeList == null || nodeList.Count == 0 )
        return;
      _Questions = new List<Question>();
      foreach( XmlNode node in nodeList )
        _Questions.Add(new Question(node));
      _CurrentQuestion = _Questions.First();
      IsStarted = false;
    }

    #endregion

    #region Свойства

    public UserInfo User { get; private set; }

    /// <summary>
    /// Gets the started.
    /// </summary>
    public Visibility Started
    {
      get { return _IsStarted ? Visibility.Visible : Visibility.Collapsed; }
    }

    public bool IsStarted
    {
      get { return _IsStarted; }
      set
      {
        if( value == _IsStarted )
          return;
        _IsStarted = value;
        OnPropertyChanged();
      }
    }

    public Visibility ShowStartPage
    {
      get { return !_IsStarted ? Visibility.Visible : Visibility.Collapsed; }
    }


    public ICommand StartTestCommand
    {
      get { return _StartTest ?? ( _StartTest = new RelayCommand(parameter => StartTest()) ); }
    }

    /// <summary>Возвращает команду ответа "Да"</summary>
    public ICommand YesCommand
    {
      get { return _YesCommand ?? ( _YesCommand = new RelayCommand(parameter => NextQuestion(true)) ); }
    }

    /// <summary>Возвращает команду ответа "Нет"</summary>
    public ICommand NoCommand
    {
      get { return _NoCommand ?? (_NoCommand = new RelayCommand(parameter => NextQuestion(false))); }
    }

    /// <summary>Возвращает или задает текущий вопрос</summary>
    public Question CurrentQuestion
    {
      get { return _CurrentQuestion; }
      set
      {
        if (value == null || _CurrentQuestion == value)
          return;
        _CurrentQuestion = value;
        OnPropertyChanged();
      }
    }
  #endregion

    #region Методы

    private void StartTest()
    {
      User.IsStarted = true;
      IsStarted = true;
      OnPropertyChanged("Started");
      OnPropertyChanged("ShowStartPage");

    }

    /// <summary>Переходит к следующему вопросу</summary>
    /// <param name="value">Если <c>true</c> то ответ "Да".</param>
    private void NextQuestion(bool value)
    {
      _CurrentQuestion.Answer = value;
      CurrentQuestion = _Questions[_CurrentQuestion.Number];
    }
    #endregion
  }

}

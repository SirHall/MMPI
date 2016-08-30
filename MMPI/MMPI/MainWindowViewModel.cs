using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using System.Xml;

namespace MMPI
{
  /// <summary>Модель отображения последовательности вопросов</summary>
  public class MainWindowViewModel : ObservableObject
  {
    #region Поля

    /// <summary>Список вопросов</summary>
    private  List<Question> _Questions;

    /// <summary>Текущий вопрос</summary>
    private Question _CurrentQuestion;

    /// <summary>Команда ответа</summary>
    private ICommand _AnswerCommand;

    /// <summary>Команда начала теста</summary>
    private ICommand _StartTest;

    /// <summary>Признак начала теста</summary>
    private bool _IsStarted;

    #endregion

    #region Конструкторы
    /// <summary>Создаёт новый экземпляр класса <see cref="MainWindowViewModel"/>.</summary>
    /// <exception cref="System.Exception">Файл с вопросами не существует</exception>
    public MainWindowViewModel()
    {
      User = new UserInfo();
      IsStarted = false;
    }

    /// <summary>Создаёт новый экземпляр класса <see cref="MainWindowViewModel"/>.</summary>
    /// <exception cref="System.Exception">Файл с вопросами не существует</exception>
    public MainWindowViewModel(string path)
    {
      User = new UserInfo();
      IsStarted = false;
      var fileName = path;
      var doc = new XmlDocument();
      if (!File.Exists(fileName))
        throw new Exception(string.Format(Globals.FILE_NOT_FOUND, fileName));

      doc.Load(fileName);
      var nodeList = doc.SelectNodes(Globals.XML_SELECT_QUESTIONS);
      if (nodeList == null || nodeList.Count == 0)
        return;
      _Questions = new List<Question>();
      foreach (XmlNode node in nodeList)
        _Questions.Add(new Question(node));
      _CurrentQuestion = _Questions.First();
    }

    #endregion

    #region Свойства

    public Results Results { get; private set; }

    public List<Question> Questions
    {
      get { return _Questions; }
    }

    /// <summary>Возвращает информацию о пользователе, который проходит тест</summary>
    public UserInfo User { get; private set; }

    /// <summary>Возвращает или задает начало тестирования</summary>
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

    /// <summary>Возвращает команду начала тестирования</summary>
    public ICommand StartTestCommand
    {
      get { return _StartTest ?? ( _StartTest = new RelayCommand(parameter => StartTest()) ); }
    }

    /// <summary>Возвращает команду ответа</summary>
    public ICommand AnswerCommand
    {
      get { return _AnswerCommand ?? (_AnswerCommand = new RelayCommand(parameter => NextQuestion((string)parameter))); }
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

    /// <summary>Загружает список вопросов из файла</summary>
    /// <param name="isMale">Если <c>true</c> то мужские вопросы, иначе женские.</param>
    /// <exception cref="System.Exception"></exception>
    private void LoadQuestions(bool isMale)
    {
      var fileName = isMale
                 ? Application.StartupPath + "\\" + Globals.XML_MALE
                 : Application.StartupPath + "\\" + Globals.XML_FEMALE;
      var doc = new XmlDocument();
      if (!File.Exists(fileName))
        throw new Exception(string.Format(Globals.FILE_NOT_FOUND, fileName));
      doc.Load(fileName);
      var nodeList = doc.SelectNodes(Globals.XML_SELECT_QUESTIONS);
      if (nodeList == null || nodeList.Count == 0)
        return;
      _Questions = new List<Question>();
      foreach (XmlNode node in nodeList)
        _Questions.Add(new Question(node));
      CurrentQuestion = _Questions.Last();
    }

    /// <summary>Начинаем тест</summary>
    private void StartTest()
    {
      User.IsStarted = true;
      IsStarted = true;
      LoadQuestions(User.SelectedGender.Type==GenderType.Male);
    }

    /// <summary>Переходит к следующему вопросу</summary>
    /// <param name="type">Тип ответа</param>
    private void NextQuestion(string type)
    {
      if (string.IsNullOrEmpty(type))
        return;
      AnswerType answer;
      Enum.TryParse(type, true, out answer);
      _CurrentQuestion.Answer = answer;
      if (_Questions.Count < _CurrentQuestion.Number)
        CurrentQuestion = _Questions[_CurrentQuestion.Number];
      else
        ShowResults();
    }

    //Показываем результаты тестирования
    private void ShowResults()
    {
      Results = new Results();
      foreach( var scale in Globals.ScaleNames )
      {
        if( ( User.SelectedGender.Type == GenderType.Male && scale.Item1 == ScaleType.Femininity )
            || ( User.SelectedGender.Type == GenderType.Female && scale.Item1 == ScaleType.Masculinity ) )
          continue;
        Results.SetValue(scale.Item1, CalculateScale(scale.Item1));
      }
      Results.IsVisible = true;
      IsStarted = false;
      OnPropertyChanged("Results");
    }

    /// <summary>Возвращает количество ответов "Не знаю"</summary>
    /// <returns>Количество ответов "Не знаю"</returns>
    public int GetDontKnowCount()
    {
      return _Questions.Count(question => question.Answer == AnswerType.MbDontKnow);
    }

    /// <summary>Рассчитывает значение шкалы в T-баллах</summary>
    /// <param name="type">Тип шкалы.</param>
    /// <returns>Значение шкалы</returns>
    public double CalculateScale( ScaleType type )
    {
      var statisticItem = User.SelectedGender.Type == GenderType.Male ? Globals.MaleStatistic.First(item => item.Item1 == type) : Globals.FemaleStatistic.First(item => item.Item1 == type);
      var listTypeYes = Globals.Scales.First(item => item.Item1 == type).Item2;
      var listTypeNo = Globals.Scales.First(item => item.Item1 == type).Item3;
      var counter = listTypeYes!=null ? listTypeYes.Select(item => _Questions.First(question => question.Number == item)).Count(quest => quest != null && quest.Answer == AnswerType.MbYes):0;
      counter +=listTypeNo!=null ? listTypeNo.Select(item => _Questions.First(question => question.Number == item)).Count(quest => quest != null && quest.Answer == AnswerType.MbNo):counter;
      return 50 + (10 * (counter - statisticItem.Item2) / statisticItem.Item3);
    }

    #endregion
  }

}

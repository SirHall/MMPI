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
    /// <summary>Рандомайзер для заполнения</summary>
    private readonly Random _Random = new Random();

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
      User = new UserViewModel();
      IsStarted = false;
    }

    /// <summary>Создаёт новый экземпляр класса <see cref="MainWindowViewModel"/>.</summary>
    /// <exception cref="System.Exception">Файл с вопросами не существует</exception>
    public MainWindowViewModel(string path)
    {
      User = new UserViewModel();
      IsStarted = false;
      var fileName = path;
      var doc = new XmlDocument();
      if (!File.Exists(fileName))
        throw new Exception(string.Format(Globals.FILE_NOT_FOUND, fileName));

      doc.Load(fileName);
      var nodeList = doc.SelectNodes(Globals.XML_SELECT_QUESTIONS);
      if (nodeList == null || nodeList.Count == 0)
        return;
      Questions = new List<Question>();
      foreach (XmlNode node in nodeList)
        Questions.Add(new Question(node));
      _CurrentQuestion = Questions.First();
    }

    #endregion

    #region Свойства

    /// <summary>Возвращает результаты прохождения теста</summary>
    public ResultsViewModel Results { get; private set; }

    /// <summary>Возвращает список вопросов теста</summary>
    public List<Question> Questions { get; private set; }

    /// <summary>Возвращает информацию о пользователе, который проходит тест</summary>
    public UserViewModel User { get; private set; }

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
      Questions = new List<Question>();
      foreach (XmlNode node in nodeList)
        Questions.Add(new Question(node));
      CurrentQuestion = Questions.Last();
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
      if (!Enum.TryParse(type, true, out answer))
        throw new AccessViolationException("answer type don't find");
      _CurrentQuestion.Answer = answer;
      if (Questions.Count < _CurrentQuestion.Number)
        CurrentQuestion = Questions[_CurrentQuestion.Number];
      else
        ShowResults();
    }
    /// <summary>Заполняет случайными ответами</summary>
    private void FillQuestionsRandom(int maxDontKnow)
    {
      foreach (var question in Questions)
      {
        var random = _Random.Next(0, 3);
        switch (random)
        {
          case 0:
            {
              question.Answer = AnswerType.MbNo;
              break;
            }
          case 1:
            {
              question.Answer = AnswerType.MbYes;
              break;
            }
          case 2:
            {
              if (maxDontKnow == 0)
              {
                random = _Random.Next(0, 2);
                switch (random)
                {
                  case 0:
                    {
                      question.Answer = AnswerType.MbYes;
                      break;
                    }
                  case 1:
                    {
                      question.Answer = AnswerType.MbNo;
                      break;
                    }
                }
              }
              else
              {
                question.Answer = AnswerType.MbDontKnow;
                maxDontKnow--;
              }
              break;
            }
        }
      }
    }
    //Показываем результаты тестирования
    private void ShowResults()
    {
      FillQuestionsRandom(30);
      Results = new ResultsViewModel();
      Results.SetValue(ScaleType.DontKnow, GetDontKnowCount());
      foreach( var scale in Globals.ScaleNames.Where(scale => ( User.SelectedGender.Type != GenderType.Male || scale.Item1 != ScaleType.Femininity ) && ( User.SelectedGender.Type != GenderType.Female || scale.Item1 != ScaleType.Masculinity )&&scale.Item1!=ScaleType.DontKnow) )
        Results.SetValue(scale.Item1, CalculateScale(scale.Item1));
      Results.IsVisible = true;
      IsStarted = false;
      OnPropertyChanged("Results");
    }

    /// <summary>Возвращает количество ответов "Не знаю"</summary>
    /// <returns>Количество ответов "Не знаю"</returns>
    public int GetDontKnowCount()
    {
      return Questions.Count(question => question.Answer == AnswerType.MbDontKnow);
    }

    /// <summary>Рассчитывает значение шкалы в T-баллах</summary>
    /// <param name="type">Тип шкалы.</param>
    /// <returns>Значение шкалы</returns>
    public double CalculateScale( ScaleType type )
    {
      var statisticItem = User.SelectedGender.Type == GenderType.Male ? Globals.MaleStatistic.First(item => item.Item1 == type) : Globals.FemaleStatistic.First(item => item.Item1 == type);
      var listTypeYes = Globals.Scales.First(item => item.Item1 == type).Item2;
      var listTypeNo = Globals.Scales.First(item => item.Item1 == type).Item3;
      var counter = listTypeYes!=null ? listTypeYes.Select(item => Questions.First(question => question.Number == item)).Count(quest => quest != null && quest.Answer == AnswerType.MbYes):0;
      counter +=listTypeNo!=null ? listTypeNo.Select(item => Questions.First(question => question.Number == item)).Count(quest => quest != null && quest.Answer == AnswerType.MbNo):counter;
      return 50 + (10 * (counter - statisticItem.Item2) / statisticItem.Item3);
    }

    #endregion
  }

}

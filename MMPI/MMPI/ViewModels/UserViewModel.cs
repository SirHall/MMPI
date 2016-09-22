using System;
using System.Collections.Generic;
using System.Linq;

namespace MMPI
{
  /// <summary>Класс описывает пользователя, который проходит тест</summary>
  public class UserViewModel:ObservableObject
  {
    #region Поля
    /// <summary>Признак начала тестирования</summary>
    private bool _IsStarted;
    #endregion

    #region Конструкторы
    /// <summary>Создаёт новый экземпляр класса <see cref="UserViewModel"/>.</summary>
    public UserViewModel()
    {
      Birthday = DateTime.Now;
      GenderList = new List<Gender> { new Gender { Type = GenderType.Male, Name = Globals.GENDER_MALE }, new Gender { Type = GenderType.Female, Name = Globals.GENDER_FEMALE } };
      SelectedGender = GenderList.First();
    }
    #endregion

    #region Свойства
    /// <summary>Возвращает или задает имя пользователя</summary>
    public string Name { get; set; }

    /// <summary>Возвращает или задает дату рождения пользователя</summary>
    public DateTime Birthday { get; set; }

    /// <summary>Возвращает количество полных лет</summary>
    public int Age { get; private set; }

    /// <summary>Возвращает список возможных полов</summary>
    public List<Gender> GenderList { get; private set; }

    /// <summary>Возвращает или задает выбранный пол</summary>
    public Gender SelectedGender { get; set; }

    /// <summary>Возвращает или задает начало тестирования</summary>
    public bool IsStarted
    {
      get { return _IsStarted; }
      set
      {
        if (value == _IsStarted)
          return;
        _IsStarted = value;
        Age = DateTime.Now.Year - Birthday.Year;
        if( DateTime.Now.Month < Birthday.Month || ( DateTime.Now.Month == Birthday.Month && DateTime.Now.Day < Birthday.Day ) )
          Age--;
        OnPropertyChanged();
      }
    }
    #endregion
  }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace MMPI
{

  /// <summary>Класс описывает пользователя, который проходит тест</summary>
  public class UserInfo:ObservableObject
  {
    #region Поля
    /// <summary>Признак начала тестирования</summary>
    private bool _IsStarted;
    #endregion

    #region Конструкторы
    /// <summary>Создаёт новый экземпляр класса <see cref="UserInfo"/>.</summary>
    public UserInfo()
    {
      Birthday = DateTime.Now;
      GenderList = new List<Gender> { new Gender { Type = GenderType.Male, Name = Globals.GENDER_MALE }, new Gender { Type = GenderType.Female, Name = Globals.GENDER_FEMALE } };
      SelectedGender = GenderList.First();
    }
    #endregion


    public string Name { get; set; }

    public DateTime Birthday { get; set; }

    public List<Gender> GenderList { get; private set; }

    public Gender SelectedGender { get; set; }

    public bool IsStarted
    {
      get { return _IsStarted; }
      set
      {
        if (value==_IsStarted)
          return;
        _IsStarted = value;
        OnPropertyChanged();
      }
    }
  }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace MMPI
{
  internal struct Gender
  {
    public GenderType Type;

    public string Name { get; set; }
  }

  /// <summary>Класс описывает пользователя, который проходит тест</summary>
  internal class UserInfo:ObservableObject
  {
    private bool _IsStarted ;
    public UserInfo()
    {
      Birthday = DateTime.Now;
      GenderList = new List<Gender>{new Gender{Type=GenderType.Male,Name = "Мужской"}, new Gender{Type = GenderType.Female,Name="Женский"}};
      SelectedGender = GenderList.First();
    }

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

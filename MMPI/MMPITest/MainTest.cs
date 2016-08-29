using System;

using MMPI;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMPITest
{
  [TestClass]
  public class MainTest
  {

    /// <summary>Рандомайзер для заполнения</summary>
    private readonly Random _Random = new Random();

    /// <summary>Тестируемая модель</summary>
    private readonly MainWindowViewModel _ViewModel = new MainWindowViewModel(@"E:\Personal\MMPI\MMPI\MMPITest\bin\Debug\male.xml");

    /// <summary>Заполняет случайными ответами</summary>
    private void FillQuestionsRandom(int maxDontKnow)
    {
      foreach( var question in _ViewModel.Questions )
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
              if( maxDontKnow == 0 )
              {
                random = _Random.Next(0, 2);
                switch( random )
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

    [TestMethod]
    public void FillQuestionsTest()
    {
      FillQuestionsRandom(30);
      var count = _ViewModel.GetDontKnowCount();
      var lieScale = _ViewModel.CalculateScale(ScaleType.Lie);
      var truthScale = _ViewModel.CalculateScale(ScaleType.True);
    }
  }
}

using System;
using System.Collections.Generic;
using System.Windows;

namespace MMPI
{
  /// <summary>Структура для пола</summary>
  public struct Gender
  {
    /// <summary>Пол</summary>
    public GenderType Type;

    /// <summary>Возвращает или задает наименование пола</summary>
    public string Name { get; set; }
  }

  /// <summary>Класс содержит глобальные функции, переменные и константы</summary>
  internal static class Globals
  {
    #region XML

    /// <summary>Раздел xml с вопросами</summary>
    public const string XML_QUESTIONS = "Questions";

    /// <summary>Раздел xml с вопросом</summary>
    public const string XML_QUESTION = "Question";

    /// <summary>Выборка всех ответов из файла-источника</summary>
    public const string XML_SELECT_QUESTIONS = XML_QUESTIONS + "//" + XML_QUESTION;

    /// <summary>Раздел xml с номером вопроса</summary>
    public const string XML_NUMBER = "Number";

    /// <summary>Раздел xml с текстом вопроса</summary>
    public const string XML_TEXT = "Text";

    /// <summary>Xml-файл с мужскими вопросами</summary>
    public const string XML_MALE = "male.xml";

    /// <summary>Xml-файл с женскими вопросами</summary>
    public const string XML_FEMALE = "female.xml";

    #endregion

    #region Сообщения
    /// <summary>Сообщение при слишком большом количестве ответов "Не знаю"</summary>
    public static readonly string DataTooMuchDontKnow = "При прохождении теста было указано {0} вопросов с ответом Не знаю. Максимально возможное " + MAX_DONT_KNOW_COUNT + " ответов";

    /// <summary>Сообщение о недостоврености</summary>
    public static readonly string DataTooMuchLie = "По шкале лжи набрано {0}. Максимально возможное " + MAX_LIE_COUNT + " T";

    /// <summary>Максимальное значение корректности</summary>
    public static readonly string DataMaxCorrection =
      "Cвидетельствует, что испытуемый не захотел рассказать о себе откровенно и демонстрирует лишь свою социабельность и стремление произвести приятное впечатление";

    /// <summary>Среднее значение корректности</summary>
    public static readonly string DataMiddleCorrection = "Естественная защитная реакции человека на попытку вторжения в мир его сокровенных переживаний, т.е. при хороший контроль над эмоциями";

    /// <summary>Среднее значение достоверности</summary>
    public static readonly string DataMiddleTrue = "Та или иная степень дисгармонии. Находится в состоянии дискомфорта, что отражает эмоциональную неустойчивость";

    /// <summary>Среднее значение лжи</summary>
    public static readonly string DataLowLie = "Cвидетельствуют об отсутствии тенденции приукрасить свой характер";

    /// <summary>Среднее значение лжи</summary>
    public static readonly string DataMiddleLie = "Часто встречается у лиц примитивного психического склада с недостаточным самопониманием и низкими адаптивными возможностями";

    /// <summary>Сообщение о недостоврености</summary>
    public static readonly string DataTooMuchTrue = "По шкале откровенности набрано {0}. Максимально возможное " + MAX_TRUE_COUNT + " T";

    /// <summary>Сообщение при слишком большом количестве ответов "Не знаю"</summary>
    public static readonly string WarningDontKnowCount = string.Format(DATA_NOT_TRUE, DataTooMuchDontKnow);

    /// <summary>Сообщение при слишком большом количестве ответов "Не знаю"</summary>
    public static readonly string WarningLieCount = string.Format(DATA_NOT_TRUE, DataTooMuchLie);

    /// <summary>Сообщение о настороженности и неоткрытости</summary>
    public static readonly string WarningDontKnowCountMiddle = "При прохождении теста было указано {0} вопросов с ответом Не знаю, что свидетельствуют о выраженной настороженности и неоткровенности обследуемого";
    #endregion

    #region Обработка результатов

    public static readonly List<Tuple<ScaleType, string>>  ScaleNames = new List<Tuple<ScaleType, string>>
                                                                          {
                                                                            new Tuple<ScaleType, string>(ScaleType.Lie,"Достоверность"),
                                                                            new Tuple<ScaleType, string>(ScaleType.True,"Надежность"),
                                                                            new Tuple<ScaleType, string>(ScaleType.Correction,"Корректировка"),
                                                                            new Tuple<ScaleType, string>(ScaleType.Overcontrol,"Сверхконтроль"),
                                                                            new Tuple<ScaleType, string>(ScaleType.Pessimistically,"Пессимистичность"),
                                                                            new Tuple<ScaleType, string>(ScaleType.EmotionalLability,"Эмоциональная лабильность"),
                                                                            new Tuple<ScaleType, string>(ScaleType.Impulsiveness,"Импульсивность"),
                                                                            new Tuple<ScaleType, string>(ScaleType.Masculinity,"Мужественность"),
                                                                            new Tuple<ScaleType, string>(ScaleType.Femininity,"Женственность"),
                                                                            new Tuple<ScaleType, string>(ScaleType.Rigidity,"Ригидность"),
                                                                            new Tuple<ScaleType, string>(ScaleType.Anxiety,"Тревожность"),
                                                                            new Tuple<ScaleType, string>(ScaleType.Individualistic,"Индивидуалистичность"),
                                                                            new Tuple<ScaleType, string>(ScaleType.Optimistic,"Оптимистичность"),
                                                                            new Tuple<ScaleType, string>(ScaleType.Introversion,"Интроверсия"),
                                                                            new Tuple<ScaleType, string>(ScaleType.DontKnow,"Неопределенность")
                                                                          };
    /// <summary>Статистические данные для Мужчин</summary>
    public static readonly List<Tuple<ScaleType, double, double>> MaleStatistic = new List<Tuple<ScaleType, double, double>>
                                                                                  {
                                                                                    new Tuple<ScaleType, double, double>(ScaleType.Lie, 4.2,2.9),
                                                                                    new Tuple<ScaleType, double, double>(ScaleType.True, 2.76,4.67),
                                                                                    new Tuple<ScaleType, double, double>(ScaleType.Correction, 12.1,5.4),
                                                                                    new Tuple<ScaleType, double, double>(ScaleType.Overcontrol, 11.1,3.9),
                                                                                    new Tuple<ScaleType, double, double>(ScaleType.Pessimistically, 16.6,4.11),
                                                                                    new Tuple<ScaleType, double, double>(ScaleType.EmotionalLability, 16.46,5.4),
                                                                                    new Tuple<ScaleType, double, double>(ScaleType.Impulsiveness, 18.68,4.11),
                                                                                    new Tuple<ScaleType, double, double>(ScaleType.Masculinity, 20.46,5.0),
                                                                                    new Tuple<ScaleType, double, double>(ScaleType.Rigidity, 7.9,3.4),
                                                                                    new Tuple<ScaleType, double, double>(ScaleType.Anxiety, 23.06,5.0),
                                                                                    new Tuple<ScaleType, double, double>(ScaleType.Individualistic, 21.96,5.0),
                                                                                    new Tuple<ScaleType, double, double>(ScaleType.Optimistic, 17.0,4.06),
                                                                                    new Tuple<ScaleType, double, double>(ScaleType.Introversion, 25.0,10.0)
                                                                                  };

    /// <summary>Статистические данные для Женщин</summary>
    public static readonly List<Tuple<ScaleType, double, double>> FemaleStatistic = new List<Tuple<ScaleType, double, double>>
                                                                                  {
                                                                                    new Tuple<ScaleType, double, double>(ScaleType.Lie, 4.2,2.9),
                                                                                    new Tuple<ScaleType, double, double>(ScaleType.True, 2.76,4.67),
                                                                                    new Tuple<ScaleType, double, double>(ScaleType.Correction, 12.1,5.4),
                                                                                    new Tuple<ScaleType, double, double>(ScaleType.Overcontrol, 12.9,4.83),
                                                                                    new Tuple<ScaleType, double, double>(ScaleType.Pessimistically, 18.9,5.0),
                                                                                    new Tuple<ScaleType, double, double>(ScaleType.EmotionalLability, 18.66,5.38),
                                                                                    new Tuple<ScaleType, double, double>(ScaleType.Impulsiveness, 18.68,4.11),
                                                                                    new Tuple<ScaleType, double, double>(ScaleType.Femininity, 36.7,4.67),
                                                                                    new Tuple<ScaleType, double, double>(ScaleType.Rigidity, 7.9,3.4),
                                                                                    new Tuple<ScaleType, double, double>(ScaleType.Anxiety, 25.07,6.1),
                                                                                    new Tuple<ScaleType, double, double>(ScaleType.Individualistic, 22.73,6.36),
                                                                                    new Tuple<ScaleType, double, double>(ScaleType.Optimistic, 17.0,4.06),
                                                                                    new Tuple<ScaleType, double, double>(ScaleType.Introversion, 25.0,10.0)
                                                                                  };
    /// <summary>Максимальное число ответов "Не знаю" в сырых баллах</summary>
    public const int MAX_DONT_KNOW_COUNT = 70;

    /// <summary>Максимальное число ложных ответов в (T)</summary>
    public const int MAX_LIE_COUNT = 70;

    /// <summary>Максимальное число ложных ответов в (T)</summary>
    public const int MAX_TRUE_COUNT = 80;

    /// <summary>Максимальное число по шкале корректировки в (T)</summary>
    public const int MAX_CORRECTION_COUNT = 66;

    /// <summary>Нарушенная адаптация и отклонение состояния индивида от нормального</summary>
    public const int MAX_BASE_SCALE = 75;

    /// <summary>Данные теста подвергаются сомнению</summary>
    public const string  DATA_NOT_TRUE = "Данные теста недостоверны, так как {0}";

    /// <summary>Среднее значение достоверности</summary>
    public static readonly Tuple<int, int> MiddleTrueCount = new Tuple<int, int>(65, 75);

    /// <summary>Низкое значение правдиовсти</summary>
    public static readonly Tuple<int, int> LowLieCount = new Tuple<int, int>(35, 42);
    
    /// <summary>Среднее значение правдиовсти</summary>
    public static readonly Tuple<int, int> MiddleLieCount = new Tuple<int, int>(60, 69);

    /// <summary>Количество ответов "Не знаю" свидетельствуют о выраженной настороженности и неоткровенности обследуемого в сырых баллах</summary>
    public static readonly Tuple<int, int> CriticalDontKnowCount = new Tuple<int, int>(41, 60);
    
    /// <summary>Трудно интерпритируемые данные</summary>
    public static readonly Tuple<int, int> DifferentInterpritation = new Tuple<int, int>(46, 55);

    /// <summary>Ведущие тенденции, которые определяют характерологические особенности индивида</summary>
    public static readonly Tuple<int, int> HightIntensive = new Tuple<int, int>(56, 66);

    /// <summary>Акцентуированные черты, которые временами затрудняют социально-психологическую адаптацию человека</summary>
    public static readonly Tuple<int, int> MoreHightIntensive = new Tuple<int, int>(67, 75);

    /// <summary>Гармоничный показатель</summary>
    public static readonly Tuple<int, int> LinearProfile = new Tuple<int, int>(45, 55);
    
    /// <summary>Средний показатель корректности</summary>
    public static readonly Tuple<int, int> MiddleCorrectCount = new Tuple<int, int>(45, 55);
    /// <summary>Список вопросов характеризующий шкалу лжи</summary>
    public static readonly List<int> LieList = new List<int>
                                                 {
                                                   15,
                                                   30,
                                                   45,
                                                   75,
                                                   105,
                                                   135,
                                                   195,
                                                   225,
                                                   255,
                                                   285,
                                                   60,
                                                   90,
                                                   120,
                                                   150,
                                                   165
                                                 };

    /// <summary>Список вопросов, характеризующий шкалу правды с ответом "Да"</summary>
    public static readonly List<int> TrueListYes = new List<int>
                                                     {
                                                       23,
                                                       27,
                                                       31,
                                                       34,
                                                       35,
                                                       40,
                                                       42,
                                                       49,
                                                       50,
                                                       53,
                                                       56,
                                                       85,
                                                       139,
                                                       146,
                                                       156,
                                                       202,
                                                       206,
                                                       209,
                                                       210,
                                                       211,
                                                       215,
                                                       218,
                                                       227,
                                                       245,
                                                       246,
                                                       247,
                                                       252,
                                                       256,
                                                       269,
                                                       286,
                                                       291
                                                     };

    /// <summary>Список вопросов, характеризующий шкалу правды с ответом "Нет"</summary>
    public static readonly List<int> TrueListNo = new List<int>
                                                    {
                                                      17,
                                                      20,
                                                      54,
                                                      65,
                                                      75,
                                                      83,
                                                      112,
                                                      113,
                                                      115,
                                                      164,
                                                      169,
                                                      177,
                                                      185,
                                                      196,
                                                      199,
                                                      220,
                                                      257,
                                                      258,
                                                      272,
                                                      276
                                                    };

    /// <summary>Список вопросов, характеризующий шкалу корректировки с ответом "Да"</summary>
    public static readonly List<int> CorrectionListYes = new List<int> { 96 };

    /// <summary>Список вопросов, характеризующий шкалу корректировки с ответом "Нет"</summary>
    public static readonly List<int> CorrectionListNo = new List<int>
                                                          {
                                                            30,
                                                            39,
                                                            71,
                                                            89,
                                                            124,
                                                            129,
                                                            134,
                                                            138,
                                                            142,
                                                            148,
                                                            160,
                                                            170,
                                                            171,
                                                            180,
                                                            183,
                                                            217,
                                                            234,
                                                            267,
                                                            272,
                                                            296,
                                                            316,
                                                            322,
                                                            374,
                                                            383,
                                                            397,
                                                            398,
                                                            406,
                                                            461,
                                                            502
                                                          };

    /// <summary>Список вопросов, характеризующий шкалу сверхконтроля с ответом "Да"</summary>
    public static readonly List<int> OvercontrolListYes = new List<int>
                                                            {
                                                              23,
                                                              29,
                                                              43,
                                                              62,
                                                              72,
                                                              108,
                                                              114,
                                                              125,
                                                              161,
                                                              189,
                                                              273
                                                            };

    /// <summary>Список вопросов, характеризующий шкалу сверхконтроля с ответом "Нет"</summary>
    public static readonly List<int> OvercontrolListNo = new List<int>
                                                           {
                                                             2,
                                                             3,
                                                             7,
                                                             9,
                                                             18,
                                                             51,
                                                             55,
                                                             68,
                                                             103,
                                                             140,
                                                             153,
                                                             155,
                                                             163,
                                                             175,
                                                             188,
                                                             190,
                                                             192,
                                                             230,
                                                             243,
                                                             274,
                                                             281
                                                           };

    /// <summary>Список вопросов, характеризующий шкалу пессимистичности с ответом "Да"</summary>
    public static readonly List<int> PessimisticallyListYes = new List<int>
                                                                {
                                                                  5,
                                                                  13,
                                                                  23,
                                                                  32,
                                                                  41,
                                                                  43,
                                                                  52,
                                                                  67,
                                                                  86,
                                                                  104,
                                                                  130,
                                                                  138,
                                                                  142,
                                                                  158,
                                                                  159,
                                                                  189,
                                                                  193,
                                                                  236,
                                                                  259
                                                                };

    /// <summary>Список вопросов, характеризующий шкалу пессимистичности с ответом "Нет"</summary>
    public static readonly List<int> PessimisticallyListNo = new List<int>
                                                               {
                                                                 2,
                                                                 8,
                                                                 9,
                                                                 18,
                                                                 30,
                                                                 36,
                                                                 39,
                                                                 46,
                                                                 51,
                                                                 57,
                                                                 58,
                                                                 64,
                                                                 80,
                                                                 88,
                                                                 89,
                                                                 95,
                                                                 98,
                                                                 107,
                                                                 122,
                                                                 131,
                                                                 145,
                                                                 152,
                                                                 153,
                                                                 154,
                                                                 155,
                                                                 160,
                                                                 178,
                                                                 191,
                                                                 207,
                                                                 208,
                                                                 238,
                                                                 241,
                                                                 242,
                                                                 248,
                                                                 263,
                                                                 270,
                                                                 271,
                                                                 272,
                                                                 285,
                                                                 296
                                                               };

    /// <summary>Список вопросов, характеризующий шкалу эмоциональной лабильности с ответом "Да"</summary>
    public static readonly List<int> EmotionalLabilityListYes = new List<int>
                                                                  {
                                                                    10,
                                                                    23,
                                                                    32,
                                                                    43,
                                                                    44,
                                                                    47,
                                                                    76,
                                                                    114,
                                                                    179,
                                                                    186,
                                                                    189,
                                                                    238
                                                                  };

    /// <summary>Список вопросов, характеризующий шкалу 'эмоциональной лабильности с ответом "Нет"</summary>
    public static readonly List<int> EmotionalLabilityListNo = new List<int>
                                                                 {
                                                                   2,
                                                                   3,
                                                                   6,
                                                                   7,
                                                                   8,
                                                                   9,
                                                                   12,
                                                                   26,
                                                                   30,
                                                                   51,
                                                                   55,
                                                                   71,
                                                                   89,
                                                                   93,
                                                                   103,
                                                                   107,
                                                                   109,
                                                                   124,
                                                                   128,
                                                                   129,
                                                                   136,
                                                                   137,
                                                                   141,
                                                                   147,
                                                                   153,
                                                                   160,
                                                                   162,
                                                                   163,
                                                                   170,
                                                                   172,
                                                                   174,
                                                                   175,
                                                                   180,
                                                                   188,
                                                                   190,
                                                                   192,
                                                                   201,
                                                                   213,
                                                                   230,
                                                                   234,
                                                                   243,
                                                                   265,
                                                                   267,
                                                                   274,
                                                                   279,
                                                                   289,
                                                                   292
                                                                 };

    /// <summary>Список вопросов, характеризующий шкалу импульсивности с ответом "Да"</summary>
    public static readonly List<int> ImpulsivenessListYes = new List<int>
                                                              {
                                                                16,
                                                                21,
                                                                24,
                                                                32,
                                                                35,
                                                                36,
                                                                42,
                                                                61,
                                                                67,
                                                                84,
                                                                94,
                                                                102,
                                                                106,
                                                                110,
                                                                118,
                                                                127,
                                                                215,
                                                                216,
                                                                224,
                                                                239,
                                                                244,
                                                                245,
                                                                284
                                                              };

    /// <summary>Список вопросов, характеризующий шкалу импульсивности с ответом "Нет"</summary>
    public static readonly List<int> ImpulsivenessListNo = new List<int>
                                                             {
                                                               8,
                                                               20,
                                                               37,
                                                               82,
                                                               91,
                                                               96,
                                                               107,
                                                               134,
                                                               137,
                                                               141,
                                                               155,
                                                               170,
                                                               171,
                                                               173,
                                                               180,
                                                               183,
                                                               201,
                                                               231,
                                                               235,
                                                               237,
                                                               248,
                                                               267,
                                                               287,
                                                               289,
                                                               294,
                                                               296
                                                             };

    /// <summary>Список вопросов, характеризующий шкалу женственности с ответом "Нет"</summary>
    public static readonly List<int> FemininityListNo = new List<int>
                                                          {
                                                            1,
                                                            19,
                                                            26,
                                                            79,
                                                            80,
                                                            81,
                                                            89,
                                                            99,
                                                            112,
                                                            115,
                                                            116,
                                                            117,
                                                            120,
                                                            144,
                                                            176,
                                                            179,
                                                            198,
                                                            213,
                                                            219,
                                                            221,
                                                            223,
                                                            229,
                                                            231,
                                                            249,
                                                            254,
                                                            260,
                                                            262,
                                                            264,
                                                            280,
                                                            283,
                                                            297,
                                                            300
                                                          };

    /// <summary>Список вопросов, характеризующий шкалу женственности с ответом "Да"</summary>
    public static readonly List<int> FemininityListYes = new List<int>
                                                           {
                                                             4,
                                                             25,
                                                             70,
                                                             74,
                                                             77,
                                                             78,
                                                             87,
                                                             92,
                                                             126,
                                                             132,
                                                             134,
                                                             140,
                                                             149,
                                                             187,
                                                             203,
                                                             204,
                                                             217,
                                                             226,
                                                             239,
                                                             261,
                                                             278,
                                                             282,
                                                             295,
                                                             299
                                                           };

    /// <summary>Список вопросов, характеризующий шкалу мужественности с ответом "Нет"</summary>
    public static readonly List<int> MasculinityListNo = new List<int>
                                                           {
                                                             1,
                                                             19,
                                                             26,
                                                             28,
                                                             79,
                                                             80,
                                                             81,
                                                             89,
                                                             99,
                                                             112,
                                                             115,
                                                             116,
                                                             117,
                                                             120,
                                                             144,
                                                             176,
                                                             198,
                                                             213,
                                                             214,
                                                             219,
                                                             221,
                                                             223,
                                                             229,
                                                             249,
                                                             254,
                                                             260,
                                                             262,
                                                             264,
                                                             280,
                                                             283,
                                                             300
                                                           };

    /// <summary>Список вопросов, характеризующий шкалу мужественности с ответом "Да"</summary>
    public static readonly List<int> MasculinityListYes = new List<int>
                                                            {
                                                              4,
                                                              25,
                                                              70,
                                                              74,
                                                              77,
                                                              78,
                                                              87,
                                                              92,
                                                              126,
                                                              132,
                                                              134,
                                                              140,
                                                              149,
                                                              179,
                                                              187,
                                                              203,
                                                              204,
                                                              217,
                                                              226,
                                                              231,
                                                              239,
                                                              261,
                                                              278,
                                                              282,
                                                              295,
                                                              297,
                                                              299
                                                            };

    /// <summary>Список вопросов, характеризующий шкалу ригидности с ответом "Нет"</summary>
    public static readonly List<int> RigidityListNo = new List<int>
                                                        {
                                                          93,
                                                          107,
                                                          109,
                                                          111,
                                                          117,
                                                          124,
                                                          268,
                                                          281,
                                                          294,
                                                          313,
                                                          316,
                                                          319,
                                                          327,
                                                          347,
                                                          348
                                                        };

    /// <summary>Список вопросов, характеризующий шкалу ригидности с ответом "Да"</summary>
    public static readonly List<int> RigidityListYes = new List<int>
                                                         {
                                                           15,
                                                           16,
                                                           22,
                                                           24,
                                                           27,
                                                           35,
                                                           110,
                                                           127,
                                                           157,
                                                           158,
                                                           202,
                                                           284,
                                                           291,
                                                           299,
                                                           305,
                                                           317,
                                                           338,
                                                           341,
                                                           364,
                                                           365
                                                         };

    /// <summary>Список вопросов, характеризующий шкалу тревожности с ответом "Нет"</summary>
    public static readonly List<int> AnxietyListNo = new List<int> { 3, 8, 36, 122, 152, 164, 178, 329, 353 };

    /// <summary>Список вопросов, характеризующий шкалу тревожности с ответом "Да"</summary>
    public static readonly List<int> AnxietyListYes = new List<int>
                                                        {
                                                          10,
                                                          15,
                                                          22,
                                                          32,
                                                          41,
                                                          67,
                                                          76,
                                                          86,
                                                          94,
                                                          102,
                                                          106,
                                                          142,
                                                          159,
                                                          189,
                                                          217,
                                                          238,
                                                          301,
                                                          304,
                                                          305,
                                                          317,
                                                          321,
                                                          336,
                                                          337,
                                                          340,
                                                          342,
                                                          343,
                                                          344,
                                                          346,
                                                          351,
                                                          352,
                                                          356,
                                                          357,
                                                          359,
                                                          360,
                                                          361
                                                        };

    /// <summary>Список вопросов, характеризующий шкалу индивидуалистичности с ответом "Нет"</summary>
    public static readonly List<int> IndividualisticListNo = new List<int>
                                                               {
                                                                 8,
                                                                 17,
                                                                 20,
                                                                 37,
                                                                 65,
                                                                 103,
                                                                 119,
                                                                 177,
                                                                 178,
                                                                 187,
                                                                 192,
                                                                 196,
                                                                 220,
                                                                 276,
                                                                 281,
                                                                 306,
                                                                 309,
                                                                 322,
                                                                 330
                                                               };

    /// <summary>Список вопросов, характеризующий шкалу индивидуалистичности с ответом "Да"</summary>
    public static readonly List<int> IndividualisticListYes = new List<int>
                                                                {
                                                                  15,
                                                                  16,
                                                                  21,
                                                                  22,
                                                                  24,
                                                                  32,
                                                                  35,
                                                                  38,
                                                                  40,
                                                                  41,
                                                                  47,
                                                                  52,
                                                                  76,
                                                                  97,
                                                                  104,
                                                                  156,
                                                                  157,
                                                                  159,
                                                                  179,
                                                                  194,
                                                                  202,
                                                                  210,
                                                                  212,
                                                                  238,
                                                                  241,
                                                                  251,
                                                                  259,
                                                                  273,
                                                                  282,
                                                                  291,
                                                                  297,
                                                                  301,
                                                                  303,
                                                                  305,
                                                                  307,
                                                                  312,
                                                                  320,
                                                                  324,
                                                                  325,
                                                                  332,
                                                                  335,
                                                                  339,
                                                                  341,
                                                                  345,
                                                                  352,
                                                                  354,
                                                                  355,
                                                                  356,
                                                                  360,
                                                                  363,
                                                                  364
                                                                };

    /// <summary>Список вопросов, характеризующий шкалу оптисистичности с ответом "Нет"</summary>
    public static readonly List<int> OptimisticListNo = new List<int>
                                                          {
                                                            101,
                                                            105,
                                                            111,
                                                            119,
                                                            130,
                                                            148,
                                                            166,
                                                            171,
                                                            180,
                                                            267,
                                                            289
                                                          };

    /// <summary>Список вопросов, характеризующий шкалу оптимистичности с ответом "Да"</summary>
    public static readonly List<int> OptimisticListYes = new List<int>
                                                           {
                                                             11,
                                                             13,
                                                             21,
                                                             22,
                                                             59,
                                                             64,
                                                             73,
                                                             97,
                                                             100,
                                                             109,
                                                             127,
                                                             134,
                                                             145,
                                                             156,
                                                             157,
                                                             167,
                                                             181,
                                                             194,
                                                             212,
                                                             222,
                                                             226,
                                                             228,
                                                             232,
                                                             233,
                                                             238,
                                                             240,
                                                             250,
                                                             251,
                                                             263,
                                                             268,
                                                             271,
                                                             277,
                                                             279,
                                                             298
                                                           };

    /// <summary>Список вопросов, характеризующий шкалу интроверсии с ответом "Нет"</summary>
    public static readonly List<int> IntroversionListNo = new List<int>
                                                        {
                                                          25,
                                                          57,
                                                          91,
                                                          99,
                                                          119,
                                                          126,
                                                          143,
                                                          193,
                                                          208,
                                                          229,
                                                          231,
                                                          254,
                                                          262,
                                                          281,
                                                          296,
                                                          309,
                                                          353,
                                                          359,
                                                          371,
                                                          391,
                                                          400,
                                                          415,
                                                          440,
                                                          446,
                                                          449,
                                                          450,
                                                          451,
                                                          469,
                                                          479,
                                                          481,
                                                          482,
                                                          501,
                                                          521,
                                                          547
                                                        };

    /// <summary>Список вопросов, характеризующий шкалу интроверсии с ответом "Да"</summary>
    public static readonly List<int> IntroversionListYes = new List<int>
                                                         {
                                                           32,
                                                           67,
                                                           82,
                                                           111,
                                                           117,
                                                           124,
                                                           138,
                                                           147,
                                                           171,
                                                           172,
                                                           180,
                                                           201,
                                                           236,
                                                           267,
                                                           278,
                                                           292,
                                                           304,
                                                           316,
                                                           321,
                                                           332,
                                                           336,
                                                           342,
                                                           357,
                                                           377,
                                                           383,
                                                           398,
                                                           401,
                                                           427,
                                                           436,
                                                           455,
                                                           473,
                                                           467,
                                                           549,
                                                           564
                                                         };

    public static readonly List<Tuple<ScaleType,List<int>,List<int>>> Scales = new List<Tuple<ScaleType, List<int>,List<int>>>
                                                                        {
                                                                          new Tuple<ScaleType, List<int>,List<int>>(ScaleType.Lie,LieList,null),
                                                                          new Tuple<ScaleType, List<int>,List<int>>(ScaleType.True,TrueListYes,TrueListNo),
                                                                          new Tuple<ScaleType, List<int>,List<int>>(ScaleType.Correction,CorrectionListYes,CorrectionListNo),
                                                                          new Tuple<ScaleType, List<int>,List<int>>(ScaleType.Overcontrol,OvercontrolListYes,OvercontrolListNo),
                                                                          new Tuple<ScaleType, List<int>,List<int>>(ScaleType.Pessimistically,PessimisticallyListYes,PessimisticallyListNo),
                                                                          new Tuple<ScaleType, List<int>,List<int>>(ScaleType.EmotionalLability,EmotionalLabilityListYes,EmotionalLabilityListNo),
                                                                          new Tuple<ScaleType, List<int>,List<int>>(ScaleType.Impulsiveness,ImpulsivenessListYes,ImpulsivenessListNo),
                                                                          new Tuple<ScaleType, List<int>,List<int>>(ScaleType.Femininity,FemininityListYes,FemininityListNo),
                                                                          new Tuple<ScaleType, List<int>,List<int>>(ScaleType.Masculinity,MasculinityListYes,MasculinityListNo),
                                                                          new Tuple<ScaleType, List<int>,List<int>>(ScaleType.Rigidity,RigidityListYes,RigidityListNo),
                                                                          new Tuple<ScaleType, List<int>,List<int>>(ScaleType.Anxiety,AnxietyListYes,AnxietyListNo),
                                                                          new Tuple<ScaleType, List<int>,List<int>>(ScaleType.Individualistic,IndividualisticListYes,IndividualisticListNo),
                                                                          new Tuple<ScaleType, List<int>,List<int>>(ScaleType.Optimistic,OptimisticListYes,OptimisticListNo),
                                                                          new Tuple<ScaleType, List<int>,List<int>>(ScaleType.Introversion,IntroversionListYes,IntroversionListNo),
                                                                        }; 
    #endregion

    /// <summary>Ошибка с расположением файла</summary>
    public const string FILE_NOT_FOUND = "Файл с вопросами {0} не найден. Продолжение невозможно";

    /// <summary>Формат отображения вопроса</summary>
    public const string QUESTION_FORMAT = "{0}. {1}";

    /// <summary>Наименование мужского пола</summary>
    public const string GENDER_MALE = "Мужской";

    /// <summary>Наименование женского пола</summary>
    public const string GENDER_FEMALE = "Женский";

    /// <summary>Начало отсчёта для графика</summary>
    public static readonly Point CenterPoint = new Point(50,50);
  }
}

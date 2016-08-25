using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading;

namespace MMPI
{
   /// <summary> Объект, сигнализирующий об изменении значений свойств.  </summary>
  public class ObservableObject : INotifyPropertyChanging, INotifyPropertyChanged
  {
    #region События

    /// <summary> Сигнализирует об изменении свойства объекта. </summary>
    public event PropertyChangedEventHandler PropertyChanged;

    /// <summary> Сигнализирует об начале изменения свойства объекта. </summary>
    public event PropertyChangingEventHandler PropertyChanging;

    #endregion

    #region Методы

    /// <summary> Возвращает наименование свойства из выражения. </summary>
    /// <typeparam name="T"> Тип свойства. </typeparam>
    /// <param name="expr"> Выражение содержащее свойство. </param>
    /// <returns> Наименование свойства. </returns>
    /// <exception cref="System.ArgumentException">
    /// Значение <paramref name="expr"/> должно быть lambda выражением. <br/> Тело выражения должно быть ссылкой на свойство класса.
    /// </exception>
    protected static string GetPropertyName<T>(Expression<Func<T>> expr)
    {
      if (expr.NodeType != ExpressionType.Lambda)
        throw new ArgumentException("Значение должно быть lambda выражением", paramName: "expr");

      if (!(expr.Body is MemberExpression))
        throw new ArgumentException("Тело выражения должно быть ссылкой на свойство класса", paramName: "expr");

      var body = (MemberExpression)expr.Body;
      return body.Member.Name;
    }

    /// <summary> Вызывает событие изменения свойства. </summary>
    /// <param name="propertyName"> Наименование изменившегося свойства. </param>
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
      var temp = Interlocked.CompareExchange(ref PropertyChanged, null, null);
      if (temp == null)
        return;
      temp(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary> Вызывает событие начала изменения свойства. </summary>
    /// <param name="propertyName"> Наименование изменяющегося свойства. </param>
    protected virtual void OnPropertyChanging([CallerMemberName] string propertyName = "")
    {
      var temp = Interlocked.CompareExchange(ref PropertyChanging, null, null);
      if (temp == null)
        return;
      temp(this, new PropertyChangingEventArgs(propertyName));
    }

    /// <summary> Назначает новое значение полю. </summary>
    /// <typeparam name="T"> Тип изменяемого поля. </typeparam>
    /// <param name="field"> Ссылка на изменяемое поле. </param>
    /// <param name="value"> Новое значение поля. </param>
    /// <param name="propertyName"> Наименование свойства изменяющего поле. </param>
    /// <returns> true - если значение изменено, иначе - false. </returns>
    /// <example>
    /// <code lang="C#">
    /// <![CDATA[
    /// using System;
    /// using Axavo;
    ///
    /// namespace Axavo.Examples
    /// {
    ///   public class SetFieldExample : ObservableModel
    ///   {
    ///     private string _Name;
    ///     public string Name
    ///     {
    ///       get { return _Name; }
    ///       set { SetFiled(ref _Name, value, "Name"); }
    ///     }
    ///   }
    /// }
    ///
    /// ]]>
    /// </code>
    /// </example>
    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
    {
      if (EqualityComparer<T>.Default.Equals(field, value))
        return false;

// ReSharper disable ExplicitCallerInfoArgument
      OnPropertyChanging(propertyName);
      field = value;
      OnPropertyChanged(propertyName);
// ReSharper restore ExplicitCallerInfoArgument

      return true;
    }

    /// <summary> Назначает новое значение полю. </summary>
    /// <typeparam name="T"> Тип изменяемого поля. </typeparam>
    /// <param name="field"> Ссылка на изменяемое поле. </param>
    /// <param name="value"> Новое значение поля. </param>
    /// <param name="expr"> Выражение содержащее изменяемое свойство. </param>
    /// <returns> true - если значение изменено, иначе - false. </returns>
    /// <example>
    /// <code lang="C#">
    /// <![CDATA[
    /// using System;
    /// using Axavo;
    ///
    /// namespace Axavo.Examples
    /// {
    ///   public class SetFieldExample : ObservableModel
    ///   {
    ///     private string _Name;
    ///     public string Name
    ///     {
    ///       get { return _Name; }
    ///       set { SetFiled(ref _Name, value, () => Name); }
    ///     }
    ///   }
    /// }
    ///
    /// ]]>
    /// </code>
    /// </example>
    protected bool SetField<T>(ref T field, T value, Expression<Func<T>> expr)
    {
      if (EqualityComparer<T>.Default.Equals(field, value))
        return false;

      var propertyName = GetPropertyName(expr);

// ReSharper disable ExplicitCallerInfoArgument
      OnPropertyChanging(propertyName);
      field = value;
      OnPropertyChanged(propertyName);
// ReSharper restore ExplicitCallerInfoArgument

      return true;
    }

    /// <summary> Назначает новое значение полю используя делегат. </summary>
    /// <typeparam name="T"> Тип изменяемого поля. </typeparam>
    /// <param name="setter"> Делегат, изменяющий значение поля. </param>
    /// <param name="field"> Значение изменяемого поля. </param>
    /// <param name="value"> Новое значение поля. </param>
    /// <param name="propertyName"> Наименование свойства изменяющего поле. </param>
    /// <returns> true - если значение изменено, иначе - false. </returns>
    /// <example>
    /// <code lang="C#">
    /// <![CDATA[
    /// using System;
    /// using Axavo;
    ///
    /// namespace Axavo.Examples
    /// {
    ///   public class FieldStorage
    ///   {
    ///     public string Name;
    ///   }
    ///
    ///   public class SetFieldExample : ObservableModel
    ///   {
    ///     private FieldStorage _Storage;
    ///     public string Name
    ///     {
    ///       get { return _Storage.Name; }
    ///       set { SetFiled((v)=> _Storage.Name = v, _Storage.Name, value, "Name"); }
    ///     }
    ///   }
    /// }
    ///
    /// ]]>
    /// </code>
    /// </example>
    protected bool SetField<T>(Action<T> setter, T field, T value, [CallerMemberName] string propertyName = "")
    {
      if (EqualityComparer<T>.Default.Equals(field, value))
        return false;

// ReSharper disable ExplicitCallerInfoArgument
      OnPropertyChanging(propertyName);
      setter(value);
      OnPropertyChanged(propertyName);
// ReSharper restore ExplicitCallerInfoArgument

      return true;
    }

    /// <summary> Назначает новое значение полю используя делегат. </summary>
    /// <typeparam name="T"> Тип изменяемого поля. </typeparam>
    /// <param name="setter"> Делегат, изменяющий значение поля. </param>
    /// <param name="field"> Значение изменяемого поля. </param>
    /// <param name="value"> Новое значение поля. </param>
    /// <param name="expr"> Выражение содержащее изменяемое свойство. </param>
    /// <returns> true - если значение изменено, иначе - false. </returns>
    /// <example>
    /// <code lang="C#">
    /// <![CDATA[
    /// using System;
    /// using Axavo;
    ///
    /// namespace Axavo.Examples
    /// {
    ///   public class FieldStorage
    ///   {
    ///     public string Name;
    ///   }
    ///
    ///   public class SetFieldExample : ObservableModel
    ///   {
    ///     private FieldStorage _Storage;
    ///     public string Name
    ///     {
    ///       get { return _Storage.Name; }
    ///       set { SetFiled((v)=> _Storage.Name = v, _Storage.Name, value, () => Name); }
    ///     }
    ///   }
    /// }
    ///
    /// ]]>
    /// </code>
    /// </example>
    protected bool SetField<T>(Action<T> setter, T field, T value, Expression<Func<T>> expr)
    {
      if (EqualityComparer<T>.Default.Equals(field, value))
        return false;

      var propertyName = GetPropertyName(expr);

// ReSharper disable ExplicitCallerInfoArgument
      OnPropertyChanging(propertyName);
      setter(value);
      OnPropertyChanged(propertyName);
// ReSharper restore ExplicitCallerInfoArgument

      return true;
    }

    #endregion
  }
}

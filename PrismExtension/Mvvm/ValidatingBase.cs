/*
 * Auteur : Antoine Mailhot
 * Date de création : 13 novembre 2018
*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Prism.Mvvm
{
  public abstract class ValidatingBase : BindableBase, INotifyDataErrorInfo
  {
    public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

    public bool HasErrors => Errors.HasErrors;
    public IEnumerable GetErrors(string property) => Errors.GetErrors(property);

    public ValidatingBase()
    {
      Context = new ValidationContext(this);
      Errors = new ErrorsContainer<ValidationResult>(RaiseErrorsChanged);
    }

    protected override bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string property = null)
    {
      if (EqualityComparer<T>.Default.Equals(storage, value))
        return false;

      if (ValidateForProperty(value, property))
        return base.SetProperty(ref storage, value, property);

      storage = value;
      return true;
    }

    protected override bool SetProperty<T>(ref T storage, T value, Action onChanged, [CallerMemberName] string property = null)
    {
      if (EqualityComparer<T>.Default.Equals(storage, value))
        return false;

      if (ValidateForProperty(value, property))
        return base.SetProperty(ref storage, value, onChanged, property);

      storage = value;
      return true;
    }

    private bool ValidateForProperty<T>(T value, [CallerMemberName] string name = null) => ValidateForProperty(value, v => { }, name);

    private bool ValidateForProperty<T>(T value, Action<T> validate, [CallerMemberName] string name = null)
    {
      bool error;
      var iserror = Errors.GetErrors(name).Any();
      var infos = GetType().GetProperty(name).GetCustomAttributes(true).OfType<ValidationAttribute>();

      Errors.ClearErrors(name);
      validate?.Invoke(value);
      Errors.SetErrors(name, Errors.GetErrors(name).Union(from validation in infos
                                                          let result = validation.GetValidationResult(value, Context)
                                                          where result != ValidationResult.Success
                                                          select result));

      if ((error = Errors.GetErrors(name).Any()) != iserror)
        RaiseErrorsChanged(name);

      return !error;
    }

    private async Task<bool> ValidateForProperty<T>(T value, Func<T, Task> validate, [CallerMemberName] string name = null)
    {
      bool error;
      var iserror = Errors.GetErrors(name).Any();
      var infos = GetType().GetProperty(name).GetCustomAttributes(true).OfType<ValidationAttribute>();

      Errors.ClearErrors(name);
      await validate?.Invoke(value);
      Errors.SetErrors(name, Errors.GetErrors(name).Union(from validation in infos
                                                          let result = validation.GetValidationResult(value, Context)
                                                          where result != ValidationResult.Success
                                                          select result));

      if ((error = Errors.GetErrors(name).Any()) != iserror)
        RaiseErrorsChanged(name);

      return !error;
    }

    protected bool ValidateProperty<T>([CallerMemberName] string name = null) => ValidateProperty<T>(v => { }, name);

    protected bool ValidateProperty<T>(Action<T> validate, [CallerMemberName] string name = null) =>
      ValidateForProperty((T)GetType().GetProperty(name).GetMethod.Invoke(this, new object[] { }), validate, name);

    protected async Task<bool> ValidateProperty<T>(Func<T, Task> validate, [CallerMemberName] string name = null) =>
      await ValidateForProperty((T)GetType().GetProperty(name).GetMethod.Invoke(this, new object[] { }), validate, name);

    protected async Task<bool> Validate()
    {
      Errors.ClearErrors();
      await ValidateAsync();
      return !HasErrors;
    }

    protected virtual async Task ValidateAsync() { }

    protected virtual void RaiseErrorsChanged([CallerMemberName] string name = null) =>
      ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(name));

    protected ErrorsContainer<ValidationResult> Errors { get; }
    private ValidationContext Context { get; }
  }
}

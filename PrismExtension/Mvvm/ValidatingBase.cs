using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Prism.Mvvm
{
  public abstract class ValidatingBase : BindableBase, INotifyDataErrorInfo
  {
    public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

    public bool HasErrors => Errors.Count > 0;
    public IEnumerable GetErrors(string property) => Errors[property];

    public ValidatingBase()
    {
      Context = new ValidationContext(this);
    }

    protected bool ValidateProperty<T>(T value, [CallerMemberName] string name = null)
    {
      bool error;
      var iserror = Errors.ContainsKey(name) && Errors[name].Count > 0;
      var infos = GetType().GetProperty(name).GetCustomAttributes(true).OfType<ValidationAttribute>();

      Errors[name] = (from validation in infos
                      where !validation.IsValid(value)
                      select validation.FormatErrorMessage(name)).ToArray();

      if ((error = Errors[name].Count > 0) != iserror)
        RaiseErrorsChanged(name);

      return !error;
    }

    protected bool Validate()
    {
      var results = new List<ValidationResult>();

      Errors.Clear();

      if (!Validator.TryValidateObject(this, Context, results, true))
        foreach (var result in results)
          foreach (var property in result.MemberNames)
            if (Errors.ContainsKey(property))
              Errors[property].Add(result.ErrorMessage);
            else
              Errors[property] = new List<string> { result.ErrorMessage };

      RaiseAllErrorsChanged();

      return !(Errors.Count > 0 && (from pair in Errors
                                    where (pair.Value?.Count ?? 0) > 0
                                    select pair.Value).Any());
    }

    protected abstract void RaiseAllErrorsChanged();
    protected virtual void RaiseErrorsChanged([CallerMemberName] string name = null) =>
      ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(name));

    private IDictionary<string, ICollection<string>> Errors { get; } = new Dictionary<string, ICollection<string>>();
    private readonly ValidationContext Context;
  }
}

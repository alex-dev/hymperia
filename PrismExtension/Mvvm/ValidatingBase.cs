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
using MoreLinq;

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

      if (ValidateProperty(value, property))
        return base.SetProperty(ref storage, value, property);

      storage = value;
      return true;
    }

    protected override bool SetProperty<T>(ref T storage, T value, Action onChanged, [CallerMemberName] string property = null)
    {
      if (EqualityComparer<T>.Default.Equals(storage, value))
        return false;

      if (ValidateProperty(value, property))
        return base.SetProperty(ref storage, value, onChanged, property);

      storage = value;
      return true;
    }

    protected bool ValidateProperty<T>(T value, [CallerMemberName] string name = null)
    {
      bool error;
      var iserror = Errors.GetErrors(name).Any();
      var infos = GetType().GetProperty(name).GetCustomAttributes(true).OfType<ValidationAttribute>();

      Errors.SetErrors(name, from validation in infos
                             let result = validation.GetValidationResult(value, Context)
                             where result != ValidationResult.Success
                             select result);

      if ((error = Errors.GetErrors(name).Any()) != iserror)
        RaiseErrorsChanged(name);

      return !error;
    }

    protected bool Validate()
    {
      Errors.ClearErrors();
      InternalValidate();
      RaiseAllErrorsChanged();

      return !HasErrors;
    }

#pragma warning disable 1998
    protected virtual async Task<bool> ValidateAsync() => Validate();
#pragma warning restore 1998


    protected virtual void InternalValidate()
    {
      var results = new List<ValidationResult>();

      if (!Validator.TryValidateObject(this, Context, results, true))
      {
        var r = (from result in results
                 from property in result.MemberNames
                 group result by property into pair
                 select new { Property = pair.Key, Results = pair.AsEnumerable() });
        r.ForEach(pair => Errors.SetErrors(pair.Property, pair.Results));

        //(from result in results
        // from property in result.MemberNames
        // group result by property into pair
        // select new { Property = pair.Key, Results = pair.AsEnumerable() })
        //  .ForEach(pair => Errors.SetErrors(pair.Property, pair.Results));
      }
    }

    protected abstract void RaiseAllErrorsChanged();
    protected virtual void RaiseErrorsChanged([CallerMemberName] string name = null) =>
      ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(name));

    protected ErrorsContainer<ValidationResult> Errors { get; }
    private ValidationContext Context { get; }
  }
}

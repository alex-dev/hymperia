using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;

namespace Hymperia.Model.Modeles
{
  public class User
  {
    #region Attributes

    [Key]
    public int Id { get; private set; }

    [NotNull]
    [MaxLength(25)]
    public string Name { get; private set; }

    #endregion

    #region ToString

    [Pure]
    [NotNull]
    public override string ToString()
    {
      return $"{ Id } - { Name }";
    }

    #endregion
  }
}

using System.ComponentModel.DataAnnotations;

namespace Hymperia.Model
{
  public class User
  {
    #region Attributes

    [Key]
    public int Id { get; private set; }

    [MaxLength(25)]
    public string Name { get; private set; }

    #endregion

    #region ToString

    public override string ToString()
    {
      return $"{ Id } - { Name }";
    }

    #endregion
  }
}

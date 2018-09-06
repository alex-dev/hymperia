using System.ComponentModel.DataAnnotations;

namespace Hymperia.Model
{
  public class User
  {
    #region Attributes

    [Key]
    public int Id { get; set; }

    [MaxLength(25)]
    public string Name { get; set; }

    #endregion
  }
}

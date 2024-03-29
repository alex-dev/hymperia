﻿namespace Hymperia.Model.Identity
{
  /// <summary>Possède un clé unique de type <see cref="int"/>.</summary>
  public interface IIdentity
  {
    int Id { get; }
    string ToString();
  }
}

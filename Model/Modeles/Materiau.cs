﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using System.Windows.Media;
using Hymperia.Model.Identity;
using Hymperia.Model.Localization;
using Hymperia.Model.Properties;
using JetBrains.Annotations;

namespace Hymperia.Model.Modeles
{
  public class Materiau : IIdentity, IEquatable<Materiau>
  {
    #region Properties

    /// <summary>La clé primaire du matériau.</summary>
    public int Id { get; private set; }

    /// <summary>Le nom du matériau.</summary>
    /// <remarks>Alternate Key</remarks>
    [NotNull]
    [Required]
    [MinLength(1, ErrorMessage = "Le nom du matériau ne peut pas être vide.")]
    internal string _Nom { get; private set; }

    /// <summary>Le prix du matériaux par volume.</summary>
    public double Prix { get; private set; }

    protected internal byte R { get; private set; }
    protected internal byte G { get; private set; }
    protected internal byte B { get; private set; }
    protected internal byte A { get; private set; }

    #endregion

    #region Not Mapped Properties

    /// <summary>Le nom localisé du matériau.</summary>
    [NotNull]
    [NotMapped]
    public string Nom
    {
      get
      {
        var task = GetNom();
        task.Wait();
        return task.Result;
      }
    }

    [CanBeNull]
    [NotMapped]
    private SolidColorBrush CachedBrush { get; set; }

    [NotNull]
    [NotMapped]
    protected Color ColorData
    {
      get => Color.FromArgb(A, R, G, B);
      set
      {
        R = value.R;
        G = value.G;
        B = value.B;
        A = value.A;
      }
    }

    /// <summary>La <see cref="SolidBrush"/> utilisée pour représenter ce <see cref="Materiau"/>.</summary>
    [NotNull]
    [NotMapped]
    public SolidColorBrush Fill
    {
      get => CachedBrush ?? (CachedBrush = new SolidColorBrush(ColorData));
      private set
      {
        CachedBrush = value;
        ColorData = value.Color;
      }
    }

    #endregion

    #region Constructors

    /// <param name="color">Le nom de la couleur.</param>
    /// <param name="nom">Le nom du matériau.</param>
    public Materiau([NotNull] string nom, byte r, byte g, byte b, byte a)
    {
      _Nom = nom;
      R = r;
      G = g;
      B = b;
      A = a;
    }

    /// <param name="fill">La <see cref="SolidBrush"/> utilisée pour représenter ce <see cref="Materiau"/>.</param>
    /// <param name="nom">Le nom du matériau.</param>
    public Materiau([NotNull] SolidColorBrush fill, [NotNull] string nom)
    {
      Fill = fill;
      _Nom = nom;
    }

    #endregion

    #region Localization

    private LocalizedMateriau Localized;

    private async Task<string> GetNom() => (await GetLocalization())?.Nom ?? _Nom;

    private async Task<LocalizedMateriau> GetLocalization() =>
      Localized ?? (Localized = await Resources.GetMateriau(_Nom));

    #endregion

    #region ToString

    [Pure]
    [NotNull]
    public override string ToString() => Resources.MateriauToString(Id, Nom, Prix);

    #endregion

    #region IEquatable<Materiau>

    [Pure]
    public override bool Equals(object obj) => Equals(obj as Materiau);
    [Pure]
    public bool Equals(Materiau other) => IdentityEqualityComparer<Materiau>.StaticEquals(this, other) && _Nom == other._Nom;
    [Pure]
    public override int GetHashCode() => IdentityEqualityComparer<Materiau>.StaticGetHashCode(this);

    #endregion
  }
}

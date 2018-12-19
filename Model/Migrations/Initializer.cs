using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hymperia.Model.Modeles;
using Hymperia.Model.Modeles.JsonObject;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Hymperia.Model.Migrations
{
  /// <summary>Initialiseur de la base de données.</summary>
  internal sealed class Initializer
  {
    [NotNull]
    private readonly Random Random = new Random();

    [NotNull]
    [ItemNotNull]
    private Func<Materiau[], Forme>[] FormeCreators
    {
      get => new Func<Materiau[], Forme>[]
      {
        materiaux => new PrismeRectangulaire(
          materiaux[Random.Next(materiaux.Length)],
          new Point(Random.Next(-100, 100), Random.Next(-100, 100), Random.Next(-100, 100)),
          new Quaternion(Random.NextDouble(), Random.NextDouble(), Random.NextDouble(), Random.NextDouble()))
        {
          Hauteur = Random.Next(1, 15),
          Largeur = Random.Next(1, 15),
          Longueur = Random.Next(1, 15)
        },
        materiaux => new Ellipsoide(
          materiaux[Random.Next(materiaux.Length)],
          new Point(Random.Next(-100, 100), Random.Next(-100, 100), Random.Next(-100, 100)),
          new Quaternion(Random.NextDouble(), Random.NextDouble(), Random.NextDouble(), Random.NextDouble()))
        {
          RayonX = Random.Next(1, 15),
          RayonY = Random.Next(1, 15),
          RayonZ = Random.Next(1, 15)
        },
        materiaux => new Cylindre(
          materiaux[Random.Next(materiaux.Length)],
          new Point(Random.Next(-100, 100), Random.Next(-100, 100), Random.Next(-100, 100)),
          new Quaternion(Random.NextDouble(), Random.NextDouble(), Random.NextDouble(), Random.NextDouble()))
        {
          Hauteur = Random.Next(1, 15),
          Diametre = Random.Next(1, 15)
        },
        materiaux => new Cone(
          materiaux[Random.Next(materiaux.Length)],
          new Point(Random.Next(-100, 100), Random.Next(-100, 100), Random.Next(-100, 100)),
          new Quaternion(Random.Next(100), Random.Next(100), Random.Next(100), Random.NextDouble()))
        {
          Hauteur = Random.Next(1, 15),
          RayonBase = Random.Next(1, 15)
        }
      };
    }

    /// <summary>Initialise la base de données aléatoirement.</summary>
    /// <param name="context">Le <see cref="DatabaseContext"/> de la base de données à initialiser.</param>
    /// <param name="token">Un token d'annulation.</param>
    public async Task Initialize([NotNull] DatabaseContext context, [NotNull] CancellationToken token = default)
    {
      context.Utilisateurs.AddRange(InitializeUtilisateurs());
      await context.SaveChangesAsync(token).ConfigureAwait(false);

      context.Projets.AddRange(
        InitializeProjets(await context.Materiaux.ToArrayAsync(token).ConfigureAwait(false)));

      await context.SaveChangesAsync(token);

      InitializeAcces(
        await context.Utilisateurs.ToArrayAsync(token).ConfigureAwait(false),
        await context.Projets.ToArrayAsync(token).ConfigureAwait(false))
        .ToArray();
      await context.SaveChangesAsync(token).ConfigureAwait(false);
    }

    [NotNull]
    [ItemNotNull]
    private IEnumerable<Acces> InitializeAcces([NotNull][ItemNotNull] Utilisateur[] utilisateurs, [NotNull][ItemNotNull] Projet[] projets)
    {
      IEnumerable<Acces> Generate()
      {
        yield return new Acces(projets[0], utilisateurs[0], Acces.Droit.Lecture);
        yield return new Acces(projets[0], utilisateurs[1], Acces.Droit.Possession);
        yield return new Acces(projets[1], utilisateurs[1], Acces.Droit.LectureEcriture);
        yield return new Acces(projets[1], utilisateurs[2], Acces.Droit.Possession);
      }

      foreach (var acces in Generate())
      {
        acces.Utilisateur._Acces.Add(acces);
        yield return acces;
      }
    }

    [NotNull]
    private Projet InitializeFormes([NotNull][ItemNotNull] Materiau[] materiaux, [NotNull] Projet projet)
    {
      projet._Formes.AddRange(InitializeFormes(materiaux));
      return projet;
    }

    [NotNull]
    [ItemNotNull]
    private IEnumerable<Forme> InitializeFormes([NotNull][ItemNotNull] Materiau[] materiaux)
    {
      var creators = FormeCreators;
      int count = Random.Next(50, 250);

      for (int i = 0; i < count; ++i)
        yield return creators[Random.Next(creators.Length)](materiaux);
    }

    [NotNull]
    [ItemNotNull]
    private IEnumerable<Projet> InitializeProjets([NotNull][ItemNotNull] Materiau[] materiaux)
    {
      yield return InitializeFormes(materiaux, new Projet("Projet 1"));
      yield return InitializeFormes(materiaux, new Projet("Projet 2"));
    }

    [NotNull]
    [ItemNotNull]
    private IEnumerable<Utilisateur> InitializeUtilisateurs()
    {
      string password = BCrypt.Net.BCrypt.HashPassword("123", Utilisateur.PasswordWorkFactor, true);

      yield return new Utilisateur("Alexandre", password) { Langue = "en-US", Theme = "Dark" };
      yield return new Utilisateur("Guillaume", password) { Theme = "Dark" };
      yield return new Utilisateur("Antoine", password);
    }
  }
}

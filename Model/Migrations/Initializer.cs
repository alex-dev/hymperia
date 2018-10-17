using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hymperia.Model.Modeles;
using Hymperia.Model.Modeles.JsonObject;
using Microsoft.EntityFrameworkCore;

namespace Hymperia.Model.Migrations
{
  /// <summary>Initialiseur de la base de données.</summary>
  internal sealed class Initializer
  {
    private Random Random { get; set; }

    private Func<Materiau[], Forme>[] FormeCreators
    {
      get => new Func<Materiau[], Forme>[]
      {
        materiaux => new PrismeRectangulaire(
          materiaux[Random.Next(materiaux.Length)],
          new Point(Random.Next(-100, 100), Random.Next(-100, 100), Random.Next(-100, 100)),
          new Quaternion(Random.Next(100), Random.Next(100), Random.Next(100), Random.NextDouble()))
        {
          Hauteur = Random.Next(1, 15),
          Largeur = Random.Next(1, 15),
          Longueur = Random.Next(1, 15)
        },
        materiaux => new Ellipsoide(
          materiaux[Random.Next(materiaux.Length)],
          new Point(Random.Next(-100, 100), Random.Next(-100, 100), Random.Next(-100, 100)),
          new Quaternion(Random.Next(100), Random.Next(100), Random.Next(100), Random.NextDouble()))
        {
          RayonX = Random.Next(1, 15),
          RayonY = Random.Next(1, 15),
          RayonZ = Random.Next(1, 15)
        },
        materiaux => new Cylindre(
          materiaux[Random.Next(materiaux.Length)],
          new Point(Random.Next(-100, 100), Random.Next(-100, 100), Random.Next(-100, 100)),
          new Quaternion(Random.Next(100), Random.Next(100), Random.Next(100), Random.NextDouble()))
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

    public Initializer()
    {
      Random = new Random();
    }

    /// <summary>Initialise la base de données aléatoirement.</summary>
    /// <param name="context">Le <see cref="DatabaseContext"/> de la base de données à initialiser.</param>
    /// <param name="token">Un token d'annulation.</param>
    public async Task Initialize(DatabaseContext context, CancellationToken token = default)
    {
      await context.Utilisateurs.AddRangeAsync(InitializeUtilisateurs(), token);
      await context.SaveChangesAsync(token);

      await context.Projets.AddRangeAsync(InitializeProjets(await context.Materiaux.ToArrayAsync(token)), token);
      await context.SaveChangesAsync(token);

      InitializeAcces(await context.Utilisateurs.ToArrayAsync(token), await context.Projets.ToArrayAsync(token));
      await context.SaveChangesAsync(token);
    }

    private Acces[] InitializeAcces(Utilisateur[] utilisateurs, Projet[] projets)
    {
      var array = new Acces.Droit?[] { null, Acces.Droit.Lecture, Acces.Droit.LectureEcriture };
      var acces = new List<Acces> { };

      for (int i = 0; i < projets.Length; ++i)
      {
        for (int j = 0; j < utilisateurs.Length; ++j)
        {
          var droit = i == j ? Acces.Droit.Possession : array[Random.Next(array.Length)];

          if (droit is Acces.Droit _droit)
          {
            var _acces = new Acces(projets[i], utilisateurs[j], _droit);
            utilisateurs[j]._Acces.Add(_acces);
            acces.Add(_acces);
          }
        }
      }

      return acces.ToArray();
    }

    private IEnumerable<Forme> InitializeFormes(Materiau[] materiaux)
    {
      var creators = FormeCreators;
      int count = Random.Next(50, 250);

      for (int i = 0; i < count; ++i)
      {
        yield return creators[Random.Next(creators.Length)](materiaux);
      }
    }

    private Projet[] InitializeProjets(Materiau[] materiaux)
    {
      var projets = new Projet[]
      {
        new Projet("Projet 1") { _Formes = InitializeFormes(materiaux).ToList() },
        new Projet("Projet 2") { _Formes = InitializeFormes(materiaux).ToList() },
        new Projet("Projet 3") { _Formes = InitializeFormes(materiaux).ToList() }
      };

      return projets;
    }

    private Utilisateur[] InitializeUtilisateurs()
    {
      const string password = "$2y$15$eiI786bZMg0HrJP4BphbveEXb1UHmkkd5p8feoUpDqYwuvgHjik2q";
      return new Utilisateur[]
      {
        new Utilisateur("Alexandre", password),
        new Utilisateur("Guillaume", password),
        new Utilisateur("Antoine", password)
      };
    }
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Hymperia.Model.Modeles;
using Hymperia.Model.Modeles.JsonObject;

namespace Hymperia.Model.Migrations
{
  internal class Initializer
  {
    private Random Random { get; set; }

    private Func<Materiau[], Forme>[] FormeCreators
    {
      get => new Func<Materiau[], Forme>[]
      {
        materiaux => new PrismeRectangulaire(materiaux[Random.Next(materiaux.Length)])
        {
          Centre = new Point { X = Random.Next(100), Y = Random.Next(100), Z = Random.Next(100) },
          Hauteur = Random.Next(1, 15),
          Largeur = Random.Next(1, 15),
          Longueur = Random.Next(1, 15)
        },
        materiaux => new Ellipsoide(materiaux[Random.Next(materiaux.Length)])
        {
          Centre = new Point { X = Random.Next(100), Y = Random.Next(100), Z = Random.Next(100) },
          RayonX = Random.Next(1, 15),
          RayonY = Random.Next(1, 15),
          RayonZ = Random.Next(1, 15)
        },
        materiaux => new Cylindre(materiaux[Random.Next(materiaux.Length)])
        {
          Point1 = new Point { X = Random.Next(100), Y = Random.Next(100), Z = Random.Next(100) },
          Point2 = new Point { X = Random.Next(100), Y = Random.Next(100), Z = Random.Next(100) },
          Diametre = Random.Next(1, 15),
        }
      };
    }

    public Initializer()
    {
      Random = new Random();
    }

    public Task[] Initialize(DbSet<Utilisateur> utilisateurs, DbSet<Acces> acces, DbSet<Materiau> materiaux)
    {
      var _materiaux = InitializeMateriaux();
      #if DEBUG
      var _utilisateurs = InitializeUtilisateurs();
      var projets = InitializeProjets(_materiaux);
      #endif

      return new Task[]
      {
        #if DEBUG
        utilisateurs.AddRangeAsync(_utilisateurs),
        acces.AddRangeAsync(InitializeAcces(_utilisateurs, projets)),
        #endif
        materiaux.AddRangeAsync(_materiaux)
      };
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
            acces.Add(new Acces(projets[i], utilisateurs[j], _droit));
          }
        }
      }

      return acces.ToArray();
    }

    private IEnumerable<Forme> InitializeFormes(Materiau[] materiaux)
    {
      var creators = FormeCreators;
      int count = Random.Next(10, 50);

      for (int i = 0; i < count; ++i)
      {
        yield return creators[Random.Next(creators.Length)](materiaux);
      }
    }

    private Materiau[] InitializeMateriaux()
    {
      return new Materiau[]
      {
        new Materiau("Bois", 1.55),
        new Materiau("Acier", 2.55)
      };
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

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Hymperia.Model.Modeles;

namespace Hymperia.Model.Migrations
{
  internal class Initializer
  {
    private Random Random { get; set; }

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
        new Projet("Projet 1"),
        new Projet("Projet 2"),
        new Projet("Projet 3")
      };

      foreach (var projet in projets)
      {

      }

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

using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Hymperia.Model.Modeles;

namespace Hymperia.Model.Migrations
{
  internal class Initializer
  {
    public async Task Initialize(DbSet<Utilisateur> utilisateurs, DbSet<Acces> acces, DbSet<Materiau> materiaux)
    {
      string password = "$2y$15$eiI786bZMg0HrJP4BphbveEXb1UHmkkd5p8feoUpDqYwuvgHjik2q";
      var tasks = new Task[]
      {
#if DEBUG
        utilisateurs.AddRangeAsync(new Utilisateur[]
        {
          new Utilisateur("Alexandre", password),
          new Utilisateur("Guillaume", password),
          new Utilisateur("Antoine", password)
        }),

#endif
      };


    }
  }
}

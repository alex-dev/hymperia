using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hymperia.Model.Modeles;
using Hymperia.Facade.Services;
using Hymperia.Facade.ModelWrappers;

namespace UnitTests.ServiceTests
{
  /// <summary>
  /// Description résumée pour ConvertisseurFormesTests
  /// </summary>
  [TestClass]
  public class ConvertisseurFormesTests
  {
    public ConvertisseurFormesTests()
    {
      //
      // TODO: ajoutez ici la logique du constructeur
      //
    }

    private TestContext testContextInstance;

    /// <summary>
    ///Obtient ou définit le contexte de test qui fournit
    ///des informations sur la série de tests active, ainsi que ses fonctionnalités.
    ///</summary>
    public TestContext TestContext
    {
      get
      {
        return testContextInstance;
      }
      set
      {
        testContextInstance = value;
      }
    }

    #region Attributs de tests supplémentaires
    //
    // Vous pouvez utiliser les attributs supplémentaires suivants lorsque vous écrivez vos tests :
    //
    // Utilisez ClassInitialize pour exécuter du code avant d'exécuter le premier test de la classe
    // [ClassInitialize()]
    // public static void MyClassInitialize(TestContext testContext) { }
    //
    // Utilisez ClassCleanup pour exécuter du code une fois que tous les tests d'une classe ont été exécutés
    // [ClassCleanup()]
    // public static void MyClassCleanup() { }
    //
    // Utilisez TestInitialize pour exécuter du code avant d'exécuter chaque test 
    // [TestInitialize()]
    // public void MyTestInitialize() { }
    //
    // Utilisez TestCleanup pour exécuter du code après que chaque test a été exécuté
    // [TestCleanup()]
    // public void MyTestCleanup() { }
    //
    #endregion

    [TestMethod]
    public void ConeWrapperAMeshElement3D()
    {
      //Materiau materiau = new Materiau("Bois", 1.15);
      //Forme cone = new Cone(materiau);
      //ConvertisseurFormes convertisseurFormes = null;
      //ConvertisseurWrappers convertisseurWrappers = null;

      //FormeWrapper fw = convertisseurFormes.Convertir(cone);
      //Assert.IsInstanceOfType(convertisseurWrappers.Convertir(fw),typeof(Cone));
    }

    [TestMethod]
    public void CylindreWrapperAMeshElement3D()
    {

    }

    [TestMethod]
    public void EllipsoideWrapperAMeshElement3D()
    {

    }

    [TestMethod]
    public void PrismeRectangulaireWrapperAMeshElement3D()
    {

    }

  }
}

# Hymperia
 Les membres de l'équipe :
 - Antoine Mailhot
 - Alexandre Parent
 - Guillaume Le Blanc

## Configuration
La `ConnectionString` de la base de données est dans `connections.config`

## ConsoleModelTest - Application console
- `./ConsoleModelTest.exe --help`  affiche l'aide de la commande.
- `./ConsoleModelTest.exe deployer` pour déployer les migrations.
  - `--initialisation` initialise la base de données avec des données aléatoires.
- `./ConsoleModelTest.exe mateiaux` affiche les materiaux de l'application.
- `./ConsoleModelTest.exe projets` affiche les projets de l'application.
- `./ConsoleModelTest.exe utilisateurs` affiche les utilisateurs de l'application.

## Facade - Application WPF

- Les manipulateurs de redimensionnement ne sont pas fonctionnels. En conséquent, aucun manipulateur n'est afficher lors d'une sélection de redimensionnement.
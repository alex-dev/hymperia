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

### Contrôles de caméra
- Right Click (Hold): Rotation et inclinaison;
- Right Click (Double Click): Déplace la caméra au curseur;
- Middle Mouse Scroll: Zoom;
- Middle Mouse (Hold): Déplace la caméra;
- Middle Mouse (Double Click): Réinitialise la rotation de la caméra;
- Left/Right Arrows: Rotation;
- Left/Right Arrows + Left Ctrl (Hold): Rotation lente;
- Up/Down Arrows: Inclinaison;
- Up/Down Arrows + Left Ctrl (Hold): Inclinaison lente;
- Up/Down/Left/Right + Left Shift (Hold): Déplace la caméra;

### Contrôles de sélection
- Left Click + Left Alt: Sélection d'une seule forme;
- Left Click + Left Ctrl: Sélection de plusieurs formes;
- Left Click + Left Shift (Hold): Sélection rectangulaire;

### Contrôles d'édition
- Double Left Click: Ajout d'une forme au curseur;
- Delete ou Backspace: Suppression des formes sélectionnées;

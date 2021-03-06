Clément GASPARD
# JohnLemmonGame

## Introduction

Le Projet est décrit ci-dessous de manière détaillée avec les étapes qui ont été réalisé pour être mené à bien.  
En complément de cette documentation, dans le dossier *"Assets/Scripts"* se trouvent les scripts de toutes les classes utilisées dans ce projet commentées précisement (sauf *LightFLicker.cs* qui permet de varier l'intensité lumineuse des bougies aléatoirement et qui a été récupéré d'un projet demo).

## I) Description du Scénario

### A) Explications du Scénario
Le joueur se trouve dans un environnement type Château Hanté, il doit arriver à sortir du manoir sans se faire voir par les différents ennemis du Château (des ennemis statiques : les Gargouilles et des ennemis mobiles : les Fantômes).  
Tout cela doit se passer dans un environnement 3D vivant avec une spacialisation sonore réussie pour une bonne immersion du joueur.  
On fait le choix d'utiliser une vue aérienne accrochée au personnage avec un seul angle de vue (pas de rotation de la caméra).  
On fait aussi le choix de faire déplacer le personnage grâce aux touches d'un clavier, à un joystick d'une manette ou à un joystick virtuel. (On aurait pu utiliser un système de clic à l'endroit où l'on souhaite que le personnage aille, mais cela n'est pas du tout pratique avec le type de jeu que nous avons défini où l'on veut éviter de se faire voir par des ennemis)
On utilise aussi un menu Pause pour pouvoir quitter le jeu, le recommencer du début ou changer le type de commande (Clavier, Manette, Tactile)

__Commandes du jeu__ : 
- *Mouvements du personnage*  : ZQSD ou les flèches directionelles / Joystick Gauche Manette / Joystick virtuel
- *Menu Pause* : Touche "Echap", Touche "P" / Bouton Start ou Menu Manette / Bouton pause sur l'interface
### B) Carte du Jeu 

![Carte du Jeu sans annotations](./GameMap_Annotations.png)
![GIF Carte du Jeu](./GameMap_GIF.gif)

### C) Vidéos Démos

https://user-images.githubusercontent.com/28164044/147893292-60dbc3ec-d534-4276-980b-1c184750ebf5.mp4

https://user-images.githubusercontent.com/28164044/147893230-625b0f5a-0291-44ff-ba5f-7aae6bb7720f.mp4

https://user-images.githubusercontent.com/28164044/147893226-fd536d72-7f74-4ac3-9056-c1dfc2dd7319.mp4

https://user-images.githubusercontent.com/28164044/147893236-efbc038b-c2ff-4c25-ae42-e868020e3933.mp4

https://user-images.githubusercontent.com/28164044/147893238-74b5e307-a993-4137-b582-f28ea391cc8f.mp4


## II) Explication concept de Unity

Le concept de Unity que nous avons choisi d'expliquer ici est le lancer de rayon, autrement appelé méthode __Raycast__.  

Dans Unity, il est possible de vérifier s'il existe des __collisions__ (*Collider*) le long du chemin d'une ligne partant d'un point. Cette "ligne" partant d'un point précis s'appelle un __rayon__ (*Ray*). La recherche de __collisions__ le long de ce rayon s'appelle un __Raycast__.  
- La __collision__ est définie ici comme un objet obstruant le passage d'un __rayon__. 
- Le __rayon__ a besoin d'une __origine__ et d'une __direction__. 

Dans Unity, pour créer un __rayon__, on utilise un constructeur ayant comme paramètres une orgine et une direction : 

```cs
Ray rayon = new Ray(origine, direction)
```

Il existe de nombreuses méthodes de __Raycast__ différentes dans Unity, mais elles ont toutes deux choses en commun : 
1. Elles doivent définir le __rayon__ le long duquel le __Raycast__ se produit
1. Elles doivent définir des restrictions sur le type de __collisions__ que le __Raycast__ doit détecter.

La méthode de __Raycast__ que l'on utilise dans notre projet renvoie un booléen qui est vrai quand il a touché quelque chose et faux quand il n'a rien touché. 
Comme elle renvoie un booléen, il est très pratique de mettre la méthode __Raycast__ dans une instruction if. Le bloc de code de l'instruction if ne sera donc exécuté que si le __Raycast__ a touché quelque chose :

```cs
Ray rayon = new Ray(origine, direction);

if(Physics.Raycast(rayon)){/*Code executé si le rayon recontre un objet*/}
```

Pour l'instant, nous n'avons défini aucune restriction sur les objets qui peuvent être touchés. Dans notre projet, nous avons besoin des informations de ce qui est touché par le __Raycast__, afin que l'on puisse vérifier s'il s'agit du personnage du joueur.

Pour cela nous allons donc utiliser une méthode de __Raycast__ différente indiquant en sortie "out" les informations définissant l'objet qui a été touché par le rayon. Ce paramètre de sortie est d'un type "*RaycastHit*" :

```cs
Ray rayon = new Ray(origine, direction);
RaycastHit InfosObjetTouche;

if(Physics.Raycast(rayon, out InfosObjetTouche)){/*Code executé si le rayon recontre un objet*/}
```

On vérifie juste après si l'objet touché est bien celui voulu, on met donc en place une __restriction__ : 

```cs
Ray rayon = new Ray(origine, direction);
RaycastHit InfosObjetTouche;
Transform ObjetQuOnVeutToucher;

if(Physics.Raycast(rayon, out typeObjetTouche))
{
    if(InfosObjetTouche.collider.transform == ObjetQuOnVeutToucher)
    {
        /*Code executé lorsque l'objet voulu est touché*/
    }
}
```
Grâce à la méthode de __Raycast__ explicitée ici, on peut donc vérifier si un objet est touché par un des rayons du __Raycast__, stocker les informations le definissant et verifier par la suite si celui-ci est bien l'objet voulu.

Nous nous en servons donc dans notre projet pour vérifier si le personnage de notre joueur entre dans le champ de vision de notre ennemi.

Pour voir un exemple réel de son fonctionnement et comprendre comment nous avons avons utilisé cette méthode dans notre code, voici un extrait de la classe __Observer__ :
```cs
[...]
if (m_IsPlayerInRange) // Si le joueur est dans la zone de champ de vision de la Gargouille
{
    // Création d'un rayon en direction du joueur : position du joueur (entre les pieds du personnage) moins la position de l'objet "PointOfView" 
    // de la Gargouille. Cependant, comme on veut toucher le joueur à son centre de masse et non au sol entre ses deux pieds, on ajoute un vecteur (0,1,0)
    // (Vector3.up) pour remonter la direction vers le centre du joueur.
    Vector3 direction = player.position - transform.position + Vector3.up; 

    Ray ray = new Ray(transform.position, direction); // Création d'une instance de Rayon par l'utilisation du constructeur (origine, direction)
    RaycastHit raycastHit; // Définition d'un objet de type "Raycasthit" dans le but de connnaître l'objet touché par le rayon

    if (Physics.Raycast(ray, out raycastHit)) // Si le rayon a touché un objet, le raycastHit nous donne l'objet touché
    {
        if (raycastHit.collider.transform == player) // Si l'objet touché par le rayon est le joueur
        {
            gameEnding.CaughtPlayer(); // Alors, appel de la fonction CaughtPlayer() du Script GameEnding pour dire que le joueur a été attrapé
        }
    }
```

## III) Description du Projet

### A) Etapes de réalisation du Projet

- *1ère Etape* : Animation de marche du Personnage. (Classe __PlayerMovement__)

- *2ème Etape* : Ecriture du Script pour le Mouvement du personnage à partir du nouveau système de gestion de commande de Unity. (Classe __PlayerMovement__)

- *3ème Etape* : Mise en place de l'Environnement de la scène : 
    * Gestion de l'Illumination de la scène;

    * Création à la main de l'animation de la chaise volante (dans la salle de gauche au démarrage du jeu, *voir vidéo démo*);

    * Création à la main de l'animation de la sphère volante lumineuse dans la salle à manger (pièce avec la cheminée et la grande table, *voir vidéo démo*).

- *4ème Etape* : Gestion de la Caméra à partir du plug-in Cinemachine (attache de la camera au personnage principal).

- *5ème Etape* : Ajout des effets de post-processing à la caméra : 
    * Anti-aliasing, 

    * Gradient de Couleur,

    * Effet de bloom (flou sur les sources lumineuses), 

    * Occlusion Ambiante, 

    * Vignetage, 

    * Distortion (léger effet fish-eye),

    * Pronfondeur de champ.

- *6ème Etape* : Ecriture du script de fin du jeu (Classe __GameEnding__) :
    * Mise en place d'une zone avec déclencheur de fin du jeu,

    * Mise en place du Canvas de fin du jeu avec l'animation associée (le joueur perd, le joueur gagne).

- *7ème Etape* : Création des ennemis statiques : 
    * Création du Prefab avec la zone de détection du personnage (champ de vision de la Gargouille);
    
    * Animation de la Gargouille;

    * Ecriture de la fonction de détection du Personnage joueur lorsqu'il se trouve dans le champ de vision d'un ennemi grâce au lancer de rayons (Raycast). (Classe __Observer__)

- *8ème Etape* : Génération du NavMesh qui détermine la carte virtuelle où peuvent bouger les ennemis mobiles

- *9ème Etape* : Création des ennemis mobiles :
    * Création du Prefab avec Collider, zone de détection du personnage (champ de vision du fantôme);

    * Animation du Fantôme;

    * Ecriture du script de mouvement des fantômes grâce au NavMeshAgent qui permet de déplacer l'ennemi dans le NavMesh généré à l'étape précédente. (Uilisation de passage par des points de contrôle) (Classe __WaypointPatrol__).

- *10ème Etape* : Audio Spatial :
    * Mise en place de l'audio d'ambiance qui n'est pas 3D et qui est au même niveau de volume quel que soit l'endroit où se trouve le personnage dans la scène;

    * De la même manière, mise en place des musiques de fin du jeu (le joueur perd, le joueur gagne);

    * Ajout du son des pieds du joueur quand il se déplace et synchronisation avec l'animation de marche du personnage;

    * Ajout des différentes sources Audios 3D aux éléments du jeu (Chandelles sur les murs, fantômes) et ajustement des paramètres d'Audio 3D pour faire en sorte que le son diminue avec la distance et puisse être localisable avec un son binaural (casque audio) ou un système audio multicanal (home cinéma multi-enceintes);

    * Création de l'AudioListener et écriture d'un script permettant sa fixation au niveau de la tête, entre les oreilles du personnage sans qu'il ne soit affecté par la rotation de celui-ci. (Classe __AudioController_norotation__).

- *11ème Etape* : Création d'un Canvas de Menu Pause (Le joueur peut mettre en pause, recommencer au début, ou quitter le jeu). (Classe __PauseMenu__)

- *12ème Etape* : Adaptation du projet pour multi-platerforme : 
    * Ajout d'un joystick virtuel pour pouvoir jouer au jeu depuis un appareil mobile ou un ordinateur tactile (Classe __Touch__);

    * Ajout au Menu pause d'une possibilité de choisir son système de contrôle du joueur (Soit clavier, soit Manette de jeu, soit interface tactile) (Classe __PauseMenu__ et __Touch__);

    * Adaptation du niveau de qualité du jeu en fonction des types d'appareil (Haute qualité pour un PC Windows, qualité moyenne avec quelques sacrifices pour le téléphone Android).

- *13ème Etape* : Exportation du projet : 
    * Création d'un executable pour Windows (x64);

    * Création d'un apk pour téléphone ou tablette Android (Attention le framerate de l'application dépend énormément de la puissance graphique de l'appareil Android utilisé).

### B) Classes du Projet

- __AudioController_norotation__ :  
*Classe permettant de mettre l'Audio Listener au bon endroit au niveau des oreilles du personnage sans que celui-ci ne tourne en même temps que le personnage (cela evite de se retrouver avec l'emplacement 3D des sons inversés lorsque le personnage fait face au joueur).*

- __GameEnding__ :  
*Classe permettant de gérer les fins du jeu et d'afficher en conséquence l'animation de fin correspondante.*
*Les deux fins possibles sont :*
    - *Le joueur se fait voir (attraper) par un ennemi et perd (le jeu se reset et recommence au début);*

    - *Le joueur gagne, et le jeu se ferme.*  

- __Observer__ :  
*Classe permettant de gérer par lancers de rayons la détection du personnage lorsqu'il est dans le champ de vision des ennemis (Gargouilles et Fantômes).*

- __PauseMenu__ :  
*Classe permettant de mettre le jeu en Pause et d'afficher et d'interagir avec le Menu Pause.*

- __PlayerMovement__ :  
*Classe permettant de gérer les mouvements du personnages par le joueur (à partir de la nouvelle gestion des commandes de Unity) ainsi que de gérer l'animation de marche du personnage lorsqu'il se déplace et le son allant avec.*

- __Touch__ :  
*Classe permettant de gérer l'utilisation des commandes tactiles pour un PC tactile ou un appareil Android (elle permet d'activer ou déstactiver l'affichage du joystick numérique).*

- __WaypointPatrol__ :  
*Classe permettant de gérer les différents points de contrôles (Waypoints) par lesquels doivent passer les ennemis mobiles (Fantômes). Lorsque qu'un enememi a atteint le point de contrôle par lequel il devait passer, alors il passe au point suivant.*  

### C) Arborescence du Projet

```bash
Build # Fichiers executables pour les différentes plateforme
├───Windows # Application .exe pour Windows (x64)
└───Android # Application .apk à installer sur un téléphone portable ou une tablette Android

Assets
├───Animation  
│   ├───Animation # Animations pures d'un Objet 3D
│   └───Animators # Machines à états d'utilisation des Animations associées à un GameObject
├───Audio  #Fichiers Audios utilisés dans le Projet
├───Gizmos  
│   └───Cinemachine
├───Materials # Matériaux des différents Objets 3D du Projet (associés aux textures)
│   ├───Bathroom  
│   ├───BedRoom  
│   ├───Characters  
│   ├───Corridor  
│   ├───DiningRoom  
│   └───VFX  
├───Models # Modèles 3D utilisés dans le Projet (sans textures)
│   ├───Bathroom  
│   ├───Bedroom  
│   ├───Characters  
│   ├───Corridor  
│   ├───Diningroom  
│   └───VFX  
├───Prefabs # Préfariqués du Projet (Personnages, Objets de l'Environnement, Animations)
│   ├───Environment  # Préfabriqués associés à l'Environnement du Projet
│   │   ├───Bathroom  
│   │   ├───Bedroom  
│   │   ├───Corridors  
│   │   └───Dinigroom  
│   └───VFX  # Préfabriqués d'Objets animés
├───Scenes  
│   ├───SampleScene # Mesh de Navigation pour les ennemis
│   └───SampleScene_Profiles # Profil des Effets de PostProduction appliqués à la Caméra du Joueur
├───Scripts  #Scripts du Projet
├───Shaders  
└───Textures  # Textures des différents Matériaux du Projet / Images
    ├───BathRoom  
    ├───BedRoom  
    ├───Characters  
    ├───Corridor  
    ├───DiningRoom  
    ├───Icon  # Icone de l'Application Android / Logiciel Windows
    ├───UI  # Images associées aux Menu pause et à l'interface générale
    └───VFX  
```

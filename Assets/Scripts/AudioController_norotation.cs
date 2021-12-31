// Code by Clément GASPARD
//***********************************************************************************************************
//                                 Classe AudioController_norotation
//    Classe permettant de mettre l'Audio Listener au bon endroit au niveau des oreilles du personnage
//       sans que celui-ci ne tourne en même temps que le personnage (cela evite de se retrouver avec
//              l'emplacement 3D des sons inversés lorsque le personnage fait face au joueur)
//***********************************************************************************************************

// Définition de l'Audio Listener : (Objet du jeu captant les sons environnants pour les
// restituer dans les hauts parleurs du joueur, il permet d'obtenir une spatialisation 3D
// du son en fonction de l'emplacement du personnage dans le jeu)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController_norotation : MonoBehaviour
{
    public GameObject player; // Importer de l'objet "player" qui est le personnage du joueur
    public float listenerHeight = 1.155f; // Hauteur permettant de mettre l'objet virtuel qui capte les sons pour l'utilisateur à la
                                          // bonne hauteur entre les oreilles du personnage
    void Update()
    {
        Vector3 pos = player.transform.position; // Recopie de la position du joueur à chaque image dans le Vecteur 3D "pos"
        pos.y += listenerHeight; // Ajout de la hauteur à la composante Z de ce Vecteur 3D pour que la capture de son se fasse au niveau
                                 // des oreilles du personnage

        transform.position = pos; // Permet de recopier cette nouvelle position définie par le Vecteur 3D dans la composante position de l'Audio Listener
    }
}
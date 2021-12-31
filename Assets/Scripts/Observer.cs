// Code by Cl�ment GASPARD
//***********************************************************************************************************
//                                          Classe Observer
//      Classe permettant de g�rer par lancers de rayons la d�tection du personnage lorsqu'il est 
//                    Dans le champ de vision des ennemis (Gargouilles et Fant�mes)
//***********************************************************************************************************


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer : MonoBehaviour
{
    public Transform player; // Importation du personnage du joueur sous le type "Transform" pour avoir acc�s plus facilement � sa position
    public GameEnding gameEnding; // Immportation du script GameEnding pour avori acc�s � ses fonctions

    bool m_IsPlayerInRange; // Variable bool�enne d�finissant si le joueur est dans le champ de vision de la Gargouille

    // Fonction permettant de changer la valeur de la variable "m_IsPlayerInRange" en fonction de quand le joueur ENTRE dans le champ de vision de  
    // la Gargouille. La fonction est appel�e quand le Collider du joueur ENTRE dans le d�clencheur "PointOfView" du Prefab "Gargoyle"
    void OnTriggerEnter(Collider other)
    {
        if (other.transform == player) // Permet d'�tre s�r que c'est bien le Collider du joueur qui entre dans la zone
        {
            m_IsPlayerInRange = true; // Passage de la variable "m_IsPlayerInRange" � true si le joueur est entr� dans la zone
        }
    }

    // Fonction permettant de changer la valeur de la variable "m_IsPlayerInRange" en fonction de quand le joueur SORT du champ de vision de  
    // la Gargouille. La fonction est appel�e quand le Collider du joueur SORT du d�clencheur "PointOfView" du Prefab "Gargoyle"
    void OnTriggerExit(Collider other)
    {
        if (other.transform == player) // Permet d'�tre s�r que c'est bien le Collider du joueur qui sort de la zone
        {
            m_IsPlayerInRange = false; // Passage de la variable "m_IsPlayerInRange" � false si le joueur est sorti de la zone
        }
    }

    void Update()
    {
        if (m_IsPlayerInRange) // Si le joueur est dans la zone de champ de vision de la Gargouille
        {
            // Cr�ation d'un rayon en direction du joueur : position du joueur (entre les pieds du personnage) moins la position de l'objet "PointOfView" 
            // de la Gargouille. Cependant, comme on veut toucher le joueur � son centre de masse et non au sol entre ses deux pieds, on ajoute un vecteur (0,1,0)
            // (Vector3.up) pour remonter la direction vers le centre du joueur.
            Vector3 direction = player.position - transform.position + Vector3.up; 

            Ray ray = new Ray(transform.position, direction); // Cr�ation d'une instance de Rayon par l'utilisation du constructeur (origine, direction)
            RaycastHit raycastHit; // D�finition d'un objet de type "Raycasthit" dans le but de connna�tre l'objet touch� par le rayon

            if (Physics.Raycast(ray, out raycastHit)) // Si le rayon a touch� un objet, le raycastHit nous donne l'objet touch�
            {
                if (raycastHit.collider.transform == player) // Si l'objet touch� par le rayon est le joueur
                {
                    gameEnding.CaughtPlayer(); // Alors, appel de la fonction CaughtPlayer() du Script GameEnding pour dire que le joueur a �t� attrap�
                }
            }
        }
    }
}
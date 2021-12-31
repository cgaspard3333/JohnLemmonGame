// Code by Clément GASPARD
//***********************************************************************************************************
//                                             Classe Touch
//      Classe permettant de gérer l'utilisation des commandes tactiles pour un PC tactile ou un appareil
//             Android (elle permet d'activer ou déstactiver l'affichage du joystick numérique)
//***********************************************************************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touch : MonoBehaviour
{
    public bool TouchIsActive = false; // Variable globale permettant de savoir si le jeu est en mode tactile ou non
    public GameObject touchUI; // Importation de l'objet "touchUI" étant le Canvas du Joystick numérique (pour interaction tactile) dans Unity
    public void SwitchTouch() // Fonction permettant de switcher entre l'interface tactile et l'interface souris/clavier/manette
    {
        if (TouchIsActive == true) // Si l'interface tactile est déja activée
        {
            DesactivateTouch(); // Appel de la fonction permettant de désactiver l'interface tactile 
        }
        else
        {
            ActivateTouch(); // Appel de la fonction permettant d'activer l'interface tactile
        }
    }

    public void DesactivateTouch() // Fonction permettant de désactiver l'interface tactile
    {
        TouchIsActive = false; // Mettre la variable "TouchIsActive" permettant de savoir si le jeu est en mode tactile ou non à "false"
        touchUI.SetActive(false); // Désactiver l'affichage de l'interface tactile 
    }

    public void ActivateTouch() // Fonction permettant d'activer l'interface tactile
    {
        TouchIsActive = true; // Mettre la variable "TouchIsActive" permettant de savoir si le jeu est en mode tactile ou non à "true"
        touchUI.SetActive(true); // Activer l'affichage de l'interface tactile 
    }

}

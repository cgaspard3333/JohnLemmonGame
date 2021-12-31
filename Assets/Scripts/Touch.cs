// Code by Cl�ment GASPARD
//***********************************************************************************************************
//                                             Classe Touch
//      Classe permettant de g�rer l'utilisation des commandes tactiles pour un PC tactile ou un appareil
//             Android (elle permet d'activer ou d�stactiver l'affichage du joystick num�rique)
//***********************************************************************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touch : MonoBehaviour
{
    public bool TouchIsActive = false; // Variable globale permettant de savoir si le jeu est en mode tactile ou non
    public GameObject touchUI; // Importation de l'objet "touchUI" �tant le Canvas du Joystick num�rique (pour interaction tactile) dans Unity
    public void SwitchTouch() // Fonction permettant de switcher entre l'interface tactile et l'interface souris/clavier/manette
    {
        if (TouchIsActive == true) // Si l'interface tactile est d�ja activ�e
        {
            DesactivateTouch(); // Appel de la fonction permettant de d�sactiver l'interface tactile 
        }
        else
        {
            ActivateTouch(); // Appel de la fonction permettant d'activer l'interface tactile
        }
    }

    public void DesactivateTouch() // Fonction permettant de d�sactiver l'interface tactile
    {
        TouchIsActive = false; // Mettre la variable "TouchIsActive" permettant de savoir si le jeu est en mode tactile ou non � "false"
        touchUI.SetActive(false); // D�sactiver l'affichage de l'interface tactile 
    }

    public void ActivateTouch() // Fonction permettant d'activer l'interface tactile
    {
        TouchIsActive = true; // Mettre la variable "TouchIsActive" permettant de savoir si le jeu est en mode tactile ou non � "true"
        touchUI.SetActive(true); // Activer l'affichage de l'interface tactile 
    }

}

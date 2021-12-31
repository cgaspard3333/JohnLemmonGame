// Code by Clément GASPARD
//***********************************************************************************************************
//                                          Classe PauseMenu
//      Classe permettant de mettre le jeu en Pause et d'afficher et d'interagir avec le Menu Pause
//***********************************************************************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Importation de la bibliothèque de gestion des Scènes
using UnityEngine.InputSystem; // Importation de la bibliothèque de la nouvelle Gestion des commandes de Unity

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false; // Déclaration d'une variable publique permettant de savoir si le jeu est en Pause ou non
    public GameObject pauseMenuUI; // Importation de l'objet "pauseMenuUI" étant le Canvas du menu Pause dans Unity
    public void OnPauseMenu() // Fonction liée à la nouvelle gestion des commandes de Unity, elle s'active lorsque la touche "Echap" ou "P" est enfoncée
    {
        if (GameIsPaused) // Si le jeu est déjà en pause
        {
            Resume(); // On appelle la fonction permettant de reprendre le jeu et désactiver le menu Pause
        }
        else 
        {
            Pause(); // On appelle la fonction qui met le jeu en Pause
        }

    }

    public void Resume() // Fonction permettant de reprendre le jeu et d'enlever le menu de Pause
    {
        pauseMenuUI.SetActive(false); // Désactive l'objet Menu pour qu'il n'apparaisse plus à l'écran 
        Time.timeScale = 1f; // Relance le temps pour que le jeu fonctionne de nouveau à vitesse normale
        GameIsPaused = false; // Passe la variable disant que le jeu est en Pause à "false"
        AudioListener.pause = false; // Remet le son du jeu en marche
    }

    void Pause() // Fonction permettant de mettre le jeu en Pause et d'afficher le menu de Pause
    {
        pauseMenuUI.SetActive(true); // Active l'objet Menu pour qu'il apparaisse à l'écran 
        Time.timeScale = 0f; // Désactive le déroulement du temps ppur figer le jeu
        GameIsPaused = true; // // Passe la variable disant que le jeu est en Pause à "true"
        AudioListener.pause = true;
    }

    public void Restart() // Fonction permettant de redemarrer le jeu au début
    {
        SceneManager.LoadScene(0); // Recharge la scène principale en la remettant à zéro
        Resume(); // Appelle la fonction permettant de reprendre le jeu et désactiver le menu Pause
    }
    public void QuitGame() // Fonction permettant de quitter le jeu
    {
        Application.Quit(); // Quitte le jeu
    }

}

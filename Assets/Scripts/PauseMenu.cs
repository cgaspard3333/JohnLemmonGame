// Code by Cl�ment GASPARD
//***********************************************************************************************************
//                                          Classe PauseMenu
//      Classe permettant de mettre le jeu en Pause et d'afficher et d'interagir avec le Menu Pause
//***********************************************************************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Importation de la biblioth�que de gestion des Sc�nes
using UnityEngine.InputSystem; // Importation de la biblioth�que de la nouvelle Gestion des commandes de Unity

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false; // D�claration d'une variable publique permettant de savoir si le jeu est en Pause ou non
    public GameObject pauseMenuUI; // Importation de l'objet "pauseMenuUI" �tant le Canvas du menu Pause dans Unity
    public void OnPauseMenu() // Fonction li�e � la nouvelle gestion des commandes de Unity, elle s'active lorsque la touche "Echap" ou "P" est enfonc�e
    {
        if (GameIsPaused) // Si le jeu est d�j� en pause
        {
            Resume(); // On appelle la fonction permettant de reprendre le jeu et d�sactiver le menu Pause
        }
        else 
        {
            Pause(); // On appelle la fonction qui met le jeu en Pause
        }

    }

    public void Resume() // Fonction permettant de reprendre le jeu et d'enlever le menu de Pause
    {
        pauseMenuUI.SetActive(false); // D�sactive l'objet Menu pour qu'il n'apparaisse plus � l'�cran 
        Time.timeScale = 1f; // Relance le temps pour que le jeu fonctionne de nouveau � vitesse normale
        GameIsPaused = false; // Passe la variable disant que le jeu est en Pause � "false"
        AudioListener.pause = false; // Remet le son du jeu en marche
    }

    void Pause() // Fonction permettant de mettre le jeu en Pause et d'afficher le menu de Pause
    {
        pauseMenuUI.SetActive(true); // Active l'objet Menu pour qu'il apparaisse � l'�cran 
        Time.timeScale = 0f; // D�sactive le d�roulement du temps ppur figer le jeu
        GameIsPaused = true; // // Passe la variable disant que le jeu est en Pause � "true"
        AudioListener.pause = true;
    }

    public void Restart() // Fonction permettant de redemarrer le jeu au d�but
    {
        SceneManager.LoadScene(0); // Recharge la sc�ne principale en la remettant � z�ro
        Resume(); // Appelle la fonction permettant de reprendre le jeu et d�sactiver le menu Pause
    }
    public void QuitGame() // Fonction permettant de quitter le jeu
    {
        Application.Quit(); // Quitte le jeu
    }

}

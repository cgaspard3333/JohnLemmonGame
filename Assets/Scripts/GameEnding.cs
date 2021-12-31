// Code by Cl�ment GASPARD
//***********************************************************************************************************
//                                           Classe GameEnding
//       Classe permettant de g�rer les fins du jeu et d'afficher en cons�quence l'animation de fin
//                                            correspondante.
//                                     Les deux fins possibles sont :
//   - Le joueur se fait voir (attraper) par un ennemi et perd (le jeu se reset et recommence au d�but)
//                                - Le joueur gagne, et le jeu se ferme
//***********************************************************************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Importation de la biblioth�que de gestion des sc�nes

public class GameEnding : MonoBehaviour
{
    public float fadeDuration = 1f; // Dur�e de la transition vers le Canvas de fin
    public float displayImageDuration = 1f; // Temps durant lequel l'image de fin est affich�e � l'�cran
    public GameObject player; // Importation du personnage du joueur
    public CanvasGroup exitBackgroundImageCanvasGroup; // Importation du Groupe de Canvas contenant l'image � afficher pour annoncer la fin du jeu
    public CanvasGroup caughtBackgroundImageCanvasGroup; // Importation du Groupe de Canvas contenant l'image � afficher si le joueur perd (se fait attraper)
    public AudioSource exitAudio; // Importation de l'audio de fin 
    public AudioSource caughtAudio; // Importation de l'audio si le joueur perd (se fait attraper)

    bool m_IsPlayerAtExit; // Variable bool�enne d�finissant si le joueur est pass� dans le d�clencheur de Fin du jeu
    bool m_IsPlayerCaught; // Variable bool�enne d�finissant si le joueur s'est fait attrap� par les monstres
    float m_Timer; // Variable definissant un Timer qui permet de verifier que le jeu ne se finit pas avant la fin de la cin�matique de fin
    bool m_HasAudioPlayed; // Variable boll�enne pour que l'audio ne se joue qu'une seule fois

    // Fonction permettant de changer la valeur de la variable "m_IsPlayerAtExit" en fonction de si le joueur a pass� la zone de fin ou non 
    // La fonction est appel�e quand le Collider du joueur entre dans le d�clencheur "GameEnding"
    void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject == player) // Permet d'�tre s�r que c'est bien le Collider du joueur qui entre dans la zone de fin
        {
            m_IsPlayerAtExit = true; // Passage de la variable "m_IsPlayerAtExit" � true si le joueur est entr� dans la zone de fin
        }
    }

    // Fonction publique permettant de d�finir que le joueur a �t� attrap� par un monstre
    public void CaughtPlayer()
    {
        m_IsPlayerCaught = true; // Passage de la variable "m_IsPlayerCaught" � true si le joueur est entr� dans le champ de vision de l'ennemi
    }

    // Fonction definissant ce qu'il se passe � fin du jeu avec comme parametre le Canvas � afficher, "doRestart" pour savoir si le joueur a perdu ou gagn�
    // et donc redemarrer le jeu au d�but si jamais le joueur a perdu et "audioSource" pour d�finir le fichier audio � lire.
    void EndLevel(CanvasGroup imageCanvasGroup, bool doRestart, AudioSource audioSource)
    {

        if (!m_HasAudioPlayed) // Si le fichier audio n'a pas encore �t� jou�
        {
            audioSource.Play(); // Lire le fichier audio
            m_HasAudioPlayed = true; // Passer la variable "m_HasAudioPlayed" � true, pour dire que le son a commenc� sa lecture / a �t� lu
        }

        m_Timer += Time.deltaTime; // Permet de compter le temps depuis le d�but de la cin�matique de fin du jeu

        imageCanvasGroup.alpha = m_Timer / fadeDuration; // Permet de modifier la valeur du "Alpha" de notre Canvas qui est d�fini entre 0 et 1
                                                         // Cr�e donc un fondu de l'opacit� de notre image de fin � partir du moment o� le joueur
                                                         // a pass� la zone de fin

        // Permet de quitter le jeu lorsque le temps de fondu + le temps d'affichage de l'image de fin sont termin�s
        if (m_Timer > fadeDuration + displayImageDuration)
        {
            if (doRestart) // Si la variable doRestart pass�e en param�tre est vraie (si le joueur a perdu)
            {
                SceneManager.LoadScene(0); // Alors on redemarre le jeu au d�but
            }
            else
            {
                Application.Quit(); // Quitte le jeu (Fonctionne uniquement lorsque le jeu est "Build")
            }
        }
    }

    void Update()
    {
        if (m_IsPlayerAtExit) // Si le joueur est entr� ou a travers� la zone du d�clencheur de fin
        {
            // Appel de la fonction terminant le jeu avec comme parametre le Canvas de fin (le joueur gagne) et l'audio associ�
            EndLevel(exitBackgroundImageCanvasGroup, false, exitAudio); 
        }

        else if (m_IsPlayerCaught) // Si le joueur s'est fait attraper par un monstre
        {
            // Appel de la fonction terminant le jeu avec comme parametre le Canvas o� le joueur perd et l'audio associ�
            EndLevel(caughtBackgroundImageCanvasGroup, true, caughtAudio);
        }

    }
}
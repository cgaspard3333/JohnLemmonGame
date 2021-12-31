// Code by Clément GASPARD
//***********************************************************************************************************
//                                           Classe GameEnding
//       Classe permettant de gérer les fins du jeu et d'afficher en conséquence l'animation de fin
//                                            correspondante.
//                                     Les deux fins possibles sont :
//   - Le joueur se fait voir (attraper) par un ennemi et perd (le jeu se reset et recommence au début)
//                                - Le joueur gagne, et le jeu se ferme
//***********************************************************************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Importation de la bibliothèque de gestion des scènes

public class GameEnding : MonoBehaviour
{
    public float fadeDuration = 1f; // Durée de la transition vers le Canvas de fin
    public float displayImageDuration = 1f; // Temps durant lequel l'image de fin est affichée à l'écran
    public GameObject player; // Importation du personnage du joueur
    public CanvasGroup exitBackgroundImageCanvasGroup; // Importation du Groupe de Canvas contenant l'image à afficher pour annoncer la fin du jeu
    public CanvasGroup caughtBackgroundImageCanvasGroup; // Importation du Groupe de Canvas contenant l'image à afficher si le joueur perd (se fait attraper)
    public AudioSource exitAudio; // Importation de l'audio de fin 
    public AudioSource caughtAudio; // Importation de l'audio si le joueur perd (se fait attraper)

    bool m_IsPlayerAtExit; // Variable booléenne définissant si le joueur est passé dans le déclencheur de Fin du jeu
    bool m_IsPlayerCaught; // Variable booléenne définissant si le joueur s'est fait attrapé par les monstres
    float m_Timer; // Variable definissant un Timer qui permet de verifier que le jeu ne se finit pas avant la fin de la cinématique de fin
    bool m_HasAudioPlayed; // Variable bolléenne pour que l'audio ne se joue qu'une seule fois

    // Fonction permettant de changer la valeur de la variable "m_IsPlayerAtExit" en fonction de si le joueur a passé la zone de fin ou non 
    // La fonction est appelée quand le Collider du joueur entre dans le déclencheur "GameEnding"
    void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject == player) // Permet d'être sûr que c'est bien le Collider du joueur qui entre dans la zone de fin
        {
            m_IsPlayerAtExit = true; // Passage de la variable "m_IsPlayerAtExit" à true si le joueur est entré dans la zone de fin
        }
    }

    // Fonction publique permettant de définir que le joueur a été attrapé par un monstre
    public void CaughtPlayer()
    {
        m_IsPlayerCaught = true; // Passage de la variable "m_IsPlayerCaught" à true si le joueur est entré dans le champ de vision de l'ennemi
    }

    // Fonction definissant ce qu'il se passe à fin du jeu avec comme parametre le Canvas à afficher, "doRestart" pour savoir si le joueur a perdu ou gagné
    // et donc redemarrer le jeu au début si jamais le joueur a perdu et "audioSource" pour définir le fichier audio à lire.
    void EndLevel(CanvasGroup imageCanvasGroup, bool doRestart, AudioSource audioSource)
    {

        if (!m_HasAudioPlayed) // Si le fichier audio n'a pas encore été joué
        {
            audioSource.Play(); // Lire le fichier audio
            m_HasAudioPlayed = true; // Passer la variable "m_HasAudioPlayed" à true, pour dire que le son a commencé sa lecture / a été lu
        }

        m_Timer += Time.deltaTime; // Permet de compter le temps depuis le début de la cinématique de fin du jeu

        imageCanvasGroup.alpha = m_Timer / fadeDuration; // Permet de modifier la valeur du "Alpha" de notre Canvas qui est défini entre 0 et 1
                                                         // Crée donc un fondu de l'opacité de notre image de fin à partir du moment où le joueur
                                                         // a passé la zone de fin

        // Permet de quitter le jeu lorsque le temps de fondu + le temps d'affichage de l'image de fin sont terminés
        if (m_Timer > fadeDuration + displayImageDuration)
        {
            if (doRestart) // Si la variable doRestart passée en paramètre est vraie (si le joueur a perdu)
            {
                SceneManager.LoadScene(0); // Alors on redemarre le jeu au début
            }
            else
            {
                Application.Quit(); // Quitte le jeu (Fonctionne uniquement lorsque le jeu est "Build")
            }
        }
    }

    void Update()
    {
        if (m_IsPlayerAtExit) // Si le joueur est entré ou a traversé la zone du déclencheur de fin
        {
            // Appel de la fonction terminant le jeu avec comme parametre le Canvas de fin (le joueur gagne) et l'audio associé
            EndLevel(exitBackgroundImageCanvasGroup, false, exitAudio); 
        }

        else if (m_IsPlayerCaught) // Si le joueur s'est fait attraper par un monstre
        {
            // Appel de la fonction terminant le jeu avec comme parametre le Canvas où le joueur perd et l'audio associé
            EndLevel(caughtBackgroundImageCanvasGroup, true, caughtAudio);
        }

    }
}
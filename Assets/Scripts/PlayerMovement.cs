// Code by Cl�ment GASPARD
//***********************************************************************************************************
//                                          Classe PlayerMovement
//    Classe permettant de g�rer les mouvements du personnages par le joueur (� partir de la nouvelle
//         gestion des commandes de Unity) ainsi que de g�rer l'animation de marche du personnage
//                                lorsqu'il se d�place et le son allant avec.       
//***********************************************************************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // Importation de la biblioth�que de la nouvelle Gestion des commandes de Unity

public class PlayerMovement : MonoBehaviour
{
    public float turnSpeed = 20f; // D�finition de la vitesse de rotation du personnage
    Animator m_Animator; // Cr�ation d'un objet Animator
    Rigidbody m_Rigidbody; // Cr�ation d'un objet Rigidbody
    AudioSource m_AudioSource; // Cr�ation d'un objet AudioSource
    Vector3 m_Movement; // Cr�ation d'un Vecteur 3D
    Quaternion m_Rotation = Quaternion.identity; // D�finition d'une variable de type quaternion a une rotation nulle (identit�)

    // Cr�ation des variables de mouvement horizontales et verticales
    private float movementX;
    private float movementY;

    // Start est appel� une seule fois avant le rendu de la toute premi�re image
    void Start()
    {
        // R�cup�ration des composants Rigidbody, Animator et AudioSource de notre personnage
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_AudioSource = GetComponent<AudioSource>();
    }
    // Fonction li�e � la nouvelle gestion des commandes de Unity, elle s'active lorsque une des touches de d�placement est utilis�e
    // Elle recup�re les commandes verticales et horizontales de l'utilisateur
    private void OnMovement(InputValue movementValue) 
    {
        Vector2 movementVector = movementValue.Get<Vector2>(); // R�cup�re les valeurs de commandes des joysticks et o� touches directionnelles du clavier
        movementX = movementVector.x; // Copie les valeurs de d�placement en x dans la variable "movementX" 
        movementY = movementVector.y; // Copie les valeurs de d�placement en y dans la variable "movementY" 
    }

    // Pour faire simple : - Update() est appel� avant le rendu de chaque image (Varie en fonction du nombre de fps, nombre d'appel variable)
    //                     - FixedUpdate() est appel� avant les op�rations de la "physique" du jeu (Ne d�pend pas du nombre de fps, nombre d'appel fix�)
    //                       c'est a dire avant que ne soit calcul�e les collisions et autres interactions. Par d�faut cette boucle est 
    //                       appel�e 50 fois par seconde.
    void FixedUpdate()
    {
        m_Movement.Set(movementX, 0f, movementY); // Application des commandes au vecteur m_Movement

        // V�rification que les commandes verticales et horizontales soient approximativement �gales � z�ro (pour l'animation de marche)
        bool hasHorizontalInput = !Mathf.Approximately(movementX, 0f);
        bool hasVerticalInput = !Mathf.Approximately(movementY, 0f);

        bool isWalking = hasHorizontalInput || hasVerticalInput; // Combinaison des deux variables en une seule pour d�finir si le personnage marche ou non

        m_Animator.SetBool("IsWalking", isWalking); // Applique la variable bool�enne "IsWalking" d�finie juste au dessus au param�tre "IsWalking" 
                                                    // � l'int�rieur de l'Animator du personnage pour lancer l'animation de marche lorsqu'il se d�place

        if (isWalking) // Si notre personnage marche
        {
            if (!m_AudioSource.isPlaying) // Si la fichier audio de marche n'est pas entrain d'�tre jou�
            {
                m_AudioSource.Play(); // Alors, lancer la lecture 
            }
        }
        else // Si notre personnage ne marche plus
        {
            m_AudioSource.Stop(); // Arr�ter la lecture
        }
        
        // Cr�ation du vecteur donnant la rotation d�sir�e du personnage et en lui appliquant la vitesse de rotation "turnSpeed"
        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);

        // R�alise une rotation regardant dans la direction du vecteur de rotation d�sir� d�fini juste au dessus
        m_Rotation = Quaternion.LookRotation(desiredForward);
    }

    // Fonction permettant de faire bouger notre personnage dans la direction voulue � chaque image APRES le d�but de l'animation de marche
    void OnAnimatorMove()
    {
        // Permet d'appliquer le mouvement de translation et rotation au Rigidbody de notre personnage
        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude);
        m_Rigidbody.MoveRotation(m_Rotation);
    }

}

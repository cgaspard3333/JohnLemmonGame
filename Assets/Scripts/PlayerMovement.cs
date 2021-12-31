// Code by Clément GASPARD
//***********************************************************************************************************
//                                          Classe PlayerMovement
//    Classe permettant de gérer les mouvements du personnages par le joueur (à partir de la nouvelle
//         gestion des commandes de Unity) ainsi que de gérer l'animation de marche du personnage
//                                lorsqu'il se déplace et le son allant avec.       
//***********************************************************************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // Importation de la bibliothèque de la nouvelle Gestion des commandes de Unity

public class PlayerMovement : MonoBehaviour
{
    public float turnSpeed = 20f; // Définition de la vitesse de rotation du personnage
    Animator m_Animator; // Création d'un objet Animator
    Rigidbody m_Rigidbody; // Création d'un objet Rigidbody
    AudioSource m_AudioSource; // Création d'un objet AudioSource
    Vector3 m_Movement; // Création d'un Vecteur 3D
    Quaternion m_Rotation = Quaternion.identity; // Définition d'une variable de type quaternion a une rotation nulle (identité)

    // Création des variables de mouvement horizontales et verticales
    private float movementX;
    private float movementY;

    // Start est appelé une seule fois avant le rendu de la toute première image
    void Start()
    {
        // Récupération des composants Rigidbody, Animator et AudioSource de notre personnage
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_AudioSource = GetComponent<AudioSource>();
    }
    // Fonction liée à la nouvelle gestion des commandes de Unity, elle s'active lorsque une des touches de déplacement est utilisée
    // Elle recupère les commandes verticales et horizontales de l'utilisateur
    private void OnMovement(InputValue movementValue) 
    {
        Vector2 movementVector = movementValue.Get<Vector2>(); // Récupère les valeurs de commandes des joysticks et où touches directionnelles du clavier
        movementX = movementVector.x; // Copie les valeurs de déplacement en x dans la variable "movementX" 
        movementY = movementVector.y; // Copie les valeurs de déplacement en y dans la variable "movementY" 
    }

    // Pour faire simple : - Update() est appelé avant le rendu de chaque image (Varie en fonction du nombre de fps, nombre d'appel variable)
    //                     - FixedUpdate() est appelé avant les opérations de la "physique" du jeu (Ne dépend pas du nombre de fps, nombre d'appel fixé)
    //                       c'est a dire avant que ne soit calculée les collisions et autres interactions. Par défaut cette boucle est 
    //                       appelée 50 fois par seconde.
    void FixedUpdate()
    {
        m_Movement.Set(movementX, 0f, movementY); // Application des commandes au vecteur m_Movement

        // Vérification que les commandes verticales et horizontales soient approximativement égales à zéro (pour l'animation de marche)
        bool hasHorizontalInput = !Mathf.Approximately(movementX, 0f);
        bool hasVerticalInput = !Mathf.Approximately(movementY, 0f);

        bool isWalking = hasHorizontalInput || hasVerticalInput; // Combinaison des deux variables en une seule pour définir si le personnage marche ou non

        m_Animator.SetBool("IsWalking", isWalking); // Applique la variable booléenne "IsWalking" définie juste au dessus au paramètre "IsWalking" 
                                                    // à l'intérieur de l'Animator du personnage pour lancer l'animation de marche lorsqu'il se déplace

        if (isWalking) // Si notre personnage marche
        {
            if (!m_AudioSource.isPlaying) // Si la fichier audio de marche n'est pas entrain d'être joué
            {
                m_AudioSource.Play(); // Alors, lancer la lecture 
            }
        }
        else // Si notre personnage ne marche plus
        {
            m_AudioSource.Stop(); // Arrêter la lecture
        }
        
        // Création du vecteur donnant la rotation désirée du personnage et en lui appliquant la vitesse de rotation "turnSpeed"
        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);

        // Réalise une rotation regardant dans la direction du vecteur de rotation désiré défini juste au dessus
        m_Rotation = Quaternion.LookRotation(desiredForward);
    }

    // Fonction permettant de faire bouger notre personnage dans la direction voulue à chaque image APRES le début de l'animation de marche
    void OnAnimatorMove()
    {
        // Permet d'appliquer le mouvement de translation et rotation au Rigidbody de notre personnage
        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude);
        m_Rigidbody.MoveRotation(m_Rotation);
    }

}

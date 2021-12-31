// Code by Cl�ment GASPARD
//***********************************************************************************************************
//                                          Classe WaypointPatrol
//    Classe permettant de g�rer les diff�rents points de contr�les (Waypoints) par lesquels doivent
//      passer les ennemis mobiles (Fant�mes). Lorsque qu'un enememi a atteint le point de contr�le
//                    par lequel il devait passer, alors il passe au point suivant.
//***********************************************************************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // Importation de la biblioth�que IA pour la gestion des d�placement des fant�mes

public class WaypointPatrol : MonoBehaviour
{
    public NavMeshAgent navMeshAgent; // Importation du NavMeshAgent. Agent se d�pla�ant dans la carte g�n�r�e dans Unity dans laquelle le fant�me
                                      // a le droit de se d�placer (la carte emp�che le fant�me de traverser les murs et les objets)

    public Transform[] waypoints; // Vecteur permettant de stocker les diff�rentes destinations (points de passage) du fant�me

    int m_CurrentWaypointIndex; // Variable contenant le point de destination qu'est entrain d'essayer d'atteindre le fant�me

    void Start()
    {
        navMeshAgent.SetDestination(waypoints[0].position); // D�finition de la destination de d�part du "NavMesgAgent"
    }


    void Update()
    {
        // Si la distance restante avant d'atteindre la destination est inf�rieure � celle d�finie dans le NavMeshAgent (0.2)
        if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance) 
        {
            // Alors, on passe � la destination suivante dans la liste, sauf si on est au bout de la liste et on recommande donc au d�but.
            // (On ajoute 1 � l'index en cours, si celui-ci devient �gal au nombre d'�l�ments de la liste, on remet l'index � 0)
            m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length;

            // Attribution de la destination suivante au "NavMesgAgent" (correspondant � celle donn�e par l'index "m_CurrentWaypointIndex")
            navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position); 
        }
    }
}

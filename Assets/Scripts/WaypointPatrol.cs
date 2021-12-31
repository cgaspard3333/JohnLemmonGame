// Code by Clément GASPARD
//***********************************************************************************************************
//                                          Classe WaypointPatrol
//    Classe permettant de gérer les différents points de contrôles (Waypoints) par lesquels doivent
//      passer les ennemis mobiles (Fantômes). Lorsque qu'un enememi a atteint le point de contrôle
//                    par lequel il devait passer, alors il passe au point suivant.
//***********************************************************************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // Importation de la bibliothèque IA pour la gestion des déplacement des fantômes

public class WaypointPatrol : MonoBehaviour
{
    public NavMeshAgent navMeshAgent; // Importation du NavMeshAgent. Agent se déplaçant dans la carte générée dans Unity dans laquelle le fantôme
                                      // a le droit de se déplacer (la carte empêche le fantôme de traverser les murs et les objets)

    public Transform[] waypoints; // Vecteur permettant de stocker les différentes destinations (points de passage) du fantôme

    int m_CurrentWaypointIndex; // Variable contenant le point de destination qu'est entrain d'essayer d'atteindre le fantôme

    void Start()
    {
        navMeshAgent.SetDestination(waypoints[0].position); // Définition de la destination de départ du "NavMesgAgent"
    }


    void Update()
    {
        // Si la distance restante avant d'atteindre la destination est inférieure à celle définie dans le NavMeshAgent (0.2)
        if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance) 
        {
            // Alors, on passe à la destination suivante dans la liste, sauf si on est au bout de la liste et on recommande donc au début.
            // (On ajoute 1 à l'index en cours, si celui-ci devient égal au nombre d'éléments de la liste, on remet l'index à 0)
            m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length;

            // Attribution de la destination suivante au "NavMesgAgent" (correspondant à celle donnée par l'index "m_CurrentWaypointIndex")
            navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position); 
        }
    }
}

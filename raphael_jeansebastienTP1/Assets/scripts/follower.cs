using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class follower : MonoBehaviour
{
    NavMeshAgent agent;
    [SerializeField] GameObject player;
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = player.transform.position;
    }

    void Update()
    {
        if (!Game.isGameOver)
        {
            agent.destination = player.transform.position; // Destination vers le joueur
        }
    }

    private void OnTriggerEnter(Collider other) // Collision avec le joueur, la partie finis
    {
        Game.GameOver();
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PatrolComponent : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float detectionRange;
    [SerializeField] List<Transform> destinations;
    [SerializeField] float waitTime;
    [SerializeField] GameObject jumpscare;

    Node root;
    // Start is called before the first frame update
    void Start()
    {
        SetupTree();
    }
    private void SetupTree()
    {
        Node l1 = new IsWithinRange(target, transform, detectionRange);
        Node l2 = new GoToTarget(target, GetComponent<NavMeshAgent>());
        Node seq1 = new Sequence(new List<Node> { l1, l2 });

        Node l3 = new PatrolTask(destinations, GetComponent<NavMeshAgent>(), waitTime);
        Node sel1 = new Selector(new List<Node> { seq1, l3 });


        root = sel1;
    }

    // Update is called once per frame
    void Update()
    {
        if(root.Evaluate() == NodeState.Success)
        {
            Game.GameOver();
            StartCoroutine(ShowJumpscare(jumpscare));
        }
    }
    public static IEnumerator ShowJumpscare(GameObject obj)
    {
        obj.SetActive(true);
        yield return new WaitForSeconds(2);
        obj.SetActive(false);
    }
}

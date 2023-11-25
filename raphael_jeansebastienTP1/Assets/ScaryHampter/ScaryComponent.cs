using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static HampterBehavior;

[RequireComponent(typeof(NavMeshAgent))]
public class ScaryComponent : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float detectionRange;
    [SerializeField] List<Transform> destinations;
    [SerializeField] float waitTime;
    [SerializeField] float sprintSpeed = 1;
    [SerializeField] float sprintTime = 5;
    [SerializeField] float sprintCooldown = 2;
    [SerializeField] GameObject jumpscareImage;

    NavMeshAgent agent;
    Animator animator;

    Node root;
    // Start is called before the first frame update
    void Start()
    {

        agent = GetComponent<NavMeshAgent>();
       agent.updatePosition = false; //fix?

        animator = GetComponent<Animator>();
        SetupTree();
        agent.updatePosition = false;
        agent.updateRotation = true;
        animator.applyRootMotion = true;
    }
    private void SetupTree()
    {
        Node l1 = new IsWithinRangeHampter(target, transform, detectionRange);
      //  Node l2 = new GoToTargetHampter(target, agent, animator);
     
        Node run = new Run(sprintTime, sprintCooldown, sprintSpeed, agent, animator, target);
        Node walk = new Walk(agent,animator,target);

        Node sel2 = new Selector(new List<Node> {run,walk});

        Node seq1 = new Sequence(new List<Node> { l1, sel2 });

        Node l3 = new PatrolTaskHampter(destinations, agent, waitTime,animator);
        Node sel1 = new Selector(new List<Node> { seq1, l3 });


        root = sel1;
    }

    // Update is called once per frame
    void Update()
    {
        if(root.Evaluate() == NodeState.Success) {
            StartCoroutine(ShowJumpscare(jumpscareImage));
            Game.GameOver();
        }
    }
    void OnAnimatorMove()
    {
        Vector3 rPos = animator.rootPosition;
        rPos.y = agent.nextPosition.y;
        transform.position = rPos; //rotation handled par navmeshagent
        agent.nextPosition = rPos;
    }
    public static IEnumerator ShowJumpscare(GameObject obj)
    {
        obj.SetActive(true);
        yield return new WaitForSeconds(2);
        obj.SetActive(false);
    }
}

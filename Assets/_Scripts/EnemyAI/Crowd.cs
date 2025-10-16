using UnityEngine;
using UnityEngine.AI;

public class Crowd : MonoBehaviour
{
    [Header("Navigation")]
    public NavMeshAgent navMeshAgent;

    [Header("Targets")]
    public GameObject Target;
    public GameObject[] AllTargets;

    [Header("Animations")]
    public Animator animator;
    public string movementBoolName = "IsMoving";

    private bool isMoving = false;

    [Header("Collision Settings")]
    public string playerTag = "Player";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //GetComponent<Animator>().SetInteger("Mode", 1);

        IgnorePlayerCollision();

        FindTarget();
        MoveToTarget();
    }

    // Update is called once per frame
    void Update()
    {
        if(Target != null && Vector3.Distance(this.transform.position, Target.transform.position) <= 0.5f)
        {
            FindTarget();
            MoveToTarget();
        }

        bool currentlyMoving = navMeshAgent.velocity.magnitude > 0.1f;
        if (currentlyMoving != isMoving)
        {
            isMoving = currentlyMoving;
            animator.SetBool(movementBoolName, isMoving);
        }

    }

    public void FindTarget()
    {
        if(Target != null)
        {
            Target.transform.tag = "Target";
        }

        AllTargets = GameObject.FindGameObjectsWithTag("Target");

        if (AllTargets.Length == 0)
        {
            Debug.LogWarning("No available targets with tag 'Target'"); return;
        }

        Target = AllTargets[Random.Range(0, AllTargets.Length)];
        Target.transform.tag = "Untagged";

        //navMeshAgent.destination = Target.transform.position;
    }

    public void MoveToTarget()
    {
        if (Target == null || navMeshAgent == null) return;

        navMeshAgent.SetDestination(Target.transform.position);
        animator?.SetBool(movementBoolName, false);
        isMoving = false;

    }

    private void IgnorePlayerCollision()
    {
        //GameObject player = GameObject 
    } 


}

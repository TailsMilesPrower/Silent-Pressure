using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Crowd : MonoBehaviour
{
    [Header("Navigation")]
    public NavMeshAgent navMeshAgent;

    [Header("Targets")]
    public GameObject Target;
    public GameObject[] AllTargets;
    public bool useRandomTargets = true;
    public float targetReachDistance = 0.8f;
    public float retargetDelay = 0.5f;

    [Header("Animations")]
    //public Animator animator;
    //public string movementBoolName = "IsMoving";

    [Header("Collision Settings")]
    public string playerTag = "Player";

    //private bool isMoving = false;
    private bool isChangingTarget = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //GetComponent<Animator>().SetInteger("Mode", 1);

        IgnorePlayerCollision();

        FindTarget();
        //MoveToTarget();
    }

    // Update is called once per frame
    void Update()
    {
        /* //First Attempt(this plus the bool currentlyMoving part bellow)
        if(Target != null && Vector3.Distance(this.transform.position, Target.transform.position) <= 0.5f)
        {
            FindTarget();
            MoveToTarget();
        }
        */

        /* //Second Attempt
        if (Target == null || isChangingTarget) return;
        float distance = Vector3.Distance(transform.position, Target.transform.position);

        if (distance <= targetReachDistance)
        {
            StartCoroutine(ChangeTargetAfterDelay());
        }

        bool currentlyMoving = navMeshAgent.velocity.magnitude > 0.1f;
        if (currentlyMoving != isMoving)
        {
            isMoving = currentlyMoving;
            //animator.SetBool(movementBoolName, isMoving);
        }
        */
        
        if (Target == null || navMeshAgent == null || isChangingTarget) return;
        if (navMeshAgent.pathPending) return;
        float remaining = navMeshAgent.remainingDistance;
        bool arrived = false;

        if(!navMeshAgent.pathPending && remaining != Mathf.Infinity && !float.IsNaN(remaining))
        {
            arrived = remaining <= Mathf.Max(navMeshAgent.stoppingDistance, targetReachDistance);
        }
        else
        {
            arrived = Vector3.Distance(transform.position, Target.transform.position) <= targetReachDistance;
        }

        if (arrived)
        {
            StartCoroutine(ChangeTargetAfterDelay());
        }

    }

    IEnumerator ChangeTargetAfterDelay()
    {
        if (isChangingTarget) yield break;
        isChangingTarget = true;
        navMeshAgent.isStopped = true;

        //navMeshAgent.ResetPath();

        yield return new WaitForSeconds(retargetDelay);

        if (Target != null && navMeshAgent.isOnNavMesh)
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(Target.transform.position);
        }

        FindTarget();
        isChangingTarget = false;
    }

    public void FindTarget()
    {
        /*
        if (Target != null)
        {
            Target.transform.tag = "Target";
        }
        */

        AllTargets = GameObject.FindGameObjectsWithTag("Target");

        if (AllTargets == null || AllTargets.Length == 0)
        {
            Debug.LogWarning("No available targets with tag 'Target'"); 
            return;
        }

        GameObject newTarget = null;
        if (useRandomTargets)
        {
            if (AllTargets.Length == 1)
            {
                newTarget = AllTargets[0];
            }
            else
            {
                int attempts = 0;
                do
                {
                    newTarget = AllTargets[Random.Range(0, AllTargets.Length)];
                    attempts++;
                }
                while (newTarget == Target && attempts < 10);
            }

        }
        else
        {
            int currentIndex = System.Array.IndexOf(AllTargets, Target);
            int nextIndex = (currentIndex + 1) % AllTargets.Length;
            //if (nextIndex >= AllTargets.Length) nextIndex = 0;
            newTarget = AllTargets[nextIndex];
        }
        
        Target = newTarget;

        if (Target == null && navMeshAgent.isOnNavMesh)
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(Target.transform.position);
        }

        //MoveToTarget();

        //Target = AllTargets[Random.Range(0, AllTargets.Length)];
        //Target.transform.tag = "Untagged";

        //navMeshAgent.destination = Target.transform.position;
    }

    /*
    public void MoveToTarget()
    {
        if (Target == null || navMeshAgent == null) return;

        navMeshAgent.SetDestination(Target.transform.position);
        //animator?.SetBool(movementBoolName, false);
        isMoving = true;

    }
    */

    /*
    public void StopMoving()
    {
        navMeshAgent.ResetPath();
        //animator?.SetBool(movementBoolName, false);
        isMoving = false;
    }
    */

    private void IgnorePlayerCollision()
    {
        GameObject player = GameObject.FindGameObjectWithTag(playerTag);
        if (player == null) return;

        Collider playerCollider = player.GetComponent<Collider>();
        Collider myCollider = GetComponent<Collider>();

        if (playerCollider != null && myCollider != null)
        {
            Physics.IgnoreCollision(myCollider, playerCollider, true);
        }

    } 


}

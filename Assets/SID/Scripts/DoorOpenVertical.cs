using UnityEngine;
using System.Collections;

// AI MADE. For some reason my version never worked right, so I had to ask AI for help.
public class DoorOpenVertical : MonoBehaviour
{
    [Header("Door Movement")]
    public float moveDistance = 2f;       // Positive or negative distance
    public float moveSpeed = 2f;          // Units per second
    public float closeDelay = 2f;         // Seconds before closing

    private Vector3 closedPos;
    private Vector3 openPos;
    private Coroutine moveRoutine;
    private bool playerInside;

    void Start()
    {
        // Save closed position
        closedPos = transform.position;

        // Calculate open position in LOCAL space, then convert to world
        openPos = transform.TransformPoint(new Vector3(moveDistance, 0f, 0f));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = true;
        MoveTo(openPos);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = false;
        StartCoroutine(CloseAfterDelay());
    }

    IEnumerator CloseAfterDelay()
    {
        yield return new WaitForSeconds(closeDelay);
        if (!playerInside)
            MoveTo(closedPos);
    }

    void MoveTo(Vector3 target)
    {
        if (moveRoutine != null) StopCoroutine(moveRoutine);
        moveRoutine = StartCoroutine(Move(target));
    }

    IEnumerator Move(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                target,
                moveSpeed * Time.deltaTime
            );
            yield return null;
        }
        transform.position = target;
    }
}

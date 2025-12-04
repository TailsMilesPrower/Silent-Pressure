using UnityEngine;
using System.Collections;

// AI MADE. For some reason my version never worked right, so I had to ask AI for help.
public class DoorOpen : MonoBehaviour
{
    [Header("Door Movement Settings")]
    public float moveDistance = 2f;
    public float moveSpeed = 2f;
    public float closeDelay = 3f;

    [Header("Trigger Setup")]
    public Collider triggerZone;

    private bool playerInside = false;
    private Vector3 closedPosition;
    private Vector3 openPosition;
    private Coroutine moveRoutine;

    void Start()
    {
        closedPosition = transform.position;
        openPosition = closedPosition + new Vector3(moveDistance, 0f, 0f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            StartMoving(openPosition);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            StartCoroutine(CloseDoorAfterDelay());
        }
    }

    IEnumerator CloseDoorAfterDelay()
    {
        yield return new WaitForSeconds(closeDelay);
        if (!playerInside) StartMoving(closedPosition);
    }

    void StartMoving(Vector3 target)
    {
        if (moveRoutine != null) StopCoroutine(moveRoutine);
        moveRoutine = StartCoroutine(MoveDoor(target));
    }

    IEnumerator MoveDoor(Vector3 target)
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

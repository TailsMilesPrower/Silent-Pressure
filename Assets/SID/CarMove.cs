using UnityEngine;

public class CarMove: MonoBehaviour
{
    public float speed = 2f;
    private float distanceToTravel = 634f;
    private float distanceMoved = 0f;

    private void Update()
    {
        float moveStep = speed * Time.deltaTime;

        transform.Translate(0f, 0f, moveStep);

        distanceMoved += moveStep;

        if (distanceMoved >= distanceToTravel)
        {
            Destroy(gameObject);
        }
    }
}

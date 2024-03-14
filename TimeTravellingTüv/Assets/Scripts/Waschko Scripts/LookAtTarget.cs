using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    public Transform target;
    public float maxDistance = 15f;

    private void Update()
    {
        if (target == null)
        {
            Debug.LogError("Target object is not assigned! Please assign a target in the Inspector.");
            return;
        }

        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        if (distanceToTarget <= maxDistance)
        {
            Vector3 directionToTarget = target.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime);
        }
    }
}
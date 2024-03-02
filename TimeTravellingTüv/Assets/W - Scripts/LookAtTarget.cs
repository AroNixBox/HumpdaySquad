using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    // The target object to look at
    public Transform target;

    // Update is called once per frame
    void Update()
    {
        // Check if the target is assigned
        if (target != null)
        {
            // Get the direction from the current position to the target position
            Vector3 directionToTarget = target.position - transform.position;

            // Calculate the rotation to face the target
            Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);

            // Smoothly rotate towards the target over time
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime);
        }
        else
        {
            Debug.LogError("Target object is not assigned! Please assign a target in the Inspector.");
        }
    }
}

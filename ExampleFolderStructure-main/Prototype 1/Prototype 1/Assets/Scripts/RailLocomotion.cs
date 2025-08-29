using UnityEngine;
using UnityEngine.InputSystem;

public class RailLocomotion : MonoBehaviour
{
    public Transform[] waypoints;
    public float moveSpeed = 1.6f;
    public float stopThreshold = 0.1f;
    public InputActionProperty activateAction; // XRI RightHand Interaction/Activate

    int currentIndex = 0;

    void OnEnable(){ activateAction.action?.Enable(); }
    void OnDisable(){ activateAction.action?.Disable(); }

    void Update()
    {
        if (waypoints == null || waypoints.Length < 2) return;
        if (!(activateAction.action != null && activateAction.action.IsPressed())) return;

        Vector3 target = waypoints[currentIndex].position;
        Vector3 pos = transform.position;
        Vector3 flatTarget = new Vector3(target.x, pos.y, target.z);

        transform.position = Vector3.MoveTowards(pos, flatTarget, moveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.LookRotation(flatTarget - pos, Vector3.up), 8f * Time.deltaTime);

        if (Vector3.Distance(transform.position, flatTarget) < stopThreshold)
            currentIndex = Mathf.Min(currentIndex + 1, waypoints.Length - 1);
    }
}

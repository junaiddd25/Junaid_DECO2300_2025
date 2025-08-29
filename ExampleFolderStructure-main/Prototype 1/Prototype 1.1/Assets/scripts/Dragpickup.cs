using UnityEngine;

public class DragPickup : MonoBehaviour
{
    public float minDistance = 0.6f;
    public float maxDistance = 8f;
    public float followSpeed = 25f;

    Rigidbody held;
    float holdDistance;

    void Update()
    {
        // Scroll to adjust distance while holding
        if (held != null)
        {
            holdDistance = Mathf.Clamp(
                holdDistance + Input.GetAxis("Mouse ScrollWheel") * 2f,
                minDistance, maxDistance
            );
        }

        if (Input.GetMouseButtonDown(0)) TryPick();
        if (Input.GetMouseButton(0) && held) DragFollow();
        if (Input.GetMouseButtonUp(0)) Drop();
    }

    void TryPick()
    {
        Camera cam = Camera.main;
        if (!cam) return;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, ~0, QueryTriggerInteraction.Ignore))
        {
            var rb = hit.rigidbody;
            if (rb && !rb.isKinematic)
            {
                held = rb;
                held.useGravity = false;
                held.velocity = Vector3.zero;
                held.angularVelocity = Vector3.zero;

                // lock the initial distance from camera to hit point
                holdDistance = Mathf.Clamp(hit.distance, minDistance, maxDistance);
            }
        }
    }

    void DragFollow()
    {
        Camera cam = Camera.main;
        if (!cam) return;

        // target = along current mouse ray at locked distance
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Vector3 target = ray.origin + ray.direction * holdDistance;

        Vector3 newPos = Vector3.Lerp(held.position, target, Time.deltaTime * followSpeed);
        held.MovePosition(newPos);
    }

    void Drop()
    {
        if (!held) return;
        held.useGravity = true;
        held = null;
    }
}

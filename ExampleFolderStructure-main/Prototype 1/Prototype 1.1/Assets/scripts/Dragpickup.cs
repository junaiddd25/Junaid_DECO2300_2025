using UnityEngine;

public class DragPickup : MonoBehaviour
{
    [Header("Hold distance & feel")]
    public float minDistance = 0.6f;
    public float maxDistance = 8f;
    public float followSpeed = 25f;

    [Header("Layers to pick")]
    public LayerMask pickableMask = ~0; // all layers by default

    Rigidbody held;
    float holdDistance;

    void Update()
    {
        if (held != null)
        {
            holdDistance = Mathf.Clamp(
                holdDistance + Input.GetAxis("Mouse ScrollWheel") * 2f,
                minDistance, maxDistance
            );
        }

        if (Input.GetMouseButtonDown(0)) TryPickCenterRay();
        if (Input.GetMouseButton(0) && held) DragFollowCenterRay();
        if (Input.GetMouseButtonUp(0)) Drop();
    }

    void TryPickCenterRay()
    {
        Camera cam = Camera.main;
        if (!cam) { Debug.LogWarning("No MainCamera found!"); return; }

        // Ray from the screen CENTER (ignores mouse position)
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.green, 1.5f); // shows in Scene view

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, pickableMask, QueryTriggerInteraction.Ignore))
        {
            Debug.Log("Ray HIT: " + hit.collider.name + " at " + hit.distance.ToString("F2") + "m");
            var rb = hit.rigidbody;
            if (rb && !rb.isKinematic)
            {
                held = rb;
                held.useGravity = false;
                held.velocity = Vector3.zero;
                held.angularVelocity = Vector3.zero;
                holdDistance = Mathf.Clamp(hit.distance, minDistance, maxDistance);
            }
            else
            {
                Debug.Log("Hit object has NO Rigidbody or is kinematic.");
            }
        }
        else
        {
            Debug.Log("Ray MISSED (nothing in front of crosshair).");
        }
    }

    void DragFollowCenterRay()
    {
        Camera cam = Camera.main; if (!cam) return;
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)); // center again
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

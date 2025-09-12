using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SimpleDraggable : MonoBehaviour
{
    Rigidbody rb;
    bool dragging;
    float holdDistance;

    void Awake()
    {
        rb = GetComponent<Rigidbody>(); // ok if null (will move Transform)
    }

    void OnMouseDown()
    {
        var cam = Camera.main; if (!cam) return;
        // lock distance from camera to this object at click time
        holdDistance = Vector3.Distance(cam.transform.position, transform.position);
        dragging = true;

        if (rb)
        {
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    void OnMouseUp()
    {
        dragging = false;
        if (rb) rb.useGravity = true;
    }

    void Update()
    {
        if (!dragging) return;
        var cam = Camera.main; if (!cam) return;

        // point along the current mouse ray, at the locked distance
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Vector3 target = ray.origin + ray.direction * holdDistance;

        if (rb)
            rb.MovePosition(Vector3.Lerp(rb.position, target, Time.deltaTime * 25f));
        else
            transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * 25f);
    }
}

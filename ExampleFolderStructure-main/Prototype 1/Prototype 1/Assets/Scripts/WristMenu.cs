using UnityEngine;

public class WristMenu : MonoBehaviour
{
    public GameObject stickyNotePrefab;
    public Transform cameraTransform;
    public float spawnDistance = 1.2f;

    void Start()
    {
        if (!cameraTransform) cameraTransform = Camera.main.transform;
    }

    public void SpawnSticky()
    {
        if (!stickyNotePrefab || !cameraTransform) return;
        Vector3 pos = cameraTransform.position + cameraTransform.forward * spawnDistance;
        Quaternion rot = Quaternion.LookRotation(cameraTransform.forward, Vector3.up);
        Instantiate(stickyNotePrefab, pos, rot);
    }

    void LateUpdate()
    {
        // Keep menu facing the user for readability
        if (!cameraTransform) return;
        transform.LookAt(cameraTransform);
        transform.Rotate(0, 180, 0);
    }
}

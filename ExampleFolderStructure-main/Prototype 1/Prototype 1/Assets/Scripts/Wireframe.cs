using UnityEngine;
using UnityEngine.InputSystem;

public class CreateWireframe : MonoBehaviour
{
    public GameObject wireframePrefab;
    public float spawnDistance = 1.0f;
    public float cooldown = 0.35f;
    public InputActionProperty spawnAction; // XRI RightHand Interaction/Select

    float lastSpawn = -999f;

    void OnEnable(){ spawnAction.action?.Enable(); }
    void OnDisable(){ spawnAction.action?.Disable(); }

    void Update()
    {
        if (Time.time - lastSpawn < cooldown) return;
        if (spawnAction.action != null && spawnAction.action.WasPressedThisFrame())
        {
            Vector3 pos = transform.position + transform.forward * spawnDistance;
            Instantiate(wireframePrefab, pos, Quaternion.LookRotation(transform.forward, Vector3.up));
            lastSpawn = Time.time;
        }
    }
}

using UnityEngine;
using UnityEngine.InputSystem; // Uses the new Input System directly

[RequireComponent(typeof(CharacterController))]
public class FirstPersonMover : MonoBehaviour
{
    public Transform cameraPivot;     // Assign CameraPivot
    public float moveSpeed = 4.5f;
    public float lookSensitivity = 0.12f;
    public float jumpSpeed = 5f;
    public float gravity = -9.81f;

    CharacterController cc;
    float yaw, pitch;   // yaw = horizontal (body), pitch = vertical (head)
    float yVel;         // vertical velocity for jump/gravity

    void Awake()
    {
        cc = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked; // Lock cursor for mouselook
    }

    void Update()
    {
        // --- Mouse look ---
        if (Mouse.current != null)
        {
            Vector2 m = Mouse.current.delta.ReadValue();
            yaw   += m.x * lookSensitivity;
            pitch -= m.y * lookSensitivity;
            pitch = Mathf.Clamp(pitch, -85f, 85f);
        }
        transform.rotation = Quaternion.Euler(0, yaw, 0);
        if (cameraPivot) cameraPivot.localRotation = Quaternion.Euler(pitch, 0, 0);

        // --- WASD movement (relative to facing) ---
        Vector2 dir = Vector2.zero;
        var kb = Keyboard.current;
        if (kb != null)
        {
            if (kb.wKey.isPressed) dir.y += 1;
            if (kb.sKey.isPressed) dir.y -= 1;
            if (kb.dKey.isPressed) dir.x += 1;
            if (kb.aKey.isPressed) dir.x -= 1;
        }
        Vector3 move = (transform.right * dir.x + transform.forward * dir.y).normalized * moveSpeed;

        // --- Gravity + Jump ---
        if (cc.isGrounded)
        {
            yVel = -0.5f; // small push down to keep grounded
            if (kb != null && kb.spaceKey.wasPressedThisFrame) yVel = jumpSpeed;
        }
        else
        {
            yVel += gravity * Time.deltaTime;
        }

        move.y = yVel;
        cc.Move(move * Time.deltaTime);
    }
}

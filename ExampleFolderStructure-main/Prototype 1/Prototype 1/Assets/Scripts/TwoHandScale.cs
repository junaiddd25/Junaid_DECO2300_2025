using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class TwoHandScale : MonoBehaviour
{
    public float scaleSpeed = 0.6f;
    public Vector2 clamp = new Vector2(0.3f, 2.5f);
    public InputActionProperty rightThumbstick; // XRI RightHand Locomotion/Move (Vector2)

    XRGrabInteractable grab;

    void Awake(){ grab = GetComponent<XRGrabInteractable>(); }
    void OnEnable(){ rightThumbstick.action?.Enable(); }
    void OnDisable(){ rightThumbstick.action?.Disable(); }

    void Update()
    {
        if (grab == null || !grab.isSelected) return;
        Vector2 stick = rightThumbstick.action != null ? rightThumbstick.action.ReadValue<Vector2>() : Vector2.zero;
        float delta = stick.y * scaleSpeed * Time.deltaTime;

        float s = Mathf.Clamp(transform.localScale.x + delta, clamp.x, clamp.y);
        transform.localScale = new Vector3(s, s, s);
    }
}

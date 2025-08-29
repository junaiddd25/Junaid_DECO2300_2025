using UnityEngine;
using System.Collections.Generic;

public class SnapZone : MonoBehaviour
{
    public enum Shape { Circle, Grid }
    public Shape shape = Shape.Circle;

    [Header("Circle")]
    public int circleSlots = 12;
    public float radius = 1.6f;

    [Header("Grid")]
    public int cols = 3, rows = 2;
    public float cell = 0.6f;

    [Header("Snap Tuning")]
    public float captureRadius = 0.4f;
    public float lerpSpeed = 12f;

    readonly List<Transform> inside = new();

    void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody && other.GetComponent<Snappable>() && !inside.Contains(other.transform))
            inside.Add(other.transform);
    }
    void OnTriggerExit(Collider other){ inside.Remove(other.transform); }

    void LateUpdate()
    {
        for (int i = 0; i < inside.Count; i++)
        {
            Transform t = inside[i];
            Vector3 target = GetSlot(i);
            if (Vector3.Distance(t.position, target) < captureRadius)
                t.position = Vector3.Lerp(t.position, target, Time.deltaTime * lerpSpeed);
        }
    }

    Vector3 GetSlot(int i)
    {
        if (shape == Shape.Circle)
        {
            float ang = (i % circleSlots) * Mathf.Deg2Rad * (360f / circleSlots);
            return transform.position + new Vector3(Mathf.Cos(ang), 0f, Mathf.Sin(ang)) * radius;
        }
        int r = i / cols, c = i % cols;
        Vector3 origin = transform.position - new Vector3((cols - 1) * 0.5f * cell, 0, (rows - 1) * 0.5f * cell);
        return origin + new Vector3(c * cell, 0, r * cell);
    }
}

using UnityEngine;

public class VectorProducts : MonoBehaviour
{
    public Transform vectorA;
    public Transform vectorB;
    
    public float dotProduct;
    public float crossProductMagnitude;
    public Vector3 crossProductVector;
    
    void Update()
    {
        // Calculate dot product (measures how aligned two vectors are)
        Vector3 directionA = vectorA.forward;
        Vector3 directionB = (vectorB.position - vectorA.position).normalized;
        dotProduct = Vector3.Dot(directionA, directionB);
        
        // Calculate cross product (creates a perpendicular vector)
        crossProductVector = Vector3.Cross(directionA, directionB);
        crossProductMagnitude = crossProductVector.magnitude;
    }
    
    void OnDrawGizmos()
    {
        // Draw vector A (blue)
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(vectorA.position, vectorA.forward * 2f);
        
        // Draw vector B (red)
        Gizmos.color = Color.red;
        Gizmos.DrawRay(vectorA.position, (vectorB.position - vectorA.position).normalized * 2f);
        
        // Draw cross product vector (green) - perpendicular to both A and B
        Gizmos.color = Color.green;
        Gizmos.DrawRay(vectorA.position, crossProductVector * 1f);
        
        // Draw a small sphere at the end of the cross product vector
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(vectorA.position + crossProductVector * 1f, 0.2f);
        
        // Visualize dot product with color intensity and sphere size
        // The closer the dot product is to 1, the more aligned the vectors are
        float alignment = (dotProduct + 1f) / 2f; // Convert from [-1,1] to [0,1]
        Color dotColor = Color.Lerp(Color.red, Color.green, alignment);
        Gizmos.color = dotColor;
        
        // Draw a sphere whose size and color show the dot product
        float sphereSize = 0.3f + (alignment * 0.5f); // Size varies from 0.3 to 0.8
        Gizmos.DrawWireSphere(vectorA.position, sphereSize);
        
        // Draw a line connecting the vectors when they're well-aligned
        if (dotProduct > 0.7f)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(vectorA.position, vectorB.position);
        }
    }
}
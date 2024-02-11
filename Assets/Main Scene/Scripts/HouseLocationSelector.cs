using Unity.VisualScripting;
using UnityEngine;

public class HouseLocationSelector : MonoBehaviour
{
    private Vector3 initialMousePosition;
    private Vector3 initialCharacterPosition;
    public float coordinateLock = 1;
    void Start()
    {
        
    }
    void Update()
    {
        initialMousePosition = Input.mousePosition;
        initialCharacterPosition = transform.position;
        Vector3 currentMousePosition = Input.mousePosition;

        // Convert mouse position to world space
        Ray ray = Camera.main.ScreenPointToRay(currentMousePosition);
        Plane plane = new Plane(Vector3.up, initialCharacterPosition);
        float distance;
        if (plane.Raycast(ray, out distance))
        {
            Vector3 targetPosition = ray.GetPoint(distance);

            // Snap the target position to integer coordinates
            targetPosition.x = Mathf.Round(targetPosition.x / coordinateLock) * coordinateLock;
            targetPosition.y = 0.01f;
            targetPosition.z = Mathf.Round(targetPosition.z / coordinateLock) * coordinateLock;

            transform.position = targetPosition;
        }
    }
}

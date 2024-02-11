using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMove : MonoBehaviour
{
    public UIManager uiManager;
    private Vector3 initialMousePosition;
    private Vector3 initialCharacterPosition;
    private bool isDragging = false;
    private bool wasDraggedIntoCollision = false;
    private GameObject draggedIntoObject = null;
    public TMP_Text homeUI;
    public TMP_Text nameText;
    public GameObject returnHome;
    public TMP_Text job;

    private GameObject home;

    void Update()
    {
        // Handle mouse drag to move character
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject) // Check if clicked object is the character
                {
                    initialMousePosition = Input.mousePosition;
                    initialCharacterPosition = transform.position;
                    isDragging = true;
                }
            }
        }
        else if (Input.GetMouseButton(0) && isDragging)
        {
            Vector3 currentMousePosition = Input.mousePosition;

            // Convert mouse position to world space
            Ray ray = Camera.main.ScreenPointToRay(currentMousePosition);
            Plane plane = new Plane(Vector3.up, initialCharacterPosition);
            float distance;
            if (plane.Raycast(ray, out distance))
            {
                Vector3 targetPosition = ray.GetPoint(distance);
                transform.position = targetPosition;
            }
        }
        else if (Input.GetMouseButtonUp(0) && isDragging)
        {
            isDragging = false;

            if (wasDraggedIntoCollision && draggedIntoObject != null)
            {
                home = draggedIntoObject;
            }

            // Reset variables
            wasDraggedIntoCollision = false;
            draggedIntoObject = null;
        }

        // Update the UI based on selection status
        uiManager.UpdateSelectionUI(isDragging);

        if (home == null)
        {
            homeUI.text = "Homeless";
            returnHome.gameObject.SetActive(false);
        }
        else
        {
            homeUI.text = "";
            returnHome.gameObject.SetActive(true);
        }
        if (nameText.text != "Person")
        {
            homeUI.gameObject.SetActive(false);
            returnHome.gameObject.SetActive(false);
            job.gameObject.SetActive(false);
        }
        else
        {
            homeUI.gameObject.SetActive(true);
            job.gameObject.SetActive(true);
        }
    }

    // Cancel Selection
    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isDragging = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isDragging)
        {
            if (collision.collider.CompareTag("House"))
            {
                wasDraggedIntoCollision = true;
                draggedIntoObject = collision.gameObject;
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!isDragging && wasDraggedIntoCollision && home == null && collision.gameObject == draggedIntoObject)
        {
            home = draggedIntoObject;
            Debug.Log("home assigned");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == draggedIntoObject)
        {
            wasDraggedIntoCollision = false;
            draggedIntoObject = null;
        }
    }
}

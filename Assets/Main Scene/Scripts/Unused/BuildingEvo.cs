using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BuildingEvo : MonoBehaviour
{
    public UIManager ui; // Make sure this is assigned in the inspector
    public int requiredGold = 10;
    public int requiredWood = 20;
    public int requiredStone = 8;
    public int requiredMetal = 0;
    public int buildingLvl = 1;
    private int lvlCheck = 1;
    public bool selected = false;
    public Button UpgradeButton; // Change the type to Button
    public TMP_Text UpgradeUICost; // Ensure this is assigned in the inspector
    private BoxCollider collider;
    private Rigidbody rb;
    public Transform floor;

    void Start()
    {
        UpgradeUICost.text = "a = 1\nb = 1\nc = 1"; // Use proper new line characters (\n)

        UpgradeButton.onClick.AddListener(OnButtonClick);

        // Assign the BoxCollider component
        collider = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        rb.constraints |= RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;

    }

    void Update()
    {
        if (lvlCheck > buildingLvl)
        {
            buildingLvl += 1;
            requiredGold = buildingLvl * (buildingLvl * requiredGold);
        }

        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    selected = true;
                }
            }
        }
    }

    void OnButtonClick()
    {
        if (ResourceCheck())
        {
            Debug.Log("Upgrade Successful");
        }
        else
        {
            Debug.Log("Insufficient Resources");
        }
    }

    public bool ResourceCheck()
    {
        if (ui.coins >= requiredGold && ui.wood >= requiredWood && ui.stone >= requiredStone && ui.steel >= requiredMetal)
        {
            ui.coins -= requiredGold;
            ui.wood -= requiredWood;
            ui.stone -= requiredStone;
            ui.steel -= requiredMetal;
            buildingLvl += 1;

            // Slow down the scaling rate
            float scalingFactor = 0.5f; // Adjust as needed
            Vector3 newScale = transform.localScale;
            newScale.y += buildingLvl * scalingFactor;
            transform.localScale = newScale;

            // Calculate the amount to move the object upward to keep its bottom touching the floor
            float halfHeight = collider.size.y * 0.5f; // Assuming collider is BoxCollider
            float newPositionY = floor.position.y + halfHeight;
            transform.position = new Vector3(transform.position.x, newPositionY, transform.position.z);
            if (transform.position.y >= floor.position.y)
            {
                transform.position -= new Vector3(0, 0.1f, 0);
            }
            else
            {
                // Object is below the floor
            }
            return true;
        }
        else
        {
            return false;
        }
    }


    void UpdateText(string multiLineText)
    {
        if (UpgradeUICost != null)
        {
            UpgradeUICost.text = multiLineText;
        }
        else
        {
            Debug.LogError("TMP_Text not assigned to a gameObject");
        }
    }
}

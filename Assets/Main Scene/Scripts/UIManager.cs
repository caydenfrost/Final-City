using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    public TMP_Text rssText;
    public TMP_Text selectionText;
    public int wood;
    public int stone;
    public int steel;
    public int coins;
    public GameObject zonePrefab;
    public GameObject housePrefab;
    public bool zoning = false;
    private GameObject instantiatedZone;
    public bool canPlaceHouse = true;
    void Start()
    {
        
    }
    void Update()
    {
        GameObject[] houses = GameObject.FindGameObjectsWithTag("House");
        canPlaceHouse = true; // Assume the house can be placed unless proven otherwise

        if (instantiatedZone != null)
        {
            foreach (GameObject house in houses)
            {
                if (house.transform.position.x == instantiatedZone.transform.position.x &&
                    house.transform.position.z == instantiatedZone.transform.position.z)
                {
                    canPlaceHouse = false;
                    break; // No need to check further, as we've found a house at the same position
                }
            }
        }
        rssText.text = "Wood: " + wood + "\n" + "Stone: " + stone + "\n" + "Metal: " + steel + "\n" + "Coins: " + coins;
    }

    public void UpdateSelectionUI(bool isSelected)
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            // Mouse is clicking on a UI element, do nothing for now
        }
        else
        {
            // Mouse is clicking on something other than a UI element
            if (Input.GetMouseButtonDown(0))
            {
                // Perform a raycast to detect objects clicked by the mouse
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (!hit.collider.gameObject.CompareTag("Floor") && !hit.collider.gameObject.CompareTag("Untagged"))
                    {
                        // Display the tag of the clicked object
                        selectionText.text = hit.collider.gameObject.tag;
                    }
                    else
                    {
                        selectionText.text = "";
                    }
                }
            }
        }
        if (zoning && Input.GetKeyDown(KeyCode.Escape))
        {
            Destroy(instantiatedZone.transform.root.gameObject);
            instantiatedZone = null;
            zoning = false;
        }
        if (zoning && Input.GetMouseButtonDown(0) && canPlaceHouse)
        {
            Instantiate(housePrefab, instantiatedZone.transform.position, Quaternion.identity);
            Destroy(instantiatedZone.transform.root.gameObject);
            instantiatedZone = null;
            zoning = false;
        }
    }

    public void OnButtonClick()
    {
        if (!zoning)
        {
            instantiatedZone = Instantiate(zonePrefab, Vector3.zero, Quaternion.identity);
            zoning = true;
        }
    }
}

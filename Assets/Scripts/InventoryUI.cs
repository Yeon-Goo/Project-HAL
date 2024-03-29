using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    private GameObject slotPrefab;
    public const int numSlots = 5;
    Image[] itemImages = new Image[numSlots];
    Item[] items = new Item[numSlots];
    GameObject[] slots = new GameObject[numSlots];

    // Start is called before the first frame update
    void Start()
    {
        slotPrefab = Resources.Load<GameObject>("Prefabs/UI/Inventory/SlotUI");
        if (slotPrefab == null)
        {
#if DEBUG
            Debug.Log("Inventory::Start() Error! SlotUI Prefab is NULL");
#endif
            return;
        }
        CreateSlots();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateSlots()
    {
        if (slotPrefab != null)
        {
            for (int i = 0; i < numSlots; i++)
            {
                GameObject newSlot = Instantiate(slotPrefab);
                newSlot.name = "ItemSlot_" + i;

                // gameObject.transform.GetChild(0)Àº InventoryBackground
                newSlot.transform.SetParent(gameObject.transform.GetChild(0).transform);

                slots[i] = newSlot;

                // newSlot.transform.GetChild(1)Àº ItemImage
                itemImages[i] = newSlot.transform.GetChild(1).GetComponent<Image>();
            }
        }
    }

    public bool AddItem(Item itemToAdd)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null && items[i].itemType == itemToAdd.itemType && itemToAdd.stackable == true)
            {
                items[i].SetQuantity(items[i].GetQuantity() + 1);

                InventorySlotUI slotScript = slots[i].GetComponent<InventorySlotUI>();
                TMP_Text qtyText = slotScript.qtyText;

                qtyText.enabled = true;
                qtyText.text = items[i].GetQuantity().ToString();
                //Debug.Log("New Quantity = " + items[i].GetQuantity().ToString());

                return true;
            }

            if (items[i] == null)
            {
                items[i] = Instantiate(itemToAdd);
                items[i].SetQuantity(1);
                itemImages[i].sprite = itemToAdd.GetSprite();
                itemImages[i].enabled = true;
                InventorySlotUI slotScript = slots[i].GetComponent<InventorySlotUI>();
                TMP_Text qtyText = slotScript.qtyText;

                qtyText.enabled = true;
                qtyText.text = items[i].GetQuantity().ToString();
                //Debug.Log("New Quantity = " + items[i].GetQuantity().ToString());
                return true;
            }
        }
        return false;
    }
}

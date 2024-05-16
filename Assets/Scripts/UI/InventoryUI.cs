using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class InventoryUI : MonoBehaviour, IPointerClickHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
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

                // gameObject.transform.GetChild(0)은 InventoryBackground
                newSlot.transform.SetParent(gameObject.transform.GetChild(0).transform);

                slots[i] = newSlot;

                // newSlot.transform.GetChild(1)은 ItemImage
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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Vector2 mouse_position = eventData.position;
            Debug.Log("Click at " + mouse_position);

            RectTransform pivot_RectTransform = slots[0].transform.parent.gameObject.GetComponent<RectTransform>();
            Vector2 pivot_position = new Vector2(1920 / 2, 1080) + pivot_RectTransform.anchoredPosition;
            float slot_width = pivot_RectTransform.rect.width;
            float slot_height = pivot_RectTransform.rect.height;
            for (int i = 0; i < numSlots; i++)
            {
                RectTransform slot_RectTransform = slots[i].GetComponent<RectTransform>();
                Vector2 slot_position = pivot_position + slot_RectTransform.anchoredPosition;
                Debug.Log("slot_" + i + " : (" + (slot_position.x - slot_width) + ", " + (slot_position.y - slot_height) + " ~ (" + slot_position.x + ", " + slot_position.y + ")");


                if (slot_position.x - slot_width <= mouse_position.x && mouse_position.x <= slot_position.x && slot_position.y - slot_height <= mouse_position.y && mouse_position.y <= slot_position.y)
                {
                    Debug.Log("Click at slot_" + i);
                }
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Pointer Drag");
        //throw new System.NotImplementedException();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
        
        //Debug.Log("Pointer Enter to " + eventData.pointerEnter.transform.parent.name);
        //throw new System.NotImplementedException();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("Pointer Exit");
    }
}
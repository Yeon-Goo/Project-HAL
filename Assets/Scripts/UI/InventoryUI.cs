using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using System.Security.Cryptography;

public class InventoryUI : MonoBehaviour, IPointerClickHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    private GameObject slotPrefab;
    public const int numSlots = 5;
    Image[] itemImages = new Image[numSlots];
    Item[] items = new Item[numSlots];
    GameObject[] slots = new GameObject[numSlots];

    GameObject duplicatedSlot;
    RectTransform duplicatedSlot_RectTransform;
    TextMeshProUGUI duplicatedSlot_quantityTxt;
    Image duplicatedSlot_Image;

    GameObject inventory_background;
    RectTransform pivot_RectTransform;
    Vector2 pivot_position;
    float slot_width;
    float slot_height;

    [SerializeField]
    bool is_item_clicked = false;

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
        inventory_background = gameObject.transform.GetChild(0).transform.gameObject;
        pivot_RectTransform = inventory_background.GetComponent<RectTransform>();
        pivot_position = new Vector2(1920 / 2, 1080) + pivot_RectTransform.anchoredPosition + new Vector2(-pivot_RectTransform.rect.width / 2, pivot_RectTransform.rect.height / 2);

        CreateSlots();
        
        slot_width = slots[0].GetComponent<RectTransform>().rect.width;
        slot_height = slots[0].GetComponent<RectTransform>().rect.height;

        this.transform.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (is_item_clicked)
        {
            duplicatedSlot_RectTransform.position = Input.mousePosition;
        }
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
                newSlot.transform.SetParent(inventory_background.transform);

                slots[i] = newSlot;

                // newSlot.transform.GetChild(1)은 ItemImage
                itemImages[i] = newSlot.transform.GetChild(1).GetComponent<Image>();
            }

            duplicatedSlot = Instantiate(slotPrefab);
            duplicatedSlot_RectTransform = duplicatedSlot.GetComponent<RectTransform>();
            duplicatedSlot_quantityTxt = duplicatedSlot.transform.GetComponentsInChildren<TextMeshProUGUI>()[0];
            duplicatedSlot_Image = duplicatedSlot.transform.GetComponentsInChildren<Image>()[0];

            GameObject duplicatedSlot_background = duplicatedSlot.transform.GetChild(0).gameObject;
            GameObject duplicatedSlot_tray = duplicatedSlot_background.transform.GetChild(0).gameObject;
            duplicatedSlot_background.GetComponent<Image>().enabled = false;
            duplicatedSlot_tray.GetComponent<Image>().enabled = false;
            duplicatedSlot.name = "DuplicateSlot";
            duplicatedSlot.transform.SetParent(inventory_background.transform);
        }
    }

    public bool AddItem(Item itemToAdd)
    {
        for (int i = 0; i < items.Length - 1; i++)
        {
            if (items[i] != null && items[i].itemType == itemToAdd.itemType && itemToAdd.stackable == true)
            {
                items[i].SetQuantity(items[i].GetQuantity() + 1);

                InventorySlotUI slotScript = slots[i].GetComponent<InventorySlotUI>();
                TMP_Text qtyText = slotScript.transform.GetComponentsInChildren<TMP_Text>()[0];

                if (qtyText != null)
                {
                    qtyText.enabled = true;
                    qtyText.text = items[i].GetQuantity().ToString();
                }

                return true;
            }

            if (items[i] == null)
            {
                items[i] = Instantiate(itemToAdd);
                items[i].SetQuantity(1);
                itemImages[i].sprite = itemToAdd.GetSprite();
                itemImages[i].enabled = true;
                InventorySlotUI slotScript = slots[i].GetComponent<InventorySlotUI>();
                TMP_Text qtyText = slotScript.transform.GetComponentsInChildren<TMP_Text>()[0];

                if (qtyText != null)
                {
                    qtyText.enabled = true;
                    qtyText.text = items[i].GetQuantity().ToString();
                }

                return true;
            }
        }
        return false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Vector2 mouse_position = eventData.position;
        int clicked_slot = GetClickedSlot(eventData.position);
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (0 <= clicked_slot && clicked_slot <= numSlots)
            {
                //Debug.Log("Click at slot_" + clicked_slot);
                if (is_item_clicked)
                {
                    is_item_clicked = false;

                    TMP_Text qty_Text = duplicatedSlot.transform.GetComponentsInChildren<TMP_Text>()[0];
                    Image image = duplicatedSlot.transform.GetChild(1).GetComponent<Image>();
                    qty_Text.enabled = false;
                    image.enabled = false;
                }
                else
                {
                    if (itemImages[clicked_slot].IsActive())
                    {
                        is_item_clicked = true;
                        CopySlot(slots[clicked_slot], duplicatedSlot);

                        TMP_Text qty_Text = duplicatedSlot.transform.GetComponentsInChildren<TMP_Text>()[0];
                        Image image = duplicatedSlot.transform.GetChild(1).GetComponent<Image>();

                        qty_Text.enabled = true;
                        image.enabled = true;
                    }
                }
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (0 <= clicked_slot && clicked_slot <= numSlots)
            {
                Debug.Log("Right Click at slot_" + clicked_slot);
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 mouse_position = eventData.position;
        //Debug.Log("Drag at " + mouse_position);
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

    private int GetClickedSlot(Vector2 mouse_position)
    {
        for (int i = 0; i < numSlots; i++)
        {
            Vector2 slot_pos_leftbotton = new Vector2(pivot_position.x + (40 + slot_width) * i, pivot_position.y - slot_height);
            Vector2 slot_pos_rightupper = new Vector2(slot_pos_leftbotton.x + slot_width, pivot_position.y);

            if (slot_pos_leftbotton.x <= mouse_position.x && mouse_position.x <= slot_pos_rightupper.x && slot_pos_leftbotton.y <= mouse_position.y && mouse_position.y <= slot_pos_rightupper.y)
            {
                return i;
            }
        }

        return -1;
    }

    private void CopySlot(GameObject src, GameObject dst)
    {
        TMP_Text src_qty_text = src.transform.GetComponentsInChildren<TMP_Text>()[0];
        Image src_image = src.transform.GetChild(1).GetComponent<Image>();

        TMP_Text dst_qty_Text = dst.transform.GetComponentsInChildren<TMP_Text>()[0];
        Image dst_image = dst.transform.GetChild(1).GetComponent<Image>();

        dst_qty_Text.text = src_qty_text.text;
        dst_image.sprite = src_image.sprite;
    }

    private void ClearSlot(GameObject slot)
    {
        /*
        items[i] = Instantiate(itemToAdd);
        items[i].SetQuantity(1);
        itemImages[i].sprite = itemToAdd.GetSprite();
        itemImages[i].enabled = true;
        InventorySlotUI slotScript = slots[i].GetComponent<InventorySlotUI>();
        TMP_Text qtyText = slotScript.transform.GetComponentsInChildren<TMP_Text>()[0];

        if (qtyText != null)
        {
            qtyText.enabled = true;
            qtyText.text = items[i].GetQuantity().ToString();
        }

        return true;
        */
    }
}
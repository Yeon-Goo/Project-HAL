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
    [SerializeField]
    Item[] items = new Item[numSlots];
    [SerializeField]
    Image[] itemImages = new Image[numSlots];
    [SerializeField]
    GameObject[] slots = new GameObject[numSlots];

    [SerializeField]
    GameObject duplicatedSlot;
    RectTransform duplicatedSlot_RectTransform;
    Item itemToDrop;
    Image duplicatedSlot_Image;
    TMP_Text duplicatedSlot_QtyText;
    
    GameObject inventory_background;
    RectTransform pivot_RectTransform;
    Vector2 pivot_position;
    float slot_width;
    float slot_height;

    [SerializeField]
    bool is_item_clicked = false;
    int clicked_slot;

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
        inventory_background = gameObject.transform.GetChild(0).gameObject;
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

            if (Input.GetMouseButtonDown(0))
            {
                if (GetClickedSlot(Input.mousePosition) == -1)
                {
                    //ClearSlot(clicked_slot);
                    //ClearSlot(numSlots);

                    //DropItem(clicked_slot, int.Parse(slots[numSlots].GetComponent<InventorySlotUI>().transform.GetComponentsInChildren<TMP_Text>()[0].text));
                    DropItem();

                    is_item_clicked = false;
                }
            }
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
            duplicatedSlot.name = "ItemDuplicateSlot";
            duplicatedSlot.transform.SetParent(inventory_background.transform);
            duplicatedSlot_RectTransform = duplicatedSlot.GetComponent<RectTransform>();
            duplicatedSlot_Image = duplicatedSlot.transform.GetChild(1).GetComponent<Image>();
            duplicatedSlot_QtyText = duplicatedSlot.transform.GetComponentsInChildren<TMP_Text>()[0];
            // Disable Slot Background Image
            duplicatedSlot.transform.GetChild(0).gameObject.GetComponent<Image>().enabled = false;
            // Disable Slot Tray Image
            duplicatedSlot.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>().enabled = false;
        }
    }
    //
    // 요약:
    //     targetSlotNum에 해당하는 Slot을 비웁니다.
    //     만약 targetSlotNum이 numSlots이면 duplicatedSlot을 비웁니다.
    public void ClearSlot(int targetSlotNum)
    {
        // Duplicated Slot
        if (targetSlotNum == numSlots)
        {
            Destroy(itemToDrop, .0f);
            itemToDrop = null;
            duplicatedSlot_Image.enabled = false;
            duplicatedSlot_Image.sprite = null;
            duplicatedSlot_QtyText.text = "";
            duplicatedSlot_QtyText.enabled = false;
        }
        // Item Slot
        else
        {
            TMP_Text qtyText = slots[targetSlotNum].transform.GetComponentsInChildren<TMP_Text>()[0];

            Destroy(items[targetSlotNum], .0f);
            items[targetSlotNum] = null;
            itemImages[targetSlotNum].enabled = false;
            itemImages[targetSlotNum].sprite = null;
            qtyText.text = "";
            qtyText.enabled = false;
        }
        
        Resources.UnloadUnusedAssets();
    }
    //
    // 요약:
    //     duplicatedSlot을 월드에 드랍합니다.
    private bool DropItem()
    {
        // Drop Clicked Slot's Item
        string prefab_path = null;
        GameObject prefab_to_spawn;

        prefab_path = "Prefabs/";

        switch (itemToDrop.ItemType)
        {
            case Item.ItemTypeEnum.COIN:
                prefab_path += "CoinObject";
                break;
            case Item.ItemTypeEnum.GRASS:
                prefab_path += "Glass_PickableObject";
                break;
            case Item.ItemTypeEnum.STONE:
                prefab_path += "Stone_PickableObject";
                break;
        };

        Debug.Log(prefab_path);
        if (prefab_path != null)
        {
            prefab_to_spawn = Resources.Load<GameObject>(prefab_path);

            PlayerEntity player = GameObject.FindWithTag("Player").GetComponent<PlayerEntity>();
            Vector2 playerPos = player.GetPos();
            //Vector2 mousePos = player.GetMousePos().normalized;
            Vector3 spawnPos = new Vector3(playerPos.x, playerPos.y - 0.5f, 0.0f);
            GameObject spawnObject = Instantiate(prefab_to_spawn, spawnPos, Quaternion.identity);
            PickableObjects spawnItem = spawnObject.GetComponent<PickableObjects>();
            spawnItem.Quantity = int.Parse(duplicatedSlot_QtyText.text);
            //Debug.Log("spawnItem.Quantity = " + spawnItem.Quantity);

            // Clear Slot
            ClearSlot(clicked_slot);
            ClearSlot(numSlots);

            return true;
        }
        return false;
    }
    //
    // 요약:
    //     targetSlotNum에 해당하는 아이템을 quantity만큼 월드에 드랍합니다.
    private void DropItem(int targetSlotNum, int quantity)
    {
        
    }
    //
    // 요약:
    //     itemToAdd를 Inventory에 가지고 있다면 itemToAdd가 들어 있는 가장 앞쪽 slot에 itemToAdd를 추가합니다.
    //     itemToAdd를 Inventory에 가지고 있지 않다면 비어 있는 가장 앞쪽 slot에 itemToAdd를 추가합니다.
    //     itemToAdd를 Inventory에 추가하는 데에 성공하면 true를 반환하고, 실패하면 false를 반환합니다.
    public bool AddItem(PickableObjects itemToAdd)
    {
        Debug.Log("itemToAdd = " + itemToAdd + ", itemToAdd.item.ItemType = " + itemToAdd.Item.ItemType);
        
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null && items[i].ItemType == itemToAdd.Item.ItemType && itemToAdd.Item.Stackable == true)
            {
                InventorySlotUI slotScript = slots[i].GetComponent<InventorySlotUI>();
                TMP_Text qtyText = slotScript.transform.GetComponentsInChildren<TMP_Text>()[0];

                if (qtyText != null)
                {
                    //qtyText.enabled = true;
                    int qty = int.Parse(qtyText.text);
                    qty += itemToAdd.Quantity;
                    qtyText.text = qty.ToString();
                }

                return true;
            }
        }
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                items[i] = Instantiate(itemToAdd.Item);
                itemImages[i].sprite = itemToAdd.Item.Sprite;
                itemImages[i].enabled = true;
                InventorySlotUI slotScript = slots[i].GetComponent<InventorySlotUI>();
                TMP_Text qtyText = slotScript.transform.GetComponentsInChildren<TMP_Text>()[0];

                if (qtyText != null)
                {
                    qtyText.enabled = true;
                    int qty = itemToAdd.Quantity;
                    qtyText.text = qty.ToString();
                }

                return true;
            }
        }
        return false;
    }
    //
    // 요약:
    //     targetSlotNum의 slot이 itemToAdd를 가지고 있거나 비어 있다면 itemToAdd를 추가하고 true를 반환합니다.
    //     targetSlotNum의 slot이 itemToAdd를 가지고 있지 않고 비어 있지도 않으면 false를 반환합니다.
    public bool AddItemAt(PickableObjects itemToAdd, int targetSlotNum)
    {
        Debug.Log("Add Item At");
        if (items[targetSlotNum] != null && items[targetSlotNum].ItemType == itemToAdd.Item.ItemType && itemToAdd.Item.Stackable == true)
        {

            InventorySlotUI slotScript = slots[targetSlotNum].GetComponent<InventorySlotUI>();
            TMP_Text qtyText = slotScript.transform.GetComponentsInChildren<TMP_Text>()[0];

            if (qtyText != null)
            {
                //qtyText.enabled = true;
                int qty = int.Parse(qtyText.text);
                Debug.Log("qty = " + qty + ", itemToAdd.Quantity = " + itemToAdd.Quantity);
                qty += itemToAdd.Quantity;
                Debug.Log("qty = " + qty);
                qtyText.text = qty.ToString();
            }

            return true;
        }
        else if (items[targetSlotNum] == null)
        {
            Debug.Log("targetSlot is null");
            items[targetSlotNum] = Instantiate(itemToAdd.Item);
            itemImages[targetSlotNum].sprite = itemToAdd.Item.Sprite;
            itemImages[targetSlotNum].enabled = true;
            InventorySlotUI slotScript = slots[targetSlotNum].GetComponent<InventorySlotUI>();
            TMP_Text qtyText = slotScript.transform.GetComponentsInChildren<TMP_Text>()[0];

            if (qtyText != null)
            {
                qtyText.enabled = true;
                int qty = itemToAdd.Quantity;
                qtyText.text = qty.ToString();
            }

            return true;
        }
        return false;
    }
    //
    // 요약:
    //     targetSlotNum이 비어 있다면 duplicatedSlot을 targetSlotNum에 추가합니다.
    //     targetSlotNum이 비어 있지 않다면 duplicatedSlot과 targetSlotNum를 서로 바꿉니다.
    public bool MoveItem(int targetSlotNum)
    {
        GameObject src = duplicatedSlot;
        TMP_Text src_qty_text = duplicatedSlot_QtyText;
        Image src_image = duplicatedSlot_Image;

        GameObject dst = slots[targetSlotNum];
        TMP_Text dst_qty_text = dst.transform.GetComponentsInChildren<TMP_Text>()[0];
        Image dst_image = itemImages[targetSlotNum];

        int quantity = int.Parse(duplicatedSlot_QtyText.text);
        // Destination Slot is empty
        if (items[targetSlotNum] != null)
        {
            //Debug.Log("item.objectname = " + items[targetSlotNum].ObjectName + ", item.sprite = " + items[targetSlotNum].Sprite + "items.stackable" + items[targetSlotNum].Stackable + "items.itemType" + items[targetSlotNum].ItemType);
        }
            

        if (AddItemAt(new PickableObjects(items[clicked_slot], quantity), targetSlotNum))
        {
            //Debug.Log("Slot is empty");
            if (int.Parse(slots[clicked_slot].transform.GetComponentsInChildren<TMP_Text>()[0].text) - quantity != 0)
            {
                return DeleteItem(clicked_slot, quantity);
            }
            else
            {
                ClearSlot(clicked_slot);
                ClearSlot(numSlots);
                return true;
            }
        }
        // Destination Slot is not empty
        else
        {
            //SwapSlot();
            // Swap
            if (int.Parse(src_qty_text.text) == quantity)
            {
                GameObject srcSlots = slots[clicked_slot];
                string srcQty = srcSlots.transform.GetComponentsInChildren<TMP_Text>()[0].text;
                PickableObjects srcTmp = new PickableObjects(items[clicked_slot], int.Parse(srcQty));
                ClearSlot(clicked_slot);

                GameObject dstSlots = slots[targetSlotNum];
                string dstQty = dstSlots.transform.GetComponentsInChildren<TMP_Text>()[0].text;
                PickableObjects dstTmp = new PickableObjects(items[targetSlotNum], int.Parse(dstQty));
                ClearSlot(targetSlotNum);

                AddItemAt(srcTmp, targetSlotNum);
                AddItemAt(dstTmp, clicked_slot);

                ClearSlot(numSlots);

                return true;
            }
            else
            {
                /*
                Item tmpItems = new Item(items[targetSlotNum]);
                Image tmpItemImages = itemImages[targetSlotNum];
                GameObject tmpSlots = slots[targetSlotNum];

                items[targetSlotNum] = items[targetSlotNum];
                itemImages[targetSlotNum] = itemImages[targetSlotNum];
                slots[targetSlotNum] = slots[targetSlotNum];

                items[targetSlotNum] = tmpItems;
                itemImages[targetSlotNum] = tmpItemImages;
                slots[targetSlotNum] = tmpSlots;


                */
                ClearSlot(numSlots);
                
                return true;
            }

        }
        return false;
    }

    public bool SwapSlot(int src_slot_num, int dst_slot_num)
    {
        return false;
    }
    //
    // 요약:
    //     targetSlotNum이 비어 있다면 duplicatedSlot을 targetSlotNum에 추가합니다.
    //     targetSlotNum이 비어 있지 않다면 duplicatedSlot과 targetSlotNum를 서로 바꿉니다.
    public bool DeleteItem(int target_slot_num, int quantity)
    {
        InventorySlotUI slotScript = slots[target_slot_num].GetComponent<InventorySlotUI>();
        TMP_Text qtyText = slotScript.transform.GetComponentsInChildren<TMP_Text>()[0];

        int new_quantity = int.Parse(qtyText.text) - quantity;
        if (new_quantity > 0)
        {
            qtyText.text = (int.Parse(qtyText.text) - quantity).ToString();
            return true;
        }
        return false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Vector2 mouse_position = eventData.position;
        int current_clicked_slot = GetClickedSlot(eventData.position);
        
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (0 <= current_clicked_slot && current_clicked_slot <= numSlots)
            {
                if (is_item_clicked)
                {
                    is_item_clicked = false;

                    Debug.Log("clicked_slot = " + clicked_slot + ", current_clicked_slot = " + current_clicked_slot);
                    if (current_clicked_slot != clicked_slot)
                    {
                        MoveItem(current_clicked_slot);
                    }
                    ClearSlot(numSlots);
                }
                // 좌클릭으로 모든 수량 선택
                else
                {
                    if (itemImages[current_clicked_slot].IsActive())
                    {
                        is_item_clicked = true;
                        clicked_slot = current_clicked_slot;

                        GameObject src = slots[clicked_slot];
                        TMP_Text src_qty_text = src.transform.GetComponentsInChildren<TMP_Text>()[0];
                        Image src_image = itemImages[clicked_slot];

                        itemToDrop = new Item(items[clicked_slot]);
                        duplicatedSlot_Image.sprite = src_image.sprite;
                        duplicatedSlot_Image.enabled = true;
                        duplicatedSlot_QtyText.text = src_qty_text.text;
                        duplicatedSlot_QtyText.enabled = true;
                    }
                }
            }
        }
        // 우클릭으로 수량을 하나씩 선택
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (0 <= current_clicked_slot && current_clicked_slot <= numSlots)
            {
                // 선택된 아이템이 있음
                if (is_item_clicked)
                {
                    if (current_clicked_slot != clicked_slot)
                    {
                        // 기존에 선택된 아이템을 해제하고 새롭게 아이템을 선택
                    }
                    else
                    {
                        GameObject src = slots[clicked_slot];
                        TMP_Text src_qty_text = src.transform.GetComponentsInChildren<TMP_Text>()[0];

                        int dst_qty;
                        if (duplicatedSlot_QtyText.text.Equals(""))
                        {
                            dst_qty = 0;
                        }
                        else
                        {
                            dst_qty = int.Parse(duplicatedSlot_QtyText.text);
                        }

                        if (dst_qty < int.Parse(src_qty_text.text))
                        {
                            dst_qty++;
                            duplicatedSlot_QtyText.text = dst_qty.ToString();
                        }
                    }
                }
                // 선택된 아이템이 없음
                else
                {
                    if (itemImages[current_clicked_slot].IsActive())
                    {
                        is_item_clicked = true;
                        clicked_slot = current_clicked_slot;

                        GameObject src = slots[clicked_slot];
                        TMP_Text src_qty_text = src.transform.GetComponentsInChildren<TMP_Text>()[0];
                        Image src_image = itemImages[clicked_slot];

                        itemToDrop = new Item(items[clicked_slot]);
                        duplicatedSlot_Image.sprite = src_image.sprite;
                        duplicatedSlot_Image.enabled = true;
                        duplicatedSlot_QtyText.enabled = true;

                        int dst_qty;
                        if (duplicatedSlot_QtyText.text.Equals(""))
                        {
                            dst_qty = 0;
                        }
                        else
                        {
                            dst_qty = int.Parse(duplicatedSlot_QtyText.text);
                        }

                        if (dst_qty < int.Parse(src_qty_text.text))
                        {
                            dst_qty++;
                            duplicatedSlot_QtyText.text = dst_qty.ToString();
                        }
                    }
                }
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

    
}
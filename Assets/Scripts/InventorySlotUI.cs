using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    // 아이템의 수량을 나타내는 TMP_Text
    public TMP_Text qtyText;

    // Start is called before the first frame update
    void Start()
    {
        qtyText = GetComponentInChildren<TMP_Text>();
    }
}

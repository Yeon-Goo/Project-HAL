using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    // �������� ������ ��Ÿ���� TMP_Text
    public TMP_Text qtyText;

    // Start is called before the first frame update
    void Start()
    {
        qtyText = GetComponentInChildren<TMP_Text>();
    }
}

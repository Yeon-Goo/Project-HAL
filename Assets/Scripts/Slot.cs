using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    // quantity_text;
    public TMP_Text qtyText;

    // Start is called before the first frame update
    void Start()
    {
        qtyText = GetComponentInChildren<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

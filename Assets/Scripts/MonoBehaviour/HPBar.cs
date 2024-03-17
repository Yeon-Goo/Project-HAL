using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HPBar : MonoBehaviour
{
    public HP cur_HP;
    [HideInInspector]
    private PlayerEntity player;
    public Image hpbar_image;
    public TMP_Text hpbar_text;
    private float max_HP;

    // Start is called before the first frame update
    void Start()
    {
        if (player != null)
        {
            max_HP = player.max_HP;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            hpbar_image.fillAmount = cur_HP.value / max_HP;
            hpbar_text.text = "HP:" + (hpbar_image.fillAmount * 100);
        }
    }

    public void SetPlayer(PlayerEntity player)
    {
        this.player = player;
    }
}

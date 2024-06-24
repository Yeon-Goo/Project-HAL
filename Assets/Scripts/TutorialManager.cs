using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public Image[] images; // 표시할 이미지들을 배열로 설정
    private int currentImageIndex = 0;

    void Start()
    {
        // 모든 이미지를 비활성화
        foreach (var img in images)
        {
            img.gameObject.SetActive(false);
        }

        // 첫 번째 이미지를 활성화
        if (images.Length > 0)
        {
            images[currentImageIndex].gameObject.SetActive(true);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DisplayNextImage();
        }
    }

    private void DisplayNextImage()
    {
        if (currentImageIndex < images.Length)
        {
            // 현재 이미지를 비활성화
            images[currentImageIndex].gameObject.SetActive(false);

            // 다음 이미지 인덱스로 이동
            currentImageIndex++;

            // 다음 이미지가 있으면 활성화
            if (currentImageIndex < images.Length)
            {
                images[currentImageIndex].gameObject.SetActive(true);
            }
        }
    }
}

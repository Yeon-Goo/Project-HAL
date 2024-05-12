using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 게임 매니저는 오직 하나여야 한다
    public static GameManager sharedInstance = null;
    public static VirtualCameraManager virtualcameracontroller = null;

    public SpawnPoint playerSpawnPoint;

    private void Awake()
    {
        if (sharedInstance != null && sharedInstance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            sharedInstance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SetupScene();
    }

    public void SetupScene()
    {
        SetupVirtualCameraController();
        SpawnPlayer();
    }

    private void SetupVirtualCameraController()
    {
        if (virtualcameracontroller != null && virtualcameracontroller != this)
        {
            Debug.Log("Destroy virtual camera controller\n");
            Destroy(virtualcameracontroller);
        }
        else
        {
            virtualcameracontroller = GetComponent<VirtualCameraManager>();
        }
    }

    public void SpawnPlayer()
    {
        if (playerSpawnPoint != null)
        {
            GameObject player_entity = playerSpawnPoint.SpawnObject();
        }
    }
}

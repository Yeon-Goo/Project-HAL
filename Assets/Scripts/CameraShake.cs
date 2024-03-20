using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private CinemachineVirtualCamera virtualcamera;
    private CinemachineFramingTransposer framingtransposer;

    public float ShakeAmount;
    float ShakeTime;
    Vector3 initialPosition;

    public void VibrateForTime(float time)
    {
        Debug.Log("vibrate for " + time);
        ShakeTime = time;
    }

    private void Start()
    {
        virtualcamera = GetComponent<CinemachineVirtualCamera>();
        framingtransposer = virtualcamera.GetCinemachineComponent<CinemachineFramingTransposer>();

        initialPosition = new Vector2(0.5f, 0.5f);
    }

    private void Update()
    {
        if (ShakeTime > 0)
        {
            Vector2 tmp_vec = Random.insideUnitSphere * ShakeAmount + initialPosition;
            framingtransposer.m_ScreenX = tmp_vec.x;
            framingtransposer.m_ScreenY = tmp_vec.y;
            ShakeTime -= Time.deltaTime;
        }
        else
        {
            ShakeTime = 0.0f;
            framingtransposer.m_ScreenX = initialPosition.x;
            framingtransposer.m_ScreenY = initialPosition.y;
        }
    }
}
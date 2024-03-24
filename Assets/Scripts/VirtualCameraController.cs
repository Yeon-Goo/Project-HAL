using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualCameraController : CinemachineExtension
{
    // 유닛(타일) 하나의 픽셀
    private float PixelsPerUnit = 32.0f;
    // 유닛 하나를 얼만큼 확대할지
    private int pixelsPerUnitScale = 3;

    private CinemachineVirtualCamera virtualcamera;
    private CinemachineFramingTransposer framingtransposer;
    
    private float ShakeTime = 0.0f;
    private const float ShakeAmount = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        virtualcamera = GetComponent<CinemachineVirtualCamera>();
        framingtransposer = virtualcamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        //camera_shake = virtualcamera.GetComponent<CameraShake>();

        // 가상 카메라의 크기 (수직 해상도 / PPU) * 0.5
        virtualcamera.m_Lens.OrthographicSize = (Screen.height / (PixelsPerUnit * pixelsPerUnitScale)) * 0.5f;

        // 가상 카메라의 데드 존이 트래킹 포인트를 따라 잡을 때까지의 시간
        framingtransposer.m_XDamping = 0.0f;
        framingtransposer.m_YDamping = 0.0f;

        // 가상 카메라의 데드 존의 크기
        framingtransposer.m_DeadZoneWidth = 0.0f;
        framingtransposer.m_DeadZoneHeight = 0.0f;
    }

    
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            ShakeTime = 1.0f;
        }

        // 화면 흔들기
        if (ShakeTime > 0)
        {
            Vector2 tmp_vec = Random.insideUnitSphere * ShakeAmount;
            Debug.Log("Vector2 = " + tmp_vec);
            framingtransposer.m_ScreenX = 0.5f + tmp_vec.x;
            framingtransposer.m_ScreenY = 0.5f + tmp_vec.y;
            ShakeTime -= Time.deltaTime;
        }
        else
        {
            ShakeTime = 0.0f;
            framingtransposer.m_ScreenX = 0.5f;
            framingtransposer.m_ScreenY = 0.5f;
        }
    }
    

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        // 화면 지터링 해결 코드
        if (stage == CinemachineCore.Stage.Body)
        {
            // 가상 카메라의 좌표
            Vector3 pos = state.FinalPosition;
            // 픽셀 단위로 반올림한 가상 카메라의 좌표
            Vector3 pos2 = new Vector3(Round(pos.x), Round(pos.y), pos.z);

            // 위의 두 좌표의 차이를 가상 카메라에 반영
            state.PositionCorrection += pos2 - pos;
        }
    }

    float Round(float x)
    {
        return Mathf.Round(x * PixelsPerUnit) / PixelsPerUnit;
    }
}

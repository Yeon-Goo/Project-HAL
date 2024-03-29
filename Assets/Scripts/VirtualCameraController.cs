using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualCameraController : CinemachineExtension
{
    // 유닛(타일) 하나의 픽셀
    private float pixelsperunit = 32.0f;
    // 유닛 하나를 얼만큼 확대할지
    private int pixelsperunit_scale = 3;

    // Component Variables
    private CinemachineVirtualCamera virtualcamera;
    private CinemachineFramingTransposer framingtransposer;

    // Camera Shake
    private float camera_shake_amount;
    private float camera_shake_time;
    private Vector3 camera_shake_initial_pos;

    // Start is called before the first frame update
    void Start()
    {
        virtualcamera = GetComponent<CinemachineVirtualCamera>();
        framingtransposer = virtualcamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        //camera_shake = virtualcamera.GetComponent<CameraShake>();

        // 가상 카메라의 크기 (수직 해상도 / PPU) * 0.5
        virtualcamera.m_Lens.OrthographicSize = (Screen.height / (pixelsperunit * pixelsperunit_scale)) * 0.5f;

        // 가상 카메라의 데드 존이 트래킹 포인트를 따라 잡을 때까지의 시간
        framingtransposer.m_XDamping = 0.0f;
        framingtransposer.m_YDamping = 0.0f;

        // 가상 카메라의 데드 존의 크기
        framingtransposer.m_DeadZoneWidth = 0.0f;
        framingtransposer.m_DeadZoneHeight = 0.0f;

        camera_shake_amount = 0.0f;
        camera_shake_time = 0.0f;
        camera_shake_initial_pos = new Vector3(0.5f, 0.5f, 0.0f);
    }

    
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            VibrateForTimeAndAmount();
        }

        CameraShake();
    }

    public void VibrateForTimeAndAmount(float time = 0.5f, float amount = 0.0075f)
    {
        Debug.Log("vibrate for " + time + "with amount " + amount);
        camera_shake_time = time;
        camera_shake_amount = amount;
    }

    private void CameraShake()
    {
        // 화면 흔들기
        if (camera_shake_time > 0)
        {
            Vector2 tmp_vec = Random.insideUnitSphere * camera_shake_amount;
            framingtransposer.m_ScreenX = 0.5f + tmp_vec.x;
            framingtransposer.m_ScreenY = 0.5f + tmp_vec.y;
            camera_shake_time -= Time.deltaTime;
        }
        else
        {
            camera_shake_time = 0.0f;
            framingtransposer.m_ScreenX = camera_shake_initial_pos.x;
            framingtransposer.m_ScreenY = camera_shake_initial_pos.y;
        }
    }

    // 화면 지터링 해결
    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
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

    // 반올림
    private float Round(float x)
    {
        return Mathf.Round(x * pixelsperunit) / pixelsperunit;
    }
}

using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualCameraController : CinemachineExtension
{
    // ����(Ÿ��) �ϳ��� �ȼ�
    private float PixelsPerUnit = 32.0f;
    // ���� �ϳ��� ��ŭ Ȯ������
    private int pixelsPerUnitScale = 3;

    private CinemachineVirtualCamera virtualcamera;
    private CinemachineFramingTransposer framingtransposer;
    
    /*
    private CameraShake camera_shake;
    private const float ShakeTime = 1.0f;
    private const float ShakeAmount;
    */

    // Start is called before the first frame update
    void Start()
    {
        virtualcamera = GetComponent<CinemachineVirtualCamera>();
        framingtransposer = virtualcamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        //camera_shake = virtualcamera.GetComponent<CameraShake>();

        // ���� ī�޶��� ũ�� (���� �ػ� / PPU) * 0.5
        virtualcamera.m_Lens.OrthographicSize = (Screen.height / (PixelsPerUnit * pixelsPerUnitScale)) * 0.5f;

        // ���� ī�޶��� ���� ���� Ʈ��ŷ ����Ʈ�� ���� ���� �������� �ð�
        framingtransposer.m_XDamping = 0.0f;
        framingtransposer.m_YDamping = 0.0f;

        // ���� ī�޶��� ���� ���� ũ��
        framingtransposer.m_DeadZoneWidth = 0.0f;
        framingtransposer.m_DeadZoneHeight = 0.0f;
    }

    /*
    private void Update()
    {
        // ȭ�� ����
        if (Input.GetKeyUp(KeyCode.Space))
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
    */

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        // ȭ�� ���͸� �ذ� �ڵ�
        if (stage == CinemachineCore.Stage.Body)
        {
            // ���� ī�޶��� ��ǥ
            Vector3 pos = state.FinalPosition;
            // �ȼ� ������ �ݿø��� ���� ī�޶��� ��ǥ
            Vector3 pos2 = new Vector3(Round(pos.x), Round(pos.y), pos.z);

            // ���� �� ��ǥ�� ���̸� ���� ī�޶� �ݿ�
            state.PositionCorrection += pos2 - pos;
        }
    }

    float Round(float x)
    {
        return Mathf.Round(x * PixelsPerUnit) / PixelsPerUnit;
    }
}

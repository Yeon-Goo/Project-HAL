using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualCameraController : CinemachineExtension
{
    // ����(Ÿ��) �ϳ��� �ȼ�
    private float pixelsperunit = 32.0f;
    // ���� �ϳ��� ��ŭ Ȯ������
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

        // ���� ī�޶��� ũ�� (���� �ػ� / PPU) * 0.5
        virtualcamera.m_Lens.OrthographicSize = (Screen.height / (pixelsperunit * pixelsperunit_scale)) * 0.5f;

        // ���� ī�޶��� ���� ���� Ʈ��ŷ ����Ʈ�� ���� ���� �������� �ð�
        framingtransposer.m_XDamping = 0.0f;
        framingtransposer.m_YDamping = 0.0f;

        // ���� ī�޶��� ���� ���� ũ��
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
        // ȭ�� ����
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

    // ȭ�� ���͸� �ذ�
    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
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

    // �ݿø�
    private float Round(float x)
    {
        return Mathf.Round(x * pixelsperunit) / pixelsperunit;
    }
}

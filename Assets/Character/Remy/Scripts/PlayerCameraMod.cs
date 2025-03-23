using Unity.Cinemachine;
using UnityEngine;

public class PlayerCameraMod : MonoBehaviour
{
    public CinemachineCamera _cinemachineCamera;

    [Header("Camera Targets")]
    public Transform _normalTarget;
    public Transform _OTS_Target;

    public void SetCameraTrackimgTarget(Transform target)
    {
        _cinemachineCamera.Follow = target;
        _cinemachineCamera.LookAt = target;
    }
}

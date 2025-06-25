using Unity.Cinemachine;
using UnityEngine;

public class CameraForce : MonoBehaviour
{
    [SerializeField] private CinemachineCamera virtualCamera;
    [SerializeField] private Transform player;

    private void Start()
    {
        if (virtualCamera != null && player != null)
        {
            virtualCamera.Follow = player;
            
        }
        
    }
}

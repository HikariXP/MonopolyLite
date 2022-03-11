using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 管理摄像机
/// </summary>
public class CameraController : MonoBehaviour
{
    public static CameraController Instance;

    public CinemachineVirtualCamera FollowPlayerCamera;

    public CinemachineVirtualCamera FreeLookCamera;

    private void Awake()
    {
        Instance = this;
    }

    public void FollowThePlayer(Player player)
    {
        FollowPlayerCamera.Follow = player.transform;
    }
}

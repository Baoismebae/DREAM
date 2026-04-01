using UnityEngine;

public class SyncCameraLens : MonoBehaviour
{
    private Camera myCam;
    private Camera parentCam;

    void Start()
    {
        myCam = GetComponent<Camera>();
        if (transform.parent != null)
        {
            parentCam = transform.parent.GetComponent<Camera>();
        }
    }

    void LateUpdate()
    {
        // Liên tục copy độ zoom từ Camera Mẹ sang Camera Con
        if (parentCam != null && myCam != null)
        {
            myCam.orthographicSize = parentCam.orthographicSize;
        }
    }
}

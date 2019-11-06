using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    #region Variables
    [SerializeField] private Transform player;
    private Vector3 target, mousePos, refVel, shakeOffset, shakeVector;
    [Tooltip("how far the camera should be when the mouse is at the end of the screan")]
    private float cameraDistance = 3.5f;
    private float smoothTime = 0.2f;
    private float zStart;
    private float shakeMag, shakeTimeEnd;
    private bool shaking;
    #endregion

    private void Start()
    {
        target = this.transform.position;
        zStart = this.transform.position.z;
    }

    private void FixedUpdate()
    {
        try
        {
            mousePos = CaptureMousePos();
            shakeOffset = UpdateShake();
            target = UpdateTargetPos();
            UpdateCameraPosition();
        }
        catch { Debug.Log("Camera.maim not found"); }
    }

    private Vector3 CaptureMousePos()
    {
        Vector2 ret = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        ret *= 2;
        ret -= Vector2.one;
        float max = 0.9f;
        if (Mathf.Abs(ret.x) > max || Mathf.Abs(ret.y) > max)
            ret = ret.normalized;
        return ret;
    }

    private Vector3 UpdateTargetPos()
    {
        Vector3 mouseOffset = mousePos * cameraDistance;
        Vector3 ret = player.position + mouseOffset;
        ret += shakeOffset;
        ret.z = zStart;
        return ret;
    }

    private void UpdateCameraPosition()
    {
        Vector3 tempPos = Vector3.SmoothDamp(this.transform.position, target, ref refVel, smoothTime);
        transform.position = tempPos;
    }

    public void Shake(Vector3 direction, float magnitude, float lenth)
    {
        shaking = true;
        shakeVector = direction;
        shakeMag = magnitude;
        shakeTimeEnd = Time.time + lenth;
    }

    private Vector3 UpdateShake()
    {
        if (!shaking || Time.time > shakeTimeEnd)
        {
            shaking = false;
            return Vector3.zero;
        }
        Vector3 tempOffset = shakeVector;
        shakeOffset *= shakeMag;
        return tempOffset;
    }

    public void SetPlayer(GameObject player)
    {
        this.player = player.transform;
        this.Start();
    }

}
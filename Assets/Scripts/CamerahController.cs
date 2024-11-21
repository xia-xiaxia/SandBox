using System;
using System.Collections;
using UnityEngine;
public class CameraController : MonoBehaviour
{
    //WSAD相机移动速度
    public float moveSpeed = 1.0f;
    //相机当前移动方向
    private Vector3 moveDirection;


    //方向键控制缩放
    public float zoomSpeed = 10f;
    private float zoomDistance = 0f;

    //视角旋转
    Vector3 originPosition;
    public Vector3 angle;
    public float xSpeed;
    public float ySpeed;
    public float yMax;
    public float yMin;
    public float smoothTime = 0.1f;


    private void Start()
    {
        Application.targetFrameRate = 60; //帧率改变（一秒内调用Update的次数）
        originPosition = transform.position;
        angle = transform.eulerAngles;
    }
    void Update()
    {
        {
            float moveX = 0f;
            float moveZ = 0f;

            if (Input.GetKey(KeyCode.A))
                moveX = -1f;
            else if (Input.GetKey(KeyCode.D))
                moveX = 1f;

            if (Input.GetKey(KeyCode.W))
                moveZ = 1f;
            else if (Input.GetKey(KeyCode.S))
                moveZ = -1f;

            Vector3 moveDirection = new Vector3(moveX, 0, moveZ);

            // 使用Translate移动
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
        }

        //实现第一人称视角相机跟随鼠标移动
        angle.x += Input.GetAxis("Mouse X") * xSpeed * Time.deltaTime;
        angle.y -= Input.GetAxis("Mouse Y") * ySpeed * Time.deltaTime;
        angle.y = Mathf.Clamp(angle.y, yMin, yMax);
        Quaternion rotation = Quaternion.Euler(angle.y, angle.x, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, smoothTime);

        StartCoroutine(Zoom());
    }

    IEnumerator Zoom()
    {
        Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, 30f, 100f);
        if (Input.GetKey(KeyCode.UpArrow))
        {
            yield return new WaitForSeconds(0.01f);
            Camera.main.fieldOfView--;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            yield return new WaitForSeconds(0.01f);
            Camera.main.fieldOfView++;
        }
    }
}
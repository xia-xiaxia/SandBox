using System;
using System.Collections;
using UnityEngine;
public class CameraController : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    private Vector3 moveDirection;


    public float zoomSpeed = 10f;
    private float zoomDistance = 0f;

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
        angle.y = -90;
        
    }
    void Update()
    {
        {
            float moveX = 0f;
            float moveZ = 0f;
            float moveY = 0f;  

            if (Input.GetKey(KeyCode.A))
                moveX = -1f;
            else if (Input.GetKey(KeyCode.D))
                moveX = 1f;

            if (Input.GetKey(KeyCode.W))
                moveZ = 1f;
            else if (Input.GetKey(KeyCode.S))
                moveZ = -1f;

            if(Input.GetKey(KeyCode.DownArrow))
                moveY = -1f;
            else if(Input.GetKey(KeyCode.UpArrow))
                moveY = 1f;



            Vector3 moveDirection = new Vector3(moveX, moveY, moveZ);

            // 使用Translate移动
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
        }

        StartCoroutine(Rotation());
        angle.y = Mathf.Clamp(angle.y, yMin, yMax);
        Quaternion rotation = Quaternion.Euler(50, angle.y, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, smoothTime);

        StartCoroutine(Zoom());



    }

    IEnumerator Rotation()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            yield return new WaitForSeconds(0.01f);
            angle.y += xSpeed * Time.deltaTime;
        }
        else if ((Input.GetKey(KeyCode.LeftArrow)))
        {
            yield return new WaitForSeconds(0.01f);
            angle.y -= ySpeed * Time.deltaTime;
        }
    }


    IEnumerator Zoom()
    {
        Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, 30f, 100f);
        if (Input.GetKey(KeyCode.Q))
        {
            yield return new WaitForSeconds(0.01f);
            Camera.main.fieldOfView--;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            yield return new WaitForSeconds(0.01f);
            Camera.main.fieldOfView++;
        }
    }
}
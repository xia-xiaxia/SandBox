using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPreview : MonoBehaviour
{
    public GameObject[] previewObject;  // 预览物体（透明物体）
    private CreatModeState ObjectsCreatAndDestory;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        previewObject[ObjectsCreatAndDestory.signCount].SetActive(true);  // 初始化时显示预览物体
    }

    void Update()
    {
        UpdatePreviewObjectPosition();
    }

    void UpdatePreviewObjectPosition()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))  // 如果射线击中某个物体
        {
            previewObject[ObjectsCreatAndDestory.signCount].transform.position = hit.point;  // 更新预览物体位置
            previewObject[ObjectsCreatAndDestory.signCount].transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);  // 调整预览物体的旋转，使其与表面平行
        }
    }

}



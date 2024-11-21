using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPreview : MonoBehaviour
{
    public GameObject[] previewObject;  // Ԥ�����壨͸�����壩
    private CreatModeState ObjectsCreatAndDestory;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        previewObject[ObjectsCreatAndDestory.signCount].SetActive(true);  // ��ʼ��ʱ��ʾԤ������
    }

    void Update()
    {
        UpdatePreviewObjectPosition();
    }

    void UpdatePreviewObjectPosition()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))  // ������߻���ĳ������
        {
            previewObject[ObjectsCreatAndDestory.signCount].transform.position = hit.point;  // ����Ԥ������λ��
            previewObject[ObjectsCreatAndDestory.signCount].transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);  // ����Ԥ���������ת��ʹ�������ƽ��
        }
    }

}



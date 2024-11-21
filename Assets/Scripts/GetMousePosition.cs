using UnityEngine;

public class GetMousePosition : MonoBehaviour
{
    public Vector3 mousePosition;

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            mousePosition = hit.point;
        }
        else
        {
            // ���δ����ʱ��Ĭ��ָ������ԭ���ĳ��Ĭ��ֵ
            mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
        }
    }
}


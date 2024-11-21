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
            // 鼠标未命中时，默认指向世界原点或某个默认值
            mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
        }
    }
}


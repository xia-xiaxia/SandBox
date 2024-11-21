using System;
using UnityEngine;


public class CreatModeState : IState<StateID>
{
    public int signCount;     // 当前生成物体的序号
    private int maxSignCount;  // 能够生成物体的数量
    private Vector3 mousePosition;       // 鼠标位置
    private float mouseDepth;            // 鼠标位置的深度
    public GameObject[] prefabs;
    public GameObject[] previewObject;
    private LayerMask hitLayer;
    private GameObject previewInstance;
    private GameObject currentPreviewInstance;
    private LineRenderer lineRenderer;
    public GameObject mainCamera;
    public GetMousePosition GMP;

    public CreatModeState(GameObject camera, int maxCount, GameObject[] prefabArray, GameObject[] previewArray, LayerMask layer, LineRenderer renderer, float mouseDepthValue, GetMousePosition gmp)
    {
        mainCamera = camera;
        maxSignCount = maxCount;
        prefabs = prefabArray;
        previewObject = previewArray;
        hitLayer = layer;
        lineRenderer = renderer;
        mouseDepth = mouseDepthValue;
        GMP = gmp;

        signCount = 0;
        previewInstance = null;
        currentPreviewInstance = null;
    }

    public StateID Id => StateID.Creat_Mode;

    public void OnEnterState()
    {
        Debug.Log("Entering Create Mode");
        SetupLineRenderer();
    }

    public void OnUpdateState()
    {
        MouseWheelSwitchItems();
        UpdatePreviewVisibility();
        UpdatePreviewObjectPosition();
        DrawPreviewLine();

        mousePosition = GMP.mousePosition;
        Vector3 screenPosition = Input.mousePosition;
        screenPosition.z = mouseDepth;
        mousePosition = mainCamera.GetComponent<Camera>().ScreenToWorldPoint(screenPosition);

        if (Input.GetMouseButtonDown(0))
        {
            CreatePrefabObject(signCount);
        }

        if (Input.GetMouseButtonDown(1))
        {
            DeleteObjectAtMousePosition();
        }
    }

    public void OnExitState()
    {
        Debug.Log("Exiting Create Mode");
        lineRenderer.enabled = false;
        if (previewInstance != null)
        {
            GameObject.Destroy(previewInstance);
            previewInstance = null;
        }
    }

    public bool TransitionState(out StateID id)
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            id = StateID.Free_Mode;
            return true;
        }

        id = default;
        return false;
    }

    private void SetupLineRenderer()
    {
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.green;
        lineRenderer.endColor = Color.green;
        lineRenderer.enabled = true;
    }

    private void CreatePrefabObject(int i)
    {
        GameObject obj = GameObject.Instantiate(prefabs[i], mousePosition, Quaternion.identity);
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotationZ;

            if (Input.GetMouseButtonUp(0))
            {
                rb.constraints = ~RigidbodyConstraints.FreezeRotationZ;
            }
        }
    }

    private void DeleteObjectAtMousePosition()
    {
        Ray ray = mainCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, hitLayer))
        {
            GameObject objectInFront = hit.collider.gameObject;
            if (IsDestructible(objectInFront))
            {
                GameObject.Destroy(objectInFront);
            }
        }
    }

    private void UpdatePreviewObjectPosition()
    {
        if (previewInstance == null)
        {
            previewInstance = GameObject.Instantiate(previewObject[signCount], mousePosition, Quaternion.identity);
            currentPreviewInstance = previewInstance;
        }
        else
        {
            if (signCount != Array.IndexOf(previewObject, currentPreviewInstance))
            {
                GameObject.Destroy(currentPreviewInstance);
                previewInstance = GameObject.Instantiate(previewObject[signCount], mousePosition, Quaternion.identity);
                currentPreviewInstance = previewInstance;
            }
            previewInstance.transform.position = mousePosition;
            previewInstance.transform.rotation = Quaternion.identity;

            Renderer renderer = previewInstance.GetComponent<Renderer>();
            if (renderer != null)
            {
                Color color = renderer.material.color;
                color.a = 0.5f;
                renderer.material.color = color;
            }
        }
    }

    private void MouseWheelSwitchItems()
    {
        float scroll = Input.mouseScrollDelta.y;
        signCount += (scroll > 0) ? 1 : (scroll < 0) ? -1 : 0;
        if (signCount > maxSignCount-1)
            signCount = 0;
        else if (signCount < 0)
            signCount = maxSignCount-1;
    }

    private void UpdatePreviewVisibility()
    {
        for (int i = 0; i < previewObject.Length; i++)
        {
            previewObject[i].SetActive(i == signCount);
        }
    }

    private bool IsDestructible(GameObject gameObject)
    {
        if (gameObject.name == "Ground")
            return false;

        foreach (var preview in previewObject)
        {
            if (gameObject == preview)
                return false;
        }

        return true;
    }

    private void DrawPreviewLine()
    {
        // 从鼠标位置发射向下的射线
        Ray ray = new Ray(mousePosition, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, hitLayer))
        {
            lineRenderer.SetPosition(0, mousePosition);
            lineRenderer.SetPosition(1, hit.point);
        }else
        {
            lineRenderer.SetPosition(0, mousePosition);
            lineRenderer.SetPosition(1, mousePosition);
        }
    }

}

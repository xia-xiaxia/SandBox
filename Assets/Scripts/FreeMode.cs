using UnityEngine;

public class FreeModeState : IState<StateID>
{
    private Vector3 mousePosition;
    private float mouseDepth;
    private LayerMask hitLayer;
    private LineRenderer lineRenderer;
    private GameObject mainCamera;
    private GetMousePosition GMP;
    private Vector3 offset;
    private float zDistance;
    private bool isDragging = false;
    private GameObject objectInFront;
    private BoomController boomController;
    private BlackHole blackHoleController;
    private Vector3 currentGravityDirection = Vector3.down;

    public StateID Id => StateID.Free_Mode;

    public FreeModeState(GameObject camera, LineRenderer renderer, float mouseDepthValue, GetMousePosition gmp, LayerMask layer, Vector3 currentGravityDirection)
    {
        mainCamera = camera;
        hitLayer = layer;
        lineRenderer = renderer;
        mouseDepth = mouseDepthValue;
        GMP = gmp;
        this.currentGravityDirection = currentGravityDirection;

    }

    public void OnEnterState()
    {
        Debug.Log("Enter Free Mode");
        lineRenderer.enabled = false;
    }

    public void OnUpdateState()
    {
        mousePosition = GMP.mousePosition;
        DragController();
        DrawPreviewLine();
        if (Input.GetKeyDown(KeyCode.X))
        {
            ToggleGravityDirection();
        }
    }

    public void OnExitState()
    {
        Debug.Log("Exiting Free Mode");
        lineRenderer.enabled = false;
    }

    public bool TransitionState(out StateID id)
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            id = StateID.Creat_Mode;
            return true;
        }

        id = default;
        return false;
    }

    private void DragController()
    {
        if (Input.GetMouseButtonDown(0))
            StartDrag();

        if (isDragging)
            Drag();

        if (Input.GetMouseButtonUp(0))
            StopDrag();
    }

    private void StartDrag()
    {
        Ray ray = mainCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, hitLayer))
        {
            GameObject hitObject = hit.collider.gameObject;

            if (!IsGround(hitObject))
            {
                objectInFront = hitObject;
                zDistance = mainCamera.GetComponent<Camera>().WorldToScreenPoint(objectInFront.transform.position).z;
                offset = objectInFront.transform.position -
                         mainCamera.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, zDistance));
                isDragging = true;

                lineRenderer.enabled = true;
            }
        }
        OnOff();
    }


    private void Drag()
    {
        if (objectInFront == null) return;

        Vector3 cursorScreenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, zDistance);
        Vector3 cursorWorldPosition = mainCamera.GetComponent<Camera>().ScreenToWorldPoint(cursorScreenPosition);
        objectInFront.transform.position = cursorWorldPosition + offset;
    }

    private void StopDrag()
    {
        isDragging = false;
        objectInFront = null;
        lineRenderer.enabled = false;
    }

    private bool IsGround(GameObject gameObject)
    {
        return gameObject.CompareTag("Ground");
    }

    private void DrawPreviewLine()
    {
        if (!isDragging || objectInFront == null) return;

        Ray ray = new Ray(objectInFront.transform.position, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, hitLayer))
        {
            lineRenderer.SetPosition(0, objectInFront.transform.position);
            lineRenderer.SetPosition(1, hit.point);
        }
    }

    private void OnOff()
    {
        if (objectInFront == null) return;

        if (objectInFront.CompareTag("Bomb"))
        {
            ToggleController(boomController);
        }
        else if (objectInFront.CompareTag("BlackHole"))
        {
            ToggleController(blackHoleController);
        }
        else return;
    }

    private void ToggleController(MonoBehaviour controller)
    {
        if (controller != null)
        {
            if (Input.GetMouseButtonDown(1))
            {
                controller.enabled = !controller.enabled;
                Debug.Log($"{controller.GetType().Name} toggled: {controller.enabled}");
            }
        }
    }

    private void InitializeObjectControllers(GameObject obj)
    {
        if (obj.CompareTag("Bomb"))
        {
            boomController = obj.GetComponent<BoomController>();
        }
        else if (obj.CompareTag("BlackHole"))
        {
            blackHoleController = obj.GetComponent<BlackHole>();
        }
    }
    public void ToggleGravityDirection()
    {
        if (currentGravityDirection == Vector3.down)
        {
            currentGravityDirection = Vector3.up;
        }
        else
            currentGravityDirection = Vector3.down;

        Physics.gravity = currentGravityDirection;
    }
}




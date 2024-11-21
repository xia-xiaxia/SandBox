using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    private StateManager<StateID> stateManager; 
    private CreatModeState creatModeState;      
    private FreeModeState freeModeState;        

    public int signCount;                      
    public int maxSignCount;                   
    public GameObject[] cprefabs;              
    public GameObject[] cpreviewObject;        
    public GameObject mainCamera;              
    public GetMousePosition GMP;               
    public LayerMask hitLayer1;                
    public LineRenderer clineRenderer;         
    private float mouseDepth = 10f;            
    private Transform cameraTransform;
    private Vector3 currentGravityDirection;

    public float speedUpFactor = 4.0f;
    public float slowDownFactor = 0.1f;
    public float normalFactor = 1.0f;

    private float currentTimeScale = 1.0f;


    public void Start()
    {
        cameraTransform = mainCamera.transform;

        creatModeState = new CreatModeState(
            mainCamera,
            cprefabs.Length,
            cprefabs,
            cpreviewObject,
            hitLayer1,
            clineRenderer,
            mouseDepth,
            GMP);

        freeModeState = new FreeModeState(
            mainCamera,
            clineRenderer,
            mouseDepth,
            GMP,
            hitLayer1,
            currentGravityDirection);

        var states = new Dictionary<StateID, IState<StateID>>
        {
            { StateID.Creat_Mode, creatModeState },
            { StateID.Free_Mode, freeModeState }
        };

        stateManager = new StateManager<StateID>(states);

        stateManager.SetState(StateID.Creat_Mode);
    }

    void Update()
    {
        stateManager.UpdateState();
            Timepase();
    }

    private void Timepase()
    {
        Debug.Log(1);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(0);
            Time.timeScale = currentTimeScale == 0 ? 1.0f : 0;

        }

        if (Input.GetKeyDown(KeyCode.Tab) && currentTimeScale == normalFactor )
        {
            Debug.Log(2);
            currentTimeScale = speedUpFactor;
            Time.timeScale = currentTimeScale;
        }

        if (Input.GetKeyDown(KeyCode.Tab) && currentTimeScale == speedUpFactor)
        {
            Debug.Log(0.5);
            currentTimeScale = slowDownFactor;
            Time.timeScale = currentTimeScale;
        }

        if (Input.GetKeyDown(KeyCode.Tab) && currentTimeScale == slowDownFactor)
        {
            Debug.Log(1);
            currentTimeScale = 1.0f;
            Time.timeScale = currentTimeScale;
        }
    }
}

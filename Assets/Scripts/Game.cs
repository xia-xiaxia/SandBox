using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

    private int sign = 0;

    private const float NormalSpeed = 1.0f; 
    private const float SlowSpeed = 0.4f;   
    private const float FastSpeed = 5.0f;   

    public float currentTimeScale ;

    public void Start()
    {
        cameraTransform = mainCamera.transform;
        currentTimeScale=1;

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
            TimeController();
    }

    private void TimeController()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("切换暂停状态");
            Time.timeScale = (Time.timeScale == NormalSpeed) ? 0f : NormalSpeed;
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            sign++; 
            if (sign >= 3)
            {
                sign = 0; 
            }
            UpdateTimeScale(); 
        }
    }

    private void UpdateTimeScale()
    {
        switch (sign)
        {
            case 0:
                Time.timeScale = NormalSpeed;
                Debug.Log("时间状态：正常速度");
                break;
            case 1:
                Time.timeScale = FastSpeed;
                Debug.Log("时间状态：快速速度");
                break;
            case 2:
                Time.timeScale = SlowSpeed;
                Debug.Log("时间状态：慢速");
                break;
            default:
                Debug.LogError("未知状态");
                break;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VehiclePhysics.Utility;

public enum CATEGORY_TYPE
{
    // Vehicle
    SPEED = 0,
    RPM = 1,
    LOAD = 6,
    TORQUE = 7,
    POWER = 8,
    FUEL = 9,
    GEAR = 12,
    STEERING =22,

    // Input
    THROTTLE = 101,
    BRAKE = 102,
    CLUTCH = 104,

    // Other
    LONG_G = 10000,
    LAT_G = 10001,
    WHEEL_FL = 10002,
    WHEEL_FR = 10003,
    WHEEL_RL = 10004,
    WHEEL_RR = 10005,

    WHEEL_SUSPENSION_FL = 10006,
    WHEEL_SUSPENSION_FR = 10007,
    WHEEL_SUSPENSION_RL = 10008,    
    WHEEL_SUSPENSION_RR = 10009,       

    WHEEL_LOAD_FL = 10010,
    WHEEL_LOAD_FR = 10011,
    WHEEL_LOAD_RL = 10012,    
    WHEEL_LOAD_RR = 10013,        
}

public enum DATA_TYPE
{
    VEHICLE,
    INPUT,
    OTHER,
}

public class GTSChartController : Singleton<GTSChartController>
{
    Dictionary<CATEGORY_TYPE, string> _dicUnit = new Dictionary<CATEGORY_TYPE, string>();
    public Dictionary<CATEGORY_TYPE, string> DicUnit {get => _dicUnit;}
    
    [SerializeField] Transform _diagnostics;
    List<GraphBase> _listDiagnosticsGraph = new List<GraphBase>();
    void Awake()
    {
        for(int i = 0; i < _diagnostics.childCount; i++)
        {
            _listDiagnosticsGraph.Add(_diagnostics.GetChild(i).GetComponent<GraphBase>());
        }
    }

    void Start()
    {    
        // Vehicle
        _dicUnit.Add(CATEGORY_TYPE.SPEED, "km/h");
        _dicUnit.Add(CATEGORY_TYPE.RPM, "rpm");
        _dicUnit.Add(CATEGORY_TYPE.LOAD, "%");
        _dicUnit.Add(CATEGORY_TYPE.TORQUE, "Nm");
        _dicUnit.Add(CATEGORY_TYPE.POWER, "KW");
        _dicUnit.Add(CATEGORY_TYPE.FUEL, "g/KWh");
        _dicUnit.Add(CATEGORY_TYPE.GEAR, "");

        // Input
        _dicUnit.Add(CATEGORY_TYPE.STEERING, "");
        _dicUnit.Add(CATEGORY_TYPE.THROTTLE, "%");
        _dicUnit.Add(CATEGORY_TYPE.BRAKE, "%");
        _dicUnit.Add(CATEGORY_TYPE.CLUTCH, "%");

        // Other
        _dicUnit.Add(CATEGORY_TYPE.LONG_G, "G");
        _dicUnit.Add(CATEGORY_TYPE.LAT_G, "G");
        _dicUnit.Add(CATEGORY_TYPE.WHEEL_FL, "km/h");
        _dicUnit.Add(CATEGORY_TYPE.WHEEL_FR, "km/h");
        _dicUnit.Add(CATEGORY_TYPE.WHEEL_RL, "km/h");
        _dicUnit.Add(CATEGORY_TYPE.WHEEL_RR, "km/h");

        _dicUnit.Add(CATEGORY_TYPE.WHEEL_SUSPENSION_FL, "mm");
        _dicUnit.Add(CATEGORY_TYPE.WHEEL_SUSPENSION_FR, "mm");
        _dicUnit.Add(CATEGORY_TYPE.WHEEL_SUSPENSION_RL, "mm");
        _dicUnit.Add(CATEGORY_TYPE.WHEEL_SUSPENSION_RR, "mm");

        _dicUnit.Add(CATEGORY_TYPE.WHEEL_LOAD_FL, "kg");
        _dicUnit.Add(CATEGORY_TYPE.WHEEL_LOAD_FR, "kg");
        _dicUnit.Add(CATEGORY_TYPE.WHEEL_LOAD_RL, "kg");
        _dicUnit.Add(CATEGORY_TYPE.WHEEL_LOAD_RR, "kg");                                        
    }

    public void HideDiagnosticGraphs()
    {
        for (int i = 0; i < _listDiagnosticsGraph.Count; i++)
        {
            _listDiagnosticsGraph[i].transform.localScale = Vector3.zero;
        }
    }

    public void ShowDiagnosticGraphs()
    {
        for (int i = 0; i < _listDiagnosticsGraph.Count; i++)
        {
            _listDiagnosticsGraph[i].transform.localScale = Vector3.one;
        }
    }
}

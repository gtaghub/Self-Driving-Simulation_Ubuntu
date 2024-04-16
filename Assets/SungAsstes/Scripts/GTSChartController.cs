using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VehiclePhysics.Utility;

public enum CATEGORY_TYPE
{
    RPM = 1,
    GEAR = 12,
    SPEED = 0,
    STEERING =22,
    THROTTLE = 101,
    BRAKE = 102,
    CLUTCH = 104,
}

public enum DATA_TYPE
{
    VEHICLE,
    INPUT,
}

public class GTSChartController : Singleton<GTSChartController>
{
    Dictionary<CATEGORY_TYPE, string> _dicUnit = new Dictionary<CATEGORY_TYPE, string>();
    public Dictionary<CATEGORY_TYPE, string> DicUnit {get => _dicUnit;}

    void Start()
    {
        _dicUnit.Add(CATEGORY_TYPE.RPM, "rpm");
        _dicUnit.Add(CATEGORY_TYPE.GEAR, "");
        _dicUnit.Add(CATEGORY_TYPE.SPEED, "km/h");
        _dicUnit.Add(CATEGORY_TYPE.STEERING, "");
        _dicUnit.Add(CATEGORY_TYPE.THROTTLE, "%");
        _dicUnit.Add(CATEGORY_TYPE.BRAKE, "%");
        _dicUnit.Add(CATEGORY_TYPE.CLUTCH, "%");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

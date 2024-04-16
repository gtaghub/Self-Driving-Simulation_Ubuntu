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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VehiclePhysics;

public class GTSKeyController : MonoBehaviour
{
    [SerializeField] List<VPPerformanceDisplay> _listPerformanceDisplay;
    [SerializeField] VPTelemetry _telemetry;

    bool _onChartUI = false;
    public bool OnChartUI 
    {
        get
        {
            return _onChartUI;
        }
        set
        {
            _onChartUI = value;
            SwitchingChartUI(value);
        }
    }

    public void SwitchingChartUI(bool value)
    {
        for (int i = 0; i < _listPerformanceDisplay.Count; i++)
            _listPerformanceDisplay[i].visible = value;

        _telemetry.showTelemetry = value;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F12))
            OnChartUI = !OnChartUI;

    }
}

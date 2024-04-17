using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VehiclePhysics;

public class GTSKeyController : MonoBehaviour
{
    [SerializeField] List<VPPerformanceDisplay> _listPerformanceDisplay;
    [SerializeField] VPTelemetry _telemetry;

    bool _onChartUI = false;
    bool _onNewChartUI = false;

    void Start()
    {
        OnChartUI = false;
        OnNewChartUI = false;
    }

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

    public bool OnNewChartUI
    {
        get
        {
            return _onNewChartUI;
        }
        set
        {
            _onNewChartUI = value;
            if (value == true)
                GTSChartController.Instance.ShowDiagnosticGraphs();
            else
                GTSChartController.Instance.HideDiagnosticGraphs();
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

        if(Input.GetKeyDown(KeyCode.F11))
            OnNewChartUI = !OnNewChartUI;

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChartAndGraph;
using VehiclePhysics;
using TMPro;

public class GraphBase : MonoBehaviour
{
    [SerializeField] protected GraphChart chart;
    [SerializeField] protected VehicleBase vehicle;
    [SerializeField] protected TMP_Text _TmpFrame;
    [SerializeField] protected TMP_Text _TmpTitle;

    [SerializeField] protected Transform _vehicleStateTextParent;

    protected List<VehicleStatText> _listVehicleStatText = new List<VehicleStatText>();

    protected float X = 0f;
    protected int frameCount = 0;
    protected float targetTime = 0.5f;
    protected float blendTime = 0.5f;
    void Start()
    {
        int count = _vehicleStateTextParent.childCount;
        for (int i = 0; i < count; i++)
        {
            VehicleStatText child = _vehicleStateTextParent.GetChild(i).GetComponent<VehicleStatText>();
            _listVehicleStatText.Add(child);
        }

        // It is also best Practice to enclose graph changes in startBatch and EndBacth calls
        chart.DataSource.StartBatch();

        // It is best practice to clear a category before filling it with new data
        chart.DataSource.ClearCategory("Illusion");
        chart.DataSource.AddPointToCategory("Illusion", 10, 0f);

        // each startBatch call must be matched width an EndBatch call !!!
        chart.DataSource.EndBatch();

        // graph is redrawn after EndBatch is called
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VehicleStatText : MonoBehaviour
{
    [SerializeField] CATEGORY_TYPE _categoryType;
    [SerializeField] DATA_TYPE _dataType;
    TMP_Text _tmp;

    // Start is called before the first frame update
    void Start()
    {
        _tmp = GetComponent<TMP_Text>();
    }

    public void UpdateText(float[] vehicleData, float[]inputData)
    {
        if (_dataType == DATA_TYPE.OTHER)
        {
            return;
        }

        var dicUnit = GTSChartController.Instance.DicUnit;
        if (_dataType == DATA_TYPE.VEHICLE)
            _tmp.text = vehicleData[(int)_categoryType].ToString("F1");
        else
            _tmp.text = inputData[(int)_categoryType - 100].ToString("F1");

        _tmp.text += (" " + dicUnit[_categoryType]);
    }

    public void UpdateText(float value)
    {
        var dicUnit = GTSChartController.Instance.DicUnit;
        _tmp.text = value.ToString("F1");
        _tmp.text += (" " + dicUnit[_categoryType]);
    }
}

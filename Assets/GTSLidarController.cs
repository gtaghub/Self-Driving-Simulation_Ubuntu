using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RGLUnityPlugin;
using UnityEngine.Events;

public enum LIDAR_SLIDER
{
    MinRange,
    MaxRange,
    MinAngle,
    MaxAngle,
}

public class GTSLidarController : MonoBehaviour
{
    [SerializeField] GTSLidar _lidar;
    [SerializeField] TMP_Text _tmpName;

    [SerializeField] Transform _slidersPanel;
    [SerializeField] List<GTSSlider> _listSlider = new List<GTSSlider>();

    // Slider SetUp
    List<float> _listEssentialData = new List<float>();
    List<UnityAction<float>> _listSliderEvents = new List<UnityAction<float>>();

    public void SetUp(GTSLidar lidar)
    {
        // 생성되면 라이다정보를 담는다.
        _lidar = lidar;
        _tmpName.text = lidar.gameObject.name;

        // UI에 보여지는 필수데이터를 담는다.
        _listEssentialData.Add(_lidar.LidarSensor.configuration.minRange);
        _listEssentialData.Add(_lidar.LidarSensor.configuration.maxRange);
        _listEssentialData.Add(_lidar.LidarSensor.configuration.minHAngle);
        _listEssentialData.Add(_lidar.LidarSensor.configuration.maxHAngle);

        _listSliderEvents.Add(OnMinRangeChanged);
        _listSliderEvents.Add(OnMaxRangeChanged);
        _listSliderEvents.Add(OnMinAngleChanged);
        _listSliderEvents.Add(OnMaxAngleChanged);

        // 각 Slider에 세팅한다.
        for (int i = 0; i < _slidersPanel.childCount; i++)
        {
            GTSSlider child = _slidersPanel.GetChild(i).GetComponent<GTSSlider>();
            child.SetUp(_listSliderEvents[i], _listEssentialData[i]);
            _listSlider.Add(child);
        }
    }

    public void OnToggleClicked(bool value)
    {
        _lidar.Switching(value);
    }

    public void OnMinRangeChanged(float value)
    {
        
        if (value > _lidar.LidarSensor.configuration.maxRange)
        {
            _listSlider[(int)LIDAR_SLIDER.MinRange].Slider.value = _lidar.LidarSensor.configuration.maxRange;
            _listSlider[(int)LIDAR_SLIDER.MinRange].UpdateText(_lidar.LidarSensor.configuration.maxRange);
        }
        else
        {
            _lidar.LidarSensor.configuration.minRange = value;
            _listSlider[(int)LIDAR_SLIDER.MinRange].UpdateText(value);
        }

        _lidar.UpdateConfiguration();
    }

    public void OnMaxRangeChanged(float value)
    {
        if (value < _lidar.LidarSensor.configuration.minRange)
        {
            _listSlider[(int)LIDAR_SLIDER.MaxRange].Slider.value = _lidar.LidarSensor.configuration.minRange;
            _listSlider[(int)LIDAR_SLIDER.MaxRange].UpdateText(_lidar.LidarSensor.configuration.minRange);
        }
        else
        {
            _lidar.LidarSensor.configuration.maxRange = value;
            _listSlider[(int)LIDAR_SLIDER.MaxRange].UpdateText(value);
        }

        _lidar.UpdateConfiguration();
    }

    public void OnMinAngleChanged(float value)
    {
        if (value > _lidar.LidarSensor.configuration.maxHAngle)
        {
            _listSlider[(int)LIDAR_SLIDER.MinAngle].Slider.value = _lidar.LidarSensor.configuration.maxHAngle;
            _listSlider[(int)LIDAR_SLIDER.MinAngle].UpdateText(_lidar.LidarSensor.configuration.maxHAngle);
        }

        else if (_lidar.LidarSensor.configuration.maxHAngle - value <= 360.0f)
        {
            _lidar.LidarSensor.configuration.minHAngle = value;
            _listSlider[(int)LIDAR_SLIDER.MinAngle].UpdateText(value);
        }

        else
        {
            _listSlider[(int)LIDAR_SLIDER.MinAngle].Slider.value = _lidar.LidarSensor.configuration.maxHAngle - 360.0f;
            _listSlider[(int)LIDAR_SLIDER.MinAngle].UpdateText(_lidar.LidarSensor.configuration.maxHAngle - 360.0f);
        }

        _lidar.UpdateConfiguration();
    }

    public void OnMaxAngleChanged(float value)
    {
        if (value < _lidar.LidarSensor.configuration.minHAngle)
        {
            _listSlider[(int)LIDAR_SLIDER.MaxAngle].Slider.value = _lidar.LidarSensor.configuration.minHAngle;
            _listSlider[(int)LIDAR_SLIDER.MaxAngle].UpdateText(_lidar.LidarSensor.configuration.minHAngle);
        }

        else if (value - _lidar.LidarSensor.configuration.minHAngle <= 360.0f)
        {
            _lidar.LidarSensor.configuration.maxHAngle = value;
            _listSlider[(int)LIDAR_SLIDER.MaxAngle].UpdateText(value);
        }
        
        else
        {
            _listSlider[(int)LIDAR_SLIDER.MaxAngle].Slider.value = _lidar.LidarSensor.configuration.minHAngle + 360.0f;
            _listSlider[(int)LIDAR_SLIDER.MaxAngle].UpdateText(_lidar.LidarSensor.configuration.minHAngle + 360.0f);
        }

        _lidar.UpdateConfiguration();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RGLUnityPlugin;
using AWSIM;
using MBaske.Sensors.Grid;

public class GTSLidar : MonoBehaviour, ISwitchable
{
    // 라이다
    LidarSensor _lidarSensor;
    public LidarSensor LidarSensor { get => _lidarSensor; }

    // 라이다 그리드 가이더
    GridSensorComponent3D _gridSensor;
    public GridSensorComponent3D GridSensor { get => _gridSensor; }

    // ROS 통신
    RglLidarPublisher _rglLidarPublisher;

    // 월드상의 포인트클라우드
    PointCloudVisualization _pointCloudVisualization;


    void Start()
    {
        _lidarSensor = GetComponent<LidarSensor>();
        _rglLidarPublisher = GetComponent<RglLidarPublisher>();
        _pointCloudVisualization = GetComponent<PointCloudVisualization>();
        _gridSensor = GetComponentInChildren<GridSensorComponent3D>();
        Init();
    }

    public bool _isActive = true;
    public bool IsActive => _isActive;
    public void Switching(bool value)
    {
        if (value == true)
        {
            Activate();
        }
        else
        {
            DeActivate();
        }
    }

    public void Activate()
    {
        _isActive = true;
        _pointCloudVisualization.OnVisable = true;
        _rglLidarPublisher.Activate();
    }

    public void DeActivate()
    {
        _isActive = false;
        _pointCloudVisualization.OnVisable = false;
        _rglLidarPublisher.DeActivate();
    }

    public void Init()
    {
        if (_lidarSensor == null || _gridSensor == null)
            return;

        UpdateConfiguration();

        //  ==> GTSDatsSpawner.cs
        EventBus.LidarConfigSetupEvent?.Invoke(this);
    }

    public void UpdateConfiguration()
    {
        // Angle
        float minAngle = _lidarSensor.configuration.minHAngle;
        float maxAngle = _lidarSensor.configuration.maxHAngle;
        float centerAngle = (maxAngle + minAngle) / 2;
        float angleRange = Mathf.Abs((maxAngle - minAngle) / 2);

        _gridSensor.transform.localRotation = Quaternion.Euler(new Vector3(0, centerAngle, 0));
        _gridSensor.LonAngle = angleRange;

        // Range
        float minRange = _lidarSensor.configuration.minRange;
        float maxRange = _lidarSensor.configuration.maxRange;

        _gridSensor.MinDistance = minRange;
        _gridSensor.MaxDistance = maxRange;

        // 바뀐값 라이다 센서 적용
        _lidarSensor.UpdateConfiguration();
    }

    //public void UpdateConfiguration()
    //{
    //    // Angle
    //    float centerAngle = _gridSensor.transform.eulerAngles.y;
    //    float range = _gridSensor.LonAngle;

    //    float minAngle = centerAngle - range / 2;
    //    float maxAngle = centerAngle + range / 2;

    //    _lidarSensor.configuration.minHAngle = minAngle;
    //    _lidarSensor.configuration.maxHAngle = maxAngle;
    //}
}

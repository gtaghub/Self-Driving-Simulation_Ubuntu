using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RGLUnityPlugin;
using AWSIM;

public class GTSLidar : MonoBehaviour, ISwitchable
{
    LidarSensor _lidarSensor;
    RglLidarPublisher _rglLidarPublisher;

    PointCloudVisualization _pointCloudVisualization;

    [SerializeField] LIDAR_POSITION _lidarPosition;
    public LIDAR_POSITION LidarPosition =>_lidarPosition;

    void Start()
    {
        _lidarSensor = GetComponent<LidarSensor>();
        _rglLidarPublisher = GetComponent<RglLidarPublisher>();
        _pointCloudVisualization = GetComponent<PointCloudVisualization>();
    }

    public bool _isActive = true;
    public bool IsActive => _isActive;
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
}

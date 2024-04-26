using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GTSDataSpawner : MonoBehaviour
{
    [Header(" ¡Ý LidarController UI")]
    [SerializeField] GameObject _lidarControllerPf;
    [SerializeField] Transform _lidarControllerParent;

    private void OnEnable()
    {
        EventBus.LidarConfigSetupEvent += SpawnLidarController;
    }

    public void SpawnLidarController(GTSLidar lidar)
    {
        GameObject newObject = Instantiate(_lidarControllerPf, _lidarControllerParent);
        GTSLidarController lidarController = newObject.GetComponent<GTSLidarController>();
        lidarController.SetUp(lidar);
    }

    private void OnDisable()
    {
        EventBus.LidarConfigSetupEvent -= SpawnLidarController;
    }
}

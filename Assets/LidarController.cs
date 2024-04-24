using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LIDAR_POSITION
{
    TOP,
    LEFT,
    RIGHT,
}

public class LidarController : MonoBehaviour
{
    [SerializeField] List<GTSLidar> _listLidar;

    public void Toggle(int id)
    {
        for (int i = 0; i < _listLidar.Count; i++)
        {
            if (id == (int)_listLidar[i].LidarPosition)
            {
                if (_listLidar[i].IsActive)
                    _listLidar[i].DeActivate();
                else
                    _listLidar[i].Activate();
            }
        }
    }
}

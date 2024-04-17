using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VehiclePhysics;

public class EnginePerformanceGraph : GraphBase
{
    private void Start()
    {
        StartCoroutine(Co_UpdateRendering());
    }

    IEnumerator Co_UpdateRendering()
    {
        while (true)
        {
            frameCount++;
            _TmpFrame.text = "FRM    " + frameCount.ToString();

            // 차량 데이터 가져오기
            int[] vehicleData = vehicle.data.Get(Channel.Vehicle);
            float[] tempVehicleData = new float[vehicleData.Length];
            tempVehicleData[VehicleData.EngineRpm] = vehicleData[VehicleData.EngineRpm] / 1000.0f;
            tempVehicleData[VehicleData.GearboxGear] = vehicleData[VehicleData.GearboxGear];
            tempVehicleData[VehicleData.Speed] = vehicleData[VehicleData.Speed] * 3.6f / 1000.0f;

            // 엔진 토크
            float engineTorque = vehicleData[VehicleData.EngineTorque];
            if (engineTorque <= 0)
                tempVehicleData[VehicleData.EngineTorque] = 0;
            else
                tempVehicleData[VehicleData.EngineTorque] = vehicleData[VehicleData.EngineTorque] / 1000.0f;

            // 엔진 파워
            float enginePower = vehicleData[VehicleData.EnginePower];
            if (enginePower <= 0)
                tempVehicleData[VehicleData.EnginePower] = 0;
            else
                tempVehicleData[VehicleData.EnginePower] = vehicleData[VehicleData.EnginePower] / 1000.0f;

            // 엔진 연료
            float engineFuelRate = vehicleData[VehicleData.EngineFuelRate] / 1000.0f;

            if (engineFuelRate <= 0)
                tempVehicleData[VehicleData.EngineFuelRate] = 0;
            else
            {
                tempVehicleData[VehicleData.EngineFuelRate] = engineFuelRate / tempVehicleData[VehicleData.EnginePower] * 3600.0f;
                tempVehicleData[VehicleData.EngineLoad] = vehicleData[VehicleData.EngineLoad] / 1000.0f;
            }

            // 그래프 보간
            float engineRpmChart = 3f + tempVehicleData[VehicleData.EngineRpm] / 1000.0f / 3f;
            float geerChart = 7f + tempVehicleData[VehicleData.GearboxGear] / 10f;
            float speedChart = 7f + tempVehicleData[VehicleData.Speed] / 100.0f;

            float torqueChart = 2f + tempVehicleData[VehicleData.EngineTorque] / 100f;
            float powerChart = 2f + tempVehicleData[VehicleData.EnginePower] / 100f;
            float fuelChart = 2f + tempVehicleData[VehicleData.EngineFuelRate] / 1000f;
            float load = 1f + tempVehicleData[VehicleData.EngineLoad];

            // 수치 텍스트 적용
            for (int i = 0; i < _listVehicleStatText.Count; i++)
            {
                _listVehicleStatText[i].UpdateText(tempVehicleData, null);
            }

            chart.DataSource.AddPointToCategoryRealtime("RPM", X, engineRpmChart, blendTime); // now we can also set the animation time for the streaming value
            chart.DataSource.AddPointToCategoryRealtime("Gear", X, geerChart, blendTime); // setting it to 1f will make it blend in 1 second
            chart.DataSource.AddPointToCategoryRealtime("Speed", X, speedChart, blendTime); // setting it to 1f will make it blend in 1 second

            chart.DataSource.AddPointToCategoryRealtime("Torque", X, torqueChart, blendTime); // now we can also set the animation time for the streaming value
            chart.DataSource.AddPointToCategoryRealtime("Power", X, powerChart, blendTime); // setting it to 1f will make it blend in 1 second
            chart.DataSource.AddPointToCategoryRealtime("Fuel", X, fuelChart, blendTime); // setting it to 1f will make it blend in 1 second
            chart.DataSource.AddPointToCategoryRealtime("Load", X, load, blendTime); // setting it to 1f will make it blend in 1 second
            X++; // increase the X value so the next point is 1 unity away

            yield return new WaitForSeconds(targetTime); // 지정한 시간만큼 대기
        }
    }
}

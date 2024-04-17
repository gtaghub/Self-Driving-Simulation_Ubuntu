using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VehiclePhysics;

public class EssentialGraph : GraphBase
{
    private void Start()
    {
        StartCoroutine(Co_UpdateRendering());
    }

    IEnumerator Co_UpdateRendering()
    {
        while (true) // 무한 루프
        {
            frameCount++;
            _TmpFrame.text = "FRM    " + frameCount.ToString();

            // 차량 데이터 가져오기
            int[] vehicleData = vehicle.data.Get(Channel.Vehicle);
            float[] tempVehicleData = new float[vehicleData.Length];
            tempVehicleData[VehicleData.EngineRpm] = vehicleData[VehicleData.EngineRpm] / 1000.0f;
            tempVehicleData[VehicleData.GearboxGear] = vehicleData[VehicleData.GearboxGear];
            tempVehicleData[VehicleData.Speed] = vehicleData[VehicleData.Speed] * 3.6f / 1000.0f;
            tempVehicleData[VehicleData.AidedSteer] = vehicleData[VehicleData.AidedSteer] / 10000.0f;

            // 인풋데이터 가져오기
            int[] inputData = vehicle.data.Get(Channel.Input);
            float[] tempInputData = new float[vehicleData.Length];
            tempInputData[InputData.Throttle] = inputData[InputData.Throttle] / 10000.0f;
            tempInputData[InputData.Brake] = inputData[InputData.Brake] / 10000.0f;
            tempInputData[InputData.Clutch] = inputData[InputData.Clutch] / 10000.0f;

            // 그래프 보간
            float engineRpmChart = 7f + tempVehicleData[VehicleData.EngineRpm] / 1000.0f / 3f;
            float geerChart = 6f + tempVehicleData[VehicleData.GearboxGear] / 10f;
            float speedChart = 4f + tempVehicleData[VehicleData.Speed] / 100.0f;
            float steeringChart = 3f + tempVehicleData[VehicleData.AidedSteer];

            float throttleChart = 1f + tempInputData[InputData.Throttle];
            float brakeChart = 1f + tempInputData[InputData.Brake];
            float ClutchChart = 1f + tempInputData[InputData.Clutch];

            // 수치 텍스트 적용
            for (int i = 0; i < _listVehicleStatText.Count; i++)
            {
                _listVehicleStatText[i].UpdateText(tempVehicleData, tempInputData);
            }

            // Debug.Log($"engineRpm : {engineRpm}, currentGear : {currentGear}, speed : {speed}, steering : {steering}, throttle : {throttle}, brake : {brake}, clutch : {clutch}");
            chart.DataSource.AddPointToCategoryRealtime("RPM", X, engineRpmChart, blendTime); // now we can also set the animation time for the streaming value
            chart.DataSource.AddPointToCategoryRealtime("Gear", X, geerChart, blendTime); // setting it to 1f will make it blend in 1 second
            chart.DataSource.AddPointToCategoryRealtime("Speed", X, speedChart, blendTime); // setting it to 1f will make it blend in 1 second
            chart.DataSource.AddPointToCategoryRealtime("Steering", X, steeringChart, blendTime); // setting it to 1f will make it blend in 1 second
            chart.DataSource.AddPointToCategoryRealtime("Throttle", X, throttleChart, blendTime); // setting it to 1f will make it blend in 1 second
            chart.DataSource.AddPointToCategoryRealtime("Brake", X, brakeChart, blendTime); // setting it to 1f will make it blend in 1 second
            chart.DataSource.AddPointToCategoryRealtime("Clutch", X, ClutchChart, blendTime); // setting it to 1f will make it blend in 1 second
            X++; // increase the X value so the next point is 1 unity away

            yield return new WaitForSeconds(targetTime); // 지정한 시간만큼 대기
        }
    }
}

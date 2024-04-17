using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VehiclePhysics;

public class WheelLoadGraph : GraphBase
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
            tempVehicleData[VehicleData.Speed] = vehicleData[VehicleData.Speed] * 3.6f / 1000.0f;

            // // 휠
            float[] tempWheelData = new float[4];
            VehicleBase.WheelState ws = vehicle.wheelState[0];
            tempWheelData[0] = ws.suspensionLoad * Gravity.forceToMass;

            ws = vehicle.wheelState[1];
            tempWheelData[1] = ws.suspensionLoad * Gravity.forceToMass;

            ws = vehicle.wheelState[2];
            tempWheelData[2] = ws.suspensionLoad * Gravity.forceToMass;

            ws = vehicle.wheelState[3];
            tempWheelData[3] = ws.suspensionLoad * Gravity.forceToMass;

            // 그래프 보간
            float speedChart = 4f + tempVehicleData[VehicleData.Speed] / 20.0f;
            float wheelFLChart = 1f + tempWheelData[0] / 200f;
            float wheelFRChart = 1f + tempWheelData[1] / 200f;
            float wheelRLChart = 1f + tempWheelData[2] / 200f;
            float wheelRRChart = 1f + tempWheelData[3] / 200f;


            //// 수치 텍스트 적용
            for (int i = 0; i < _listVehicleStatText.Count; i++)
            {
                if (i == 0)
                    _listVehicleStatText[i].UpdateText(tempVehicleData, null);

                if (i > 0)
                    _listVehicleStatText[i].UpdateText(tempWheelData[i - 1]);
            }

            chart.DataSource.AddPointToCategoryRealtime("Speed", X, speedChart, blendTime); // setting it to 1f will make it blend in 1 second
            chart.DataSource.AddPointToCategoryRealtime("WheelFL", X, wheelFLChart, blendTime); // setting it to 1f will make it blend in 1 second
            chart.DataSource.AddPointToCategoryRealtime("WheelFR", X, wheelFRChart, blendTime); // setting it to 1f will make it blend in 1 second
            chart.DataSource.AddPointToCategoryRealtime("WheelRL", X, wheelRLChart, blendTime); // setting it to 1f will make it blend in 1 second
            chart.DataSource.AddPointToCategoryRealtime("WheelRR", X, wheelRRChart, blendTime); // setting it to 1f will make it blend in 1 second            
            X++; // increase the X value so the next point is 1 unity away


            yield return new WaitForSeconds(targetTime); // 지정한 시간만큼 대기
        }
    }
}

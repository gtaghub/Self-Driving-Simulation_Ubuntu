using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class GTSSlider : MonoBehaviour
{
    [SerializeField] TMP_Text _tmpValue;
    [SerializeField] Slider _slider;
    public Slider Slider { get => _slider; }
    [SerializeField] float minValue = -360f;
    [SerializeField] float maxValue = 360f;

    private void Start()
    {
        _slider.minValue = minValue;
        _slider.maxValue = maxValue;
    }

    public void SetUp(UnityAction<float> slidrEvent, float value)
    {
        _slider.value = value;
        _tmpValue.text = value.ToString();
        _slider.onValueChanged.AddListener(slidrEvent);
    }

    public void UpdateText(float value)
    {
        _tmpValue.text = value.ToString();
    }
}

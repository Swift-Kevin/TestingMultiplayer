using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    private const float MIN_CAM_SENS = 0.1f;
    private const float MAX_CAM_SENS = 100f;
    private const float CAM_DEF_VAL = 25f;

    public static SettingsManager Instance;
    [Range(MIN_CAM_SENS, MAX_CAM_SENS)]
    [SerializeField] private float camSens = 25f;

    [SerializeField] private Slider slider_CamSensSlider;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        slider_CamSensSlider.onValueChanged.AddListener(SetCamSens);
        slider_CamSensSlider.minValue = MIN_CAM_SENS;
        slider_CamSensSlider.maxValue = MAX_CAM_SENS;
        slider_CamSensSlider.value = CAM_DEF_VAL;
        camSens = CAM_DEF_VAL;
    }

    public void SetCamSens(float val)
    {
        camSens = val;
    }

    public float GetCamSens()
    {
        return camSens;
    }
}

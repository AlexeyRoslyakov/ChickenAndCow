using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public enum TimeOfDay
    {
        Morning,
        Day,
        Night
    }

    public TimeOfDay currentMode = TimeOfDay.Morning;

    [Header("Time Settings")] public float morningDuration = 30f;
    public float dayDuration = 60f;
    public float nightDuration = 30f;

    private float modeTimer;

    private void Start()
    {
        SetMode(currentMode);
    }

    private void Update()
    {
        modeTimer -= Time.deltaTime;

        if (modeTimer <= 0)
        {
            SwitchToNextMode();
        }
    }

    private void SetMode(TimeOfDay newMode)
    {
        currentMode = newMode;

        switch (currentMode)
        {
            case TimeOfDay.Morning:
                modeTimer = morningDuration;
                SetLightingForMorning();
                break;
            case TimeOfDay.Day:
                modeTimer = dayDuration;
                SetLightingForDay();
                break;
            case TimeOfDay.Night:
                modeTimer = nightDuration;
                SetLightingForNight();
                break;
        }
    }

    private void SwitchToNextMode()
    {
        switch (currentMode)
        {
            case TimeOfDay.Morning:
                SetMode(TimeOfDay.Day);
                break;
            case TimeOfDay.Day:
                SetMode(TimeOfDay.Night);
                break;
            case TimeOfDay.Night:
                SetMode(TimeOfDay.Morning);
                break;
        }
    }

    private void SetLightingForMorning()
    {
        RenderSettings.ambientLight = new Color(1f, 0.8f, 0.6f); // Soft, warm color
    }

    private void SetLightingForDay()
    {
        RenderSettings.ambientLight = new Color(1f, 1f, 1f); // Bright, full light
    }

    private void SetLightingForNight()
    {
        RenderSettings.ambientLight = new Color(0.1f, 0.1f, 0.3f); // Dark, cool color
    }
}
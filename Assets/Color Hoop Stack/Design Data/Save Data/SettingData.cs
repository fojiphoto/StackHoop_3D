public class SettingData
{
    public bool enabledVibration = true;
    public bool enabledSound = true;

    public SettingData()
    {
        enabledVibration = true;
        enabledSound = true;
    }

    public SettingData(bool enabledSound, bool enabledVibration)
    {
        this.enabledVibration = enabledVibration;
        this.enabledSound = enabledSound;
    }
}

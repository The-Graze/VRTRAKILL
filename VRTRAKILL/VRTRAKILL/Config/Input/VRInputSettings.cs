﻿using Newtonsoft.Json;

namespace Plugin.VRTRAKILL.Config.Input
{
    internal class VRInputSettings
    {
        [JsonProperty("Joystick deadzone (from 0 to 1)")] public float Deadzone { get; set; } = 0.4f;
        [JsonProperty("Snap turning (unused)")] public bool SnapTurning { get; set; } = false;
        [JsonProperty("Snap turning angles (unused)")] public float SnapTurningAngles { get; set; } = 45;
        [JsonProperty("Smooth turning speed")] public float SmoothTurningSpeed { get; set; } = 300;

        [JsonProperty("Controller Haptics (UNUSED)")] public ControllerHaptics Haptics { get; set; }

        public class ControllerHaptics
        {
            [JsonProperty("Enable Haptics")] public bool EnableHaptics { get; set; } = false;
        }

        public VRInputSettings()
        {
            Haptics = new ControllerHaptics();
        }
    }
}

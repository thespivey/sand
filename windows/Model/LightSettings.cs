using System;

namespace Sand.Model
{
    internal struct LightSettings
    {
        public float Brightness;
        public bool EnableBreath;
        public bool EnableColorShift;
        public TimeSpan ColorShiftInterval;

        public override string ToString()
        {
            return $"Light (brightness={Brightness}" +
                (EnableColorShift ? $" shift={ColorShiftInterval.TotalSeconds}s" : "") +
                (EnableBreath ? " breath" : "") + 
                ")";
        }
    }
}

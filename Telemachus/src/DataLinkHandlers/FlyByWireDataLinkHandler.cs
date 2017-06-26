using UnityEngine;

namespace Telemachus.DataLinkHandlers
{
    public class FlyByWireDataLinkHandler : DataLinkHandler
    {
        #region Fields

        static float yaw = 0, pitch = 0, roll = 0, x = 0, y = 0, z = 0;
        static int on_attitude = 0;

        #endregion

        #region Initialisation

        public FlyByWireDataLinkHandler(FormatterProvider formatters)
            : base(formatters)
        {
            registerAPI(new ActionAPIEntry(
                dataSources => { yaw = checkFlightStateParameters(float.Parse(dataSources.args[0])); return 0; },
                "v.setYaw", "Yaw [float yaw]", formatters.Default));

            registerAPI(new ActionAPIEntry(
                dataSources => { pitch = checkFlightStateParameters(float.Parse(dataSources.args[0])); return 0; },
                "v.setPitch", "Pitch [float pitch]", formatters.Default));

            registerAPI(new ActionAPIEntry(
                dataSources => { roll = checkFlightStateParameters(float.Parse(dataSources.args[0])); return 0; },
                "v.setRoll", "Roll [float roll]", formatters.Default));

            registerAPI(new ActionAPIEntry(
                dataSources => { on_attitude = int.Parse(dataSources.args[0]); return 0; },
                "v.setFbW", "Set Fly by Wire On or Off [int state]", formatters.Default));

            registerAPI(new ActionAPIEntry(
                dataSources =>
                {
                    pitch = checkFlightStateParameters(float.Parse(dataSources.args[0]));
                    yaw = checkFlightStateParameters(float.Parse(dataSources.args[1]));
                    roll = checkFlightStateParameters(float.Parse(dataSources.args[2]));
                    x = checkFlightStateParameters(float.Parse(dataSources.args[3]));
                    y = checkFlightStateParameters(float.Parse(dataSources.args[4]));
                    z = checkFlightStateParameters(float.Parse(dataSources.args[5]));

                    return 0;
                },
                "v.setPitchYawRollXYZ", "Set pitch, yaw, roll, X, Y and Z [float pitch, yaw, roll, x, y, z]", formatters.Default));
                
            registerAPI(new ActionAPIEntry(
                dataSources =>
                {
                    pitch = checkFlightStateParameters(float.Parse(dataSources.args[0]));
                    yaw = checkFlightStateParameters(float.Parse(dataSources.args[1]));
                    roll = checkFlightStateParameters(float.Parse(dataSources.args[2]));

                    return 0;
                },
                "v.setAttitude", "Set pitch, yaw, roll [float pitch, yaw, roll]", formatters.Default));
            
            registerAPI(new ActionAPIEntry(
                dataSources =>
                {   
                    x = checkFlightStateParameters(float.Parse(dataSources.args[0]));
                    y = checkFlightStateParameters(float.Parse(dataSources.args[1]));
                    z = checkFlightStateParameters(float.Parse(dataSources.args[2]));

                    return 0;
                },
                "v.setTranslation", "Set X, Y and Z [float x, y, z]", formatters.Default));
        }

        #endregion

        #region Methods

        public static void onFlyByWire(FlightCtrlState fcs)
        {
            if (on_attitude > 0)
            {
                fcs.yaw = yaw;
                fcs.pitch = pitch;
                fcs.roll = roll;
                fcs.X = x < 0 ? -1 : (x > 0 ? 1 : 0);
                fcs.Y = y < 0 ? -1 : (y > 0 ? 1 : 0);
                fcs.Z = z < 0 ? -1 : (z > 0 ? 1 : 0);
            }
        }

        public static void reset()
        {
            yaw = 0;
            pitch = 0;
            roll = 0;
            x = 0;
            y = 0;
            z = 0;
            on_attitude = 0;
        }

        private float checkFlightStateParameters(float f)
        {
            if (float.IsNaN(f))
            {
                f = 0;
            }

            return Mathf.Clamp(f, -1f, 1f);
        }

        #endregion

        #region DataLinkHandler

        protected override int pausedHandler()
        {
            return PausedDataLinkHandler.partPaused();
        }

        #endregion
    }
}
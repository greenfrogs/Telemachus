namespace Telemachus.DataLinkHandlers
{
    public class TimeWarpDataLinkHandler : DataLinkHandler
    {
        #region Initialisation

        public TimeWarpDataLinkHandler(FormatterProvider formatters)
            : base(formatters)
        {
            registerAPI(new ActionAPIEntry(
                dataSources =>
                {
                    TelemachusBehaviour.instance.BroadcastMessage("queueDelayedAPI", new DelayedAPIEntry(dataSources.Clone(),
                            (x) => { TimeWarp.SetRate(int.Parse(x.args[0]), false); return 0d; }),
                        UnityEngine.SendMessageOptions.DontRequireReceiver); return false;
                },
                "t.timeWarp", "Time Warp [int rate]", formatters.Default));

            registerAPI(new PlotableAPIEntry(
                dataSources => { return Planetarium.GetUniversalTime(); },
                "t.universalTime", "Universal Time", formatters.Default, APIEntry.UnitType.DATE, true));

            registerAPI(new ActionAPIEntry(
                dataSources =>
                {
                    TelemachusBehaviour.instance.BroadcastMessage("queueDelayedAPI", new DelayedAPIEntry(dataSources.Clone(),
                            (x) => { FlightDriver.SetPause(true); return 0d; }),
                        UnityEngine.SendMessageOptions.DontRequireReceiver); return false;
                },
                "t.pause", "Pause game", formatters.Default));

            registerAPI(new ActionAPIEntry(
                dataSources =>
                {
                    TelemachusBehaviour.instance.BroadcastMessage("queueDelayedAPI", new DelayedAPIEntry(dataSources.Clone(),
                            (x) => { FlightDriver.SetPause(false); return 0d; }),
                        UnityEngine.SendMessageOptions.DontRequireReceiver); return false;
                },
                "t.unpause", "Unpause game", formatters.Default));
        }

        #endregion
    }
}
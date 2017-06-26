using System;
using KSP.UI.Screens;

namespace Telemachus.DataLinkHandlers
{
    public class FlightDataLinkHandler : DataLinkHandler
    {
        #region Initialisation

        public FlightDataLinkHandler(FormatterProvider formatters)
            : base(formatters)
        {
            registerAPI(new ActionAPIEntry(
                dataSources =>
                {
                    TelemachusBehaviour.instance.BroadcastMessage("queueDelayedAPI", new DelayedAPIEntry(dataSources.Clone(),
                        (x) => { StageManager.ActivateNextStage(); return 0d; }), UnityEngine.SendMessageOptions.DontRequireReceiver);
                    return predictFailure(dataSources.vessel);
                }, "f.stage", "Stage", formatters.Default));

            registerAPI(new ActionAPIEntry(
                dataSources =>
                {
                    TelemachusBehaviour.instance.BroadcastMessage("queueDelayedAPI", new DelayedAPIEntry(dataSources.Clone(),
                        (x) => { setThrottle(x); return 0d; }), UnityEngine.SendMessageOptions.DontRequireReceiver);
                    return predictFailure(dataSources.vessel);
                },
                "f.setThrottle", "Set Throttle [float magnitude]", formatters.Default));

            registerAPI(new PlotableAPIEntry(
                dataSources =>
                {
                    float t = dataSources.vessel.ctrlState.mainThrottle;
                    return t;
                },
                "f.throttle", "Throttle", formatters.Default, APIEntry.UnitType.UNITLESS));

            registerAPI(new ActionAPIEntry(
                dataSources =>
                {
                    TelemachusBehaviour.instance.BroadcastMessage("queueDelayedAPI", new DelayedAPIEntry(dataSources.Clone(),
                        (x) => { throttleUp(); return 0d; }), UnityEngine.SendMessageOptions.DontRequireReceiver);
                    return predictFailure(dataSources.vessel);
                },
                "f.throttleUp", "Throttle Up", formatters.Default));

            registerAPI(new ActionAPIEntry(
                dataSources =>
                {
                    TelemachusBehaviour.instance.BroadcastMessage("queueDelayedAPI", new DelayedAPIEntry(dataSources.Clone(),
                        (x) => { throttleZero(); return 0d; }), UnityEngine.SendMessageOptions.DontRequireReceiver);
                    return predictFailure(dataSources.vessel);
                },
                "f.throttleZero", "Throttle Zero", formatters.Default));

            registerAPI(new ActionAPIEntry(
                dataSources =>
                {
                    TelemachusBehaviour.instance.BroadcastMessage("queueDelayedAPI", new DelayedAPIEntry(dataSources.Clone(),
                        (x) => { throttleFull(); return 0d; }), UnityEngine.SendMessageOptions.DontRequireReceiver);
                    return predictFailure(dataSources.vessel);
                },
                "f.throttleFull", "Throttle Full", formatters.Default));

            registerAPI(new ActionAPIEntry(
                dataSources =>
                {
                    TelemachusBehaviour.instance.BroadcastMessage("queueDelayedAPI", new DelayedAPIEntry(dataSources.Clone(),
                        (x) => { throttleDown(); return 0d; }), UnityEngine.SendMessageOptions.DontRequireReceiver);
                    return predictFailure(dataSources.vessel);
                },
                "f.throttleDown", "Throttle Down", formatters.Default));

            registerAPI(new ActionAPIEntry(
                buildActionGroupToggleDelayedLamda(KSPActionGroup.RCS),
                "f.rcs", "RCS [optional bool on/off]", formatters.Default));

            registerAPI(new ActionAPIEntry(
                buildActionGroupToggleDelayedLamda(KSPActionGroup.SAS),
                "f.sas", "SAS [optional bool on/off]", formatters.Default));

            registerAPI(new ActionAPIEntry(
                buildActionGroupToggleDelayedLamda(KSPActionGroup.Light),
                "f.light", "Light [optional bool on/off]", formatters.Default));

            registerAPI(new ActionAPIEntry(
                buildActionGroupToggleDelayedLamda(KSPActionGroup.Gear),
                "f.gear", "Gear [optional bool on/off]", formatters.Default));

            registerAPI(new ActionAPIEntry(
                buildActionGroupToggleDelayedLamda(KSPActionGroup.Brakes),
                "f.brake", "Brake [optional bool on/off]", formatters.Default));

            registerAPI(new ActionAPIEntry(
                buildActionGroupToggleDelayedLamda(KSPActionGroup.Abort),
                "f.abort", "Abort [optional bool on/off]", formatters.Default));

            registerAPI(new ActionAPIEntry(
                dataSources => 
                {
                    bool state = dataSources.args.Count > 0 ? bool.Parse(dataSources.args[0]) : !FlightInputHandler.fetch.precisionMode;
                    
                    TelemachusBehaviour.instance.BroadcastMessage("queueDelayedAPI", new DelayedAPIEntry(dataSources.Clone(),
                            (x) => {

                                FlightInputHandler.fetch.precisionMode = state;
                                // Update the UI.
                                // MOARdV: In 1.1, this only affects the normal flight display,
                                // not the docking mode display.
                                var gauges = UnityEngine.Object.FindObjectOfType<KSP.UI.Screens.Flight.LinearControlGauges>();
                                if (gauges != null)
                                {
                                    //JUtil.LogMessage(this, "{0} input gauge images", gauges.inputGaugeImages.Count);
                                    for (int i = 0; i < gauges.inputGaugeImages.Count; ++i)
                                    {
                                        gauges.inputGaugeImages[i].color = (state) ? XKCDColors.BrightCyan : XKCDColors.Orange;
                                    }
                                }
                                return Convert.ToInt32(state);
                            }),
                        UnityEngine.SendMessageOptions.DontRequireReceiver);
                    return predictFailure(dataSources.vessel);
                },
                "f.precisionControl", "Precision controls [optional bool on/off]", formatters.Default));

            registerAPI(new ActionAPIEntry(
                buildActionGroupToggleDelayedLamda(KSPActionGroup.Custom01),
                "f.ag1", "Action Group 1 [optional bool on/off]", formatters.Default));

            registerAPI(new ActionAPIEntry(
                buildActionGroupToggleDelayedLamda(KSPActionGroup.Custom02),
                "f.ag2", "Action Group 2 [optional bool on/off]", formatters.Default));

            registerAPI(new ActionAPIEntry(
                buildActionGroupToggleDelayedLamda(KSPActionGroup.Custom03),
                "f.ag3", "Action Group 3 [optional bool on/off]", formatters.Default));

            registerAPI(new ActionAPIEntry(
                buildActionGroupToggleDelayedLamda(KSPActionGroup.Custom04),
                "f.ag4", "Action Group 4 [optional bool on/off]", formatters.Default));

            registerAPI(new ActionAPIEntry(
                buildActionGroupToggleDelayedLamda(KSPActionGroup.Custom05),
                "f.ag5", "Action Group 5 [optional bool on/off]", formatters.Default));

            registerAPI(new ActionAPIEntry(
                buildActionGroupToggleDelayedLamda(KSPActionGroup.Custom06),
                "f.ag6", "Action Group 6 [optional bool on/off]", formatters.Default));

            registerAPI(new ActionAPIEntry(
                buildActionGroupToggleDelayedLamda(KSPActionGroup.Custom07),
                "f.ag7", "Action Group 7 [optional bool on/off]", formatters.Default));

            registerAPI(new ActionAPIEntry(
                buildActionGroupToggleDelayedLamda(KSPActionGroup.Custom08),
                "f.ag8", "Action Group 8 [optional bool on/off]", formatters.Default));

            registerAPI(new ActionAPIEntry(
                buildActionGroupToggleDelayedLamda(KSPActionGroup.Custom09),
                "f.ag9", "Action Group 9 [optional bool on/off]", formatters.Default));

            registerAPI(new ActionAPIEntry(
                buildActionGroupToggleDelayedLamda(KSPActionGroup.Custom10),
                "f.ag10", "Action Group 10 [optional bool on/off]", formatters.Default));

            registerAPI(new PlotableAPIEntry(
                dataSources => { return dataSources.vessel.ActionGroups[KSPActionGroup.RCS]; },
                "v.rcsValue", "Query RCS value", formatters.Default, APIEntry.UnitType.UNITLESS));

            registerAPI(new PlotableAPIEntry(
                dataSources => { return dataSources.vessel.ActionGroups[KSPActionGroup.SAS]; },
                "v.sasValue", "Query SAS value", formatters.Default, APIEntry.UnitType.UNITLESS));

            registerAPI(new PlotableAPIEntry(
                dataSources => { return dataSources.vessel.ActionGroups[KSPActionGroup.Light]; },
                "v.lightValue", "Query light value", formatters.Default, APIEntry.UnitType.UNITLESS));

            registerAPI(new PlotableAPIEntry(
                dataSources => { return dataSources.vessel.ActionGroups[KSPActionGroup.Brakes]; },
                "v.brakeValue", "Query brake value", formatters.Default, APIEntry.UnitType.UNITLESS));

            registerAPI(new PlotableAPIEntry(
                dataSources => { return dataSources.vessel.ActionGroups[KSPActionGroup.Gear]; },
                "v.gearValue", "Query gear value", formatters.Default, APIEntry.UnitType.UNITLESS));

            registerAPI(new PlotableAPIEntry(
                dataSources => { return FlightInputHandler.fetch.precisionMode; },
                "v.precisionControlValue", "Query precision controls value", formatters.Default, APIEntry.UnitType.UNITLESS));
        }

        private DataLinkHandler.APIDelegate buildActionGroupToggleDelayedLamda(KSPActionGroup actionGroup)
        {
            return dataSources =>
            {
                if (dataSources.args.Count == 0)
                {
                    TelemachusBehaviour.instance.BroadcastMessage("queueDelayedAPI", new DelayedAPIEntry(dataSources.Clone(),
                            (x) => { dataSources.vessel.ActionGroups.ToggleGroup(actionGroup); return 0d; }),
                        UnityEngine.SendMessageOptions.DontRequireReceiver); return predictFailure(dataSources.vessel);
                }
                else
                {
                    TelemachusBehaviour.instance.BroadcastMessage("queueDelayedAPI", new DelayedAPIEntry(dataSources.Clone(),
                            (x) => { dataSources.vessel.ActionGroups.SetGroup(actionGroup, bool.Parse(dataSources.args[0])); return 0d; }),
                        UnityEngine.SendMessageOptions.DontRequireReceiver); return predictFailure(dataSources.vessel);
                }
            };
        }

        #endregion

        #region DataLinkHandler

        protected override int pausedHandler()
        {
            return PausedDataLinkHandler.partPaused();
        }

        #endregion

        #region Flight Control

        private void throttleUp()
        {
            FlightInputHandler.state.mainThrottle += 0.1f;

            if (FlightInputHandler.state.mainThrottle > 1)
            {
                FlightInputHandler.state.mainThrottle = 1f;
            }
        }

        private void throttleDown()
        {
            FlightInputHandler.state.mainThrottle -= 0.1f;

            if (FlightInputHandler.state.mainThrottle < 0)
            {
                FlightInputHandler.state.mainThrottle = 0f;
            }
        }

        private void throttleZero()
        {
            FlightInputHandler.state.mainThrottle = 0f;
        }

        private void throttleFull()
        {
            FlightInputHandler.state.mainThrottle = 1f;
        }

        private void setThrottle(DataSources dataSources)
        {
            FlightInputHandler.state.mainThrottle = float.Parse(dataSources.args[0]);
        }

        private static int predictFailure(Vessel vessel)
        {
            return PausedDataLinkHandler.partPaused();
        }

        #endregion
    }
}
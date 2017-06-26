using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Telemachus.DataLinkHandlers
{
    public class MechJebDataLinkHandler : DataLinkHandler
    {
        #region Initialisation

        public MechJebDataLinkHandler(FormatterProvider formatters)
            : base(formatters)
        {
            registerAPI(new ActionAPIEntry(
                dataSources =>
                {
                    TelemachusBehaviour.instance.BroadcastMessage("queueDelayedAPI", new DelayedAPIEntry(dataSources.Clone(),
                        (x) => { return reflectOff(dataSources); }), UnityEngine.SendMessageOptions.DontRequireReceiver);
                    return predictFailure(dataSources.vessel);
                },
                "mj.smartassoff", "Smart ASS Off", formatters.Default));

            registerAPI(new ActionAPIEntry(
                dataSources =>
                {
                    TelemachusBehaviour.instance.BroadcastMessage("queueDelayedAPI", new DelayedAPIEntry(dataSources.Clone(),
                            (x) => { return reflectAttitudeTo(dataSources, Vector3d.forward, "MANEUVER_NODE"); }),
                        UnityEngine.SendMessageOptions.DontRequireReceiver); return predictFailure(dataSources.vessel);
                },
                "mj.node", "Node", formatters.Default));

            registerAPI(new ActionAPIEntry(
                dataSources =>
                {
                    TelemachusBehaviour.instance.BroadcastMessage("queueDelayedAPI", new DelayedAPIEntry(dataSources.Clone(),
                            (x) => { return reflectAttitudeTo(dataSources, Vector3d.forward, "ORBIT"); }),
                        UnityEngine.SendMessageOptions.DontRequireReceiver); return predictFailure(dataSources.vessel);
                },
                "mj.prograde", "Prograde", formatters.Default));

            registerAPI(new ActionAPIEntry(
                dataSources =>
                {
                    TelemachusBehaviour.instance.BroadcastMessage("queueDelayedAPI", new DelayedAPIEntry(dataSources.Clone(),
                            (x) => { return reflectAttitudeTo(dataSources, Vector3d.back, "ORBIT"); }),
                        UnityEngine.SendMessageOptions.DontRequireReceiver); return predictFailure(dataSources.vessel);
                },
                "mj.retrograde", "Retrograde", formatters.Default));

            registerAPI(new ActionAPIEntry(
                dataSources =>
                {
                    TelemachusBehaviour.instance.BroadcastMessage("queueDelayedAPI", new DelayedAPIEntry(dataSources.Clone(),
                            (x) => { return reflectAttitudeTo(dataSources, Vector3d.left, "ORBIT"); }),
                        UnityEngine.SendMessageOptions.DontRequireReceiver); return predictFailure(dataSources.vessel);
                },
                "mj.normalplus", "Normal Plus", formatters.Default));

            registerAPI(new ActionAPIEntry(
                dataSources =>
                {
                    TelemachusBehaviour.instance.BroadcastMessage("queueDelayedAPI", new DelayedAPIEntry(dataSources.Clone(),
                            (x) => { return reflectAttitudeTo(dataSources, Vector3d.right, "ORBIT"); }),
                        UnityEngine.SendMessageOptions.DontRequireReceiver); return predictFailure(dataSources.vessel);
                },
                "mj.normalminus", "Normal Minus", formatters.Default));

            registerAPI(new ActionAPIEntry(
                dataSources =>
                {
                    TelemachusBehaviour.instance.BroadcastMessage("queueDelayedAPI", new DelayedAPIEntry(dataSources.Clone(),
                            (x) => { return reflectAttitudeTo(dataSources, Vector3d.up, "ORBIT"); }),
                        UnityEngine.SendMessageOptions.DontRequireReceiver); return predictFailure(dataSources.vessel);
                },
                "mj.radialplus", "Radial Plus", formatters.Default));

            registerAPI(new ActionAPIEntry(
                dataSources =>
                {
                    TelemachusBehaviour.instance.BroadcastMessage("queueDelayedAPI", new DelayedAPIEntry(dataSources.Clone(),
                            (x) => { return reflectAttitudeTo(dataSources, Vector3d.down, "ORBIT"); }),
                        UnityEngine.SendMessageOptions.DontRequireReceiver); return predictFailure(dataSources.vessel);
                },
                "mj.radialminus", "Radial Minus", formatters.Default));

            registerAPI(new ActionAPIEntry(
                dataSources =>
                {
                    TelemachusBehaviour.instance.BroadcastMessage("queueDelayedAPI", new DelayedAPIEntry(dataSources.Clone(),
                            (x) => { return (FlightGlobals.fetch.VesselTarget != null ? reflectAttitudeTo(dataSources, Vector3d.forward, "TARGET") : false); }),
                        UnityEngine.SendMessageOptions.DontRequireReceiver); return predictFailure(dataSources.vessel);
                },
                "mj.targetplus", "Target Plus", formatters.Default));

            registerAPI(new ActionAPIEntry(
                dataSources =>
                {
                    TelemachusBehaviour.instance.BroadcastMessage("queueDelayedAPI", new DelayedAPIEntry(dataSources.Clone(),
                            (x) => { return (FlightGlobals.fetch.VesselTarget != null ? reflectAttitudeTo(dataSources, Vector3d.back, "TARGET") : false); }),
                        UnityEngine.SendMessageOptions.DontRequireReceiver); return predictFailure(dataSources.vessel);
                },
                "mj.targetminus", "Target Minus", formatters.Default));

            registerAPI(new ActionAPIEntry(
                dataSources =>
                {
                    TelemachusBehaviour.instance.BroadcastMessage("queueDelayedAPI", new DelayedAPIEntry(dataSources.Clone(),
                            (x) => { return (FlightGlobals.fetch.VesselTarget != null ? reflectAttitudeTo(dataSources, Vector3d.forward, "RELATIVE_VELOCITY") : false); }),
                        UnityEngine.SendMessageOptions.DontRequireReceiver); return predictFailure(dataSources.vessel);
                },
                "mj.relativeplus", "Relative Plus", formatters.Default));

            registerAPI(new ActionAPIEntry(
                dataSources =>
                {
                    TelemachusBehaviour.instance.BroadcastMessage("queueDelayedAPI", new DelayedAPIEntry(dataSources.Clone(),
                            (x) => { return (FlightGlobals.fetch.VesselTarget != null ? reflectAttitudeTo(dataSources, Vector3d.back, "RELATIVE_VELOCITY") : false); }),
                        UnityEngine.SendMessageOptions.DontRequireReceiver); return predictFailure(dataSources.vessel);
                },
                "mj.relativeminus", "Relative Minus", formatters.Default));

            registerAPI(new ActionAPIEntry(
                dataSources =>
                {
                    TelemachusBehaviour.instance.BroadcastMessage("queueDelayedAPI", new DelayedAPIEntry(dataSources.Clone(),
                            (x) => { return (FlightGlobals.fetch.VesselTarget != null ? reflectAttitudeTo(dataSources, Vector3d.forward, "TARGET_ORIENTATION") : false); }),
                        UnityEngine.SendMessageOptions.DontRequireReceiver); return predictFailure(dataSources.vessel);
                },
                "mj.parallelplus", "Parallel Plus", formatters.Default));

            registerAPI(new ActionAPIEntry(
                dataSources =>
                {
                    TelemachusBehaviour.instance.BroadcastMessage("queueDelayedAPI", new DelayedAPIEntry(dataSources.Clone(),
                            (x) => { return (FlightGlobals.fetch.VesselTarget != null ? reflectAttitudeTo(dataSources, Vector3d.back, "TARGET_ORIENTATION") : false); }),
                        UnityEngine.SendMessageOptions.DontRequireReceiver); return predictFailure(dataSources.vessel);
                },
                "mj.parallelminus", "Parallel Minus", formatters.Default));

            registerAPI(new ActionAPIEntry(
                dataSources =>
                {
                    TelemachusBehaviour.instance.BroadcastMessage("queueDelayedAPI", new DelayedAPIEntry(dataSources.Clone(),
                            (x) => { return surface(dataSources); }),
                        UnityEngine.SendMessageOptions.DontRequireReceiver);
                    return predictFailure(dataSources.vessel);
                },
                "mj.surface", "Surface [float heading, float pitch]", formatters.Default));

            registerAPI(new ActionAPIEntry(
                dataSources =>
                {
                    TelemachusBehaviour.instance.BroadcastMessage("queueDelayedAPI", new DelayedAPIEntry(dataSources.Clone(),
                        (x) =>
                        {
                            return reflectAttitudeTo(dataSources, double.Parse(dataSources.args[0]),
                                double.Parse(dataSources.args[1]), double.Parse(dataSources.args[2])
                            );
                        }), UnityEngine.SendMessageOptions.DontRequireReceiver);
                    return predictFailure(dataSources.vessel);
                },
                "mj.surface2", "Surface [double heading, double pitch, double roll]", formatters.Default));

            registerAPI(new APIEntry(
                dataSources => {
                    PluginLogger.debug("Start GET");
                    return getStagingInfo(dataSources);
                },
                "mj.stagingInfo", "Staging Info [object stagingInfo]",
                formatters.MechJebSimulation, APIEntry.UnitType.UNITLESS));
        }

        #endregion

        #region Flight Control

        private bool surface(DataSources dataSources)
        {
            Quaternion r = Quaternion.AngleAxis(float.Parse(dataSources.args[0]), Vector3.up) * Quaternion.AngleAxis(-float.Parse(dataSources.args[1]), Vector3.right);
            return reflectAttitudeTo(dataSources, r * Vector3d.forward, "SURFACE_NORTH");
        }

        private bool reflectOff(DataSources dataSources)
        {
            object attitude = null;
            Type attitudeType = getAttitudeType(dataSources, ref attitude);
            if (attitudeType != null)
            {
                MethodInfo methodInfo = attitudeType.GetMethod("attitudeDeactivate");
                methodInfo.Invoke(attitude, new object[] { });
                return true;
            }

            return false;
        }

        private bool reflectAttitudeTo(DataSources dataSources, Vector3d v, string reference)
        {
            object attitude = null;

            Type attitudeType = getAttitudeType(dataSources, ref attitude);
            if (attitudeType != null)
            {
                Type attitudeReferenceType = attitude.GetType().GetProperty("attitudeReference",
                    BindingFlags.Public | BindingFlags.Instance).GetValue(attitude, null).GetType();


                MethodInfo methodInfo = attitudeType.GetMethod("attitudeTo", new[] { typeof(Vector3d), 
                    attitudeReferenceType, typeof(object) });

                methodInfo.Invoke(attitude, new object[] { v, Enum.Parse(attitudeReferenceType, reference), this });

                return true;
            }

            return false;
        }

        private bool reflectAttitudeTo(DataSources dataSources, double heading, double pitch, double roll)
        {
            object attitude = null;

            Type attitudeType = getAttitudeType(dataSources, ref attitude);
            if (attitudeType != null)
            {
                MethodInfo methodInfo = attitudeType.GetMethod("attitudeTo", new[] { typeof(double),
                    typeof(double),typeof(double), typeof(object) });

                methodInfo.Invoke(attitude, new object[] { heading, pitch, roll, this });

                return true;
            }

            return false;
        }

        private Type getAttitudeType(DataSources dataSources, ref object attitude)
        {
            PartModule mechJebCore = findMechJeb(dataSources.vessel);

            if (mechJebCore == null)
            {
                PluginLogger.debug("No Mechjeb part installed.");
                return null;
            }
            else
            {
                try
                {
                    PluginLogger.debug("Mechjeb part installed, reflecting.");
                    Type mechJebCoreType = mechJebCore.GetType();
                    FieldInfo attitudeField = mechJebCoreType.GetField("attitude", BindingFlags.Public | BindingFlags.Instance);
                    attitude = attitudeField.GetValue(mechJebCore);

                    Type attitudeReferenceType = attitude.GetType().GetProperty("attitudeReference",
                        BindingFlags.Public | BindingFlags.Instance).GetValue(attitude, null).GetType();

                    return attitude.GetType();
                }
                catch (Exception e)
                {
                    PluginLogger.debug(e.Message + " " + e.StackTrace);
                }

                return null;
            }
        }

        private MechJebSimulation getStagingInfo(DataSources dataSources)
        {
            object stagingInfo = null;

            PluginLogger.debug("Grabbing staging info");
            Type stagingInfoType = getStagingInfoType(dataSources, ref stagingInfo);

            MechJebSimulation stats = new MechJebSimulation();

            if (stagingInfo != null)
            {
                PluginLogger.debug("Found the staging type, getting the staging info");

                stats.convertMechJebData(stagingInfoType, stagingInfo);

                return stats;
            }

            PluginLogger.debug("Could not get staging info");
            return null;
        }

        private Type getStagingInfoType(DataSources dataSources, ref object stagingInfo)
        {
            PluginLogger.debug("Finding part for staging");
            PartModule mechJebCore = findMechJeb(dataSources.vessel);

            if (mechJebCore == null)
            {
                PluginLogger.debug("No Mechjeb part installed.");
                return null;
            }
            else
            {
                try
                {
                    PluginLogger.debug("Mechjeb part installed, reflecting.");
                    Type mechJebCoreType = mechJebCore.GetType();
                    PluginLogger.debug("Trying to get computermodule Method info");
                    MethodInfo mechJebCoreGetComputerModuleMethodInfo = mechJebCoreType.GetMethod("GetComputerModule", new[] {typeof(string) } );
                    PluginLogger.debug("Trying to get calling computer method");
                    stagingInfo = mechJebCoreGetComputerModuleMethodInfo.Invoke(mechJebCore, new object[] { "MechJebModuleStageStats" });

                    PluginLogger.debug("Staging Info Type: " + stagingInfo.GetType().Name);
                    PluginLogger.print("Staging Info Type: " + stagingInfo.GetType().Name);


                    return stagingInfo.GetType();
                }
                catch (Exception e)
                {
                    PluginLogger.debug(e.Message + " " + e.StackTrace);
                }

                return null;
            }
        }

        private static int predictFailure(Vessel vessel)
        {

            int pause = PausedDataLinkHandler.partPaused();

            if (pause > 0)
            {
                return pause;
            }

            if (findMechJeb(vessel) == null)
            {
                return 5;
            }

            return 0;
        }

        private static PartModule findMechJeb(Vessel vessel)
        {
            try
            {
                List<Part> pl = vessel.parts.FindAll(p => p.Modules.Contains("MechJebCore"));

                foreach (PartModule m in pl[0].Modules)
                {
                    if (m.GetType().Name.Equals("MechJebCore"))
                    {
                        return m;
                    }
                }
            }
            catch (Exception e)
            {
                PluginLogger.debug(e.Message + " " + e.StackTrace);
            }

            return null;
        }

        #endregion

        #region DataLinkHandler

        protected override int pausedHandler()
        {
            return PausedDataLinkHandler.partPaused();
        }

        #endregion

        #region Data Structures
        public class MechJebSimulation
        {
            public List<MechJebStageSimulationStats> vacuumStats = new List<MechJebStageSimulationStats>();
            public List<MechJebStageSimulationStats> atmoStats = new List<MechJebStageSimulationStats>();

            public void convertMechJebData(Type stagingInfoType, object stagingInfo)
            {
                FieldInfo atmoStatsField = stagingInfoType.GetField("atmoStats", BindingFlags.Public | BindingFlags.Instance);
                PluginLogger.debug("Getting Atmo Stats");
                Array atmoStats = (Array)atmoStatsField.GetValue(stagingInfo);

                this.populateStats(atmoStats, this.atmoStats);

                FieldInfo vacStatsField = stagingInfoType.GetField("vacStats", BindingFlags.Public | BindingFlags.Instance);
                PluginLogger.debug("Getting Vac Stats");
                Array vacStats = (Array)vacStatsField.GetValue(stagingInfo);

                this.populateStats(vacStats, this.vacuumStats);

            }

            private void populateStats(Array mechJebStatsArray, List<MechJebStageSimulationStats> destinationStats) {
                PluginLogger.debug("Building stats object. Size: " + mechJebStatsArray.Length);
                foreach (object mechJebStat in mechJebStatsArray)
                {
                    MechJebStageSimulationStats stat = new MechJebStageSimulationStats();

                    //PluginLogger.debug("Getting Type");
                    Type statType = mechJebStat.GetType();
                    //PluginLogger.debug("Get start mass");
                    stat.startMass = (float)statType.GetField("startMass").GetValue(mechJebStat);

                    //PluginLogger.debug("Get end mass");
                    stat.endMass = (float)statType.GetField("endMass").GetValue(mechJebStat);

                    //PluginLogger.debug("Get start Thrust");
                    stat.startThrust = (float)statType.GetField("startThrust").GetValue(mechJebStat);
                    //PluginLogger.debug("startThrust: " + statType.GetField("startThrust").GetValue(mechJebStat).ToString());

                    //PluginLogger.debug("Get max Accel");
                    stat.maxAccel = (float)statType.GetField("maxAccel").GetValue(mechJebStat);

                    //PluginLogger.debug("Get deltaTime");
                    stat.deltaTime = (float)statType.GetField("deltaTime").GetValue(mechJebStat);

                    //PluginLogger.debug("Get deltaV");
                    stat.deltaV = (float)statType.GetField("deltaV").GetValue(mechJebStat);

                    //PluginLogger.debug("Get resourceMass");
                    stat.resourceMass = (float)statType.GetField("resourceMass").GetValue(mechJebStat);

                    //PluginLogger.debug("Get isp");
                    stat.isp = (float)statType.GetField("isp").GetValue(mechJebStat);
                    //PluginLogger.debug("ISP: " + stat.stagedMass.ToString());

                    //PluginLogger.debug("Get stagedMass");
                    stat.stagedMass = (float)statType.GetField("stagedMass").GetValue(mechJebStat);
                    //PluginLogger.debug("stagedMass: " + stat.stagedMass.ToString());

                    destinationStats.AddUnique(stat);

                    //PluginLogger.debug("Start Mass: " + stat.startMass.ToString());
                }
            }
        }

        public class MechJebStageSimulationStats
        {
            public float startMass;
            public float endMass;
            public float startThrust;
            public float maxAccel;
            public float deltaTime;
            public float deltaV;

            public float resourceMass;
            public float isp;
            public float stagedMass;
        }
        #endregion
    }
}
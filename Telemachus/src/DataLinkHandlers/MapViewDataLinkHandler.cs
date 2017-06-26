using UnityEngine;

namespace Telemachus.DataLinkHandlers
{
    public class MapViewDataLinkHandler : DataLinkHandler
    {

        static float ut = 0, x = 0, y = 0, z = 0;
        static int maneuver_node_id = 0;
        #region Initialisation

        public MapViewDataLinkHandler(FormatterProvider formatters)
            : base(formatters)
        {
            registerAPI(new ActionAPIEntry(
                dataSources =>
                {
                    TelemachusBehaviour.instance.BroadcastMessage("queueDelayedAPI", new DelayedAPIEntry(dataSources.Clone(),
                            (x) =>
                            {
                                if (MapView.MapIsEnabled)
                                { MapView.ExitMapView(); }
                                else { MapView.EnterMapView(); } return 0d;
                            }),
                        UnityEngine.SendMessageOptions.DontRequireReceiver); return false;
                },
                "m.toggleMapView", " Toggle Map View", formatters.Default));

            registerAPI(new ActionAPIEntry(
                dataSources =>
                {
                    TelemachusBehaviour.instance.BroadcastMessage("queueDelayedAPI", new DelayedAPIEntry(dataSources.Clone(),
                            (x) =>
                            {
                                MapView.EnterMapView(); return 0d;
                            }),
                        UnityEngine.SendMessageOptions.DontRequireReceiver); return false;
                },
                "m.enterMapView", " Enter Map View", formatters.Default));

            registerAPI(new ActionAPIEntry(
                dataSources =>
                {
                    TelemachusBehaviour.instance.BroadcastMessage("queueDelayedAPI", new DelayedAPIEntry(dataSources.Clone(),
                            (x) =>
                            {
                                MapView.ExitMapView(); return 0d;
                            }),
                        UnityEngine.SendMessageOptions.DontRequireReceiver); return false;
                },
                "m.exitMapView", " Exit Map View", formatters.Default));

            registerAPI(new APIEntry(
                dataSources => {
                    PluginLogger.debug("Start GET");
                    return dataSources.vessel.patchedConicSolver.maneuverNodes;
                },
                "o.maneuverNodes", "Maneuver Nodes  [object maneuverNodes]",
                formatters.ManeuverNodeList, APIEntry.UnitType.UNITLESS));

            registerAPI(new PlotableAPIEntry(
                dataSources => {
                    ManeuverNode node = getManueverNode(dataSources, int.Parse(dataSources.args[0]));
                    if (node == null) { return null; }

                    int index = int.Parse(dataSources.args[1]);
                    float ut = float.Parse(dataSources.args[2]);

                    Orbit orbitPatch = OrbitPatches.getOrbitPatch(node.nextPatch, index);
                    if (orbitPatch == null) { return null; }
                    return orbitPatch.TrueAnomalyAtUT(ut);
                },
                "o.maneuverNodes.trueAnomalyAtUTForManeuverNodesOrbitPatch", "For a maneuver node, The orbit patch's True Anomaly at Universal Time [int id, orbit patch index, universal time]", formatters.Default, APIEntry.UnitType.DEG));

            registerAPI(new PlotableAPIEntry(
                dataSources => {
                    ManeuverNode node = getManueverNode(dataSources, int.Parse(dataSources.args[0]));
                    if (node == null) { return null; }

                    int index = int.Parse(dataSources.args[1]);
                    float trueAnomaly = float.Parse(dataSources.args[2]);

                    Orbit orbitPatch = OrbitPatches.getOrbitPatch(node.nextPatch, index);
                    if (orbitPatch == null) { return null; }

                    double now = Planetarium.GetUniversalTime();
                    return orbitPatch.GetUTforTrueAnomaly(trueAnomaly, now);
                },
                "o.maneuverNodes.UTForTrueAnomalyForManeuverNodesOrbitPatch", "For a maneuver node, The orbit patch's True Anomaly at Universal Time [int id, orbit patch index, universal time]", formatters.Default, APIEntry.UnitType.DATE));
            registerAPI(new PlotableAPIEntry(
                dataSources => {
                    ManeuverNode node = getManueverNode(dataSources, int.Parse(dataSources.args[0]));
                    if (node == null) { return null; }

                    int index = int.Parse(dataSources.args[1]);
                    float trueAnomaly = float.Parse(dataSources.args[2]);

                    Orbit orbitPatch = OrbitPatches.getOrbitPatch(node.nextPatch, index);
                    if (orbitPatch == null) { return null; }
                    return orbitPatch.getRelativePositionFromTrueAnomaly(trueAnomaly);
                },
                "o.maneuverNodes.relativePositionAtTrueAnomalyForManeuverNodesOrbitPatch", "For a maneuver node, The orbit patch's predicted displacement from the center of the main body at the given true anomaly [int id, orbit patch index, true anomaly]", formatters.Vector3d, APIEntry.UnitType.UNITLESS));
            registerAPI(new PlotableAPIEntry(
                dataSources => {
                    ManeuverNode node = getManueverNode(dataSources, int.Parse(dataSources.args[0]));
                    if (node == null) { return null; }

                    int index = int.Parse(dataSources.args[1]);
                    double ut = double.Parse(dataSources.args[2]);

                    Orbit orbitPatch = OrbitPatches.getOrbitPatch(node.nextPatch, index);
                    if (orbitPatch == null) { return null; }

                    return orbitPatch.getRelativePositionAtUT(ut);
                },
                "o.maneuverNodes.relativePositionAtUTForManeuverNodesOrbitPatch", "For a maneuver node, The orbit patch's predicted displacement from the center of the main body at the given universal time [int id, orbit patch index, universal time]", formatters.Vector3d, APIEntry.UnitType.UNITLESS));


            registerAPI(new ActionAPIEntry(
                dataSources =>
                {

                    ut = float.Parse(dataSources.args[0]);
                    ManeuverNode node = dataSources.vessel.patchedConicSolver.AddManeuverNode(ut);

                    x = float.Parse(dataSources.args[1]);
                    y = float.Parse(dataSources.args[2]);
                    z = float.Parse(dataSources.args[3]);

                    PluginLogger.debug("x: " + x + "y: " + y + "z: " + z);

                    Vector3d deltaV = new Vector3d(x,y,z);
                    node.OnGizmoUpdated(deltaV, ut);

                    return node;
                },
                "o.addManeuverNode", "Add a manuever based on a UT and DeltaV X, Y and Z [float ut, float x, y, z]", formatters.ManeuverNode));

            registerAPI(new ActionAPIEntry(
                dataSources =>
                {
                    ManeuverNode node = getManueverNode(dataSources, int.Parse(dataSources.args[0]));
                    if (node == null) { return null; }

                            
                    ut = float.Parse(dataSources.args[1]);

                    x = float.Parse(dataSources.args[2]);
                    y = float.Parse(dataSources.args[3]);
                    z = float.Parse(dataSources.args[4]);

                    Vector3d deltaV = new Vector3d(x, y, z);
                    node.OnGizmoUpdated(deltaV, ut);
                    return node;
                        
                },
                "o.updateManeuverNode", "Set a manuever node's UT and DeltaV X, Y and Z [int id, float ut, float x, y, z]", formatters.ManeuverNode));

            registerAPI(new ActionAPIEntry(
                dataSources =>
                {
                    ManeuverNode node = getManueverNode(dataSources, int.Parse(dataSources.args[0]));
                    if (node == null) { return false; }

                    dataSources.vessel.patchedConicSolver.RemoveManeuverNode(node);
                    return true;
                },
                "o.removeManeuverNode", "Remove a manuever node [int id]", formatters.Default));
        }



        private ManeuverNode getManueverNode(DataSources datasources, int id)
        {
            PluginLogger.debug("GETTING NODE");
            //return null if the count is less than the ID or the ID is negative
            if(datasources.vessel.patchedConicSolver.maneuverNodes.Count <= id || id < 0)
            {
                return null;
            }

            PluginLogger.debug("FINDING THE RIGHT NODE. ID: " + id);
            ManeuverNode[] nodes = datasources.vessel.patchedConicSolver.maneuverNodes.ToArray();
            return (ManeuverNode) nodes.GetValue(id);
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
    }
}
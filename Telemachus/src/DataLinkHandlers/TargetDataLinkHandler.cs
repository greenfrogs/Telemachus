using System;
using UnityEngine;

namespace Telemachus.DataLinkHandlers
{
    public class TargetDataLinkHandler : DataLinkHandler
    {
        #region Initialisation

        public TargetDataLinkHandler(FormatterProvider formatters)
            : base(formatters)
        {
            registerAPI(new PlotableAPIEntry(
                dataSources =>
                {
                    return FlightGlobals.fetch.VesselTarget != null ? FlightGlobals.fetch.VesselTarget.GetName() : "No Target Selected.";
                },
                "tar.name", "Target Name", formatters.Default, APIEntry.UnitType.STRING));

            registerAPI(new PlotableAPIEntry(
                dataSources =>
                {
                    return FlightGlobals.fetch.VesselTarget != null ? FlightGlobals.fetch.VesselTarget.GetType().ToString() : "";
                },
                "tar.type", "Target Type", formatters.Default, APIEntry.UnitType.STRING));
            registerAPI(new PlotableAPIEntry(
                dataSources => { return FlightGlobals.fetch.VesselTarget != null ? Vector3.Distance(FlightGlobals.fetch.VesselTarget.GetTransform().position, dataSources.vessel.GetTransform().position) : 0; },
                "tar.distance", "Target Distance", formatters.Default, APIEntry.UnitType.DISTANCE));

            registerAPI(new PlotableAPIEntry(
                dataSources => { return FlightGlobals.fetch.VesselTarget != null ? (FlightGlobals.fetch.VesselTarget.GetOrbit().GetVel() - dataSources.vessel.orbit.GetVel()).magnitude : 0; },
                "tar.o.relativeVelocity", "Target Relative Velocity", formatters.Default, APIEntry.UnitType.VELOCITY));
            registerAPI(new PlotableAPIEntry(
                dataSources => { return FlightGlobals.fetch.VesselTarget != null ? FlightGlobals.fetch.VesselTarget.GetOrbit().vel.magnitude : 0; },
                "tar.o.velocity", "Target Velocity", formatters.Default, APIEntry.UnitType.VELOCITY));
            registerAPI(new PlotableAPIEntry(
                dataSources => { return FlightGlobals.fetch.VesselTarget != null ? FlightGlobals.fetch.VesselTarget.GetOrbit().PeA : 0; },
                "tar.o.PeA", "Target Periapsis", formatters.Default, APIEntry.UnitType.DISTANCE));
            registerAPI(new PlotableAPIEntry(
                dataSources => { return FlightGlobals.fetch.VesselTarget != null ? FlightGlobals.fetch.VesselTarget.GetOrbit().ApA : 0; },
                "tar.o.ApA", "Target Apoapsis", formatters.Default, APIEntry.UnitType.DISTANCE));
            registerAPI(new PlotableAPIEntry(
                dataSources => { return FlightGlobals.fetch.VesselTarget != null ? FlightGlobals.fetch.VesselTarget.GetOrbit().timeToAp : 0; },
                "tar.o.timeToAp", "Target Time to Apoapsis", formatters.Default, APIEntry.UnitType.TIME));
            registerAPI(new PlotableAPIEntry(
                dataSources => { return FlightGlobals.fetch.VesselTarget != null ? FlightGlobals.fetch.VesselTarget.GetOrbit().timeToPe : 0; },
                "tar.o.timeToPe", "Target Time to Periapsis", formatters.Default, APIEntry.UnitType.TIME));
            registerAPI(new PlotableAPIEntry(
                dataSources => { return FlightGlobals.fetch.VesselTarget != null ? FlightGlobals.fetch.VesselTarget.GetOrbit().inclination : 0; },
                "tar.o.inclination", "Target Inclination", formatters.Default, APIEntry.UnitType.DEG));
            registerAPI(new PlotableAPIEntry(
                dataSources => { return FlightGlobals.fetch.VesselTarget != null ? FlightGlobals.fetch.VesselTarget.GetOrbit().eccentricity : 0; },
                "tar.o.eccentricity", "Target Eccentricity", formatters.Default, APIEntry.UnitType.UNITLESS));
            registerAPI(new PlotableAPIEntry(
                dataSources => { return FlightGlobals.fetch.VesselTarget != null ? FlightGlobals.fetch.VesselTarget.GetOrbit().period : 0; },
                "tar.o.period", "Target Orbital Period", formatters.Default, APIEntry.UnitType.TIME));
            registerAPI(new PlotableAPIEntry(
                dataSources => { return FlightGlobals.fetch.VesselTarget != null ? FlightGlobals.fetch.VesselTarget.GetOrbit().argumentOfPeriapsis : 0; },
                "tar.o.argumentOfPeriapsis", "Target Argument of Periapsis", formatters.Default, APIEntry.UnitType.DEG));
            registerAPI(new PlotableAPIEntry(
                dataSources => { return FlightGlobals.fetch.VesselTarget != null ? FlightGlobals.fetch.VesselTarget.GetOrbit().timeToTransition1 : 0; },
                "tar.o.timeToTransition1", "Target Time to Transition 1", formatters.Default, APIEntry.UnitType.TIME));
            registerAPI(new PlotableAPIEntry(
                dataSources => { return FlightGlobals.fetch.VesselTarget != null ? FlightGlobals.fetch.VesselTarget.GetOrbit().timeToTransition1 : 0; },
                "tar.o.timeToTransition2", "Target Time to Transition 2", formatters.Default, APIEntry.UnitType.TIME));
            registerAPI(new PlotableAPIEntry(
                dataSources => { return FlightGlobals.fetch.VesselTarget != null ? FlightGlobals.fetch.VesselTarget.GetOrbit().semiMajorAxis : 0; },
                "tar.o.sma", "Target Semimajor Axis", formatters.Default, APIEntry.UnitType.DISTANCE));
            registerAPI(new PlotableAPIEntry(
                dataSources => { return FlightGlobals.fetch.VesselTarget != null ? FlightGlobals.fetch.VesselTarget.GetOrbit().LAN : 0; },
                "tar.o.lan", "Target Longitude of Ascending Node", formatters.Default, APIEntry.UnitType.DEG));
            registerAPI(new PlotableAPIEntry(
                dataSources =>
                {
                    if (FlightGlobals.fetch.VesselTarget == null) { return 0; }
                    Orbit orbit = FlightGlobals.fetch.VesselTarget.GetOrbit();
                    return orbit.getObtAtUT(0) / orbit.period * (2.0 * Math.PI);
                },
                "tar.o.maae", "Target Mean Anomaly at Epoch", formatters.Default, APIEntry.UnitType.UNITLESS));
            registerAPI(new PlotableAPIEntry(
                dataSources => { return FlightGlobals.fetch.VesselTarget != null ? Planetarium.GetUniversalTime() - FlightGlobals.fetch.VesselTarget.GetOrbit().ObT : 0; },
                "tar.o.timeOfPeriapsisPassage", "Target Time of Periapsis Passage", formatters.Default, APIEntry.UnitType.DATE));
            registerAPI(new PlotableAPIEntry(
                dataSources => { return FlightGlobals.fetch.VesselTarget != null ? FlightGlobals.fetch.VesselTarget.GetOrbit().TrueAnomalyAtUT(Planetarium.GetUniversalTime()) * (180.0 / Math.PI) : double.NaN; },
                "tar.o.trueAnomaly", "Target True Anomaly", formatters.Default, APIEntry.UnitType.DEG));
            registerAPI(new PlotableAPIEntry(
                dataSources => { return FlightGlobals.fetch.VesselTarget != null ? FlightGlobals.fetch.VesselTarget.GetOrbit().referenceBody.name : ""; },
                "tar.o.orbitingBody", "Target Orbiting Body", formatters.Default, APIEntry.UnitType.STRING));
            registerAPI(new APIEntry(
                dataSources =>
                {
                    if (FlightGlobals.fetch.VesselTarget == null)
                    {
                        return null;
                    }
                    return OrbitPatches.getPatchesForOrbit(FlightGlobals.fetch.VesselTarget.GetOrbit());
                },
                "tar.o.orbitPatches", "Detailed Orbit Patches Info [object orbitPatchInfo]",
                formatters.OrbitPatchList, APIEntry.UnitType.UNITLESS));

            registerAPI(new PlotableAPIEntry(
                dataSources => {
                    if (FlightGlobals.fetch.VesselTarget == null) { return null; }
                    int index = int.Parse(dataSources.args[0]);
                    float ut = float.Parse(dataSources.args[1]);

                    Orbit orbitPatch = OrbitPatches.getOrbitPatch(FlightGlobals.fetch.VesselTarget.GetOrbit(), index);
                    if (orbitPatch == null) { return null; }
                    return orbitPatch.TrueAnomalyAtUT(ut);
                },
                "tar.o.trueAnomalyAtUTForOrbitPatch", "The orbit patch's True Anomaly at Universal Time [orbit patch index, universal time]", formatters.Default, APIEntry.UnitType.DEG));

            registerAPI(new PlotableAPIEntry(
                dataSources => {
                    if (FlightGlobals.fetch.VesselTarget == null) { return null; }
                    int index = int.Parse(dataSources.args[0]);
                    float trueAnomaly = float.Parse(dataSources.args[1]);

                    Orbit orbitPatch = OrbitPatches.getOrbitPatch(FlightGlobals.fetch.VesselTarget.GetOrbit(), index);
                    if (orbitPatch == null) { return null; }

                    double now = Planetarium.GetUniversalTime();
                    return orbitPatch.GetUTforTrueAnomaly(trueAnomaly, now);
                },
                "tar.o.UTForTrueAnomalyForOrbitPatch", "The orbit patch's True Anomaly at Universal Time [orbit patch index, universal time]", formatters.Default, APIEntry.UnitType.DATE));
            registerAPI(new APIEntry(
                dataSources => {
                    if (FlightGlobals.fetch.VesselTarget == null) { return null; }
                    int index = int.Parse(dataSources.args[0]);
                    float trueAnomaly = float.Parse(dataSources.args[1]);

                    Orbit orbitPatch = OrbitPatches.getOrbitPatch(FlightGlobals.fetch.VesselTarget.GetOrbit(), index);
                    if (orbitPatch == null) { return null; }
                    return orbitPatch.getRelativePositionFromTrueAnomaly(trueAnomaly);
                },
                "tar.o.relativePositionAtTrueAnomalyForOrbitPatch", "The orbit patch's predicted displacement from the center of the main body at the given true anomaly [orbit patch index, true anomaly]", formatters.Vector3d, APIEntry.UnitType.UNITLESS));

            registerAPI(new APIEntry(
                dataSources => {
                    if (FlightGlobals.fetch.VesselTarget == null) { return null; }
                    int index = int.Parse(dataSources.args[0]);
                    double ut = double.Parse(dataSources.args[1]);

                    Orbit orbitPatch = OrbitPatches.getOrbitPatch(FlightGlobals.fetch.VesselTarget.GetOrbit(), index);
                    if (orbitPatch == null) { return null; }
                    return orbitPatch.getRelativePositionAtUT(ut);
                },
                "tar.o.relativePositionAtUTForOrbitPatch", "The orbit patch's predicted displacement from the center of the main body at the given universal time [orbit patch index, universal time]", formatters.Vector3d, APIEntry.UnitType.UNITLESS));
        }

        #endregion

        #region DataLinkHandler



        public override bool process(String API, out APIEntry result)
        {
            if (!base.process(API, out result))
            {
                if (API.StartsWith("tar."))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        #endregion
    }
}
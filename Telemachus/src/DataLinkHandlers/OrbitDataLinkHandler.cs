using System;

namespace Telemachus.DataLinkHandlers
{
    public class OrbitDataLinkHandler : DataLinkHandler
    {
        #region Initialisation

        public OrbitDataLinkHandler(FormatterProvider formatters)
            : base(formatters)
        {
            registerAPI(new PlotableAPIEntry(
                dataSources => { return dataSources.vessel.orbit.GetRelativeVel().magnitude; },
                "o.relativeVelocity", "Relative Velocity", formatters.Default, APIEntry.UnitType.VELOCITY));
            registerAPI(new PlotableAPIEntry(
                dataSources => { return dataSources.vessel.orbit.PeA; },
                "o.PeA", "Periapsis", formatters.Default, APIEntry.UnitType.DISTANCE));
            registerAPI(new PlotableAPIEntry(
                dataSources => { return dataSources.vessel.orbit.ApA; },
                "o.ApA", "Apoapsis", formatters.Default, APIEntry.UnitType.DISTANCE));
            registerAPI(new PlotableAPIEntry(
                dataSources => { return dataSources.vessel.orbit.timeToAp; },
                "o.timeToAp", "Time to Apoapsis", formatters.Default, APIEntry.UnitType.TIME));
            registerAPI(new PlotableAPIEntry(
                dataSources => { return dataSources.vessel.orbit.timeToPe; },
                "o.timeToPe", "Time to Periapsis", formatters.Default, APIEntry.UnitType.TIME));
            registerAPI(new PlotableAPIEntry(
                dataSources => { return dataSources.vessel.orbit.inclination; },
                "o.inclination", "Inclination", formatters.Default, APIEntry.UnitType.DEG));
            registerAPI(new PlotableAPIEntry(
                dataSources => { return dataSources.vessel.orbit.eccentricity; },
                "o.eccentricity", "Eccentricity", formatters.Default, APIEntry.UnitType.UNITLESS));
            registerAPI(new PlotableAPIEntry(
                dataSources => { return dataSources.vessel.orbit.epoch; },
                "o.epoch", "Epoch", formatters.Default, APIEntry.UnitType.UNITLESS));
            registerAPI(new PlotableAPIEntry(
                dataSources => { return dataSources.vessel.orbit.period; },
                "o.period", "Orbital Period", formatters.Default, APIEntry.UnitType.TIME));
            registerAPI(new PlotableAPIEntry(
                dataSources => { return dataSources.vessel.orbit.argumentOfPeriapsis; },
                "o.argumentOfPeriapsis", "Argument of Periapsis", formatters.Default, APIEntry.UnitType.DEG));
            registerAPI(new PlotableAPIEntry(
                dataSources => { return dataSources.vessel.orbit.timeToTransition1; },
                "o.timeToTransition1", "Time to Transition 1", formatters.Default, APIEntry.UnitType.TIME));
            registerAPI(new PlotableAPIEntry(
                dataSources => { return dataSources.vessel.orbit.timeToTransition1; },
                "o.timeToTransition2", "Time to Transition 2", formatters.Default, APIEntry.UnitType.TIME));
            registerAPI(new PlotableAPIEntry(
                dataSources => { return dataSources.vessel.orbit.semiMajorAxis; },
                "o.sma", "Semimajor Axis", formatters.Default, APIEntry.UnitType.DISTANCE));
            registerAPI(new PlotableAPIEntry(
                dataSources => { return dataSources.vessel.orbit.LAN; },
                "o.lan", "Longitude of Ascending Node", formatters.Default, APIEntry.UnitType.DEG));
            registerAPI(new PlotableAPIEntry(
                dataSources =>
                {
                    Orbit orbit = dataSources.vessel.orbit;
                    return orbit.getObtAtUT(0) / orbit.period * (2.0 * Math.PI);
                },
                "o.maae", "Mean Anomaly at Epoch", formatters.Default, APIEntry.UnitType.UNITLESS));
            registerAPI(new PlotableAPIEntry(
                dataSources => { return Planetarium.GetUniversalTime() - dataSources.vessel.orbit.ObT; },
                "o.timeOfPeriapsisPassage", "Time of Periapsis Passage", formatters.Default, APIEntry.UnitType.DATE));
            registerAPI(new PlotableAPIEntry(
                dataSources => { return dataSources.vessel.orbit.TrueAnomalyAtUT(Planetarium.GetUniversalTime()) * (180.0 / Math.PI); },
                "o.trueAnomaly", "True Anomaly", formatters.Default, APIEntry.UnitType.DEG));
            registerAPI(new APIEntry(
                dataSources => {
                    return OrbitPatches.getPatchesForOrbit(dataSources.vessel.orbit);
                },
                "o.orbitPatches", "Detailed Orbit Patches Info [object orbitPatchInfo]",
                formatters.OrbitPatchList, APIEntry.UnitType.UNITLESS));
            registerAPI(new PlotableAPIEntry(
                dataSources => {
                    int index = int.Parse(dataSources.args[0]);
                    float ut = float.Parse(dataSources.args[1]);

                    Orbit orbitPatch = OrbitPatches.getOrbitPatch(dataSources.vessel.orbit, index);
                    if(orbitPatch == null) { return null; }
                    return orbitPatch.TrueAnomalyAtUT(ut);
                },
                "o.trueAnomalyAtUTForOrbitPatch", "The orbit patch's True Anomaly at Universal Time [orbit patch index, universal time]", formatters.Default, APIEntry.UnitType.DEG));

            registerAPI(new PlotableAPIEntry(
                dataSources => {
                    int index = int.Parse(dataSources.args[0]);
                    float trueAnomaly = float.Parse(dataSources.args[1]);

                    Orbit orbitPatch = OrbitPatches.getOrbitPatch(dataSources.vessel.orbit, index);
                    if (orbitPatch == null) { return null; }

                    double now = Planetarium.GetUniversalTime();
                    return orbitPatch.GetUTforTrueAnomaly(trueAnomaly, now);
                },
                "o.UTForTrueAnomalyForOrbitPatch", "The orbit patch's True Anomaly at Universal Time [orbit patch index, universal time]", formatters.Default, APIEntry.UnitType.DATE));
            registerAPI(new PlotableAPIEntry(
                dataSources => {
                    int index = int.Parse(dataSources.args[0]);
                    float trueAnomaly = float.Parse(dataSources.args[1]);

                    Orbit orbitPatch = OrbitPatches.getOrbitPatch(dataSources.vessel.orbit, index);
                    if (orbitPatch == null) { return null; }
                    return orbitPatch.getRelativePositionFromTrueAnomaly(trueAnomaly);
                },
                "o.relativePositionAtTrueAnomalyForOrbitPatch", "The orbit patch's predicted displacement from the center of the main body at the given true anomaly [orbit patch index, true anomaly]", formatters.Vector3d, APIEntry.UnitType.UNITLESS));

            registerAPI(new PlotableAPIEntry(
                dataSources => {
                    int index = int.Parse(dataSources.args[0]);
                    double ut = double.Parse(dataSources.args[1]);

                    Orbit orbitPatch = OrbitPatches.getOrbitPatch(dataSources.vessel.orbit, index);
                    if (orbitPatch == null) { return null; }

                    return orbitPatch.getRelativePositionAtUT(ut);
                },
                "o.relativePositionAtUTForOrbitPatch", "The orbit patch's predicted displacement from the center of the main body at the given universal time [orbit patch index, universal time]", formatters.Vector3d, APIEntry.UnitType.UNITLESS));

        }

        #endregion
    }
}
using System;
using System.Collections.Generic;

namespace Telemachus.DataLinkHandlers
{
    public class BodyDataLinkHandler : DataLinkHandler
    {
        #region Initialisation

        public BodyDataLinkHandler(FormatterProvider formatters)
            : base(formatters)
        {
            registerAPI(new PlotableAPIEntry(
                dataSources => { return FlightGlobals.Bodies[int.Parse(dataSources.args[0])].name; },
                "b.name", "Body Name [body id]", formatters.Default, APIEntry.UnitType.STRING));
            registerAPI(new PlotableAPIEntry(
                dataSources => { return FlightGlobals.Bodies[int.Parse(dataSources.args[0])].atmosphereDepth; },
                "b.maxAtmosphere", "Body Atmosphere Depth [body id]", formatters.Default, APIEntry.UnitType.DISTANCE));
            registerAPI(new PlotableAPIEntry(
                dataSources => { return FlightGlobals.Bodies[int.Parse(dataSources.args[0])].Radius; },
                "b.radius", "Body Radius [body id]", formatters.Default, APIEntry.UnitType.DISTANCE));
            registerAPI(new PlotableAPIEntry(
                dataSources => { return FlightGlobals.Bodies[int.Parse(dataSources.args[0])].atmosphereContainsOxygen; },
                "b.atmosphereContainsOxygen", "Atmosphere contains oxygen [body id]", formatters.Default, APIEntry.UnitType.UNITLESS));
            registerAPI(new PlotableAPIEntry(
                dataSources => { return FlightGlobals.Bodies[int.Parse(dataSources.args[0])].sphereOfInfluence; },
                "b.soi", "Body Sphere of Influence [body id]", formatters.Default, APIEntry.UnitType.DISTANCE));
            registerAPI(new PlotableAPIEntry(
                dataSources => { return FlightGlobals.Bodies[int.Parse(dataSources.args[0])].rotationPeriod; },
                "b.rotationPeriod", "Rotation Period [body id]", formatters.Default, APIEntry.UnitType.UNITLESS));
            registerAPI(new PlotableAPIEntry(
                dataSources => { return FlightGlobals.Bodies[int.Parse(dataSources.args[0])].tidallyLocked; },
                "b.tidallyLocked", "Tidally Locked [body id]", formatters.Default, APIEntry.UnitType.UNITLESS));
            registerAPI(new PlotableAPIEntry(
                dataSources => { return FlightGlobals.Bodies.Count; },
                "b.number", "Number of Bodies", formatters.Default, APIEntry.UnitType.UNITLESS));
            registerAPI(new PlotableAPIEntry(
                dataSources => { return FlightGlobals.Bodies[int.Parse(dataSources.args[0])].gravParameter; },
                "b.o.gravParameter", "Body Gravitational Parameter [body id]", formatters.Default, APIEntry.UnitType.GRAV));
            registerAPI(new PlotableAPIEntry(
                dataSources => { return FlightGlobals.Bodies[int.Parse(dataSources.args[0])].orbit.GetRelativeVel().magnitude; },
                "b.o.relativeVelocity", "Relative Velocity [body id]", formatters.Default, APIEntry.UnitType.VELOCITY));
            registerAPI(new PlotableAPIEntry(
                dataSources => { return FlightGlobals.Bodies[int.Parse(dataSources.args[0])].orbit.PeA; },
                "b.o.PeA", "Periapsis [body id]", formatters.Default, APIEntry.UnitType.DISTANCE));
            registerAPI(new PlotableAPIEntry(
                dataSources => { return FlightGlobals.Bodies[int.Parse(dataSources.args[0])].orbit.ApA; },
                "b.o.ApA", "Apoapsis [body id]", formatters.Default, APIEntry.UnitType.DISTANCE));
            registerAPI(new PlotableAPIEntry(
                dataSources => { return FlightGlobals.Bodies[int.Parse(dataSources.args[0])].orbit.timeToAp; },
                "b.o.timeToAp", "Time to Apoapsis [body id]", formatters.Default, APIEntry.UnitType.TIME));
            registerAPI(new PlotableAPIEntry(
                dataSources => { return FlightGlobals.Bodies[int.Parse(dataSources.args[0])].orbit.timeToPe; },
                "b.o.timeToPe", "Time to Periapsis [body id]", formatters.Default, APIEntry.UnitType.TIME));
            registerAPI(new PlotableAPIEntry(
                dataSources => { return FlightGlobals.Bodies[int.Parse(dataSources.args[0])].orbit.inclination; },
                "b.o.inclination", "Inclination [body id]", formatters.Default, APIEntry.UnitType.DEG));
            registerAPI(new PlotableAPIEntry(
                dataSources => { return FlightGlobals.Bodies[int.Parse(dataSources.args[0])].orbit.eccentricity; },
                "b.o.eccentricity", "Eccentricity [body id]", formatters.Default, APIEntry.UnitType.UNITLESS));
            registerAPI(new PlotableAPIEntry(
                dataSources => { return FlightGlobals.Bodies[int.Parse(dataSources.args[0])].orbit.period; },
                "b.o.period", "Orbital Period [body id]", formatters.Default, APIEntry.UnitType.TIME));
            registerAPI(new PlotableAPIEntry(
                dataSources => { return FlightGlobals.Bodies[int.Parse(dataSources.args[0])].orbit.argumentOfPeriapsis; },
                "b.o.argumentOfPeriapsis", "Argument of Periapsis [body id]", formatters.Default, APIEntry.UnitType.DEG));
            registerAPI(new PlotableAPIEntry(
                dataSources => { return FlightGlobals.Bodies[int.Parse(dataSources.args[0])].orbit.timeToTransition1; },
                "b.o.timeToTransition1", "Time to Transition 1 [body id]", formatters.Default, APIEntry.UnitType.TIME));
            registerAPI(new PlotableAPIEntry(
                dataSources => { return FlightGlobals.Bodies[int.Parse(dataSources.args[0])].orbit.timeToTransition1; },
                "b.o.timeToTransition2", "Time to Transition 2 [body id]", formatters.Default, APIEntry.UnitType.TIME));
            registerAPI(new PlotableAPIEntry(
                dataSources => { return FlightGlobals.Bodies[int.Parse(dataSources.args[0])].orbit.semiMajorAxis; },
                "b.o.sma", "Semimajor Axis [body id]", formatters.Default, APIEntry.UnitType.DISTANCE));
            registerAPI(new PlotableAPIEntry(
                dataSources => { return FlightGlobals.Bodies[int.Parse(dataSources.args[0])].orbit.LAN; },
                "b.o.lan", "Longitude of Ascending Node [body id]", formatters.Default, APIEntry.UnitType.DEG));
            registerAPI(new PlotableAPIEntry(
                dataSources => { return FlightGlobals.Bodies[int.Parse(dataSources.args[0])].orbit.meanAnomalyAtEpoch; },
                "b.o.maae", "Mean Anomaly at Epoch [body id]", formatters.Default, APIEntry.UnitType.UNITLESS));
            registerAPI(new PlotableAPIEntry(
                dataSources => { return Planetarium.GetUniversalTime() - FlightGlobals.Bodies[int.Parse(dataSources.args[0])].orbit.ObT; },
                "b.o.timeOfPeriapsisPassage", "Time of Periapsis Passage [body id]", formatters.Default, APIEntry.UnitType.DATE));
            registerAPI(new PlotableAPIEntry(
                dataSources => { return FlightGlobals.Bodies[int.Parse(dataSources.args[0])].orbit.TrueAnomalyAtUT(Planetarium.GetUniversalTime()) * (180.0 / Math.PI); },
                "b.o.trueAnomaly", "True Anomaly [body id]", formatters.Default, APIEntry.UnitType.DEG));
            registerAPI(new PlotableAPIEntry(
                dataSources =>
                {
                    CelestialBody body = FlightGlobals.Bodies[int.Parse(dataSources.args[0])];

                    // Find a common reference body between vessel and body
                    List<CelestialBody> parentBodies = new List<CelestialBody>();
                    CelestialBody parentBody = dataSources.vessel.mainBody;
                    while (true)
                    {
                        if (parentBody == body)
                        {
                            return double.NaN;
                        }
                        parentBodies.Add(parentBody);
                        if (parentBody == Planetarium.fetch.Sun)
                        {
                            break;
                        }
                        else
                        {
                            parentBody = parentBody.referenceBody;
                        }
                    }

                    while (!parentBodies.Contains(body.referenceBody))
                    {
                        body = body.referenceBody;
                    }

                    Orbit orbit = dataSources.vessel.orbit;
                    while (orbit.referenceBody != body.referenceBody)
                    {
                        orbit = orbit.referenceBody.orbit;
                    }

                    // Calculate the phase angle
                    double ut = Planetarium.GetUniversalTime();
                    Vector3d vesselPos = orbit.getRelativePositionAtUT(ut);
                    Vector3d bodyPos = body.orbit.getRelativePositionAtUT(ut);
                    double phaseAngle = (Math.Atan2(bodyPos.y, bodyPos.x) - Math.Atan2(vesselPos.y, vesselPos.x)) * (180.0 / Math.PI);
                    return (phaseAngle < 0) ? phaseAngle + 360 : phaseAngle;
                },
                "b.o.phaseAngle", "Phase Angle [body id]", formatters.Default, APIEntry.UnitType.DEG));

            registerAPI(new APIEntry(
                dataSources => {
                    int bodyId = int.Parse(dataSources.args[0]);
                    float universalTime = float.Parse(dataSources.args[1]);
                    return FlightGlobals.Bodies[bodyId].getTruePositionAtUT(universalTime);
                },
                "b.o.truePositionAtUT", "True Position at the given UT [body id, universal time]", formatters.Vector3d, APIEntry.UnitType.UNITLESS));
        }

        #endregion
    }
}
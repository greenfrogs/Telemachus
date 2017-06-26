using System;

namespace Telemachus.DataLinkHandlers
{
    public class VesselDataLinkHandler : DataLinkHandler
    {
        #region Initialisation

        public VesselDataLinkHandler(FormatterProvider formatters)
            : base(formatters)
        {
            registerAPI(new PlotableAPIEntry(
                dataSources => { return dataSources.vessel.altitude; },
                "v.altitude", "Altitude", formatters.Default, APIEntry.UnitType.DISTANCE));
            registerAPI(new PlotableAPIEntry(
                dataSources => { return dataSources.vessel.heightFromTerrain; },
                "v.heightFromTerrain", "Height from Terrain", formatters.Default, APIEntry.UnitType.DISTANCE));
            registerAPI(new PlotableAPIEntry(
                dataSources => { return dataSources.vessel.altitude - dataSources.vessel.heightFromTerrain; },
                "v.terrainHeight", "Terrain Height", formatters.Default, APIEntry.UnitType.DISTANCE));
            registerAPI(new PlotableAPIEntry(
                dataSources => { return dataSources.vessel.missionTime; },
                "v.missionTime", "Mission Time", formatters.Default, APIEntry.UnitType.TIME));
            registerAPI(new PlotableAPIEntry(
                dataSources => { return dataSources.vessel.srf_velocity.magnitude; },
                "v.surfaceVelocity", "Surface Velocity", formatters.Default, APIEntry.UnitType.VELOCITY));
            registerAPI(new PlotableAPIEntry(
                dataSources => { return dataSources.vessel.srf_velocity.x; },
                "v.surfaceVelocityx", "Surface Velocity x", formatters.Default, APIEntry.UnitType.VELOCITY));
            registerAPI(new PlotableAPIEntry(
                dataSources => { return dataSources.vessel.srf_velocity.y; },
                "v.surfaceVelocityy", "Surface Velocity y", formatters.Default, APIEntry.UnitType.VELOCITY));
            registerAPI(new PlotableAPIEntry(
                dataSources => { return dataSources.vessel.srf_velocity.z; },
                "v.surfaceVelocityz", "Surface Velocity z", formatters.Default, APIEntry.UnitType.VELOCITY));
            registerAPI(new PlotableAPIEntry(
                dataSources => { return dataSources.vessel.angularVelocity.magnitude; },
                "v.angularVelocity", "Angular Velocity", formatters.Default, APIEntry.UnitType.VELOCITY));
            registerAPI(new PlotableAPIEntry(
                dataSources => { return dataSources.vessel.obt_velocity.magnitude; },
                "v.orbitalVelocity", "Orbital Velocity", formatters.Default, APIEntry.UnitType.VELOCITY));
            registerAPI(new PlotableAPIEntry(
                dataSources => { return dataSources.vessel.horizontalSrfSpeed; },
                "v.surfaceSpeed", "Surface Speed", formatters.Default, APIEntry.UnitType.VELOCITY));
            registerAPI(new PlotableAPIEntry(
                dataSources => { return dataSources.vessel.verticalSpeed; },
                "v.verticalSpeed", "Vertical Speed", formatters.Default, APIEntry.UnitType.VELOCITY));
            registerAPI(new PlotableAPIEntry(
                dataSources => { return dataSources.vessel.geeForce; },
                "v.geeForce", "G-Force", formatters.Default, APIEntry.UnitType.G));
            registerAPI(new PlotableAPIEntry(
                dataSources => {
                    double atmosphericPressure = FlightGlobals.getStaticPressure(dataSources.vessel.altitude, dataSources.vessel.mainBody);
                    double temperature = FlightGlobals.getExternalTemperature(dataSources.vessel.altitude, dataSources.vessel.mainBody);
                    double atmosphericDensityinKilograms = FlightGlobals.getAtmDensity(atmosphericPressure, temperature);
                    return atmosphericDensityinKilograms * 1000;
                    //return dataSources.vessel;
                },
                "v.atmosphericDensity", "Atmospheric Density", formatters.Default, APIEntry.UnitType.UNITLESS));
            registerAPI(new PlotableAPIEntry(
                dataSources => { return dataSources.vessel.longitude > 180 ? dataSources.vessel.longitude - 360.0 : dataSources.vessel.longitude; },
                "v.long", "Longitude", formatters.Default, APIEntry.UnitType.DEG));
            registerAPI(new PlotableAPIEntry(
                dataSources => { return dataSources.vessel.latitude; },
                "v.lat", "Latitude", formatters.Default, APIEntry.UnitType.DEG));
            registerAPI(new PlotableAPIEntry(
                dataSources => {return (dataSources.vessel.atmDensity * 0.5) * Math.Pow(dataSources.vessel.srf_velocity.magnitude, 2); },
                "v.dynamicPressure", "Dynamic Pressure", formatters.Default, APIEntry.UnitType.UNITLESS));
            registerAPI(new PlotableAPIEntry(
                dataSources => { return FlightGlobals.getStaticPressure(dataSources.vessel.altitude, dataSources.vessel.mainBody) * 1000; },
                "v.atmosphericPressurePa", "Atmospheric Pressure (Pa)", formatters.Default, APIEntry.UnitType.UNITLESS));
            registerAPI(new PlotableAPIEntry(
                dataSources => { return FlightGlobals.getStaticPressure(dataSources.vessel.altitude, dataSources.vessel.mainBody) * PhysicsGlobals.KpaToAtmospheres; },
                "v.atmosphericPressure", "Atmospheric Pressure", formatters.Default, APIEntry.UnitType.UNITLESS));
            registerAPI(new PlotableAPIEntry(
                dataSources => { return dataSources.vessel.name; },
                "v.name", "Name", formatters.Default, APIEntry.UnitType.STRING));
            registerAPI(new PlotableAPIEntry(
                dataSources => { return dataSources.vessel.orbit.referenceBody.name; },
                "v.body", "Body Name", formatters.Default, APIEntry.UnitType.STRING));
            registerAPI(new PlotableAPIEntry(
                dataSources =>
                {
                    if (dataSources.vessel.mainBody == Planetarium.fetch.Sun)
                    {
                        return double.NaN;
                    }
                    else
                    {
                        double ut = Planetarium.GetUniversalTime();
                        CelestialBody body = dataSources.vessel.mainBody;
                        Vector3d bodyPrograde = body.orbit.getOrbitalVelocityAtUT(ut);
                        Vector3d bodyNormal = body.orbit.GetOrbitNormal();
                        Vector3d vesselPos = dataSources.vessel.orbit.getRelativePositionAtUT(ut);
                        Vector3d vesselPosInPlane = Vector3d.Exclude(bodyNormal, vesselPos); // Project the vessel position into the body's orbital plane
                        double angle = Vector3d.Angle(vesselPosInPlane, bodyPrograde);
                        if (Vector3d.Dot(Vector3d.Cross(vesselPosInPlane, bodyPrograde), bodyNormal) < 0)
                        { // Correct for angles > 180 degrees
                            angle = 360 - angle;
                        }
                        if (dataSources.vessel.orbit.GetOrbitNormal().z < 0)
                        { // Check for retrograde orbit
                            angle = 360 - angle;
                        }
                        return angle;
                    }
                },
                "v.angleToPrograde", "Angle to Prograde", formatters.Default, APIEntry.UnitType.DEG));
        }

        #endregion
    }
}
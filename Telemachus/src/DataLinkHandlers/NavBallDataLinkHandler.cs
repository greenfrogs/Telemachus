using UnityEngine;

namespace Telemachus.DataLinkHandlers
{
    public class NavBallDataLinkHandler : DataLinkHandler
    {
        #region Initialisation

        public NavBallDataLinkHandler(FormatterProvider formatters)
            : base(formatters)
        {
            registerAPI(new PlotableAPIEntry(
                dataSources =>
                {
                    Quaternion result = updateHeadingPitchRollField(dataSources.vessel, dataSources.vessel.CoM);
                    return result.eulerAngles.y;
                },
                "n.heading2", "Heading", formatters.Default, APIEntry.UnitType.DEG));

            registerAPI(new PlotableAPIEntry(
                dataSources =>
                {
                    Quaternion result = updateHeadingPitchRollField(dataSources.vessel, dataSources.vessel.CoM);
                    return (result.eulerAngles.x > 180) ? (360.0 - result.eulerAngles.x) : -result.eulerAngles.x;
                },
                "n.pitch2", "Pitch", formatters.Default, APIEntry.UnitType.DEG));

            registerAPI(new PlotableAPIEntry(
                dataSources =>
                {
                    Quaternion result = updateHeadingPitchRollField(dataSources.vessel, dataSources.vessel.CoM);
                    return (result.eulerAngles.z > 180) ?
                        (result.eulerAngles.z - 360.0) : result.eulerAngles.z;
                },
                "n.roll2", "Roll", formatters.Default, APIEntry.UnitType.DEG));

            registerAPI(new PlotableAPIEntry(
                dataSources =>
                {
                    Quaternion result = updateHeadingPitchRollField(dataSources.vessel, dataSources.vessel.CoM);
                    return result.eulerAngles.y;
                },
                "n.rawheading2", "Raw Heading", formatters.Default, APIEntry.UnitType.DEG));

            registerAPI(new PlotableAPIEntry(
                dataSources =>
                {
                    Quaternion result = updateHeadingPitchRollField(dataSources.vessel, dataSources.vessel.CoM);
                    return result.eulerAngles.x;
                },
                "n.rawpitch2", "Raw Pitch", formatters.Default, APIEntry.UnitType.DEG));

            registerAPI(new PlotableAPIEntry(
                dataSources =>
                {
                    Quaternion result = updateHeadingPitchRollField(dataSources.vessel, dataSources.vessel.CoM);
                    return result.eulerAngles.z;
                },
                "n.rawroll2", "Raw Roll", formatters.Default, APIEntry.UnitType.DEG));

            registerAPI(new PlotableAPIEntry(
                dataSources =>
                {
                    Quaternion result = updateHeadingPitchRollField(dataSources.vessel, dataSources.vessel.rootPart.transform.position);
                    return result.eulerAngles.y;
                },
                "n.heading", "Heading calculated using the position of the vessels root part", formatters.Default, APIEntry.UnitType.DEG));

            registerAPI(new PlotableAPIEntry(
                dataSources =>
                {
                    Quaternion result = updateHeadingPitchRollField(dataSources.vessel, dataSources.vessel.rootPart.transform.position);
                    return (result.eulerAngles.x > 180) ? (360.0 - result.eulerAngles.x) : -result.eulerAngles.x;
                },
                "n.pitch", "Pitch calculated using the position of the vessels root part", formatters.Default, APIEntry.UnitType.DEG));

            registerAPI(new PlotableAPIEntry(
                dataSources =>
                {
                    Quaternion result = updateHeadingPitchRollField(dataSources.vessel, dataSources.vessel.rootPart.transform.position);
                    return (result.eulerAngles.z > 180) ?
                        (result.eulerAngles.z - 360.0) : result.eulerAngles.z;
                },
                "n.roll", "Roll calculated using the position of the vessels root part", formatters.Default, APIEntry.UnitType.DEG));

            registerAPI(new PlotableAPIEntry(
                dataSources =>
                {
                    Quaternion result = updateHeadingPitchRollField(dataSources.vessel, dataSources.vessel.rootPart.transform.position);
                    return result.eulerAngles.y;
                },
                "n.rawheading", "Raw Heading calculated using the position of the vessels root part", formatters.Default, APIEntry.UnitType.DEG));

            registerAPI(new PlotableAPIEntry(
                dataSources =>
                {
                    Quaternion result = updateHeadingPitchRollField(dataSources.vessel, dataSources.vessel.rootPart.transform.position);
                    return result.eulerAngles.x;
                },
                "n.rawpitch", "Raw Pitch calculated using the position of the vessels root part", formatters.Default, APIEntry.UnitType.DEG));

            registerAPI(new PlotableAPIEntry(
                dataSources =>
                {
                    Quaternion result = updateHeadingPitchRollField(dataSources.vessel, dataSources.vessel.rootPart.transform.position);
                    return result.eulerAngles.z;
                },
                "n.rawroll", "Raw Roll calculated using the position of the vessels root part", formatters.Default, APIEntry.UnitType.DEG));
        }

        #endregion

        #region Methods

        //Borrowed from MechJeb2
        private Quaternion updateHeadingPitchRollField(Vessel v, Vector3d CoM)
        {
            Vector3d north, up;
            Quaternion rotationSurface;
         
            up = (CoM - v.mainBody.position).normalized;

            north = Vector3d.Exclude(up, (v.mainBody.position + v.mainBody.transform.up *
                                          (float)v.mainBody.Radius) - CoM).normalized;

            rotationSurface = Quaternion.LookRotation(north, up);
            return Quaternion.Inverse(Quaternion.Euler(90, 0, 0) *
                                      Quaternion.Inverse(v.GetTransform().rotation) * rotationSurface);
        }

        #endregion
    }
}
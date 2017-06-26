using UnityEngine;

namespace Telemachus.DataLinkHandlers
{
    public class DockingDataLinkHandler : DataLinkHandler
    {
        #region Initialisation

        private static Vector3 orientationDeviation = new Vector3();
        private static Vector2 translationDeviation = new Vector3();

        public DockingDataLinkHandler(FormatterProvider formatters)
            : base(formatters)
        {
            registerAPI(new PlotableAPIEntry(
                dataSources =>
                {
                    if (FlightGlobals.fetch.VesselTarget != null)
                    {
                        update();
                        return orientationDeviation.x;
                    }
                    else
                    {
                        return 0;
                    }
                },
                "dock.ax", "Docking x Angle", formatters.Default, APIEntry.UnitType.DEG));

            registerAPI(new PlotableAPIEntry(
                dataSources =>
                {
                    if (FlightGlobals.fetch.VesselTarget != null)
                    {
                        update();
                        return orientationDeviation.y;
                    }
                    else
                    {
                        return 0;
                    }
                },
                "dock.ay", "Relative Pitch Angle", formatters.Default, APIEntry.UnitType.DEG));

            registerAPI(new PlotableAPIEntry(
                dataSources =>
                {
                    if (FlightGlobals.fetch.VesselTarget != null)
                    {
                        update();
                        return orientationDeviation.z;
                    }
                    else
                    {
                        return 0;
                    }
                },
                "dock.az", "Docking z Angle", formatters.Default, APIEntry.UnitType.DEG));

            registerAPI(new PlotableAPIEntry(
                dataSources => { return FlightGlobals.fetch.VesselTarget != null ? (FlightGlobals.fetch.VesselTarget.GetTransform().position - dataSources.vessel.GetTransform().position).x : 0; },
                "dock.x", "Target x Distance", formatters.Default, APIEntry.UnitType.DISTANCE));

            registerAPI(new PlotableAPIEntry(
                dataSources => { return FlightGlobals.fetch.VesselTarget != null ? (FlightGlobals.fetch.VesselTarget.GetTransform().position - dataSources.vessel.GetTransform().position).y : 0; },
                "dock.y", "Target y Distance", formatters.Default, APIEntry.UnitType.DISTANCE));
        }

        #endregion

        #region Methods

        //Borrowed from Docking Port Alignment Indicator by NavyFish
        private void update()
        {
            ModuleDockingNode targetPort = null;
            Transform selfTransform = FlightGlobals.ActiveVessel.ReferenceTransform;
            
            targetPort = FlightGlobals.fetch.VesselTarget as ModuleDockingNode;
            
            if (targetPort == null)
            {
                return;
            }
            
            Transform targetTransform = targetPort.transform;
            Vector3 targetPortOutVector;
            Vector3 targetPortRollReferenceVector;

            if (targetPort.part.name == "dockingPortLateral")
            {
                targetPortOutVector = -targetTransform.forward.normalized;
                targetPortRollReferenceVector = -targetTransform.up;
            }
            else
            {
                targetPortOutVector = targetTransform.up.normalized;
                targetPortRollReferenceVector = targetTransform.forward;
            }

            orientationDeviation.x = AngleAroundNormal(-targetPortOutVector, selfTransform.up, selfTransform.forward);
            orientationDeviation.y = AngleAroundNormal(-targetPortOutVector, selfTransform.up, -selfTransform.right);
            orientationDeviation.z = AngleAroundNormal(targetPortRollReferenceVector, selfTransform.forward, selfTransform.up);
            orientationDeviation.z = (orientationDeviation.z + 360) % 360;

            Vector3 targetToOwnship = selfTransform.position - targetTransform.position;

            translationDeviation.x = AngleAroundNormal(targetToOwnship, targetPortOutVector, selfTransform.forward);
            translationDeviation.y = AngleAroundNormal(targetToOwnship, targetPortOutVector, -selfTransform.right);
        }

        //return signed angle in relation to normal's 2d plane
        private float AngleAroundNormal(Vector3 a, Vector3 b, Vector3 up)
        {
            return AngleSigned(Vector3.Cross(up, a), Vector3.Cross(up, b), up);
        }

        //-180 to 180 angle
        private float AngleSigned(Vector3 v1, Vector3 v2, Vector3 up)
        {
            if (Vector3.Dot(Vector3.Cross(v1, v2), up) < 0) //greater than 90 i.e v1 left of v2
                return -Vector3.Angle(v1, v2);
            return Vector3.Angle(v1, v2);
        }

        #endregion
    }
}
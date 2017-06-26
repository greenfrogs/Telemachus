using System;
using System.Collections.Generic;

namespace Telemachus.DataLinkHandlers
{
    public class ResourceDataLinkHandler : DataLinkHandler
    {

        #region Fields

        DataLinkHandlers.ResourceCache resourceCache = null;
        ActiveResourceCache activeResourceCache = null;

        #endregion

        #region Initialisation

        public ResourceDataLinkHandler(VesselChangeDetector vesselChangeDetector, FormatterProvider formatters)
            : base(formatters)
        {

            resourceCache = new DataLinkHandlers.ResourceCache(vesselChangeDetector);
            activeResourceCache = new ActiveResourceCache(vesselChangeDetector);

            registerAPI(new APIEntry(
                dataSources => { return getsResourceValues(dataSources); },
                "r.resource", "Resource Information [string resource type]",
                formatters.ResourceList, APIEntry.UnitType.UNITLESS));

            registerAPI(new APIEntry(
                dataSources =>
                {
                    return getsActiveResourceValues(dataSources);
                },
                "r.resourceCurrent", "Resource Information for Current Stage [string resource type]",
                formatters.ActiveResourceList, APIEntry.UnitType.UNITLESS));

            registerAPI(new APIEntry(
                dataSources =>
                {
                    return getsActiveResourceValues(dataSources);
                },
                "r.resourceCurrentMax", "Max Resource Information for Current Stage [string resource type]",
                formatters.MaxCurrentResourceList, APIEntry.UnitType.UNITLESS));

            registerAPI(new APIEntry(
                dataSources => { return getsResourceValues(dataSources); },
                "r.resourceMax", "Max Resource Information [string resource type]",
                formatters.MaxResourceList, APIEntry.UnitType.UNITLESS));
            registerAPI(new APIEntry(
                dataSources => {
                    List<String> names = new List<String>();
                    PartResourceDefinitionList resourceDefinitionList = PartResourceLibrary.Instance.resourceDefinitions;
                    foreach (PartResourceDefinition resourceDefinition in resourceDefinitionList)
                    {
                        names.Add(resourceDefinition.name);
                    }

                    return names;
                },
                "r.resourceNameList", "List of resource names",
                formatters.StringArray, APIEntry.UnitType.UNITLESS));
        }

        #endregion

        #region Resources

        protected List<PartResource> getsResourceValues(DataSources datasources)
        {
            resourceCache.vessel = datasources.vessel;
            return resourceCache.get(datasources);
        }

        protected List<SimplifiedResource> getsActiveResourceValues(DataSources datasources)
        {
            activeResourceCache.vessel = datasources.vessel;
            return activeResourceCache.get(datasources);
        }

        #endregion
    }
}
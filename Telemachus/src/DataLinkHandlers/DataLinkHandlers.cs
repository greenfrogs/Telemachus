//Author: Richard Bunt

using System;
using System.Collections.Generic;
using System.Linq;

namespace Telemachus.DataLinkHandlers
{

    //TODO: Refactor remainig classes
    public abstract class ModuleCache<T>
    {
        #region Constants

        protected const int ACCESS_REFRESH = 10;

        #endregion

        #region Fields

        protected event EventHandler<EventArgs> VesselPropertyChanged;

        private Vessel theVessel = null;
        public Vessel vessel
        {
            get { return theVessel; }
            set {
                if (theVessel == value) return;
                theVessel = value;
                if (VesselPropertyChanged != null) VesselPropertyChanged(this, EventArgs.Empty);
            }
        }

        protected Dictionary<string, List<T>> partModules = new Dictionary<string, List<T>>();

        #endregion

        #region Lock

        readonly protected System.Object cacheLock = new System.Object();

        #endregion

        #region Cache

        public List<T> get(DataSources dataSources)
        {
            string ID = dataSources.args[0].ToLowerInvariant();
            List<T> avail = null, ret = null;

            lock (cacheLock)
            {
                partModules.TryGetValue(ID, out avail);
            }

            if (avail != null)
            {
                ret = new List<T>(avail);
            }
            else
            {
                ret = new List<T>();
            }

            return ret;
        }

        protected abstract void refresh(Vessel vessel);

        #endregion
    }

    public class SimplifiedResource
    {
        public double amount { get; set; }
        public double maxAmount { get; set; }

        public SimplifiedResource(double amount, double maxAmount)
        {
            this.amount = amount;
            this.maxAmount = maxAmount;
        }
    }

    public class ActiveResourceCache : ModuleCache<SimplifiedResource>
    {
        #region ModuleCache

        public ActiveResourceCache(VesselChangeDetector vesselChangeDetector)
        {
            vesselChangeDetector.UpdateNotify += update;
        }

        private void update(object sender, EventArgs eventArgs)
        {
            if (vessel != null)
            {
                lock (cacheLock)
                {
                    refresh(vessel);
                }
            }
        }

        protected override void refresh(Vessel vessel)
        {
            try
            {
                partModules.Clear();
                HashSet<Part> activeParts = new HashSet<Part>();
                foreach(Part part in vessel.GetActiveParts())
                {
                    if (part.inverseStage == vessel.currentStage)
                    {
                        activeParts.Add(part);
                        activeParts.UnionWith(part.crossfeedPartSet.GetParts());
                    }
                }

                PartSet activePartSet = new PartSet(activeParts);
                PartResourceDefinitionList resourceDefinitionList = PartResourceLibrary.Instance.resourceDefinitions;

                foreach(PartResourceDefinition resourceDefinition in resourceDefinitionList)
                {
                    String key = resourceDefinition.name.ToString().ToLowerInvariant();
                    double amount = 0;
                    double maxAmount = 0;
                    bool pulling = true;

                    activePartSet.GetConnectedResourceTotals(resourceDefinition.id, out amount, out maxAmount, pulling);

                    if(!partModules.ContainsKey(key)){
                        partModules[key] = new List<SimplifiedResource>();
                    }

                    partModules[key].Add(new SimplifiedResource(amount, maxAmount));
                    PluginLogger.debug("SIZE OF " + key + " " + partModules[key].Count + " " + amount );
                }
            }
            catch (Exception e)
            {
                PluginLogger.debug(e.Message);
            }
        }

        #endregion
    }

    public class ResourceCache : ModuleCache<PartResource>
    {
        #region ModuleCache

        public ResourceCache(VesselChangeDetector vesselChangeDetector)
        {
            vesselChangeDetector.UpdateNotify += update;
        }

        private void update(object sender, EventArgs eventArgs)
        {
            if (vessel != null)
            {
                lock (cacheLock)
                {
                    refresh(vessel);
                }
            }
        }
        
        protected override void refresh(Vessel vessel)
        {
            try
            {
                partModules.Clear();

                foreach (Part part in vessel.parts)
                {
                    if (part.Resources.Count > 0)
                    {

                        foreach (PartResource partResource in part.Resources)
                        {
                            String key = partResource.resourceName.ToLowerInvariant();
                            List<PartResource> list = null;
                            partModules.TryGetValue(key, out list);
                            if (list == null)
                            {
                                list = new List<PartResource>();
                                partModules[key] = list;

                            }

                            list.Add(partResource);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                PluginLogger.debug(e.Message);
            }
        }

        #endregion
    }

    public class SensorCache : ModuleCache<ModuleEnviroSensor>
    {
        #region ModuleCache

        public SensorCache(VesselChangeDetector vesselChangeDetector)
        {
            vesselChangeDetector.UpdateNotify += update;
            VesselPropertyChanged += update;
        }

        private void update(object sender, EventArgs eventArgs)
        {
            if (vessel != null)
            {
                lock (cacheLock)
                {
                    refresh(vessel);
                }
            }
        }

        protected override void refresh(Vessel vessel)
        {
            try
            {
                partModules.Clear();

                List<Part> partsWithSensors = vessel.parts.FindAll(p => p.Modules.Contains("ModuleEnviroSensor"));
                foreach (Part part in partsWithSensors)
                {
                    foreach (var module in part.Modules.OfType<ModuleEnviroSensor>())
                    {
                        if (!partModules.ContainsKey(module.sensorType.ToString()))
                        {
                            partModules[module.sensorType.ToString()] = new List<ModuleEnviroSensor>();
                        }
                        partModules[module.sensorType.ToString()].Add(module);
                    }
                }
            }
            catch (Exception e)
            {
                PluginLogger.debug(e.Message + " " + e.StackTrace);
            }
        }

        #endregion
    }

    public class PausedDataLinkHandler : DataLinkHandler
    {
        #region Initialisation

        public PausedDataLinkHandler(FormatterProvider formatters)
            : base(formatters)
        {
            registerAPI(new PlotableAPIEntry(
                dataSources => { return partPaused(); },
                "p.paused", 
                "Returns an integer indicating the state of antenna. 0 - Flight scene; 1 - Paused; 2 - No power; 3 - Off; 4 - Not found.", 
                formatters.Default, APIEntry.UnitType.UNITLESS, true));
        }

        #endregion

        #region Methods

        public static int partPaused()
        {
            bool result = FlightDriver.Pause ||
                !TelemachusPowerDrain.isActive ||
                !TelemachusPowerDrain.activeToggle ||
                !VesselChangeDetector.hasTelemachusPart ||
                !HighLogic.LoadedSceneIsFlight;
            
            if (result)
            {
                // If we aren't even in the flight scene, say so
                if (!HighLogic.LoadedSceneIsFlight)
                {
                    return 5;
                }

                if (FlightDriver.Pause)
                {
                    return 1;
                }

                if (!TelemachusPowerDrain.isActive)
                {
                    return 2;
                }

                if (!TelemachusPowerDrain.activeToggle)
                {
                    return 3;
                }

                if (!VesselChangeDetector.hasTelemachusPart)
                {
                    return 4;
                }
            }
            else
            {
                return 0;
            }

            return 5;
        }

        #endregion
    }

    public class APIDataLinkHandler : DataLinkHandler
    {
        #region Initialisation

        public APIDataLinkHandler(IKSPAPI kspAPI, FormatterProvider formatters,
            ServerConfiguration serverConfiguration)
            : base(formatters)
        {
            registerAPI(new APIEntry(
                dataSources =>
                {
                    List<APIEntry> APIList = new List<APIEntry>();
                    kspAPI.getAPIList(ref APIList); return APIList;
                },
                "a.api", "API Listing", formatters.APIEntry, APIEntry.UnitType.UNITLESS, true));

            registerAPI(new APIEntry(
                dataSources =>
                {
                    List<String> IPList = new List<String>();

                    foreach (System.Net.IPAddress a in serverConfiguration.ValidIpAddresses)
                    {
                        IPList.Add(a.ToString());
                    }

                    return IPList;
                },
                "a.ip", "IP Addresses", formatters.StringArray, APIEntry.UnitType.UNITLESS, true));

            registerAPI(new APIEntry(
                dataSources =>
                {
                    List<APIEntry> APIList = new List<APIEntry>();
                    foreach (string apiRequest in dataSources.args)
                    {
                        kspAPI.getAPIEntry(apiRequest, ref APIList);
                    }

                    return APIList;
                },
                "a.apiSubSet",
                "Subset of the API Listing [string api1, string api2, ... , string apiN]",
                formatters.APIEntry, APIEntry.UnitType.STRING, true));

            registerAPI(new PlotableAPIEntry(
                dataSources => { return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(); },
                "a.version", "Telemachus Version", formatters.Default, APIEntry.UnitType.UNITLESS, true));
        }

        #endregion
    }

    public class CompoundDataLinkHandler : DataLinkHandler
    {
        #region Initialisation

        public CompoundDataLinkHandler(List<DataLinkHandler> APIHandlers, FormatterProvider formatters)
            : base(formatters)
        {
            foreach (DataLinkHandler dlh in APIHandlers)
            {
                foreach (KeyValuePair<string, APIEntry> entry in dlh.API)
                {
                    registerAPI(entry.Value);
                }
            }
        }

        #endregion
    }

    public abstract class DataLinkHandler
    {
        #region API Delegates

        public delegate object APIDelegate(DataSources datasources);

        #endregion

        #region API Fields

        private Dictionary<string, APIEntry> APIEntries =
           new Dictionary<string, APIEntry>();
        APIEntry nullAPI = null;
        protected FormatterProvider formatters = null;

        #endregion

        #region Initialisation

        public DataLinkHandler(FormatterProvider formatters)
        {
            this.formatters = formatters;
            nullAPI = new APIEntry(
                dataSources =>
                {
                    return pausedHandler();
                },
                "", "", formatters.Default, APIEntry.UnitType.UNITLESS);
        }

        #endregion

        #region DataLinkHandler

        public IEnumerable<KeyValuePair<string, APIEntry>> API
        {
            get
            {
                return APIEntries;
            }
        }

        public virtual bool process(String API, out APIEntry result)
        {
            APIEntry entry = null;

            APIEntries.TryGetValue(API, out entry);

            if (entry == null)
            {
                result = null;
                return false;
            }
            else
            {
                if (pausedHandler() == 0)
                {
                    result = entry;
                }
                else
                {
                    result = nullAPI;
                }

                return true;
            }
        }

        public void appendAPIList(ref List<APIEntry> APIList)
        {
            foreach (KeyValuePair<String, APIEntry> entry in APIEntries)
            {
                APIList.Add(entry.Value);
            }
        }

        protected void registerAPI(APIEntry entry)
        {
            APIEntries.Add(entry.APIString, entry);
        }

        protected virtual int pausedHandler()
        {
            return 0;
        }

        #endregion
    }

    public class APIEntry
    {
        #region Enumeration

        public enum UnitType { UNITLESS, VELOCITY, DEG, DISTANCE, TIME, STRING, TEMP, PRES, GRAV, ACC, DENSITY, DYNAMICPRESSURE, G, DATE };

        #endregion

        #region Fields

        public DataLinkHandler.APIDelegate function { get; set; }
        public string APIString { get; set; }
        public string name { get; set; }
        public UnitType units { get; set; }
        public bool plotable { get; set; }
        public DataSourceResultFormatter formatter { get; set; }
        public bool alwaysEvaluable { get; set; }
        #endregion

        #region Initialisation

        public APIEntry(DataLinkHandler.APIDelegate function, string APIString,
            string name, DataSourceResultFormatter formatter, UnitType units, bool alwaysEvaluable = false)
        {
            this.function = function;
            this.APIString = APIString;
            this.name = name;
            this.formatter = formatter;
            this.units = units;
            this.alwaysEvaluable = alwaysEvaluable;
        }

        #endregion
    }

    public class OrbitPatches
    {
        public static List<Orbit> getPatchesForOrbit(Orbit orbit)
        {
            List<Orbit> orbitPatches = new List<Orbit>();
            //the "next" orbit patch is the root patch, to make the method cleaner
            var nextOrbitPatch = orbit;

            while(nextOrbitPatch != null && nextOrbitPatch.activePatch)
            {
                orbitPatches.Add(nextOrbitPatch);
                if (nextOrbitPatch.patchEndTransition == Orbit.PatchTransitionType.MANEUVER)
                {
                    break;
                }
                else
                {
                    nextOrbitPatch = nextOrbitPatch.nextPatch;
                }
            }

            return orbitPatches;
        }

        public static Orbit getOrbitPatch(Orbit orbit, int index)
        {
            List<Orbit> orbitPatches = getPatchesForOrbit(orbit);
            if(index >= orbitPatches.Count) { return null; }
            return orbitPatches[index];
        }
    }

    public class ActionAPIEntry : APIEntry
    {
        #region Initialisation

        public ActionAPIEntry(DataLinkHandler.APIDelegate function,
            string APIString, string name, DataSourceResultFormatter formatter)
            : base(function, APIString, name, formatter, APIEntry.UnitType.UNITLESS)
        {
            plotable = false;
        }

        #endregion
    }

    public class PlotableAPIEntry : APIEntry
    {
        #region Initialisation

        public PlotableAPIEntry(DataLinkHandler.APIDelegate function, string APIString, string name,
           DataSourceResultFormatter formatter, UnitType units, bool alwaysEvaluable = false)
            : base(function, APIString, name, formatter, units, alwaysEvaluable)
        {
            this.plotable = true;
        }

        #endregion
    }

    public class DelayedAPIEntry : APIEntry
    {
        #region Fields

        private DataSources dataSources = null;

        #endregion

        #region Initialisation

        public DelayedAPIEntry(DataSources dataSources, DataLinkHandler.APIDelegate function)
            : base(function, "", "", null, UnitType.UNITLESS)
        {
            this.dataSources = dataSources;
        }

        #endregion

        #region Methods

        public void call()
        {
            try
            {
                function(dataSources);
            }
            catch (Exception e)
            {
                PluginLogger.debug(e.Message);
            }
        }

        #endregion
    }
}

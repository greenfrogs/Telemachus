using System.Collections.Generic;

namespace Telemachus.DataLinkHandlers
{
    public class SensorDataLinkHandler : DataLinkHandler
    {
        #region Fields

        SensorCache sensorCache = null;

        #endregion

        #region Initialisation

        public SensorDataLinkHandler(VesselChangeDetector vesselChangeDetector, FormatterProvider formatters)
            : base(formatters)
        {
            sensorCache = new SensorCache(vesselChangeDetector);

            registerAPI(new PlotableAPIEntry(
                dataSources => { return getsSensorValues(dataSources); },
                "s.sensor", "Sensor Information [string sensor type]", formatters.SensorModuleList,
                APIEntry.UnitType.UNITLESS));
            registerAPI(new PlotableAPIEntry(
                dataSources => { dataSources.args.Add("TEMP"); return getsSensorValues(dataSources); },
                "s.sensor.temp", "Temperature sensor information", formatters.SensorModuleList,
                APIEntry.UnitType.TEMP));
            registerAPI(new PlotableAPIEntry(
                dataSources => { dataSources.args.Add("PRES"); return getsSensorValues(dataSources); },
                "s.sensor.pres", "Pressure sensor information", formatters.SensorModuleList,
                APIEntry.UnitType.PRES));
            registerAPI(new PlotableAPIEntry(
                dataSources => { dataSources.args.Add("GRAV"); return getsSensorValues(dataSources); },
                "s.sensor.grav", "Gravity sensor information", formatters.SensorModuleList,
                APIEntry.UnitType.GRAV));
            registerAPI(new PlotableAPIEntry(
                dataSources => { dataSources.args.Add("ACC"); return getsSensorValues(dataSources); },
                "s.sensor.acc", "Acceleration sensor information", formatters.SensorModuleList,
                APIEntry.UnitType.ACC));
        }

        #endregion

        #region Sensors

        protected List<ModuleEnviroSensor> getsSensorValues(DataSources datasources)
        {
            sensorCache.vessel = datasources.vessel;
            return sensorCache.get(datasources);
        }

        #endregion
    }
}
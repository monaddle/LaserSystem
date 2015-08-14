using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace LaserSystemLibrary
{
    public class ComSettings
    {
        public string lComName;
        public int lBaudRate;
        public string rComName;
        public int rBaudRate;
        public string gpsComName;
        public int gpsBaudRate;
        public string TopLeftACSComName;
        public string BottomLeftACSComName;
        public string TopRightACSComName;
        public string BottomRightACSComName;
        
    }

    public class ScanningOptions
    {
        public double laserHeight;
        public double rowDistance;
        public double samplingDistance;
        public int samplingDistanceSelectedIndex;
        public bool useLeftLaser;
        public bool useRightLaser;
        public double minHeight;

        public ComSettings comSettings;

        public string OutputFilePath;
        public string OutputFileName;

        private double defaultLaserHeight = 8;
        private double defaultRowDistance = 16;
        private double defaultSamplingDistance = 1;
        private int defaultSamplingDistanceSelectedIndex = 1;
        private double defaultMinHeight = 3;
            
        private string settingsFile =  Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + "settings1.xml";
        public string outputTableName;
        public bool fakeReadings;
        public bool saveData;
        public double laserOffset;
        public bool UsePolygonLayer;
        public void LoadSettings()
        {
            // try to open file
            if (File.Exists(settingsFile))
            {
                LoadSettingsFromFile();
            }
            // if no file, set settings:
            else
            {
                laserHeight = defaultLaserHeight;
                rowDistance = defaultRowDistance;
                samplingDistance = defaultSamplingDistance;
                samplingDistanceSelectedIndex = defaultSamplingDistanceSelectedIndex;
                minHeight = defaultMinHeight;
                useLeftLaser = true;
                useRightLaser = true;
                fakeReadings = false;
                UsePolygonLayer = true;
                comSettings = new ComSettings();
            }
        }
        public void SaveSettings()
        {
            XmlSerializer ser = new XmlSerializer(this.GetType());
            StreamWriter writer = new StreamWriter(settingsFile);
            ser.Serialize(writer, this);
            writer.Close();
        }

        private void LoadSettingsFromFile()
        {
            ScanningOptions tempSettings = new ScanningOptions();
            XmlSerializer ser = new XmlSerializer(this.GetType());
            FileStream stream = new FileStream(settingsFile, FileMode.Open);
            tempSettings = (ScanningOptions)ser.Deserialize(stream);
            OutputFilePath = tempSettings.OutputFilePath;
            laserHeight = tempSettings.laserHeight;
            rowDistance = tempSettings.rowDistance;
            samplingDistance = tempSettings.samplingDistance;
            samplingDistanceSelectedIndex = tempSettings.samplingDistanceSelectedIndex;
            minHeight = tempSettings.minHeight;    
            useLeftLaser = true;
            useRightLaser = true;
            comSettings = tempSettings.comSettings;
            UsePolygonLayer = tempSettings.UsePolygonLayer;
            stream.Close();
        }

    }
}

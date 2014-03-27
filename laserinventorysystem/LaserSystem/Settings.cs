using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
namespace LaserSystem
{
    public class ComSettings
    {
        public string lComName;
        public int lBaudRate;
        public string rComName;
        public int rBaudRate;
        public string gpsComName;
        public int gpsBaudRate;
    }
    public class Settings
    {
        public double laserHeight;
        public double rowDistance;
        public double samplingDistance;
        public int samplingDistanceSelectedIndex;
        public bool useLeftLaser;
        public bool useRightLaser;

        public ComSettings comSettings;

        public string OutputFilePath;
        public string OutputFileName;
        
        private double defaultLaserHeight = 8;
        private double defaultRowDistance = 16;
        private double defaultSamplingDistance = 1;
        private int defaultSamplingDistanceSelectedIndex = 1;
        private string settingsFile = "settings1.xml";
        private double minHeight = 3;
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
                useLeftLaser = true;
                useRightLaser = true;
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
            Settings tempSettings = new Settings();
            XmlSerializer ser = new XmlSerializer(this.GetType());
            FileStream stream = new FileStream(settingsFile, FileMode.Open);
            tempSettings = (Settings)ser.Deserialize(stream);

            laserHeight = tempSettings.laserHeight;
            rowDistance = tempSettings.rowDistance;
            samplingDistance = tempSettings.samplingDistance;
            samplingDistanceSelectedIndex = tempSettings.samplingDistanceSelectedIndex;

            useLeftLaser = true;
            useRightLaser = true;
            comSettings = tempSettings.comSettings;
            stream.Close();
        }
    }
}

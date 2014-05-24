using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using LaserSystemLibrary;

namespace LaserSystemLibrary
{
    class SensorLogger
    {
        Stream TopLeftACSStream;
        Stream BottomLeftACSStream;
        Stream TopRightACSStream;
        Stream BottomRightACSStream;
        Stream LeftLMSStream;
        Stream RightLMSStream;
        Stream GPSStream;
        BinaryFormatter serializer = new BinaryFormatter();
        
        string FileTag;

        public SensorLogger(string filePath, string fileTag)
        {
            this.FileTag = fileTag;
            TopLeftACSStream = File.Create(filePath + fileTag + "TopLeftACS.bin");
            BottomLeftACSStream = File.Create(filePath + fileTag + "BottomLeftACS.bin");
            TopRightACSStream = File.Create(filePath + fileTag + "TopRightACS.bin");
            BottomRightACSStream = File.Create(filePath + fileTag + "BottomRightACS.bin");
            LeftLMSStream = File.Create(filePath + fileTag + "LeftLMS.bin");
            RightLMSStream = File.Create(filePath + fileTag + "RightLMS.bin");
            GPSStream = File.Create(filePath + fileTag + "GPS.bin");
        }

        private void LogACS(Stream stream, ACS430Reading reading)
        {
            serializer.Serialize(stream, reading);
        }

        public void LogTopLeftACS(ACS430Reading reading)
        {
            LogACS(TopLeftACSStream, reading);
        }
        public void LogBottomLeftACS(ACS430Reading reading)
        {
            LogACS(BottomLeftACSStream, reading);
        }
        public void LogTopRightACS(ACS430Reading reading)
        {
            LogACS(TopRightACSStream, reading);
        }
        public void LogBottomRightACS(ACS430Reading reading)
        {
            LogACS(BottomRightACSStream, reading);
        }
        public void LogLeftLMS(LmsScan2 scan)
        {
            serializer.Serialize(LeftLMSStream, scan);
        }
        public void LogRightLMS(LmsScan2 scan)
        {
            serializer.Serialize(RightLMSStream, scan);
        }
        public void LogGPS(NmeaSentence sentence)
        {
            serializer.Serialize(GPSStream, sentence);
        }

        public void Close()
        {
            TopLeftACSStream.Close();
            BottomLeftACSStream.Close();
            TopRightACSStream.Close();
            BottomRightACSStream.Close();
            LeftLMSStream.Close();
            RightLMSStream.Close();
            GPSStream.Close();
        }
    }
}

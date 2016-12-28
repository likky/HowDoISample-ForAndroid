using System.IO;

namespace CSHowDoISamples
{
    internal class SampleHelper
    {
        public readonly static string AssetsDataDictionary = @"AppData";
        public readonly static string SampleDataDictionary = @"mnt/sdcard/MapSuiteSampleData/HowDoISamples";

        public static string GetDataPath(string fileName)
        {
            return Path.Combine(SampleDataDictionary, AssetsDataDictionary, fileName);
        }
    }
}
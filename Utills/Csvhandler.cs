using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peak_deconvolution_for_OES_and_Actinometry.Utills
{
    internal class Csvhandler
    { 
        public class OESIntensityRecord
        {
            public DateTime Timestamp { get; set; }
            public int[] Intensity { get; set; }
        }

        public Csvhandler() { }
        // Method to read CSV file and return data as a list of strings
        public static (float[] Wavelengths, List<OESIntensityRecord> Records) ReadCsv(string filePath, char delimiter)
        {
            var lines = System.IO.File.ReadAllLines(filePath);
            if (lines.Length < 2)
                throw new Exception("CSV file is empty or has no data.");

            var header = lines[0].Split(delimiter);
            var wavelengths = header.Skip(1).Select(s => float.Parse(s)).ToArray();

            var records = new List<OESIntensityRecord>();
            for (int i = 1; i < lines.Length; i++)
            {
                var parts = lines[i].Split(delimiter);
                if (parts.Length != wavelengths.Length + 1)
                    throw new Exception("CSV file format is incorrect.");
                var timestamp = DateTime.ParseExact(parts[0], "yyyyMMdd_HH:mm:ss.fff", null);
                var intensity = parts.Skip(1).Select(s => int.Parse(s)).ToArray();
                records.Add(new OESIntensityRecord
                {
                    Timestamp = timestamp,
                    Intensity = intensity
                });
            }
            return (wavelengths, records);
        }
        public static (double[], double[,]) RecordsToMatrix(List<OESIntensityRecord> records, int numWavelengths)
        {
            double[] Timestamp = new double[records.Count];
            double[,] matrix = new double[records.Count, numWavelengths];
            var StartTime = records[0].Timestamp;
            for (int i = 0; i < records.Count; i++)
            {
                Timestamp[i] = (records[i].Timestamp - StartTime).TotalSeconds;
                for (int j = 0; j < numWavelengths; j++)
                {
                    matrix[i, j] = records[i].Intensity[j];
                }
            }
            return (Timestamp, matrix);
        }

        // Method to write data to a CSV file
        public void WriteCsv(string filePath, List<string> data)
        {
            try
            {
                System.IO.File.WriteAllLines(filePath, data);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to CSV file: {ex.Message}");
            }
        }
    }
}

using GlassesFrame;
using InventorApi;
using Microsoft.VisualBasic.Devices;
using System;
using System.Diagnostics;
using System.IO;

namespace LoadTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            LoadTesting();
        }

        private static void LoadTesting()
        {
            var builder = new GlassesFrameBuilder();
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var glassesFrameParameters = new GlassesFrameParameters();
            var streamWriter = new StreamWriter($"log.txt", true);
            Process currentProcess = System.Diagnostics.Process.GetCurrentProcess();
            var count = 0;
            while (true)
            {
                builder.BuilRoundGlassesFrame(glassesFrameParameters);
                var computerInfo = new ComputerInfo();
                var usedMemory = (computerInfo.TotalPhysicalMemory - computerInfo.AvailablePhysicalMemory) *
                         0.000000000931322574615478515625;
                streamWriter.WriteLine(
                  $"{++count}\t{stopWatch.Elapsed:hh\\:mm\\:ss}\t{usedMemory}");
                streamWriter.Flush();
            }

            stopWatch.Stop();
            streamWriter.Close();
            streamWriter.Dispose();
            Console.Write($"End {new ComputerInfo().TotalPhysicalMemory}");
        }
    }
}

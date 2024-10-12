using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using static System.Net.Mime.MediaTypeNames;

namespace NAudioTest
{
    public class NaudioTest
    {
        List<string> audioFilesFlagged = new List<string>();
        public void RunTest()
        {
            Console.WriteLine("Starting Audiofiles with silence identifier");
            var unvalidPath = true;
            var unvalidThreads = true;
            var path = "";
            var threads = 0;
            while (unvalidPath)
            {
                Console.WriteLine("Please provide Path where Audiofiles exist");
                path = Console.ReadLine();
                try
                {
                    Path.GetFullPath(path);
                    unvalidPath = false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Provided Path is not valid");
                }
            }

            while(unvalidThreads)
            {
                Console.WriteLine("Please provide number of parallel Threads");
                var threadNumber = Console.ReadLine();
                try
                {
                    threads = Convert.ToInt32(threadNumber);
                    if (threads == 0)
                    {
                        throw new Exception();
                    }
                    unvalidThreads = false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Please provide a number between 1 and 100");
                }
            }





            var audioFiles = Directory.GetFiles(path, "*.mp3", SearchOption.AllDirectories);

            //DEBUG FILES
            //List<string> audioFiles = new List<string>();
            //audioFiles.Add(@"C:\Users\cindy\Music\Music\Bloc Party\Bloc Party - Silent Alarm (Remixed)\13 - Compliments (Shibuyaka Remix by Nick Zinner).mp3");
            //audioFiles.Add(@"C:\Users\cindy\Music\Music\50 Cent\50 Cent - Blown Away (2009)\03 - 50 Cent - If Dead Men Can Talk.mp3");

            Console.WriteLine("Test is starting, Found " + audioFiles.Length + " to test");
            Parallel.ForEach(audioFiles, new ParallelOptions { MaxDegreeOfParallelism = threads }, audiofile => testSingleMp3(audiofile));

            File.WriteAllLines(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\AudioFileScan" + DateTime.Now.ToString("yyyyMMddhhmmss") +".txt", audioFilesFlagged);
            Console.WriteLine("Check is done, File-Paths are saved to Desktop");

        }

        private async void testSingleMp3(string file)
        {

            var maxPeakProvider = new MaxPeakProvider();

            using (var waveStream = new AudioFileReader(file))
            {
                int bytesPerSample = (waveStream.WaveFormat.BitsPerSample / 8);
                var samples = waveStream.Length / (bytesPerSample);
                var samplesPerPixel = (int)(samples / waveStream.TotalTime.TotalSeconds);
                maxPeakProvider.Init(waveStream.ToSampleProvider(), samplesPerPixel);
                int x = 0;
                var currentPeak = maxPeakProvider.GetNextPeak();
                bool hasSilence = false;
                int countSilence = 0;
                while (x < (int)(waveStream.TotalTime.TotalSeconds / 2))
                {
                    if (currentPeak.Max == 0)
                    {
                        countSilence++;
                    }
                    else
                    {
                        countSilence = 0;
                    }
                    var nextPeak = maxPeakProvider.GetNextPeak();
                    // DEBUG INFOS
                    //Console.WriteLine(x + " Current Peak Max: " + 50 * currentPeak.Max);
                    //Console.WriteLine(x + " Current Peak Min: " + 30 * currentPeak.Min);
                    x++;
                    currentPeak = maxPeakProvider.GetNextPeak();
                    if (countSilence > 2)
                    {
                        hasSilence = true;
                        break;
                    }
                }
                if (hasSilence)
                {
                    audioFilesFlagged.Add(waveStream.FileName);
                    Console.WriteLine("AUDIOFILE HAS SILENCE OVER 2 SECONDS: " + waveStream.FileName);
                }
                else
                {
                    //Console.WriteLine(waveStream.FileName + " has no silence");
                }
            }
        }
    }
}

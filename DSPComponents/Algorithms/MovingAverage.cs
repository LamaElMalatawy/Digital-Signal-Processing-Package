using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class MovingAverage : Algorithm
    {
        public Signal InputSignal { get; set; }
        public int InputWindowSize { get; set; }
        public Signal OutputAverageSignal { get; set; }
 
        public override void Run()
        {
            int avr = InputWindowSize / 2;
            List<float> outSamples = new List<float>();
            List<float> samples = InputSignal.Samples;
            int count = 0;
            for (int i = avr; i < samples.Count - avr; i++)
            {
                float sum = 0;
                for (int j = count; j < InputWindowSize + count; j++)
                {
                    sum += samples[j];

                }
                float result = sum / InputWindowSize;
                outSamples.Add(result);
                count++;

            }
            OutputAverageSignal = new Signal(outSamples, false);
            //throw new NotImplementedException();
        }
    }
}

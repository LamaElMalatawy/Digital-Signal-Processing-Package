using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class QuantizationAndEncoding : Algorithm
    {
        // You will have only one of (InputLevel or InputNumBits), the other property will take a negative value
        // If InputNumBits is given, you need to calculate and set InputLevel value and vice versa
        public int InputLevel { get; set; }
        public int InputNumBits { get; set; }
        public Signal InputSignal { get; set; }
        public Signal OutputQuantizedSignal { get; set; }
        public List<int> OutputIntervalIndices { get; set; }
        public List<string> OutputEncodedSignal { get; set; }
        public List<float> OutputSamplesError { get; set; }

        public override void Run()
        {   OutputQuantizedSignal = new Signal(new List<float>(),false);
            OutputIntervalIndices = new List<int>();
            OutputEncodedSignal = new List<string>();
            OutputSamplesError = new List<float>();
           
            if (InputLevel > 0)
            {
                InputNumBits = (int)Math.Ceiling(Math.Log(InputLevel));
            }
            else
            {
                InputLevel = (int)(Math.Pow(2, InputNumBits));
            }
            
            float quantization = (InputSignal.Samples.Max() - InputSignal.Samples.Min())/InputLevel ;
            float[] intervalsMax = new float[InputLevel];
            float[] midpoints = new float[InputLevel];
            // make the intervalsMax
            for (int i = 0; i < InputLevel; i++)
            {
                intervalsMax[i]= InputSignal.Samples.Min() + (quantization*(i+1));
            }
           //dedicate the midpoints
            for(int i = 0; i < InputLevel; i++)
            {
                if (i == 0) { midpoints[i] = (intervalsMax[i]+intervalsMax[i]-quantization)/2; }
                else
                    midpoints[i]= (intervalsMax[i]+intervalsMax[i-1])/2;
            }
            // replace each signal with the midpoint of the suitable interval
            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                for (int j = 0; j < InputLevel; j++)
                {
                    if (InputSignal.Samples[i] < intervalsMax[j] || j==InputLevel-1)
                    {
                        OutputIntervalIndices.Add(j+1);
                        OutputQuantizedSignal.Samples.Add(midpoints[j]);
                        OutputQuantizedSignal.SamplesIndices.Add(i);
                        OutputEncodedSignal.Add(Convert.ToString(j,2).PadLeft(InputNumBits,'0'));
                        break;
                    }
                }
            }
            // determine the error
            for (int i = 0; i < InputSignal.Samples.Count ; i++)
            {
                float error = OutputQuantizedSignal.Samples[i] - InputSignal.Samples[i];
                OutputSamplesError.Add(error);
            }


        }
    }
}

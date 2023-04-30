using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class FIR : Algorithm
    {
        public Signal InputTimeDomainSignal { get; set; }
        public FILTER_TYPES InputFilterType { get; set; }
        public float InputFS { get; set; }
        public float? InputCutOffFrequency { get; set; }
        public float? InputF1 { get; set; }
        public float? InputF2 { get; set; }
        public float InputStopBandAttenuation { get; set; }
        public float InputTransitionBand { get; set; }
        public Signal OutputHn { get; set; }
        public Signal OutputYn { get; set; }

        public override void Run()
        {
            OutputHn = new Signal(new List<float>(), new List<int>(), false);
            float tranWidth = 0.0f;
            string windowType = "";
            if (InputStopBandAttenuation <= 21)
            {
                tranWidth = 0.9f;
                windowType = "rect";
            }
            else if (InputStopBandAttenuation > 21 && InputStopBandAttenuation <= 44)
            {
                tranWidth = 3.1f;
                windowType = "hanning";
            }
            else if (InputStopBandAttenuation > 44 && InputStopBandAttenuation <= 53)
            {
                tranWidth = 3.3f;
                windowType = "hamming";
            }
            else if (InputStopBandAttenuation > 53 && InputStopBandAttenuation <= 74)
            {
                tranWidth = 5.5f;
                windowType = "blackman";
            }

           int N = (int)Math.Ceiling(tranWidth / (InputTransitionBand / InputFS));
            if (N % 2 == 0)  
                N++;
            
            int n = (int)-N / 2;
            for (int i = 0; i < N; i++)
            {
                
                OutputHn.SamplesIndices.Add(n);
                n++;
            }

           

            if (InputFilterType == FILTER_TYPES.LOW)
            {
                float normCutOffFreq = (float)(((InputCutOffFrequency + (InputTransitionBand / 2)))/InputFS); 
                for (int i = 0; i < N; i++)
                {
                    int index = Math.Abs(OutputHn.SamplesIndices[i]);
                    if (OutputHn.SamplesIndices[i] == 0)
                    {
                        float hn = 2 * normCutOffFreq;
                        float window = window_method(windowType, index, N);
                        OutputHn.Samples.Add(hn * window);
                    }
                    else
                    {
                        float Omega = (float)(2 * Math.PI * normCutOffFreq * index);
                        float hn = (float)(2 * normCutOffFreq * Math.Sin(Omega) / Omega);
                        float window = window_method(windowType, index, N);
                        OutputHn.Samples.Add(hn * window);
                    }
                }
            }
            else if (InputFilterType == FILTER_TYPES.HIGH)
            {
                float normCutOffFreq = (float)(((InputCutOffFrequency +(InputTransitionBand / 2))) / InputFS);
                for (int i = 0; i < N; i++)
                {
                    int index = Math.Abs(OutputHn.SamplesIndices[i]);
                    if (OutputHn.SamplesIndices[i] == 0)
                    {
                        float hn = 1 - (2 * normCutOffFreq);
                        float window = window_method(windowType, index, N);
                        OutputHn.Samples.Add(hn * window);
                    }
                    else
                    {
                        float Omega = (float)(2 * Math.PI * normCutOffFreq * index);
                        float hn = -(float)(2 * normCutOffFreq * Math.Sin(Omega) / Omega);
                        float window = window_method(windowType, index, N);
                        OutputHn.Samples.Add(hn * window);
                    }
                }

            }
            else if (InputFilterType == FILTER_TYPES.BAND_PASS)
            {
                float normCutOffFreq1 = (float)((InputF1 - (InputTransitionBand / 2))) / InputFS;
                float normCutOffFreq2 = (float)((InputF2 + (InputTransitionBand / 2))) / InputFS;
                for (int i = 0; i < N; i++)
                {
                    int index = Math.Abs(OutputHn.SamplesIndices[i]);
                    if (OutputHn.SamplesIndices[i] == 0)
                    {
                        float hn = 2 * (normCutOffFreq2 - normCutOffFreq1);
                        float window = window_method(windowType, index, N);
                        OutputHn.Samples.Add(hn * window);
                    }
                    else
                    {
                        float Omega1 = (float)(2 * Math.PI * normCutOffFreq1 * index);
                        float Omega2 = (float)(2 * Math.PI * normCutOffFreq2 * index);
                        float hn = (float)((2 * normCutOffFreq2 * Math.Sin(Omega2) / Omega2) - (2 * normCutOffFreq1 * Math.Sin(Omega1) / Omega1));

                        float window = (window_method(windowType, index, N));
                        OutputHn.Samples.Add(hn * window);
                    }
                }
            }
            else if (InputFilterType == FILTER_TYPES.BAND_STOP)
            {
                float normCutOffFreq1 = (float)((InputF1 - (InputTransitionBand / 2))) / InputFS;
                float normCutOffFreq2 = (float)((InputF2 + (InputTransitionBand / 2))) / InputFS;
                for (int i = 0; i < N; i++)
                {
                    int index = Math.Abs(OutputHn.SamplesIndices[i]);
                    if (OutputHn.SamplesIndices[i] == 0)
                    {
                        float hn = 1 - (2 * (normCutOffFreq2 - normCutOffFreq1));
                        float window = window_method(windowType, index, N);
                        OutputHn.Samples.Add(hn * window);
                    }
                    else
                    {
                        float Omega1 = (float)(2 * Math.PI * normCutOffFreq1 * index);
                        float Omega2 = (float)(2 * Math.PI * normCutOffFreq2 * index);
                        float hn = (float)((2 * normCutOffFreq1 * Math.Sin(Omega1) / Omega1) - (2 * normCutOffFreq2 * Math.Sin(Omega2) / Omega2));

                        float window = (window_method(windowType, index, N));
                        OutputHn.Samples.Add(hn * window);
                    }
                }
            }

            DirectConvolution c = new DirectConvolution();
            c.InputSignal1 = InputTimeDomainSignal;
            c.InputSignal2 = OutputHn;
            c.Run();
            OutputYn = c.OutputConvolvedSignal;

        }
        public float window_method(String windowType, int n, int N)
        {
            float result = 0.0f;
            if (windowType == "rect")
            {
                result = 1;
            }
            else if (windowType == "hanning")
            {
                result = (float)0.5 + (float)(0.5 * Math.Cos((2 * Math.PI * n) / N));
            }
            else if (windowType == "hamming")
            {
                result = (float)0.54 + (float)(0.46 * Math.Cos((2 * Math.PI * n) / N));
            }
            else if (windowType == "blackman")
            {
                result = (float)(0.42 + (0.5 * Math.Cos((2 * Math.PI * n) / (N - 1))) + (0.08 * Math.Cos((4 * Math.PI * n) / (N - 1))));
            }

            return result;
        }
    }
}

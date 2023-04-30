using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class Sampling : Algorithm
    {
        public int L { get; set; } //upsampling factor
        public int M { get; set; } //downsampling factor
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }




        public override void Run()
        {
            OutputSignal = new Signal(new List<float>(), false);

            List<float> re_sampling = new List<float>();
            int count = InputSignal.Samples.Count;
            int N;


            if (L == 0 && M == 0)
            {
                return;
            }

            if (M == 0 && L > 0)
            {

                for (int i = 0; i < count; i++)
                {
                    re_sampling.Add(InputSignal.Samples[i]);
                    for (int j = 0; j < L - 1; j++)
                    {
                        re_sampling.Add(0);
                    }
                }
                N = count * L; // N = 53
                int n = (int)-N / 2;
                for (int i = 0; i < N; i++) // -26 < n < 26
                {
                    OutputSignal.SamplesIndices.Add(n);
                     n++;
                }

                //low bass filter
                FIR c = new FIR();
                c.InputFilterType = DSPAlgorithms.DataStructures.FILTER_TYPES.LOW;
                c.InputFS = 8000;
                c.InputStopBandAttenuation = 50;
                c.InputCutOffFrequency = 1500;
                c.InputTransitionBand = 500;
                c.InputTimeDomainSignal = new Signal(re_sampling, false);
                c.Run();
                List<float> samples = c.OutputYn.Samples;
                OutputSignal.Samples = samples;

            }
            else if (M > 0 && L == 0)
            {
                //low bass filter
                FIR c = new FIR();
                c.InputFilterType = DSPAlgorithms.DataStructures.FILTER_TYPES.LOW;
                c.InputFS = 8000;
                c.InputStopBandAttenuation = 50;
                c.InputCutOffFrequency = 1500;
                c.InputTransitionBand = 500;
                c.InputTimeDomainSignal = new Signal(InputSignal.Samples, false);
                c.Run();
                re_sampling = c.OutputYn.Samples;
                List<float> re_sampling2 = new List<float>();
                for (int i = 0; i < count; i += M)
                {
                    re_sampling2.Add(re_sampling[i]);
                }
                N = count / M;
                int n = (int)-N / 2;
                for (int i = 0; i < N; i++)
                {
                    OutputSignal.SamplesIndices.Add(n);
                    n++;
                }
                OutputSignal.Samples = re_sampling2;
            }
            else if (M > 0 && L > 0)
            {

                for (int i = 0; i < count; i++)
                {
                    re_sampling.Add(InputSignal.Samples[i]);
                    for (int j = 0; j < L - 1; j++)
                    {
                        re_sampling.Add(0);
                    }
                }
                N = count * L;
                int n = (int)-N / 2;
                for (int i = 0; i < N; i++)
                {
                    OutputSignal.SamplesIndices.Add(n);
                    n++;
                }
                //low bass filter
                FIR c = new FIR();
                c.InputFilterType = DSPAlgorithms.DataStructures.FILTER_TYPES.LOW;
                c.InputFS = 8000;
                c.InputStopBandAttenuation = 50;
                c.InputCutOffFrequency = 1500;
                c.InputTransitionBand = 500;
                c.InputTimeDomainSignal = new Signal(re_sampling, false);
                c.Run();
                List<float> samples = c.OutputYn.Samples;

                N = count / M;
                n = (int)-N / 2;
                for (int i = 0; i < N; i++)
                {
                    OutputSignal.SamplesIndices.Add(n);
                    n++;
                }
                List<float> updated_samples = new List<float>();
                for (int i = 0; i < samples.Count; i += M)
                {
                    updated_samples.Add(samples[i]);
                }
                OutputSignal.Samples = updated_samples;
            }


        }
    }

}

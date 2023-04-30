using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;
using System.Numerics;

namespace DSPAlgorithms.Algorithms
{
    public class DiscreteFourierTransform : Algorithm
    {
        public Signal InputTimeDomainSignal { get; set; }
        public float InputSamplingFrequency { get; set; }
        public Signal OutputFreqDomainSignal { get; set; }

        public override void Run()
        {
            OutputFreqDomainSignal = new Signal(new List<float>(),false);
            int N = InputTimeDomainSignal.Samples.Count;
            int n, k;
            float pi = (float)(Math.PI);
            float real;
            float imaginary;
            List<Complex> complexList = new List<Complex>();
            for (k = 0; k < N; k++)
            {
                real= 0;
                imaginary = 0;
                for (n = 0; n < N; n++)
                {
                    real += (float)(InputTimeDomainSignal.Samples[n] * Math.Cos((2 * pi* k * n )/N));
                    imaginary+= (float)( InputTimeDomainSignal.Samples[n] * -Math.Sin((2 * pi * k * n) / N));
                }
                complexList.Add(new Complex(real, imaginary));
            }
            List<float> phase = new List<float>();
            List<float> amp = new List<float>();
            for(int i=0; i < complexList.Count; i++)
            {
                amp.Add((float)(complexList[i].Magnitude));
                phase.Add((float)(complexList[i].Phase));
            }
            OutputFreqDomainSignal.FrequenciesAmplitudes = amp;
            OutputFreqDomainSignal.FrequenciesPhaseShifts = phase;

            //throw new NotImplementedException();
        }
    }
}

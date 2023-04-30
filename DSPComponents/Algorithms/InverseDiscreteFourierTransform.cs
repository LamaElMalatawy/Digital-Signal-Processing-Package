using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;
using System.Numerics;

namespace DSPAlgorithms.Algorithms
{
    public class InverseDiscreteFourierTransform : Algorithm
    {
        public Signal InputFreqDomainSignal { get; set; }
        public Signal OutputTimeDomainSignal { get; set; }

        public override void Run()
        {
            List<float> amp = InputFreqDomainSignal.FrequenciesAmplitudes;
            List<float> phase = InputFreqDomainSignal.FrequenciesPhaseShifts;
            int N = InputFreqDomainSignal.Frequencies.Count;

            List<Complex> com = new List<Complex>();
            float real = 0.0f;
            float imag = 0.0f;

            for (int i = 0; i < N; i++)
            {
                real = (float)(amp[i] * Math.Cos(phase[i]));
                imag = (float)(amp[i] * Math.Sin(phase[i]));

                com.Add(new Complex(real, imag));

            }
            List<float> value = new List<float>();
            float result;
            for (int n = 0; n < N; n++)
            {
                result = 0;
                for (int k = 0; k < N; k++)
                {
                    float ph = (2 * (float)Math.PI * n * k) / N;
                    Complex c = new Complex(Math.Cos(ph), Math.Sin(ph));
                    Complex temp = Complex.Multiply(c, com[k]);
                    result += (float)(temp.Real + temp.Imaginary);




                }

                value.Add((float)Math.Round((float)(result / InputFreqDomainSignal.Frequencies.Count)));
                Console.WriteLine(value[n]);



            }

            OutputTimeDomainSignal = new Signal(value, false);
            OutputTimeDomainSignal.Samples = value;


            //throw new NotImplementedException();
        }
    }
}

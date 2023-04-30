using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;
using DSPAlgorithms.Algorithms;

namespace DSPAlgorithms.Algorithms
{
    public class DirectCorrelation : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public List<float> OutputNonNormalizedCorrelation { get; set; }
        public List<float> OutputNormalizedCorrelation { get; set; }

        public override void Run()
        {
            List<float> result = new List<float>();
            List<float> resultNormalized = new List<float>();
            float resultTemp;
            float resultNormalizedTemp;

            if (InputSignal2 == null)
            {
                for (int j = 0; j < InputSignal1.Samples.Count; j++)
                {
                    resultTemp = 0;
                    resultNormalizedTemp = 0;
                    for (int n = 0; n < InputSignal1.Samples.Count; n++)
                    {
                        resultNormalizedTemp += (float)Math.Pow(InputSignal1.Samples[n], 2);
                        if (InputSignal1.Periodic) resultTemp += InputSignal1.Samples[n] * InputSignal1.Samples[(n + j) % InputSignal1.Samples.Count];

                        else
                        {
                            if (n + j < InputSignal1.Samples.Count) resultTemp += InputSignal1.Samples[n] * InputSignal1.Samples[(n + j)];

                        }
                    }
                    resultTemp /= InputSignal1.Samples.Count;
                    result.Add(resultTemp);

                    resultNormalizedTemp = (float)Math.Pow(resultNormalizedTemp, 2);
                    resultNormalizedTemp = (float)Math.Sqrt(resultNormalizedTemp) / InputSignal1.Samples.Count;
                    resultNormalized.Add(resultTemp / resultNormalizedTemp);

                }
            }
            else
            {
                List<float> Input1 = new List<float>();
                List<float> Input2 = new List<float>();
                for (int i = 0; i < InputSignal1.Samples.Count; i++)
                    Input1.Add(InputSignal1.Samples[i]);
                for (int i = 0; i < InputSignal2.Samples.Count; i++)
                    Input2.Add(InputSignal2.Samples[i]);


                if (Input1 != Input2)
                {
                    int cross_length = Input1.Count + Input2.Count - 1;
                    for (int i = Input1.Count; i < cross_length; i++)
                        Input1.Add(0);
                    for (int i = Input2.Count; i < cross_length; i++)
                        Input2.Add(0);
                }


                float resultNormalizedTemp1;
                float resultNormalizedTemp2;
                for (int j = 0; j < Input1.Count; j++)
                {
                    resultTemp = 0;
                    resultNormalizedTemp1 = 0;
                    resultNormalizedTemp2 = 0;
                    resultNormalizedTemp = 0;
                    for (int n = 0; n < Input1.Count; n++)
                    {
                        resultNormalizedTemp1 += Input1[n] * Input1[n];
                        resultNormalizedTemp2 += Input2[n] * Input2[n];

                        if (InputSignal1.Periodic) resultTemp += Input1[n] * Input2[(n + j) % Input2.Count];
                        else
                        {
                            if (n + j < Input1.Count) resultTemp += Input1[n] * Input2[(n + j)];

                        }
                    }
                    resultTemp /= Input1.Count;
                    result.Add(resultTemp);

                    resultNormalizedTemp = resultNormalizedTemp1 * resultNormalizedTemp2;
                    resultNormalizedTemp = (float)Math.Sqrt(resultNormalizedTemp) / Input1.Count;
                    resultNormalized.Add(resultTemp / resultNormalizedTemp);
                }
            }

            OutputNonNormalizedCorrelation = result;
            OutputNormalizedCorrelation = resultNormalized;





            //throw new NotImplementedException();
        }
    }
}
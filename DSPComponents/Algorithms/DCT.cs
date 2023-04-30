using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class DCT : Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            List<float> outsig = new List<float>();
            int count = InputSignal.Samples.Count;
            double sqrtvalue = Math.Sqrt(2.0 / count);

            for (int u = 0; u < count; u++)
            {
                double sum = 0;
                for (int k = 0; k < count; k++)
                {
                    sum += InputSignal.Samples[k] * Math.Cos((((2 * k) + 1) * u * Math.PI) / (2.0 * count));
                }
                if (u == 0)
                {
                    outsig.Add((float)(Math.Sqrt(1.0 / count) * sum));
                }
                else
                {
                    outsig.Add((float)(sqrtvalue * sum));
                }
            }

            OutputSignal = new Signal(outsig, false);
        }
    }
}

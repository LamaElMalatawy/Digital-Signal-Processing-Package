using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Shifter : Algorithm
    {
        public Signal InputSignal { get; set; }
        public int ShiftingValue { get; set; }
        public Signal OutputShiftedSignal { get; set; }

        public override void Run()
        {
            List<float> samples = InputSignal.Samples;
            int head;
            List<int> outputIndex = new List<int>();

            if (ShiftingValue > 0)
            {
                head = InputSignal.SamplesIndices[0] - ShiftingValue;

                for (int i = 0; i < InputSignal.SamplesIndices.Count; i++)
                {
                    outputIndex.Add(head + i);


                }

            }
            else
            {

                int tail = InputSignal.SamplesIndices[0] - ShiftingValue;

                for (int i = 0; i < InputSignal.SamplesIndices.Count; i++)
                {
                    outputIndex.Add(tail + i);


                }

            }






            OutputShiftedSignal = new Signal(samples, outputIndex, false);

            //throw new NotImplementedException();
        }
    }
}

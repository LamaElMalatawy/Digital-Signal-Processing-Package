using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DirectConvolution : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputConvolvedSignal { get; set; }

        /// <summary>
        /// Convolved InputSignal1 (considered as X) with InputSignal2 (considered as H)
        /// </summary>
        public override void Run()
        {
            List<float> X = InputSignal1.Samples;
            List<float> H = InputSignal2.Samples;
            int row = X.Count;
            int col = H.Count;
            float[,] grid = new float[row, col];

            for (int r = 0; r < row; r++)
            {
                for (int c = 0; c < col; c++)
                {
                    grid[r, c] = X[r] * H[c];
                }
            }

            List<float> output = new List<float>();

            for (int k = 0; k < row; k++)
            {
                float sum = 0;
                int r = k;
                int c = 0;
                while (r >= 0 & c < col)
                {

                    sum += grid[r, c];
                    r -= 1;
                    c += 1;

                }
                output.Add(sum);
            }


            for (int k = 1; k < col; k++)
            {
                float sum = 0;
                int r = row - 1;
                int c = k;
                while (c < col & r >= 0)
                {

                    sum += grid[r, c];
                    r -= 1;
                    c += 1;

                }
                output.Add(sum);
            }



            List<int> index = new List<int>();
            int start = InputSignal1.SamplesIndices[0] + InputSignal2.SamplesIndices[0];

            for (int i = 0; i < output.Count; i++)
            {
                index.Add(start);
                Console.WriteLine(index[i] + " " + output[i]);
                start++;
            }


            OutputConvolvedSignal = new Signal(output, index, false);
        }
    }

}

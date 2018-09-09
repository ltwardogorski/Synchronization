using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dewiacja
{
    public partial class Form1 : Form
    {
        // parametry generatorów
        private ulong _grain = 1179;
        private const ulong M = 2147483647;
        private const ulong A = 16807;

        //generator próbek o rozkładzie równomiernym (0, 1)
        public double make_grain()
        {
            _grain = _grain * A % M;
            return _grain / (double) M;
        }

        //generator próbek o rozkładzie równomiernym (min, max, number_of_sample)
        public double[] uniform_distribution(int min, int max, ulong numberOfSample)
        {
            var tableOfSample = new double[numberOfSample];
            for (var i = 0; i < tableOfSample.Length; i++)
            {
                tableOfSample[i] = min + (max - min) * make_grain();
            }

            return tableOfSample;
        }

        //generator próbek o rozkładzie geometrycznym (p (average [0, 1]), number_of_sample)
        public double[] geometric_distribution(double p, ulong numberOfSample)
        {
            var tableOfSample = new double[numberOfSample];
            for (var i = 0; i < tableOfSample.Length; i++)
            {
                double kernel = 1;

                while (true)
                {
                    if (make_grain() > p)
                    {
                        kernel++;
                        continue;
                    }

                    break;
                }

                tableOfSample[i] = kernel;
            }

            return tableOfSample;
        }

        //generator próbek o rozkładzie normalnym(e (average), ew (variance), number_of_sample)
        public double[] normal_distribution(double e, double ew, ulong numberOfSample)
        {
            var tableOfSample = new double[numberOfSample];
            for (var i = 0; i < tableOfSample.Length; i++)
            {
                double kernel = 1;
                double temp = 0;
                for (var j = 0; j < 12; j++)
                {
                    temp += make_grain();
                }

                temp -= 6.0;
                kernel = temp * ew + e;

                tableOfSample[i] = kernel;
            }

            return tableOfSample;
        }

        //generator próbek o rozkładzie wykładniczym(lambda [0,1])
        public double[] exponential_distribution(double lambda, ulong numberOfSample)
        {
            var tableOfSample = new double[numberOfSample];
            for (var i = 0; i < tableOfSample.Length; i++)
            {
                double kernel = 1;
                kernel = -1 / lambda * Math.Log(make_grain());

                tableOfSample[i] = kernel;
            }

            return tableOfSample;
        }

        //drukownie wartosci probek zapisanych w tablicy
        public void print_test(double[] tableOfSample)
        {
            foreach (var t in tableOfSample)
            {
                Console.WriteLine(t.ToString(CultureInfo.InvariantCulture));
            }
        }

        public Form1()
        {
            // uniform_distribution(-10, 10, 100);     // wywołanie generatora o rozkładzie równomiernym i zapis do [table_of_sample]
            // geometric_distribution(0.1, 100);       // wywołanie generatora o rozkładzie geometrycznym i zapis do [table_of_sample]
            // normal_distribution(100, 10, 100);      // wywołanie generatora o rozkładzie normalnym i zais do [table_of_sample]
            // exponential_distribution(0.1, 100);     // wywołanie generatora o rozkładzie wykładnicznym i zapis do [table_of_sample]

            print_test(uniform_distribution(-10, 10, 100));

            InitializeComponent();
        }
    }
}
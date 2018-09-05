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
        UInt64 grain = 1179; 
        const UInt64 m = 2147483647;
        const UInt64 a = 16807;
    
        //generator próbek o rozkładzie równomiernym (0, 1)
        public double make_grain()
        {
            grain = grain * a % m;
            return grain;
        }

        //generator próbek o rozkładzie równomiernym (min, max, number_of_sample)
        public void uniform_distribution(int min, int max, UInt64 number_of_sample)
        {
            var table_of_sample = new double[number_of_sample];
            for (var i = 0; i < table_of_sample.Length; i++)
            {
                table_of_sample[i] = min + (max - min)* (make_grain()/m);
                Console.WriteLine(table_of_sample[i].ToString(CultureInfo.InvariantCulture));
            }
        }

        //generator próbek o rozkładzie geometrycznym (p (average [0, 1]), number_of_sample)
        public void geometric_distribution(double p, UInt64 number_of_sample)
        {
            var table_of_sample = new double[number_of_sample];
            for (var i = 0; i < table_of_sample.Length; i++)
            {
                double kernel = 1;

                    while (true){
                        if ((make_grain() / m) > p)
                        {
                           kernel ++;
                           continue;
                        }
                         break;
                        }

                table_of_sample[i] =kernel;
                Console.WriteLine(table_of_sample[i].ToString(CultureInfo.InvariantCulture));
            }
        }

        //generator próbek o rozkładzie normalnym(e (average), ew (variance), number_of_sample)
        public void normal_distribution(double e, double ew, UInt64 number_of_sample)
        {
            var table_of_sample = new double[number_of_sample];
            for (var i = 0; i < table_of_sample.Length; i++)
            {
                double kernel = 1;
                double temp = 0;
                for (var j = 0; j< 12; j++)
                {
                    temp += (make_grain() / m);
                }
                temp -= 6.0;
                kernel = temp* ew + e;

                table_of_sample[i] = kernel;
                Console.WriteLine(table_of_sample[i].ToString(CultureInfo.InvariantCulture));
            }
        }

        //generator próbek o rozkładzie wykładniczym(lambda [0,1])
        public void exponential_distribution(double lambda, UInt64 number_of_sample)
        {
            var table_of_sample = new double[number_of_sample];
            for (var i = 0; i < table_of_sample.Length; i++)
            {
                double kernel = 1;
                kernel = -1 / lambda* Math.Log((make_grain() / m));

                table_of_sample[i] = kernel;
                Console.WriteLine(table_of_sample[i].ToString(CultureInfo.InvariantCulture));
            }
        }


    public Form1()
        {

            // uniform_distribution(-10, 10, 100);     // wywołanie generatora o rozkładzie równomiernym i zapis do [table_of_sample]
            // geometric_distribution(0.1, 100);       // wywołanie generatora o rozkładzie geometrycznym i zapis do [table_of_sample]
            // normal_distribution(100, 10, 100);      // wywołanie generatora o rozkładzie normalnym i zais do [table_of_sample]
            exponential_distribution(0.1, 100);        // wywołanie generatora o rozkładzie wykładnicznym i zapis do [table_of_sample]

            InitializeComponent();
        }
    }
}


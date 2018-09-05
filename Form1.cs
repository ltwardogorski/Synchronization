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


        public Form1()
        {
            uniform_distribution(-10, 10, 100);     // wywołanie generatora o rozkładzie równomiernym i zapis do [table_of_sample]
            InitializeComponent();
        }
    }
}


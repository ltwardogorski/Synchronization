using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dewiacja
{
    public partial class Form1 : Form
    {

        //generator próbek o rozkładzie równomiernym
        public void uniform_distribution()
        {
            const UInt64 m = 2147483647;
            const UInt64 a = 16807;
            UInt64 grain = 1179;
            double[] table_of_sample= new double[1000];
            for(var i=0; i<table_of_sample.Length;i++)
            {
                grain = (grain * a % m);
                table_of_sample[i] = 10*grain / m;
                Console.WriteLine(table_of_sample[i]);
            }
            
            
        }


        public Form1()
        {
            uniform_distribution();
            InitializeComponent();
        }
    }
}

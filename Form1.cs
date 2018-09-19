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
using ZedGraph;

namespace dewiacja
{
    public partial class Form1 : Form
    {
       // private PointPairList _uniform_sample_noise;

        //sample ZedGraph
        public Form1()
        {
            // uniform_distribution(-10, 10, 100);     // wywołanie generatora o rozkładzie równomiernym i zapis do [table_of_sample]
            // geometric_distribution(0.1, 100);       // wywołanie generatora o rozkładzie geometrycznym i zapis do [table_of_sample]
            // normal_distribution(100, 10, 100);      // wywołanie generatora o rozkładzie normalnym i zais do [table_of_sample]
            // exponential_distribution(0.1, 100);     // wywołanie generatora o rozkładzie wykładnicznym i zapis do [table_of_sample]

            // print_test(uniform_distribution(-10, 10, 100));

            InitializeComponent();
   
            plotGraph();

        }

        private void zedGraphControl1_Load(object sender, EventArgs e)
        {
            plotGraph();
            SetSize();
        }


        private int[] buildTeamAData()
        {
            int[] goalsScored = new int[10];
            for (int i = 0; i < 10; i++)
            {
                goalsScored[i] = (i + 1) / 10;
            }

            return goalsScored;
        }

        private int[] buildTeamBData()
        {
            int[] goalsScored = new int[10];
            for (int i = 0; i < 10; i++)
            {
                goalsScored[i] = (i + 10) * 11;
            }

            return goalsScored;
        }

        private void plotGraph()
        {
            
            GraphPane myPane = zedGraphControl1.GraphPane;
            myPane.CurveList.Clear();

            uint number_of_sample = 500;

            // Set the Titles
            myPane.Title.Text = "Time Error [ms]";
            myPane.XAxis.Title.Text = "Number of Sample";
            myPane.YAxis.Title.Text = "Value [ms]";
            /* myPane.XAxis.Scale.MajorStep = 50;
             myPane.YAxis.Scale.Mag = 0;
             myPane.XAxis.Scale.Max = 1000;*/

            PointPairList uniformPairList = new PointPairList();
            PointPairList geometricPairList = new PointPairList();
            PointPairList normalPairList = new PointPairList();
            PointPairList exponentialPairList = new PointPairList();

            double[] uniform_table_of_Sample = uniform_distribution(-1,1,number_of_sample);
            double[] geometric_table_of_Sample = geometric_distribution(0.1, number_of_sample);
            double[] normal_table_of_Sample = normal_distribution(1, 1, number_of_sample);
            double[] exponential_table_of_Sample = exponential_distribution(0.1, number_of_sample);


            for (int i = 0; i < uniform_table_of_Sample.Length; i++)
                uniformPairList.Add(i, uniform_table_of_Sample[i]);

            for (int i = 0; i < uniform_table_of_Sample.Length; i++)
                geometricPairList.Add(i, geometric_table_of_Sample[i]);

            for (int i = 0; i < uniform_table_of_Sample.Length; i++)
                normalPairList.Add(i, normal_table_of_Sample[i]);

            for (int i = 0; i < uniform_table_of_Sample.Length; i++)
                exponentialPairList.Add(i, exponential_table_of_Sample[i]);


            LineItem uniformCurve = myPane.AddCurve("Uniform Distribution",
                uniformPairList, Color.Red, SymbolType.Circle);

            LineItem geometricCurve = myPane.AddCurve("Geometric Distribution",
                geometricPairList, Color.Blue, SymbolType.Circle);

            LineItem normalCurve = myPane.AddCurve("Normal Distribution",
                normalPairList, Color.Black, SymbolType.Circle);

            LineItem exponentialCurve = myPane.AddCurve("Exponential Distribution",
                exponentialPairList, Color.Green, SymbolType.Circle);
 
            zedGraphControl1.AxisChange();
        }

        private void SetSize()
        {
            zedGraphControl1.Location = new Point(0, 0);
            zedGraphControl1.IsShowPointValues = true;
            zedGraphControl1.Size = new Size(this.ClientRectangle.Width - 20, this.ClientRectangle.Height - 50);

        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            SetSize();
        }



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


    }
}
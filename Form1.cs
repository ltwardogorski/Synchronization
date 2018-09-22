using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;

namespace dewiacja
{

    public partial class Form1 : Form
    {

        public enum plot_2D : int
        {
            Uniform,
            Geomtric,
            Normal,
            Exponential
        }

        private plot_2D choice_2D_plot=plot_2D.Uniform;

        uint number_of_sample = 0;

        public Form1()
        {
            // uniform_distribution(-10, 10, 100);     // wywołanie generatora o rozkładzie równomiernym i zapis do [table_of_sample]
            // geometric_distribution(0.1, 100);       // wywołanie generatora o rozkładzie geometrycznym i zapis do [table_of_sample]
            // normal_distribution(100, 10, 100);      // wywołanie generatora o rozkładzie normalnym i zais do [table_of_sample]
            // exponential_distribution(0.1, 100);     // wywołanie generatora o rozkładzie wykładnicznym i zapis do [table_of_sample]

            // print_test(uniform_distribution(-10, 10, 100));

            InitializeComponent();
        }

        private void zedGraphControl1_Load(object sender, EventArgs e)
        {
            SetSize();
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

        private void zedGraphRefresh()
        {
            zedGraphControl1.AxisChange();
            Refresh();
        }

        private void plotGraph2D_Uniform(uint number_of_sample)
        {
            
            GraphPane myPane = zedGraphControl1.GraphPane;
            myPane.CurveList.Clear();

            // Set the Titles
            myPane.Title.Text = "Time Error [ms]";
            myPane.XAxis.Title.Text = "Number of Sample";
            myPane.YAxis.Title.Text = "Value [ms]";

            PointPairList uniformPairList = new PointPairList();

            double[] uniform_table_of_Sample = uniform_distribution(-1,1,number_of_sample);


            for (int i = 0; i < uniform_table_of_Sample.Length; i++)
                uniformPairList.Add(i, uniform_table_of_Sample[i]);

            LineItem uniformCurve = myPane.AddCurve("Uniform Distribution",
                uniformPairList, Color.Red, SymbolType.Circle);
        }

        private void plotGraph2D_Geometric(uint number_of_sample)
        {

            GraphPane myPane = zedGraphControl1.GraphPane;
            myPane.CurveList.Clear();

            // Set the Titles
            myPane.Title.Text = "Time Error [ms]";
            myPane.XAxis.Title.Text = "Number of Sample";
            myPane.YAxis.Title.Text = "Value [ms]";

            PointPairList geometricPairList = new PointPairList();

            double[] geometric_table_of_Sample = geometric_distribution(0.1, number_of_sample);


            for (int i = 0; i < geometric_table_of_Sample.Length; i++)
                geometricPairList.Add(i, geometric_table_of_Sample[i]);


            LineItem geometricCurve = myPane.AddCurve("Geometric Distribution",
                geometricPairList, Color.Blue, SymbolType.Circle);
        }

        private void plotGraph2D_Normal(uint number_of_sample)
        {

            GraphPane myPane = zedGraphControl1.GraphPane;
            myPane.CurveList.Clear();

            // Set the Titles
            myPane.Title.Text = "Time Error [ms]";
            myPane.XAxis.Title.Text = "Number of Sample";
            myPane.YAxis.Title.Text = "Value [ms]";

            PointPairList normalPairList = new PointPairList();

            double[] normal_table_of_Sample = normal_distribution(1, 1, number_of_sample);

            for (int i = 0; i < normal_table_of_Sample.Length; i++)
                normalPairList.Add(i, normal_table_of_Sample[i]);


            LineItem normalCurve = myPane.AddCurve("Normal Distribution",
                normalPairList, Color.Black, SymbolType.Circle);
        }

        private void plotGraph2D_Exponential(uint number_of_sample)
        {

            GraphPane myPane = zedGraphControl1.GraphPane;
            myPane.CurveList.Clear();

            // Set the Titles
            myPane.Title.Text = "Time Error [ms]";
            myPane.XAxis.Title.Text = "Number of Sample";
            myPane.YAxis.Title.Text = "Value [ms]";

            PointPairList exponentialPairList = new PointPairList();


            double[] exponential_table_of_Sample = exponential_distribution(0.1, number_of_sample);


            for (int i = 0; i < exponential_table_of_Sample.Length; i++)
                exponentialPairList.Add(i, exponential_table_of_Sample[i]);

            LineItem exponentialCurve = myPane.AddCurve("Exponential Distribution",
                exponentialPairList, Color.Green, SymbolType.Circle);
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
/*

        // drukownie wartosci probek zapisanych w tablicy
        public void print_test(double[] tableOfSample)
        {
            foreach (var t in tableOfSample)
            {
                Console.WriteLine(t.ToString(CultureInfo.InvariantCulture));
            }
        }
*/

        // button generate
        private void button1_Click(object sender, EventArgs e)
        {
            zedGraphControl1.GraphPane.CurveList.Clear();
            zedGraphControl1.Refresh();
            switch (choice_2D_plot)
            {
                case plot_2D.Uniform:
                    plotGraph2D_Uniform(number_of_sample);
                    break;

                case plot_2D.Geomtric:
                    plotGraph2D_Geometric(number_of_sample);
                    break;
                
                case plot_2D.Normal:
                    plotGraph2D_Normal(number_of_sample);
                    break;

                case plot_2D.Exponential:
                    plotGraph2D_Exponential(number_of_sample);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();

            }
            zedGraphRefresh();

        }

        // button Uniform Distribution
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            choice_2D_plot = plot_2D.Uniform;
        }

        // button Geometric Distribution
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            choice_2D_plot = plot_2D.Geomtric;
        }

        // button Normal Distribution
        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            choice_2D_plot = plot_2D.Normal;
        }

        // button Exponential Distribution
        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            choice_2D_plot = plot_2D.Exponential;
        }

        // number of sample time error 2D
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            number_of_sample = (uint)numericUpDown1.Value;
        }
    }
}
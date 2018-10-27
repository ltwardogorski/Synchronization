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
using AwokeKnowing.GnuplotCSharp;
using System.Threading;

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

        private plot_2D choice_2D_plot = plot_2D.Uniform;

        uint number_of_sample = 100;
        long move = 0;
        long move_tdev_in = 0;
        long move_tdev_out = 0;
        uint size = 3;
        uint segments = 5;
        uint tau_max = 100;
        uint N = 3;

        double[] uniform_table_of_Sample;

        public Form1()
        {
            // uniform_distribution(-10, 10, 100);     // wywołanie generatora o rozkładzie równomiernym i zapis do [table_of_sample]
            // geometric_distribution(0.1, 100);       // wywołanie generatora o rozkładzie geometrycznym i zapis do [table_of_sample]
            // normal_distribution(100, 10, 100);      // wywołanie generatora o rozkładzie normalnym i zais do [table_of_sample]
            // exponential_distribution(0.1, 100);     // wywołanie generatora o rozkładzie wykładnicznym i zapis do [table_of_sample]

            // print_test(uniform_distribution(-10, 10, 100));

            InitializeComponent();
            generate_table_sample(number_of_sample);
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
        private void zedGraphControl1_Load(object sender, EventArgs e)
        {
            SetSize1();
        }

        private void SetSize1()
        {
            zedGraphControl1.Location = new Point(0, 0);
            zedGraphControl1.IsShowPointValues = true;
            zedGraphControl1.Size = new Size(this.ClientRectangle.Width - 20, this.ClientRectangle.Height - 50);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            SetSize1();
        }

        private void zedGraphRefresh1()
        {
            zedGraphControl1.AxisChange();
            Refresh();
        }

        private void generate_table_sample(uint number_of_sample)
        {
            uniform_table_of_Sample = uniform_distribution(-1, 1, number_of_sample);
        }


        int number_of_clik_step = 0;
        bool line_in_start = false;
        bool new_segment = false;
        int temp_sample = 0;

        PointPairList uniformPairList = new PointPairList();
        PointPairList segment = new PointPairList();


        private void button1_Step()
        {
            plotGraph2D();
            number_of_clik_step++;
            temp_sample++;


            if (temp_sample <= 10)
                segment.Add(0, 0);

            if (temp_sample <= 20 && temp_sample > 10)
                segment.Add(10, 0);

            if (temp_sample <= 30 && temp_sample > 20)
                segment.Add(20, 0);

            if (temp_sample <= 40 && temp_sample > 30)
                segment.Add(30, 0);

            if (temp_sample <= 50 && temp_sample > 40)
                segment.Add(40, 0);

            if (temp_sample <= 60 && temp_sample > 50)
                segment.Add(50, 0);

            if (temp_sample <= 70 && temp_sample > 60)
                segment.Add(60, 0);

            if (temp_sample <= 80 && temp_sample > 70)
                segment.Add(70, 0);

            if (temp_sample <= 90 && temp_sample > 80)
                segment.Add(80, 0);

            if (temp_sample < 100 && temp_sample >= 90)
                segment.Add(90, 0);

           zedGraphControl1.GraphPane.AddCurve("Segment Limit", segment, Color.Blue, SymbolType.Circle);
        }

  

        private void plotGraph2D()
        {
            GraphPane myPane = zedGraphControl1.GraphPane;
            //myPane.CurveList.Clear();

            // Set the Titles
            myPane.Title.Text = "Time Error [ns]";
            myPane.XAxis.Title.Text = "Number of Sample";
            myPane.YAxis.Title.Text = "Value [ns]";

            ////////////////OPERATORY////////////////////////////////  ->kształty
            PointPairList operator_ADEV_PairList = new PointPairList();
            PointPairList operator_ADEV_PairList_n3 = new PointPairList();
/*            PointPairList operator_ADEV_PairList_n4 = new PointPairList();
            PointPairList operator_ADEV_PairList_n5 = new PointPairList();*/


            PointPairList operator_TDEV_internal_PairList = new PointPairList();
            PointPairList operator_TDEV_internal_PairList_n3 = new PointPairList();
            /*PointPairList operator_TDEV_internal_PairList_n4 = new PointPairList();
            PointPairList operator_TDEV_internal_PairList_n5 = new PointPairList();*/


            PointPairList operator_TDEV_external_PairList = new PointPairList();
/*            PointPairList operator_TDEV_external_PairList_n3 = new PointPairList();*/


            double[] operator_ADEV = {-1.5, -2, -1.5, -2, -1.5}; //n=2;
            double[] operator_ADEV_n3 = {-1.5, -2, -2, -1.5, -2, -2, -1.5}; //n=3
            /*double[] operator_ADEV_n4 = {-1.5, -2, -2, -2, -1.5, -2, -2, -2, -1.5}; //n=4
            double[] operator_ADEV_n5 = {-1.5, -2, -2, -2, -2, -1.5, -2, -2, -2, -2, -1.5}; //n=5*/


            double[] operator_TDEV_internal = {-2.5, -3, -2.5, -3, -2.5}; //n=2
            double[] operator_TDEV_internal_n3 = {-2.5, -3, -3, -2.5, -3, -3, -2.5}; //n=3
/*
            double[] operator_TDEV_internal_n4 = {-2.5, -3, -3, -3, -2.5, -3, -3, -3, -2.5}; //n=4
            double[] operator_TDEV_internal_n5 = {-2.5, -3, -3, -3, -3, -2.5, -3, -3, -3, -3, -2.5}; //n=5
*/


            double[] operator_TDEV_external = {-3.5, -4, -3.5,  -4, -3.5,  -4, -3.5};
//           double[] operator_TDEV_external_n3 = {-3.5, -4, -4, -4, -3.5, -4, -4, -4, -3.5, -4, -4, -4, -3.5};
            ////////////////OPERATORY////////////////////////////////  ->kształty


            for (int i = 0; i < operator_ADEV.Length; i++)
            {
                operator_ADEV_PairList.Add(i + move, operator_ADEV[i]);
                operator_TDEV_internal_PairList.Add(i + move_tdev_in, operator_TDEV_internal[i]);
            }

            for (var i = 0; i < operator_ADEV_n3.Length; i++)
            {
                operator_ADEV_PairList_n3.Add(i + move, operator_ADEV_n3[i]);
                operator_TDEV_internal_PairList_n3.Add(i + move_tdev_in, operator_TDEV_internal_n3[i]);
            }
/*

            for (var i = 0; i < operator_ADEV_n4.Length; i++)
            {
                operator_ADEV_PairList_n4.Add(i + move, operator_ADEV_n4[i]);
                operator_TDEV_internal_PairList_n4.Add(i + move, operator_TDEV_internal_n4[i]);
            }*/

/*            for (var i = 0; i < operator_ADEV_n5.Length; i++)
            {
                operator_ADEV_PairList_n5.Add(i + move, operator_ADEV_n5[i]);
                operator_TDEV_internal_PairList_n5.Add(i + move, operator_TDEV_internal_n5[i]);
            }*/

            for (var i = 0; i < operator_TDEV_external.Length; i++)
            {
                operator_TDEV_external_PairList.Add(i + move_tdev_out, operator_TDEV_external[i]);
            }

/*            for (var i = 0; i < operator_TDEV_external_n3.Length; i++)
            {
                operator_TDEV_external_PairList_n3.Add(i + move, operator_TDEV_external_n3[i]);
            }*/
            ////////////////////////OPERATOR////////////////////////////////////////

            if (number_of_clik_step < uniform_table_of_Sample.Length)
            {
                if (line_in_start == false)
                {
                    uniformPairList.Add(temp_sample,
                        uniform_table_of_Sample[temp_sample]); //drukowanie wykresu po step
                }
                else
                {
                    temp_sample--;
                }


                LineItem uniformCurve = myPane.AddCurve("Uniform Distribution",
                    uniformPairList, Color.Red, SymbolType.None);


                if (number_of_clik_step < 4)
                {
                    LineItem operatorCurve_ADEV =
                        myPane.AddCurve("ADEV", operator_ADEV_PairList, Color.Gold, SymbolType.None);
                }

                if (number_of_clik_step >= 4)
                {
                    LineItem operatorCurve_ADEV =
                        myPane.AddCurve("ADEV", operator_ADEV_PairList, Color.Gold, SymbolType.None);
                    move++;
                }




                if (number_of_clik_step < 4)
                {
                    LineItem operatorCurve_TDEV_IN =
                        myPane.AddCurve("TDEV IN", operator_TDEV_internal_PairList, Color.Black, SymbolType.None);
                }

                if (number_of_clik_step >= 4)
                {
                    LineItem operatorCurve_TDEV_IN =
                        myPane.AddCurve("TDEV IN", operator_TDEV_internal_PairList, Color.Black, SymbolType.None);
                    move_tdev_in++;
                }




                if (number_of_clik_step < 6)
                {
                    LineItem operatorCurve_TDEV_EX =
                        myPane.AddCurve("TDEV EX", operator_TDEV_external_PairList, Color.Green, SymbolType.None);
                }

                if (number_of_clik_step >= 6)
                {
                    LineItem operatorCurve_TDEV_EX =
                        myPane.AddCurve("TDEV EX", operator_TDEV_external_PairList, Color.Green, SymbolType.None);
                  
                }




                if (number_of_clik_step == 6)
                {
                    line_in_start = true;
                    zedGraphControl1.GraphPane.CurveList.Clear();
                    LineItem uni_2 = myPane.AddCurve("Uniform Distribution", uniformPairList, Color.Red,
                        SymbolType.None);
                    LineItem operatorCurve_ADEV_n3 =
                        myPane.AddCurve("ADEV", operator_ADEV_PairList, Color.Gold, SymbolType.None);
                    move=move-3;

                    LineItem operatorCurve_TDEV_IN =
                        myPane.AddCurve("TDEV IN", operator_TDEV_internal_PairList, Color.Black, SymbolType.None);
                    move_tdev_in=move_tdev_in-3;

                    LineItem operatorCurve_TDEV_EX =
                        myPane.AddCurve("TDEV EX", operator_TDEV_external_PairList, Color.Green, SymbolType.None);
                  

                }


                if (number_of_clik_step == 7)
                {
                    line_in_start = false;
                    zedGraphControl1.GraphPane.CurveList.Clear();
                    LineItem uni_2 = myPane.AddCurve("Uniform Distribution", uniformPairList, Color.Red,
                        SymbolType.None);
                    LineItem operatorCurve_ADEV_n3 =
                        myPane.AddCurve("ADEV", operator_ADEV_PairList_n3, Color.Gold, SymbolType.None);
                    move=move+2;

                    LineItem operatorCurve_TDEV_IN =
                        myPane.AddCurve("TDEV IN", operator_TDEV_internal_PairList_n3, Color.Black, SymbolType.None);
                    move_tdev_in = move_tdev_in + 2;

                    LineItem operatorCurve_TDEV_EX =
                        myPane.AddCurve("TDEV EX", operator_TDEV_external_PairList, Color.Green, SymbolType.None);
                    move_tdev_out++;
                }




                if (number_of_clik_step == 8)
                {
                    line_in_start = true;
                    zedGraphControl1.GraphPane.CurveList.Clear();
                    LineItem uni_2 = myPane.AddCurve("Uniform Distribution", uniformPairList, Color.Red,
                        SymbolType.None);
                    LineItem operatorCurve_ADEV_n3 =
                        myPane.AddCurve("ADEV", operator_ADEV_PairList, Color.Gold, SymbolType.None);
                    move--;

                    LineItem operatorCurve_TDEV_IN =
                        myPane.AddCurve("TDEV IN", operator_TDEV_internal_PairList, Color.Black, SymbolType.None);
                    move_tdev_in = move_tdev_in - 3;

                    LineItem operatorCurve_TDEV_EX =
                        myPane.AddCurve("TDEV EX", operator_TDEV_external_PairList, Color.Green, SymbolType.None);
  
                }


                if (number_of_clik_step == 9)
                {
                    line_in_start = true;
                    zedGraphControl1.GraphPane.CurveList.Clear();
                    LineItem uni_2 = myPane.AddCurve("Uniform Distribution", uniformPairList, Color.Red,
                        SymbolType.None);
                    LineItem operatorCurve_ADEV_n3 =
                        myPane.AddCurve("ADEV", operator_ADEV_PairList, Color.Gold, SymbolType.None);
                    move--;

                    LineItem operatorCurve_TDEV_IN =
                        myPane.AddCurve("TDEV IN", operator_TDEV_internal_PairList, Color.Black, SymbolType.None);

                    LineItem operatorCurve_TDEV_EX =
                        myPane.AddCurve("TDEV EX", operator_TDEV_external_PairList, Color.Green, SymbolType.None);
                }


                if (number_of_clik_step == 10)
                {
                    line_in_start = true;
                    zedGraphControl1.GraphPane.CurveList.Clear();
                    LineItem uni_2 = myPane.AddCurve("Uniform Distribution", uniformPairList, Color.Red,
                        SymbolType.None);
                    LineItem operatorCurve_ADEV_n3 =
                        myPane.AddCurve("ADEV", operator_ADEV_PairList, Color.Gold, SymbolType.None);
                    move--;
                    LineItem operatorCurve_TDEV_IN =
                        myPane.AddCurve("TDEV IN", operator_TDEV_internal_PairList, Color.Black, SymbolType.None);

                    LineItem operatorCurve_TDEV_EX =
                        myPane.AddCurve("TDEV EX", operator_TDEV_external_PairList, Color.Green, SymbolType.None);
                }


                if (number_of_clik_step == 11)
                {
                    line_in_start = true;
                    zedGraphControl1.GraphPane.CurveList.Clear();
                    LineItem uni_2 = myPane.AddCurve("Uniform Distribution", uniformPairList, Color.Red,
                        SymbolType.None);
                    LineItem operatorCurve_ADEV_n3 =
                        myPane.AddCurve("ADEV", operator_ADEV_PairList, Color.Gold, SymbolType.None);
                    move = move - 3;
                    LineItem operatorCurve_TDEV_IN =
                        myPane.AddCurve("TDEV IN", operator_TDEV_internal_PairList, Color.Black, SymbolType.None);
                    move_tdev_in = move_tdev_in - 3;

                    LineItem operatorCurve_TDEV_EX =
                        myPane.AddCurve("TDEV EX", operator_TDEV_external_PairList, Color.Green, SymbolType.None);

                }



                if (number_of_clik_step == 12)
                {
                    line_in_start = false;
                    zedGraphControl1.GraphPane.CurveList.Clear();
                    LineItem uni_2 = myPane.AddCurve("Uniform Distribution", uniformPairList, Color.Red,
                        SymbolType.None);
               
                    LineItem operatorCurve_ADEV_n3 =
                        myPane.AddCurve("ADEV", operator_ADEV_PairList_n3, Color.Gold, SymbolType.None);
                    move = move + 2;

                    LineItem operatorCurve_TDEV_IN =
                        myPane.AddCurve("TDEV IN", operator_TDEV_internal_PairList_n3, Color.Black, SymbolType.None);
                    move_tdev_in=move_tdev_in+2;

                    LineItem operatorCurve_TDEV_EX =
                        myPane.AddCurve("TDEV EX", operator_TDEV_external_PairList, Color.Green, SymbolType.None);
                    move_tdev_out++;
                }


                if (number_of_clik_step == 13)
                {
                    line_in_start = true;
                    zedGraphControl1.GraphPane.CurveList.Clear();
                    LineItem uni_2 = myPane.AddCurve("Uniform Distribution", uniformPairList, Color.Red,
                        SymbolType.None);
                    LineItem operatorCurve_ADEV_n3 =
                        myPane.AddCurve("ADEV", operator_ADEV_PairList, Color.Gold, SymbolType.None);
                    move--;
                    LineItem operatorCurve_TDEV_IN =
                        myPane.AddCurve("TDEV IN", operator_TDEV_internal_PairList, Color.Black, SymbolType.None);
                    move_tdev_in = move_tdev_in - 3;

                    LineItem operatorCurve_TDEV_EX =
                        myPane.AddCurve("TDEV EX", operator_TDEV_external_PairList, Color.Green, SymbolType.None);

                }


                if (number_of_clik_step == 14)
                {
                    line_in_start = true;
                    zedGraphControl1.GraphPane.CurveList.Clear();
                    LineItem uni_2 = myPane.AddCurve("Uniform Distribution", uniformPairList, Color.Red,
                        SymbolType.None);
                    LineItem operatorCurve_ADEV_n3 =
                        myPane.AddCurve("ADEV", operator_ADEV_PairList, Color.Gold, SymbolType.None);
                    move--;
                    LineItem operatorCurve_TDEV_IN =
                        myPane.AddCurve("TDEV IN", operator_TDEV_internal_PairList, Color.Black, SymbolType.None);

                    LineItem operatorCurve_TDEV_EX =
                        myPane.AddCurve("TDEV EX", operator_TDEV_external_PairList, Color.Green, SymbolType.None);
                }

                if (number_of_clik_step == 15)
                {
                    line_in_start = true;
                    zedGraphControl1.GraphPane.CurveList.Clear();
                    LineItem uni_2 = myPane.AddCurve("Uniform Distribution", uniformPairList, Color.Red,
                        SymbolType.None);
                    LineItem operatorCurve_ADEV_n3 =
                        myPane.AddCurve("ADEV", operator_ADEV_PairList, Color.Gold, SymbolType.None);
                    move--;
                    LineItem operatorCurve_TDEV_IN =
                        myPane.AddCurve("TDEV IN", operator_TDEV_internal_PairList, Color.Black, SymbolType.None);

                    LineItem operatorCurve_TDEV_EX =
                        myPane.AddCurve("TDEV EX", operator_TDEV_external_PairList, Color.Green, SymbolType.None);

                }


                if (number_of_clik_step == 16)
                {
                    line_in_start = true;
                    zedGraphControl1.GraphPane.CurveList.Clear();
                    LineItem uni_2 = myPane.AddCurve("Uniform Distribution", uniformPairList, Color.Red,
                        SymbolType.None);
                    LineItem operatorCurve_ADEV_n3 =
                        myPane.AddCurve("ADEV", operator_ADEV_PairList, Color.Gold, SymbolType.None);
                    move = move - 3;
                    LineItem operatorCurve_TDEV_IN =
                        myPane.AddCurve("TDEV IN", operator_TDEV_internal_PairList, Color.Black, SymbolType.None);
                    move_tdev_in = move_tdev_in - 3;

                    LineItem operatorCurve_TDEV_EX =
                        myPane.AddCurve("TDEV EX", operator_TDEV_external_PairList, Color.Green, SymbolType.None);
                }


                if (number_of_clik_step == 17)
                {
                    line_in_start = false;
                    zedGraphControl1.GraphPane.CurveList.Clear();
                    LineItem uni_2 = myPane.AddCurve("Uniform Distribution", uniformPairList, Color.Red,
                        SymbolType.None);

                    LineItem operatorCurve_ADEV_n3 =
                        myPane.AddCurve("ADEV", operator_ADEV_PairList_n3, Color.Gold, SymbolType.None);
                    move = move + 2;

                    LineItem operatorCurve_TDEV_IN =
                        myPane.AddCurve("TDEV IN", operator_TDEV_internal_PairList_n3, Color.Black, SymbolType.None);
                    move_tdev_in = move_tdev_in + 2;

                    LineItem operatorCurve_TDEV_EX =
                        myPane.AddCurve("TDEV EX", operator_TDEV_external_PairList, Color.Green, SymbolType.None);
                    move_tdev_out++;
                }


                if (number_of_clik_step == 18)
                {
                    line_in_start = true;
                    zedGraphControl1.GraphPane.CurveList.Clear();
                    LineItem uni_2 = myPane.AddCurve("Uniform Distribution", uniformPairList, Color.Red,
                        SymbolType.None);
                    LineItem operatorCurve_ADEV_n3 =
                        myPane.AddCurve("ADEV", operator_ADEV_PairList, Color.Gold, SymbolType.None);
                    move--;
                    LineItem operatorCurve_TDEV_IN =
                        myPane.AddCurve("TDEV IN", operator_TDEV_internal_PairList, Color.Black, SymbolType.None);
                    move_tdev_in = move_tdev_in - 3;

                    LineItem operatorCurve_TDEV_EX =
                        myPane.AddCurve("TDEV EX", operator_TDEV_external_PairList, Color.Green, SymbolType.None);

                }


                if (number_of_clik_step == 19)
                {
                    line_in_start = true;
                    zedGraphControl1.GraphPane.CurveList.Clear();
                    LineItem uni_2 = myPane.AddCurve("Uniform Distribution", uniformPairList, Color.Red,
                        SymbolType.None);
                    LineItem operatorCurve_ADEV_n3 =
                        myPane.AddCurve("ADEV", operator_ADEV_PairList, Color.Gold, SymbolType.None);
                    move--;
                    LineItem operatorCurve_TDEV_IN =
                        myPane.AddCurve("TDEV IN", operator_TDEV_internal_PairList, Color.Black, SymbolType.None);

                    LineItem operatorCurve_TDEV_EX =
                        myPane.AddCurve("TDEV EX", operator_TDEV_external_PairList, Color.Green, SymbolType.None);

                }

                if (number_of_clik_step == 20)
                {
                    line_in_start = true;
                    zedGraphControl1.GraphPane.CurveList.Clear();
                    LineItem uni_2 = myPane.AddCurve("Uniform Distribution", uniformPairList, Color.Red,
                        SymbolType.None);
                    LineItem operatorCurve_ADEV_n3 =
                        myPane.AddCurve("ADEV", operator_ADEV_PairList, Color.Gold, SymbolType.None);
                    move--;
                    LineItem operatorCurve_TDEV_IN =
                        myPane.AddCurve("TDEV IN", operator_TDEV_internal_PairList, Color.Black, SymbolType.None);

                    LineItem operatorCurve_TDEV_EX =
                        myPane.AddCurve("TDEV EX", operator_TDEV_external_PairList, Color.Green, SymbolType.None);

                }


                if (number_of_clik_step == 21)
                {
                    line_in_start = true;
                    zedGraphControl1.GraphPane.CurveList.Clear();
                    LineItem uni_2 = myPane.AddCurve("Uniform Distribution", uniformPairList, Color.Red,
                        SymbolType.None);
                    LineItem operatorCurve_ADEV_n3 =
                        myPane.AddCurve("ADEV", operator_ADEV_PairList, Color.Gold, SymbolType.None);
                    move = move - 3;
                    LineItem operatorCurve_TDEV_IN =
                        myPane.AddCurve("TDEV IN", operator_TDEV_internal_PairList, Color.Black, SymbolType.None);
                    move_tdev_in = move_tdev_in - 3;

                    LineItem operatorCurve_TDEV_EX =
                        myPane.AddCurve("TDEV EX", operator_TDEV_external_PairList, Color.Green, SymbolType.None);

                }

                if (number_of_clik_step == 22)
                {
                    line_in_start = false;
                    zedGraphControl1.GraphPane.CurveList.Clear();
                    LineItem uni_2 = myPane.AddCurve("Uniform Distribution", uniformPairList, Color.Red,
                        SymbolType.None);

                    LineItem operatorCurve_ADEV_n3 =
                        myPane.AddCurve("ADEV", operator_ADEV_PairList_n3, Color.Gold, SymbolType.None);
                    move = move + 2;

                    LineItem operatorCurve_TDEV_IN =
                        myPane.AddCurve("TDEV IN", operator_TDEV_internal_PairList_n3, Color.Black, SymbolType.None);


                    LineItem operatorCurve_TDEV_EX =
                        myPane.AddCurve("TDEV EX", operator_TDEV_external_PairList, Color.Green, SymbolType.None);
                    move_tdev_out++;
                }
                
                if (number_of_clik_step == 23)
                {
                    line_in_start = true;
                    zedGraphControl1.GraphPane.CurveList.Clear();
                    LineItem uni_2 = myPane.AddCurve("Uniform Distribution", uniformPairList, Color.Red,
                        SymbolType.None);
                    LineItem operatorCurve_ADEV_n3 =
                        myPane.AddCurve("ADEV", operator_ADEV_PairList, Color.Gold, SymbolType.None);
                    move--;
                    LineItem operatorCurve_TDEV_IN =
                        myPane.AddCurve("TDEV IN", operator_TDEV_internal_PairList, Color.Black, SymbolType.None);

                    LineItem operatorCurve_TDEV_EX =
                        myPane.AddCurve("TDEV EX", operator_TDEV_external_PairList, Color.Green, SymbolType.None);

                }

                if (number_of_clik_step == 24)
                {
                    line_in_start = true;
                    zedGraphControl1.GraphPane.CurveList.Clear();
                    LineItem uni_2 = myPane.AddCurve("Uniform Distribution", uniformPairList, Color.Red,
                        SymbolType.None);
                    LineItem operatorCurve_ADEV_n3 =
                        myPane.AddCurve("ADEV", operator_ADEV_PairList, Color.Gold, SymbolType.None);
                    move--;
                    LineItem operatorCurve_TDEV_IN =
                        myPane.AddCurve("TDEV IN", operator_TDEV_internal_PairList, Color.Black, SymbolType.None);

                    LineItem operatorCurve_TDEV_EX =
                        myPane.AddCurve("TDEV EX", operator_TDEV_external_PairList, Color.Green, SymbolType.None);

                }


                if (number_of_clik_step == 25)
                {
                    line_in_start = true;
                    zedGraphControl1.GraphPane.CurveList.Clear();
                    LineItem uni_2 = myPane.AddCurve("Uniform Distribution", uniformPairList, Color.Red,
                        SymbolType.None);
                    LineItem operatorCurve_ADEV_n3 =
                        myPane.AddCurve("ADEV", operator_ADEV_PairList, Color.Gold, SymbolType.None);
                    move = move - 3;
                    LineItem operatorCurve_TDEV_IN =
                        myPane.AddCurve("TDEV IN", operator_TDEV_internal_PairList, Color.Black, SymbolType.None);
                    move_tdev_in = move_tdev_in - 3;

                    LineItem operatorCurve_TDEV_EX =
                        myPane.AddCurve("TDEV EX", operator_TDEV_external_PairList, Color.Green, SymbolType.None);
                }




                if (number_of_clik_step == 26)
                {
                    line_in_start = true;
                    zedGraphControl1.GraphPane.CurveList.Clear();
                    LineItem uni_2 = myPane.AddCurve("Uniform Distribution", uniformPairList, Color.Red,
                        SymbolType.None);
                    LineItem operatorCurve_ADEV_n3 =
                        myPane.AddCurve("ADEV", operator_ADEV_PairList_n3, Color.Gold, SymbolType.None);

                    LineItem operatorCurve_TDEV_IN =
                        myPane.AddCurve("TDEV IN", operator_TDEV_internal_PairList_n3, Color.Black, SymbolType.None);
    

                    LineItem operatorCurve_TDEV_EX =
                        myPane.AddCurve("TDEV EX", operator_TDEV_external_PairList, Color.Green, SymbolType.None);


                    number_of_clik_step = 0;
                    new_segment = true;
                }

                if (number_of_clik_step < 4 && new_segment)
                {
                    line_in_start = false;
                    move = move + 5;
                    move_tdev_in = move_tdev_in + 5;
                    move_tdev_out = move_tdev_out + 6;
                    new_segment = false;
                }

            }
        }


        private void plotGraph2D_operator(uint size, long move)
        {
            GraphPane myPane = zedGraphControl1.GraphPane;

            PointPairList operator_ADEV_PairList = new PointPairList();
            PointPairList operator_TDEV_internal_PairList = new PointPairList();
            PointPairList operator_TDEV_external_PairList = new PointPairList();

            double[] operator_ADEV = {-1.5, -2, -2, -1.5, -2, -2, -1.5};
            double[] operator_TDEV_internal = {-2.5, -3, -3, -2.5, -3, -3, -2.5};
            double[] operator_TDEV_external = {-3.5, -4, -4, -3.5, -4, -4, -3.5};

            for (int i = 0; i < operator_ADEV.Length; i++)
            {
                operator_ADEV_PairList.Add(i + move, operator_ADEV[i]);
                operator_TDEV_internal_PairList.Add(i + move, operator_TDEV_internal[i]);
                operator_TDEV_external_PairList.Add(i + move, operator_TDEV_external[i]);
            }

            LineItem operatorCurve_ADEV = myPane.AddCurve("ADEV",
                operator_ADEV_PairList, Color.Gold, SymbolType.None);

            LineItem operatorCurve_TDEV_internal = myPane.AddCurve("TDEV_internal",
                operator_TDEV_internal_PairList, Color.Purple, SymbolType.None);

            LineItem operatorCurve_TDEV_exnternal = myPane.AddCurve("TDEV_exnternal",
                operator_TDEV_external_PairList, Color.Sienna, SymbolType.None);

            myPane.BarSettings.Base = BarBase.X;
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


        // button generate
        private void button1_Click(object sender, EventArgs e)
        {
            generate_table_sample(number_of_sample);

            zedGraphControl1.GraphPane.CurveList.Clear();
            switch (choice_2D_plot)
            {
                case plot_2D.Uniform:
                    button1_Step();
                    break;


                default:
                    throw new ArgumentOutOfRangeException();
            }

            zedGraphRefresh1();
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
            number_of_sample = (uint) numericUpDown1.Value;
        }

        // segments
        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            segments = (uint) numericUpDown2.Value;
        }


        private double plot3D_A(int tau_maximum, double[] samples_tab,
            int sampling_interval, int segments_num)
        {
            int tau_zero = tau_maximum / 10;
            //   int sampling_interval = tau_zero;
            int observe = tau_maximum / segments_num;

            switch (choice_2D_plot)
            {
                case plot_2D.Uniform:
                {
                    samples_tab = uniform_table_of_Sample;
                    break;
                }


                default:
                    throw new ArgumentOutOfRangeException();
            }


            //////////////////
            int number_of_elements = samples_tab.Length; //liczba próbek
            int gap = sampling_interval; //odstęp próbkowania tau

            double[] a_dev = new double[samples_tab.Length]; //tablica do przechowywania wartości dewiacji allana
            double temp = 0; //do przechowywania sumy kwadratów drugich różnic
            double[] square_sum = new double[samples_tab.Length];
            double fraction = 0;

            bool is_active = false; //flaga o aktywności operatora

            int current = 0; //aktualna próbka
            double sum = 0;
            double intervals = 2;
            int k;

            while (current < number_of_elements)
            {
                gap = 1;

                if (gap == 0)
                {
                    gap++;
                }

                while (current < 2 * gap)
                {
                    a_dev[current] = 0;
                    square_sum[current] = 0;
                    current++;
                }

                k = 0;

                while (2 * gap <= current)
                {
                    while (k < current - 2 * gap)
                    {
                        is_active = true; //spełniony warunek aktywacji operatora
                        sum = samples_tab[current] - 2 * samples_tab[current - gap] + samples_tab[current - 2 * gap];
                        temp += Math.Pow(sum, 2);
                        square_sum[current] = temp;

                        fraction = 1 / (2 * Math.Pow(intervals, 2) * Math.Pow(gap, 2) *
                                        (current - 2 * gap)); //ułamek ze wzoru

                        a_dev[current] = Math.Sqrt(fraction * (square_sum[current - 1] +
                                                               Math.Pow(
                                                                   samples_tab[current] -
                                                                   2 * samples_tab[current - gap] +
                                                                   samples_tab[current - 2 * gap], 2)));

                        k++;
                    }

                    gap++;
                }

                current++;
            }

            double[] x_axis = new double[10];

            double[] tab_1 = new double[10];
            double[] tab_2 = new double[10];
            double[] tab_3 = new double[10];
            double[] tab_4 = new double[10];
            double[] tab_5 = new double[10];
            double[] tab_6 = new double[10];
            double[] tab_7 = new double[10];
            double[] tab_8 = new double[10];
            double[] tab_9 = new double[10];
            double[] tab_10 = new double[10];

            int l = 0;
            while (l < a_dev.Length)
            {
                while (l < 10)
                {
                    tab_1[l] = a_dev[l] + l - 1;
                    l++;
                }

                while (l >= 10 && l < 20)
                {
                    for (var i = 0; i < 10; i++)
                    {
                        tab_2[i] = Math.Pow(a_dev[l], 2) + i - 1;
                        l++;
                    }
                }

                while (l >= 20 && l < 30)
                {
                    for (var i = 0; i < 10; i++)
                    {
                        tab_3[i] = Math.Pow(a_dev[l],3) - Math.Pow(a_dev[l],2) + 1 + i;
                        l++;
                    }
                }

                while (l >= 30 && l < 40)
                {
                    for (var i = 0; i < 10; i++)
                    {
                        tab_4[i] = Math.Pow(a_dev[l], 3) - Math.Pow(a_dev[l], 2)  + i;
                        l++;
                    }
                }

                while (l >= 40 && l < 50)
                {
                    for (var i = 0; i < 10; i++)
                    {
                        tab_5[i] =  Math.Pow(a_dev[l], 2) + 1 + i;
                        l++;
                    }
                }

                while (l >= 50 && l < 60)
                {
                    for (var i = 0; i < 10; i++)
                    {
                        tab_6[i] = Math.Pow(a_dev[l], 3) - Math.Pow(a_dev[l], 2) ;
                        l++;
                    }
                }

                while (l >= 60 && l < 70)
                {
                    for (var i = 0; i < 10; i++)
                    {
                        tab_7[i] = Math.Pow(a_dev[l], 3) - 1 + i;
                        l++;
                    }
                }

                while (l >= 70 && l < 80)
                {
                    for (var i = 0; i < 10; i++)
                    {
                        tab_8[i] = Math.Pow(a_dev[l], 2) + 1 + i;
                        l++;
                    }
                }

                while (l >= 80 && l < 90)
                {
                    for (var i = 0; i < 10; i++)
                    {
                        tab_9[i] = Math.Pow(a_dev[l], 3) - Math.Pow(a_dev[l], 2) + 1;
                        l++;
                    }
                }

                while (l >= 90 && l < 100)
                {
                    for (var i = 0; i < 10; i++)
                    {
                        tab_10[i] = Math.Pow(a_dev[l], 3) - Math.Pow(a_dev[l], 2) + 1 + i;
                        l++;
                    }
                }
            }

            double[] x_ax1 = new double[10];
            double[] x_ax2 = new double[10];
            double[] x_ax3 = new double[10];
            double[] x_ax4 = new double[10];
            double[] x_ax5 = new double[10];
            double[] x_ax6 = new double[10];
            double[] x_ax7 = new double[10];
            double[] x_ax8 = new double[10];
            double[] x_ax9 = new double[10];

            for (int i = 0; i < 10; i++)
            {
                x_ax1[i] = 1;
            }

            for (int i = 0; i < 10; i++)
            {
                x_ax2[i] = 2;
            }

            for (int i = 0; i < 10; i++)
            {
                x_ax3[i] = 3;
            }

            for (int i = 0; i < 10; i++)
            {
                x_ax4[i] = 4;
            }

            for (int i = 0; i < 10; i++)
            {
                x_ax5[i] = 5;
            }

            for (int i = 0; i < 10; i++)
            {
                x_ax6[i] = 6;
            }

            for (int i = 0; i < 10; i++)
            {
                x_ax7[i] = 7;
            }

            for (int i = 0; i < 10; i++)
            {
                x_ax8[i] = 8;
            }

            for (int i = 0; i < 10; i++)
            {
                x_ax9[i] = 9;
            }

            for (int i = 0; i < 10; i++)
            {
                x_axis[i] = 10;
            }

            ////////////////////////////////////////////////
            double[] y_axis = new double[samples_tab.Length];

            for (int j = 0; j < samples_tab.Length; j++)
            {
                y_axis[j] = j;
            }


            GnuPlot.Set("pm3d");
            GnuPlot.Set("autoscale");
            GnuPlot.Set("contour base");
            GnuPlot.HoldOn();

            GnuPlot.Set("isosamples 30", "hidden3d");
            GnuPlot.SPlot(x_ax1, y_axis, tab_1);
            GnuPlot.SPlot(x_ax2, y_axis, tab_2);
            GnuPlot.SPlot(x_ax3, y_axis, tab_3);
            GnuPlot.SPlot(x_ax4, y_axis, tab_4);
            GnuPlot.SPlot(x_ax5, y_axis, tab_5);
            GnuPlot.SPlot(x_ax6, y_axis, tab_6);
            GnuPlot.SPlot(x_ax7, y_axis, tab_7);
            GnuPlot.SPlot(x_ax8, y_axis, tab_8);
            GnuPlot.SPlot(x_ax9, y_axis, tab_9);
            GnuPlot.SPlot(x_axis, y_axis, tab_10);

            GnuPlot.HoldOff();

            return 0;
        }


        private double plot3D_T(int tau_maximum, double[] samples_tab,
            int sampling_interval, int segments_num)
        {
            int tau_zero = tau_maximum / 10;
            //   int sampling_interval = tau_zero;
            int observe = tau_maximum / segments_num;

            switch (choice_2D_plot)
            {
                case plot_2D.Uniform:
                {
                    samples_tab = uniform_table_of_Sample;
                    break;
                }


                default:
                    throw new ArgumentOutOfRangeException();
            }


            //////////////////
            int number_of_elements = samples_tab.Length; //liczba próbek
            int gap = sampling_interval; //odstęp próbkowania tau

            double[] a_dev = new double[samples_tab.Length]; //tablica do przechowywania wartości dewiacji allana
            double temp = 0; //do przechowywania sumy kwadratów drugich różnic
            double[] square_sum = new double[samples_tab.Length];
            double fraction = 0;

            bool is_active = false; //flaga o aktywności operatora

            int current = 0; //aktualna próbka
            double sum = 0;
            double intervals = 2;
            int k;

            while (current < number_of_elements)
            {
                gap = 1;

                if (gap == 0)
                {
                    gap++;
                }

                while (current < 2 * gap)
                {
                    a_dev[current] = 0;
                    square_sum[current] = 0;
                    current++;
                }

                k = 0;

                while (2 * gap <= current)
                {
                    while (k < current - 2 * gap)
                    {
                        is_active = true; //spełniony warunek aktywacji operatora
                        sum = samples_tab[current] - 2 * samples_tab[current - gap] + samples_tab[current - 2 * gap];
                        temp += Math.Pow(sum, 2);
                        square_sum[current] = temp;

                        fraction = 1 / (2 * Math.Pow(intervals, 2) * Math.Pow(gap, 2) *
                                        (current - 2 * gap)); //ułamek ze wzoru

                        a_dev[current] = Math.Sqrt(fraction * (square_sum[current - 1] +
                                                               Math.Pow(
                                                                   samples_tab[current] -
                                                                   2 * samples_tab[current - gap] +
                                                                   samples_tab[current - 2 * gap], 2)));

                        k++;
                    }

                    gap++;
                }

                current++;
            }

            double[] x_axis = new double[10];

            double[] tab_1 = new double[10];
            double[] tab_2 = new double[10];
            double[] tab_3 = new double[10];
            double[] tab_4 = new double[10];
            double[] tab_5 = new double[10];
            double[] tab_6 = new double[10];
            double[] tab_7 = new double[10];
            double[] tab_8 = new double[10];
            double[] tab_9 = new double[10];
            double[] tab_10 = new double[10];

            int l = 0;
            while (l < a_dev.Length)
            {
                while (l < 10)
                {
                    tab_1[l] = a_dev[l] + l - 1;
                    l++;
                }

                while (l >= 10 && l < 20)
                {
                    for (var i = 0; i < 10; i++)
                    {
                        tab_2[i] = Math.Pow(a_dev[l], 2) + i - 1;
                        l++;
                    }
                }

                while (l >= 20 && l < 30)
                {
                    for (var i = 0; i < 10; i++)
                    {
                        tab_3[i] = Math.Pow(a_dev[l], 3) - Math.Pow(a_dev[l], 2) + 1 + i;
                        l++;
                    }
                }

                while (l >= 30 && l < 40)
                {
                    for (var i = 0; i < 10; i++)
                    {
                        tab_4[i] = Math.Pow(a_dev[l], 3) - Math.Pow(a_dev[l], 2) + i;
                        l++;
                    }
                }

                while (l >= 40 && l < 50)
                {
                    for (var i = 0; i < 10; i++)
                    {
                        tab_5[i] = Math.Pow(a_dev[l], 2) + 1 + i;
                        l++;
                    }
                }

                while (l >= 50 && l < 60)
                {
                    for (var i = 0; i < 10; i++)
                    {
                        tab_6[i] = Math.Pow(a_dev[l], 3) - Math.Pow(a_dev[l], 2);
                        l++;
                    }
                }

                while (l >= 60 && l < 70)
                {
                    for (var i = 0; i < 10; i++)
                    {
                        tab_7[i] = Math.Pow(a_dev[l], 3) - 1 + i;
                        l++;
                    }
                }

                while (l >= 70 && l < 80)
                {
                    for (var i = 0; i < 10; i++)
                    {
                        tab_8[i] = Math.Pow(a_dev[l], 2) + 1 + i;
                        l++;
                    }
                }

                while (l >= 80 && l < 90)
                {
                    for (var i = 0; i < 10; i++)
                    {
                        tab_9[i] = Math.Pow(a_dev[l], 3) - Math.Pow(a_dev[l], 2) + 1;
                        l++;
                    }
                }

                while (l >= 90 && l < 100)
                {
                    for (var i = 0; i < 10; i++)
                    {
                        tab_10[i] = Math.Pow(a_dev[l], 3) - Math.Pow(a_dev[l], 2) + 1 + i;
                        l++;
                    }
                }
            }

            double[] x_ax1 = new double[10];
            double[] x_ax2 = new double[10];
            double[] x_ax3 = new double[10];
            double[] x_ax4 = new double[10];
            double[] x_ax5 = new double[10];
            double[] x_ax6 = new double[10];
            double[] x_ax7 = new double[10];
            double[] x_ax8 = new double[10];
            double[] x_ax9 = new double[10];

            for (int i = 0; i < 10; i++)
            {
                x_ax1[i] = 1;
            }

            for (int i = 0; i < 10; i++)
            {
                x_ax2[i] = 2;
            }

            for (int i = 0; i < 10; i++)
            {
                x_ax3[i] = 3;
            }

            for (int i = 0; i < 10; i++)
            {
                x_ax4[i] = 4;
            }

            for (int i = 0; i < 10; i++)
            {
                x_ax5[i] = 5;
            }

            for (int i = 0; i < 10; i++)
            {
                x_ax6[i] = 6;
            }

            for (int i = 0; i < 10; i++)
            {
                x_ax7[i] = 7;
            }

            for (int i = 0; i < 10; i++)
            {
                x_ax8[i] = 8;
            }

            for (int i = 0; i < 10; i++)
            {
                x_ax9[i] = 9;
            }

            for (int i = 0; i < 10; i++)
            {
                x_axis[i] = 10;
            }

            ////////////////////////////////////////////////
            double[] y_axis = new double[samples_tab.Length];

            for (int j = 0; j < samples_tab.Length; j++)
            {
                y_axis[j] = j;
            }


            GnuPlot.Set("pm3d");
            GnuPlot.Set("autoscale");
            GnuPlot.Set("contour base");
            GnuPlot.HoldOn();

            GnuPlot.Set("isosamples 30", "hidden3d");
            GnuPlot.SPlot(x_ax1, y_axis, tab_1);
            GnuPlot.SPlot(x_ax2, y_axis, tab_2);
            GnuPlot.SPlot(x_ax3, y_axis, tab_3);
            GnuPlot.SPlot(x_ax4, y_axis, tab_4);
            GnuPlot.SPlot(x_ax5, y_axis, tab_5);
            GnuPlot.SPlot(x_ax6, y_axis, tab_6);
            GnuPlot.SPlot(x_ax7, y_axis, tab_7);
            GnuPlot.SPlot(x_ax8, y_axis, tab_8);
            GnuPlot.SPlot(x_ax9, y_axis, tab_9);
            GnuPlot.SPlot(x_axis, y_axis, tab_10);

            GnuPlot.HoldOff();

            return 0;
        }


        // DTDEV
        private void button5_Click(object sender, EventArgs e)
        {
            switch (choice_2D_plot)
            {
                case plot_2D.Uniform:
                    plot3D_T(100, uniform_table_of_Sample, 1, 10);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        // DADEV 
        private void button6_Click(object sender, EventArgs e)
        {
            switch (choice_2D_plot)
            {
                case plot_2D.Uniform:
                    plot3D_A(100, uniform_table_of_Sample, 1, 10);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        // close
        private void button7_Click(object sender, EventArgs e)
        {
            GnuPlot.Close();
        }

        private void zedGraphControl1_Load_1(object sender, EventArgs e)
        {
        }
    }
}
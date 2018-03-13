using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SharpSprint.Elements;
using SharpSprint.IO;
using SharpSprint.Primitives;
using SharpSprint;
using SharpSprint.Points;
using SharpSprint.Plot;

namespace SprintTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string input = Properties.Resources.TestSuite;

            TokenRow[] testRows;
            if (Parser.Tokenize(input, out testRows) == 0)
            {
                uint pointer;
                Board test = new Board();

                if((pointer = test.Read(testRows)) != 0)
                    MessageBox.Show(string.Format("Error on statement {0}!", pointer));
                else
                    MessageBox.Show(string.Format("Successfully read {0} entities!", test.Count));

                string testout;
                if (test.Write(out testout))
                    MessageBox.Show(string.Format("Successfully wrote {0} characters!", testout.Length));
                else
                    MessageBox.Show("Error compiling the entities again!");
            }

            Board testBoard = new Board();
            
            Group G1 = new Group();
            Circle C1 = new Circle(Layer.CopperTop, Distance.FromMillimeters(1),
                Vector.FromMillimeters(10, 10), Distance.FromMillimeters(20));
            Circle C2 = new Circle(Layer.CopperTop, Distance.FromMillimeters(3),
                Vector.FromMillimeters(15, 20), Distance.FromMillimeters(15));

            C2.Fill = true;
            C2.Center.X.Millimeters += 5;
            C2.Radius *= 2;

            G1.Entities.Add(C1);
            G1.Entities.Add(C2);

            var cmp = new SharpSprint.Elements.Component(
                new IDText(Layer.CopperTop, Vector.FromMillimeters(0, 0), "", Distance.FromMillimeters(0)),
                new ValueText(Layer.CopperTop, Vector.FromMillimeters(0, 0), "", Distance.FromMillimeters(0)), G1);

            testBoard.Add(cmp);

            string output;

            if (testBoard.Write(out output))
                textBox1.Text = output;
            else
                textBox1.Text = "Compile FAIL!";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // CIRCLE,LAYER=3,CENTER=88900/533400,RADIUS=50800,WIDTH=20000;

            TokenRow[] testRows;
            if (Parser.Tokenize("CIRCLE,LAYER=3,CENTER=152400/139700,RADIUS=101600,WIDTH=5000,START=600,STOP=210000,FILL=false;", out testRows) == 0)
            {
                Circle testCircle;
                uint ptr = 0;

                if (Circle.Read(testRows, ref ptr, out testCircle))
                {
                    Bitmap test;
                    Rectangle box;
                    if(Plotter.Draw(testCircle, new ElementStyle(), 10, out test, out box))
                        pictureBox1.Image = test;
                    else
                        MessageBox.Show("Plot Fail!"); ;
                }
                else
                    MessageBox.Show("Read Fail!");
            }
        }
    }
}

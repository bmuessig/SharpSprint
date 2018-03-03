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
            TokenRow[] testRows;
            if (Parser.Tokenize("ZONE, LAYER=3, WIDTH=8000, HATCH=true, P0=150000 / 250000, P1=150000 / 450000, P2=250000 / 450000, P3=250000 / 650000;", out testRows) == 0)
            {
                Zone testZone;
                uint ptr = 0;

                Zone.Read(testRows, ref ptr, out testZone);
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

            testBoard.Canvas.Add(cmp);

            string output;

            if (testBoard.Compile(out output))
                textBox1.Text = output;
            else
                textBox1.Text = "Compile FAIL!";

            /*Token[][] lines;
            ushort indent = 0;
            string output;

            if (cmp.Write(out lines))
            {
                if (Compiler.CompileBlock(lines, ref indent, out output))
                    textBox1.Text = output;
                else
                    textBox1.Text = "Compile FAIL!";
            }
            else
                textBox1.Text = "Write FAIL!";*/
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}

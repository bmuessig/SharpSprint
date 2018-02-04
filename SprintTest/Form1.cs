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
            Group Test = new Group();
            Circle C1 = new Circle(SharpSprint.Layer.CopperTop, Distance.FromMillimeters(1),
                Position.FromMillimeters(10, 10), Distance.FromMillimeters(20));
            Circle C2 = new Circle(SharpSprint.Layer.CopperTop, Distance.FromMillimeters(3),
                Position.FromMillimeters(15, 20), Distance.FromMillimeters(30));

            C2.Fill = true;
            C2.Center.X.Millimeters += 5;

            Test.Entities.Add(C1);
            Test.Entities.Add(C2);

            var cmp = new SharpSprint.Elements.Component(
                new IDText(SharpSprint.Layer.CopperTop, Position.FromMillimeters(0,0), "", Distance.FromMillimeters(0)),
                new ValueText(SharpSprint.Layer.CopperTop, Position.FromMillimeters(0, 0), "", Distance.FromMillimeters(0)), Test);

            Token[][] lines;
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
                textBox1.Text = "Write FAIL!";
        }
    }
}

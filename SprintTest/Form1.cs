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
            Circle C1 = new Circle(SharpSprint.Layer.CopperTop, SharpSprint.Primitives.Size.FromMillimeters(1),
                SharpSprint.Primitives.Position.FromMillimeters(10, 10), SharpSprint.Primitives.Size.FromMillimeters(20));
            Circle C2 = new Circle(SharpSprint.Layer.CopperTop, SharpSprint.Primitives.Size.FromMillimeters(3),
                SharpSprint.Primitives.Position.FromMillimeters(15, 20), SharpSprint.Primitives.Size.FromMillimeters(30));

            C2.Fill = true;
            C2.Center.X.Millimeters += 5;

            Test.Entities.Add(C1);
            Test.Entities.Add(C2);

            Token[][] lines;
            ushort indent = 0;
            string output;

            if (Test.Write(out lines))
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

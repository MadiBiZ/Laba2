using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using OpenTK;
using OpenTK.Graphics.OpenGL;
//using OpenTK.Mathematics;

namespace Third
{
    public partial class Form1 : Form
    {
        private View Er = new View();
        public Form1()
        {
            InitializeComponent();
            

        }
        

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void glControl1_Load(object sender, EventArgs e)
        {
            

            Er.Shaders();

                }



        private void glControl1_Paint(object sender, PaintEventArgs e)
        {

            Er.InitBuffers();
        }
    }
}

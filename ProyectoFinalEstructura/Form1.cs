using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoFinalEstructura
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnArbo_Click(object sender, EventArgs e)
        {
            this.Hide();

            FormArbol formArbol = new FormArbol();
            formArbol.ShowDialog();
        }

        private void btnGrafo_Click(object sender, EventArgs e)
        {
            this.Hide();

            formGrafo formGrafo = new formGrafo();
            formGrafo.ShowDialog();
        }
    }
}

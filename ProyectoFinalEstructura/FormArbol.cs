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
    public partial class FormArbol : Form
    {

        private Nodo raiz = null;
        public FormArbol()
        {
            InitializeComponent();
        }

        private void AgregarNodo(ref Nodo r, int d)
        {
            if (r == null)
            {
                CrearNodo(ref r, d);
            }
            else
            {
                Nodo t1 = r, t2 = r, t = null;

                while (t1 != null)
                {
                    t2 = t1;
                    if (d < t2.dato)
                        t1 = t2.izquierda;
                    else
                        t1 = t2.derecha;
                }

                CrearNodo(ref t, d);

                if (d < t2.dato)
                    t2.izquierda = t;
                else
                    t2.derecha = t;
            }
        }

        private void CrearNodo(ref Nodo q, int d)
        {
            q = new Nodo(d, null, null);
            q.izquierda = null;
            q.derecha = null;
        }

        private void DibujarArbol(Nodo t, int x, int y, int distanciaX, Graphics g)
        {
            if (t != null)
            {
                g.FillEllipse(Brushes.LightBlue, x - 25, y - 25, 50, 50);
                g.DrawEllipse(Pens.Black, x - 25, y - 25, 50, 50);
                g.DrawString(t.dato.ToString(), Font, Brushes.Black, x - 10, y - 10);

                int distanciaY = 50;

                if (t.izquierda != null)
                {
                    g.DrawLine(Pens.Black, x, y, x - distanciaX, y + distanciaY);
                    DibujarArbol(t.izquierda, x - distanciaX, y + distanciaY, distanciaX / 2, g);
                }

                if (t.derecha != null)
                {
                    g.DrawLine(Pens.Black, x, y, x + distanciaX, y + distanciaY);
                    DibujarArbol(t.derecha, x + distanciaX, y + distanciaY, distanciaX / 2, g);
                }
            }

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            int distanciaXInicial = 100; // Puedes ajustar este valor según sea necesario
            DibujarArbol(raiz, panel1.Width / 2, 50, distanciaXInicial, e.Graphics);

        }

        private void FormArbol_Load(object sender, EventArgs e)
        {

        }


        public class Nodo
        {
            public int dato;
            public Nodo izquierda;
            public Nodo derecha;

            public Nodo(int dato, Nodo izq, Nodo der)
            {
                this.dato = dato;
                izquierda = izq;
                derecha = der;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            int dato;
            if (int.TryParse(txtDato.Text, out dato))
            {
                AgregarNodo(ref raiz, dato);
                panel1.Invalidate();
            }
            else
            {
                MessageBox.Show("Ingrese un número válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            txtDato.Clear();
        }

        private void EliminarArbol(Nodo nodo)
        {
            if (nodo != null)
            {
                EliminarArbol(nodo.izquierda);  // Eliminar subárbol izquierdo
                EliminarArbol(nodo.derecha);    // Eliminar subárbol derecho

                // Eliminar el nodo actual
                nodo = null;
            }
        }






        private void btnEliminarUltimo_Click(object sender, EventArgs e)
        {
            EliminarArbol(raiz);
            raiz = null;  // Asegurarse de que la raíz también se establezca en null
            panel1.Invalidate();
        }


        private int ContarHojas(Nodo nodo)
        {
            if (nodo == null)
            {
                return 0;
            }
            else if (nodo.izquierda == null && nodo.derecha == null)
            {
                // El nodo es una hoja
                return 1;
            }
            else
            {
                // El nodo tiene hijos, contamos las hojas en los subárboles izquierdo y derecho
                return ContarHojas(nodo.izquierda) + ContarHojas(nodo.derecha);
            }
        }

        private void btnContaHojas_Click(object sender, EventArgs e)
        {
            int cantidadHojas = ContarHojas(raiz);
            MessageBox.Show($"El árbol tiene {cantidadHojas} hojas.", "Cantidad de Hojas", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        private bool BuscarNodo(Nodo nodo, int valor)
        {
            if (nodo == null)
            {
                return false;
            }

            if (nodo.dato == valor)
            {
                return true;
            }

            // Busca en el subárbol izquierdo si el valor es menor, de lo contrario, busca en el subárbol derecho
            if (valor < nodo.dato)
            {
                return BuscarNodo(nodo.izquierda, valor);
            }
            else
            {
                return BuscarNodo(nodo.derecha, valor);
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            int valorABuscar;

            if (int.TryParse(txtDato.Text, out valorABuscar))
            {
                bool nodoEncontrado = BuscarNodo(raiz, valorABuscar);

                if (nodoEncontrado)
                {
                    MessageBox.Show($"El nodo con el valor {valorABuscar} se encuentra en el árbol.", "Nodo Encontrado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show($"No se encontró un nodo con el valor {valorABuscar} en el árbol.", "Nodo no Encontrado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Ingrese un número válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtDato_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();

            // Mostrar la instancia del formulario principal
            Form1 formularioPrincipal = new Form1();
            formularioPrincipal.Show();
        }
    }
}

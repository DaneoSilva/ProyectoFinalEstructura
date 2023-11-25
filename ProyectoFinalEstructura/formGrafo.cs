using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;



namespace ProyectoFinalEstructura
{
    public partial class formGrafo : Form
    {
        private Dictionary<string, Point> nodos = new Dictionary<string, Point>();
        private List<Tuple<string, string>> aristas = new List<Tuple<string, string>>();

        public formGrafo()
        {
            InitializeComponent();

            // Crear el grafo dirigido
            //AgregarArista("A", "B");
            //AgregarArista("B", "C");
            //AgregarArista("C", "A");
            //AgregarArista("C", "D");
            //AgregarArista("D", "D"); // Nodo que apunta a sí mismo


            // Mostrar el grafo
            MostrarGrafo();
        }

        private void AgregarArista(string origen, string destino)
        {
            aristas.Add(Tuple.Create(origen, destino));
            if (!nodos.ContainsKey(origen))
                nodos[origen] = Point.Empty;
            if (!nodos.ContainsKey(destino))
                nodos[destino] = Point.Empty;
        }

        private void MostrarGrafo()
        {
            // Configurar el PictureBox
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g = Graphics.FromImage(pictureBox1.Image);
            g.Clear(Color.White);

            // Configurar posición de los nodos
            nodos["A"] = new Point(100, 100);
            nodos["B"] = new Point(250, 100);
            nodos["C"] = new Point(175, 250);
            nodos["D"] = new Point(300, 250);


            // Dibujar aristas con flechas
            foreach (var arista in aristas)
            {
                Point inicio = nodos[arista.Item1];
                Point fin = nodos[arista.Item2];

                if (inicio != fin)
                {
                    // Calcular la posición de la punta de la flecha
                    double angle = Math.Atan2(fin.Y - inicio.Y, fin.X - inicio.X);
                    double arrowSize = 15;
                    PointF arrowPoint = new PointF((float)(fin.X - arrowSize * Math.Cos(angle)),
                                                  (float)(fin.Y - arrowSize * Math.Sin(angle)));

                    // Dibujar la línea
                    g.DrawLine(Pens.Black, inicio, fin);

                    // Dibujar la flecha
                    AdjustableArrowCap arrowCap = new AdjustableArrowCap(5, 5);
                    Pen arrowPen = new Pen(Color.Black);
                    arrowPen.CustomEndCap = arrowCap;
                    g.DrawLine(arrowPen, inicio, arrowPoint);
                }
                else
                {
                    // Dibujar bucle cuando el nodo apunta a sí mismo
                    int radio = 20;
                    RectangleF rect = new RectangleF(inicio.X - radio, inicio.Y - radio, 2 * radio, 2 * radio);
                    float startAngle = 180;
                    float sweepAngle = 180;
                    g.DrawArc(Pens.Black, rect, startAngle, sweepAngle);
                }
            }

            // Dibujar nodos
            foreach (var nodo in nodos)
            {
                int radio = 15; // Tamaño del radio del nodo
                Brush colorNodo = Brushes.Green; // Color del nodo

                g.FillEllipse(colorNodo, nodo.Value.X - radio, nodo.Value.Y - radio, 2 * radio, 2 * radio);
                g.DrawString(nodo.Key, DefaultFont, Brushes.Black, nodo.Value.X - 5, nodo.Value.Y - 5);
            }



            // Actualizar el PictureBox
            pictureBox1.Invalidate();
        }

        

        private void btnAgregarArista_Click_1(object sender, EventArgs e)
        {
            // Obtener los valores del TextBox para el origen y destino
            string origen = txtOrigen.Text.ToUpper(); // Convertir a mayúsculas para manejar entradas en minúsculas
            string destino = txtDestino.Text.ToUpper();

            // Validar que los valores ingresados sean A, B, C o D
            if (EsLetraPermitida(origen) && EsLetraPermitida(destino))
            {
                // Agregar la arista y mostrar el grafo
                AgregarArista(origen, destino);
                MostrarGrafo();
            }
            else
            {
                MessageBox.Show("Ingrese valores válidos para el origen y destino (A, B, C o D).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            txtOrigen.Clear();
            txtDestino.Clear();
        }

        private bool EsLetraPermitida(string letra)
        {
            // Lista de letras permitidas
            List<string> letrasPermitidas = new List<string> { "A", "B", "C", "D" };

            // Verificar si la letra está en la lista de letras permitidas
            return letrasPermitidas.Contains(letra);
        }

        private void EliminarArista()
        {
            aristas = null;
            aristas = new List<Tuple<string, string>>();
            MostrarGrafo();
        }



        private void button1_Click(object sender, EventArgs e)
        {
            EliminarArista();


        }

        private List<string> ObtenerNodosDestino(string nodoOrigen)
        {
            List<string> nodosDestino = new List<string>();

            foreach (var arista in aristas)
            {
                if (arista.Item1 == nodoOrigen)
                {
                    nodosDestino.Add(arista.Item2);
                }
            }

            return nodosDestino;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Obtener el nodo ingresado por el usuario
            string nodoOrigen = txtNodoOrigen.Text.ToUpper(); // Convertir a mayúsculas para manejar entradas en minúsculas

            // Validar que el valor ingresado sea A, B, C o D
            if (EsLetraPermitida(nodoOrigen))
            {
                // Obtener los nodos de destino
                List<string> nodosDestino = ObtenerNodosDestino(nodoOrigen);

                // Mostrar el mensaje según si hay nodos de destino o no
                if (nodosDestino.Count > 0)
                {
                    string mensaje = $"El nodo {nodoOrigen} apunta a los nodos: {string.Join(", ", nodosDestino)}";
                    MessageBox.Show(mensaje, "Nodos de Destino", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show($"El nodo {nodoOrigen} no apunta a ningún otro nodo.", "Sin Nodos de Destino", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Ingrese un valor válido para el nodo de origen (A, B, C o D).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            txtNodoOrigen.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();

            // Mostrar la instancia del formulario principal
            Form1 formularioPrincipal = new Form1();
            formularioPrincipal.Show();
        }

        
    }
}

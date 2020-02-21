using BusinessLogicLayer;
using BusinessLogicLayer.Models;
using ChromaticNumber.Enums;
using ChromaticNumber.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ChromaticNumber
{
    public partial class Form1 : Form
    {
        A1 _a1 = new A1();
        List<Point> vertices = new List<Point>();
        List<int> _edge = new List<int>();
        int[,] _adjacencyMatrix = new int[0,0];
        Events _event;
        List<Brush> _brushes = new List<Brush>();
        ResultModel _result;

        public Form1()
        {        
            InitializeComponent();
            _brushes.Add(Brushes.Blue);
            _brushes.Add(Brushes.Yellow);
            _brushes.Add(Brushes.Red);
            _brushes.Add(Brushes.Green);
            _brushes.Add(Brushes.Gray);
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void ViewPort_MouseClick(object sender, MouseEventArgs e)
        {
            if (_event == Enums.Events.CreateVertex)
            {
                vertices.Add(new Point(e.X, e.Y));
                _adjacencyMatrix = UpdateAdjacencyMatrix.AddVertex(_adjacencyMatrix, vertices.Count - 1);
                ShowAdjacencyMatrix();
            }

            if (_event == Enums.Events.AddEdge)
            {
                var index = VertexLocation(e.X, e.Y);
                if (index != null)
                {
                    _edge.Add((int)index);
                    AddEdge();
                    ShowAdjacencyMatrix();                  
                }                    
            }
            if (_event == Enums.Events.DeleteVertex)
            {
                var index = VertexLocation(e.X, e.Y);
                if(index != null)
                {
                    vertices.RemoveAt((int)index);
                    _adjacencyMatrix = UpdateAdjacencyMatrix.RemoveVertex(_adjacencyMatrix, (int)index);
                    ShowAdjacencyMatrix();
                }                
            }
            draw();
        }

        private void AddEdge()
        {
            if(_edge.Count != 2)
            {
                return;
            }
            //Добавление связей в матрицу смежности
            _adjacencyMatrix[_edge[0], _edge[1]] = 1;
            _adjacencyMatrix[_edge[1], _edge[0]] = 1;
            _edge.Clear();
        }

        public int? VertexLocation(int x, int y)
        {
            double distance;
            for (int i = 0;  i < vertices.Count; ++i)
            {                
                distance = Math.Sqrt(Math.Pow((vertices[i].X - x), 2) + Math.Pow((vertices[i].Y - y), 2));
                if (distance <= 8)
                {
                    return i;
                }
            }
            return null;
        }

        private void draw(bool painting = false)
        {
            var bitmap = new Bitmap(viewPort.Width, viewPort.Height);
            var graphics = Graphics.FromImage(bitmap);
            var pen = new Pen(Color.Blue);
            int vertexCount = 1;
          
            for(int i = 0; i < _adjacencyMatrix.GetLength(0); ++i)
            {
                for (int j = 0; j < _adjacencyMatrix.GetLength(0); ++j)
                {
                    if (_adjacencyMatrix[i, j] == 1)
                    {
                        graphics.DrawLine(pen, vertices[i], vertices[j]);
                    }
                }
            }

            foreach (var vertex in vertices)
            {
                if (!painting)
                {
                    graphics.FillEllipse(Brushes.White, vertex.X - 8, vertex.Y - 8, 16, 16);
                }
                if(painting)
                {
                    graphics.FillEllipse(_brushes[(int)_result.Vertices[vertexCount - 1].ColorIndex], vertex.X - 8, vertex.Y - 8, 16, 16);
                }
                graphics.DrawEllipse(pen, vertex.X - 8, vertex.Y - 8, 16, 16);
                graphics.DrawString(vertexCount.ToString(), new Font("Cambria", 12), Brushes.Black, vertex.X - 7, vertex.Y - 10);
                ++vertexCount;
            }           
            viewPort.Image = bitmap;
        }

        private void ShowAdjacencyMatrix()
        {
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();
            for (int i = 0; i < _adjacencyMatrix.GetLength(1); ++i)
            {
                dataGridView1.Columns.Add(i.ToString(), (i + 1).ToString());
            }
            for (int i = 0; i < _adjacencyMatrix.GetLength(0); ++i)
            {
                string[] row = new string[_adjacencyMatrix.GetLength(1)];
                for (int j = 0; j < _adjacencyMatrix.GetLength(1); ++j)
                {
                    row[j] = _adjacencyMatrix[i, j].ToString();
                }
                dataGridView1.Rows.Add(row);
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            _event = Enums.Events.CreateVertex;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            _event = Enums.Events.AddEdge;
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            _event = Enums.Events.DeleteVertex;
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            if(_adjacencyMatrix.Length == 0)
            {
                label1.Text = "0";
                return;
            }
            _result = _a1.Alhoritm(_adjacencyMatrix);
            label1.Text = string.Format("Хроматичне число = {0}", _result.ChromaticNumber);
            if (_result != null)
            {
                draw(true);
            }
        }

        private void Painting_Click(object sender, EventArgs e)
        {
            
        }

        private void ВідкритиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string filename = string.Empty;
            OpenFileDialog openFileDialog1 = new OpenFileDialog() { Filter = "Текстові файли(*.txt)|*.txt" };
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                filename = openFileDialog1.FileName;
            }
            if (string.IsNullOrEmpty(filename))
            {
                return;
            }
            vertices.Clear();
            string[] file = File.ReadAllLines(filename);
            int index = 0;
            
            for (int i = 0; i < file.Length; ++i)
            {
                if(file[i] == string.Empty)
                {
                    continue;
                }
                if (file[i] == "Adjacency Matrix")
                {
                    index = i + 1;
                    break;
                }
                var coordinates = file[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(n => int.Parse(n)).ToArray();
                vertices.Add(new Point(coordinates[0], coordinates[1]));
            }
            _adjacencyMatrix = new int[vertices.Count, vertices.Count];
            for (int i = index; i < file.Length; ++i)
            {
                if (file[i] == string.Empty)
                {
                    continue;
                }
                var row = file[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(n => int.Parse(n)).ToArray();
                for (int j = 0; j < row.Length; ++j)
                {
                    _adjacencyMatrix[i - index, j] = row[j];
                }
            }
            ShowAdjacencyMatrix();
            draw();
        }

        private void ЗберегтиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string filename = string.Empty;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog() { Filter = "Текстові файли(*.txt)|*.txt" };
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                filename = saveFileDialog1.FileName;
            }
            if (string.IsNullOrEmpty(filename))
            {
                return;
            }

            using (StreamWriter sw = new StreamWriter(filename))
            {
                foreach (var vertex in vertices)
                {
                    sw.WriteLine("{0} {1}", vertex.X, vertex.Y);
                }
                sw.WriteLine("Adjacency Matrix");
                for (int i = 0; i < _adjacencyMatrix.GetLength(0); ++i)
                {
                    for(int j = 0; j < _adjacencyMatrix.GetLength(1); ++j)
                    {
                        sw.Write(_adjacencyMatrix[i, j] + " ");
                    }
                    sw.WriteLine();
                }
            }
        }
    }
}

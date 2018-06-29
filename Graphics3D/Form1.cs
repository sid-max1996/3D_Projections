using System.Windows.Forms;
using System;
using Graphics3D.Geometry;
using System.Collections.Generic;
using System.IO;

namespace Graphics3D
{
    public partial class Form1 : Form
    {
        private Mesh CurrentMesh
        {
            get
            {
                return sceneView4.Mesh;
            }
            set
            {
                sceneView1.Mesh = value;
                sceneView2.Mesh = value;
                sceneView3.Mesh = value;
                sceneView4.Mesh = value;
                RefreshScenes();
            }
        }

        private Camera camera;

        public Form1()
        {
            InitializeComponent();
            CurrentMesh = new Tetrahedron(0.5);
            sceneView1.ViewCamera = new Camera(new Vertex(0, 0, 0), 0, 0,
                Transformations.OrthogonalProjection());
            sceneView2.ViewCamera = new Camera(new Vertex(0, 0, 0), 0, 0,
                Transformations.RotateY(-Math.PI / 2)
                * Transformations.OrthogonalProjection());
            sceneView3.ViewCamera = new Camera(new Vertex(0, 0, 0), 0, 0,
                Transformations.RotateX(Math.PI / 2)
                * Transformations.OrthogonalProjection());
            camera = new Camera(new Vertex(0, 0, 0), Math.PI / 4, -Math.PI / 4,
                        Transformations.OrthogonalProjection());
            sceneView4.ViewCamera = camera;
        }

        private static double DegToRad(double deg)
        {
            return deg / 180 * Math.PI;
        }

        private void RefreshScenes()
        {
            sceneView1.Refresh();
            sceneView2.Refresh();
            sceneView3.Refresh();
            sceneView4.Refresh();
        }

        private void Scale(object sender, EventArgs e)
        {
            double scalingX = (double)numericUpDown1.Value;
            double scalingY = (double)numericUpDown2.Value;
            double scalingZ = (double)numericUpDown3.Value;
            CurrentMesh.Apply(Transformations.Scale(scalingX, scalingY, scalingZ));
            RefreshScenes();
        }

        private void Rotate(object sender, EventArgs e)
        {
            double rotatingX = DegToRad((double)numericUpDown4.Value);
            double rotatingY = DegToRad((double)numericUpDown5.Value);
            double rotatingZ = DegToRad((double)numericUpDown6.Value);
            CurrentMesh.Apply(Transformations.RotateX(rotatingX)
                * Transformations.RotateY(rotatingY)
                * Transformations.RotateZ(rotatingZ));
            RefreshScenes();
        }

        private void Translate(object sender, EventArgs e)
        {
            double translatingX = (double)numericUpDown7.Value;
            double translatingY = (double)numericUpDown8.Value;
            double translatingZ = (double)numericUpDown9.Value;
            CurrentMesh.Apply(Transformations.Translate(translatingX, translatingY, translatingZ));
            RefreshScenes();
        }

        private void Reflect(object sender, EventArgs e)
        {
            Matrix reflection;
            if (radioButton1.Checked)
                reflection = Transformations.ReflectX();
            else if (radioButton2.Checked)
                reflection = Transformations.ReflectY();
            else if (radioButton3.Checked)
                reflection = Transformations.ReflectZ();
            else throw new Exception("Unreachable statement");
            CurrentMesh.Apply(reflection);
            RefreshScenes();
        }

        private void RotateAroundCenter(object sender, EventArgs e)
        {
            double angleX = DegToRad((double)numericUpDown10.Value);
            double angleY = DegToRad((double)numericUpDown11.Value);
            double angleZ = DegToRad((double)numericUpDown12.Value);
            var p = CurrentMesh.Center;
            CurrentMesh.Apply(Transformations.RotateAroundPoint(p, angleX, angleY, angleZ));
            RefreshScenes();
        }

        private void RotateAroundLine(object sender, EventArgs e)
        {
            Vertex a = new Vertex(
                (double)numericUpDownPoint1X.Value,
                (double)numericUpDownPoint1Y.Value,
                (double)numericUpDownPoint1Z.Value);
            Vertex b = new Vertex(
                (double)numericUpDownPoint2X.Value,
                (double)numericUpDownPoint2Y.Value,
                (double)numericUpDownPoint2Z.Value);
            var angle = DegToRad((double)numericUpDownAngle.Value);
            CurrentMesh.Apply(Transformations.RotateAroundLine(a, b, angle));
            RefreshScenes();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            double delta = 0.3;
            switch (keyData)
            {
                case Keys.Oemplus: camera.Position *= Transformations.Translate(0.1 * camera.Forward); break;
                case Keys.OemMinus: camera.Position *= Transformations.Translate(0.1 * camera.Backward); break;
                case Keys.W: camera.Position *= Transformations.Translate(0.1 * camera.Up); break;
                case Keys.A: camera.Position *= Transformations.Translate(0.1 * camera.Left); break;
                case Keys.S: camera.Position *= Transformations.Translate(0.1 * camera.Down); break;
                case Keys.D: camera.Position *= Transformations.Translate(0.1 * camera.Right); break;
                case Keys.NumPad4: camera.AngleY += delta; break;
                case Keys.NumPad6: camera.AngleY -= delta; break;
                case Keys.NumPad8: camera.AngleX += delta; break;
                case Keys.NumPad2: camera.AngleX -= delta; break;
            }
            RefreshScenes();
            return base.ProcessCmdKey(ref msg, keyData);
        }
    

        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rB = (RadioButton)sender;
            if(rB.Checked && rB == radioButtonIzPr)
            {
                radioButtonPerPr.Checked = false;
                camera = new Camera(new Vertex(0, 0, 0), Math.PI / 4, -Math.PI / 4,
                        Transformations.OrthogonalProjection());
                sceneView4.ViewCamera = camera;
                RefreshScenes();
            }
            if (rB.Checked && rB == radioButtonPerPr)
            {
                radioButtonIzPr.Checked = false;
                Matrix projection = Transformations.PerspectiveProjection(-0.1, 0.1, -0.1, 0.1, 0.1, 20);
                camera = new Camera(new Vertex(1, 1, 1), Math.PI / 4, -Math.PI / 4, projection);
                sceneView4.ViewCamera = camera;
                RefreshScenes();
            }
        }

        private void radioButtonPolyh_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rB = (RadioButton)sender;
            if (rB.Checked && rB == radioButtonTetrah)
            {
                radioButtonIcosah.Checked = false;
                CurrentMesh = new Tetrahedron(0.5);
                RefreshScenes();
            }
            if (rB.Checked && rB == radioButtonIcosah)
            {
                radioButtonTetrah.Checked = false;
                CurrentMesh = new Icosahedron(0.5);
                RefreshScenes();
            }
        }

        //Less7 code

        private void AddPoint(object sender, EventArgs e)
        {
            var x = (double)numericUpDownX.Value;
            var y = (double)numericUpDownY.Value;
            var z = (double)numericUpDownZ.Value;
            numericUpDownX.Value = 0;
            numericUpDownY.Value = 0;
            numericUpDownZ.Value = 0;
            listBoxPoints.Items.Add(new Vertex(x, y, z));
        }

        private void SelectedPointChanged(object sender, EventArgs e)
        {
            buttonRemove.Enabled = null != listBoxPoints.SelectedItem;
        }

        private void RemovePoint(object sender, EventArgs e)
        {
            listBoxPoints.Items.RemoveAt(listBoxPoints.SelectedIndex);
        }

        private void Ok(object sender, EventArgs e)
        {
            IList<Vertex> initial = new List<Vertex>(listBoxPoints.Items.Count);
            foreach (var v in listBoxPoints.Items)
                initial.Add((Vertex)v);
            int axis;
            if (radioButtonX.Checked) axis = 0;
            else if (radioButtonY.Checked) axis = 1;
            else /* if (radioButtonZ.Checked) */ axis = 2;
            var density = (int)numericUpDownDensity.Value;
            CurrentMesh = new RotationFigure(initial, axis, density);                  
        }

        private void grCreateButton_Click(object sender, EventArgs e)
        {
            CurrentMesh = new Plot(); 
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.InitialDirectory = Directory.GetCurrentDirectory();
            saveDialog.Filter = "Object Files(*.obj)|*.obj|Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    CurrentMesh.Save(saveDialog.FileName);
                }
                catch
                {
                    DialogResult rezult = MessageBox.Show("Невозможно сохранить файл",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.InitialDirectory = Directory.GetCurrentDirectory();
            openDialog.Filter = "Object Files(*.obj)|*.obj|Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (openDialog.ShowDialog() != DialogResult.OK)
                return;
            try
            {
                CurrentMesh = new Mesh(openDialog.FileName);
            }
            catch
            {
                MessageBox.Show("Ошибка при чтении файла",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            listBoxPoints.Items.Clear();
        }
    }
}

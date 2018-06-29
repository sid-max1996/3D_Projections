using Graphics3D.Geometry;
using System.Drawing;
using System.Windows.Forms;

namespace Graphics3D
{
    class SceneView : Control
    {
        public Camera ViewCamera { get; set; }
        public Mesh Mesh { get; set; }

        public SceneView() : base()
        {
            var flags = ControlStyles.AllPaintingInWmPaint
                      | ControlStyles.DoubleBuffer
                      | ControlStyles.UserPaint;
            SetStyle(flags, true);
            ResizeRedraw = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (null == Mesh) return;
            e.Graphics.Clear(SystemColors.Control);
            e.Graphics.DrawLines(Pens.Black, new Point[]
                {
                    new Point(1, 1),
                    new Point(1, Height - 1),
                    new Point(Width - 1, Height - 1),
                    new Point(Width - 1, 1),
                    new Point(1, 1)
                });
            if (null == Mesh) return;
            var graphics3D = new Graphic3D(e.Graphics, ViewCamera.ViewProjection, Width, Height);
            var x = new Vertex(0.8, 0, 0);
            var y = new Vertex(0, 0.8, 0);
            var z = new Vertex(0, 0, 0.8);
            graphics3D.DrawLine(new Vertex(0, 0, 0), x, new Pen(Color.Red, 3));
            
            graphics3D.DrawLine(new Vertex(0, 0, 0), y, new Pen(Color.Green, 3));
            
            graphics3D.DrawLine(new Vertex(0, 0, 0), z, new Pen(Color.Blue, 3));
            
            Mesh.Draw(graphics3D);
        }
    }
}

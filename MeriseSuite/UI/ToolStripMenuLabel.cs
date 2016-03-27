namespace System.Windows.Forms
{
    public class ToolStripMenuLabel : ToolStripMenuItem
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            Enabled = true;
            base.OnPaint(e);
            Enabled = false;
        }
    }
}
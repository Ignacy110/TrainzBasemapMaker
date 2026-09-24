using System.Windows.Forms;
using System.Drawing;

namespace TrainzBasemapMaker.Classes
{
    internal static class FormHelpers
    {
        public static void OnlyNumbers_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        public static void ComboBoxMapType_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            ComboBox combo = (ComboBox)sender;
            IMapSource mapSource = (IMapSource)combo.Items[e.Index];
            string text = mapSource.Name;

            e.DrawBackground();

            using (Brush textBrush = new SolidBrush(e.ForeColor))
            {
                e.Graphics.DrawString(text, e.Font, textBrush, e.Bounds);
            }

            e.DrawFocusRectangle();
        }
    }
}

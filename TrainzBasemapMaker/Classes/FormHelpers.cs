using System.Windows.Forms;
using System.Drawing;

namespace TrainzBasemapMaker.Classes
{
        public class ResolutionOption
    {
        public int Value { get; set; }
        public string DisplayText => $"{Value} x {Value}";
        public override string ToString() => DisplayText;
    }

    internal static class FormHelpers
    {
        public static int GetMaxSupportedResolution(IMapSource source)
        {
            if (source is WmtsMapSource wmts)
            {
                return wmts.AllowsHighResolution ? 8192 : 4096;
            }
            if (source is WmsMapSource wms)
            {
                return wms.AllowsHighResolution ? 4096 : 2048;
            }
            return 2048;
        }

        public static void PopulateAllResolutions(ComboBox comboBoxResolution, int defaultResolution = 2048)
        {
            var currentSelection = (comboBoxResolution.SelectedItem as ResolutionOption)?.Value ?? defaultResolution;
            comboBoxResolution.Items.Clear();

            comboBoxResolution.Items.Add(new ResolutionOption { Value = 8192 });
            comboBoxResolution.Items.Add(new ResolutionOption { Value = 4096 });
            comboBoxResolution.Items.Add(new ResolutionOption { Value = 2048 });
            comboBoxResolution.Items.Add(new ResolutionOption { Value = 1024 });
            comboBoxResolution.Items.Add(new ResolutionOption { Value = 512 });

            var match = System.Linq.Enumerable.FirstOrDefault(System.Linq.Enumerable.Cast<ResolutionOption>(comboBoxResolution.Items), r => r.Value == currentSelection);
            comboBoxResolution.SelectedItem = match ?? System.Linq.Enumerable.FirstOrDefault(System.Linq.Enumerable.Cast<ResolutionOption>(comboBoxResolution.Items), r => r.Value == 2048);
        }

        public static void UpdateResolutionComboBox(ComboBox comboBoxResolution, IMapSource selected)
        {
            var currentSelection = (comboBoxResolution.SelectedItem as ResolutionOption)?.Value;
            comboBoxResolution.Items.Clear();

            comboBoxResolution.Items.Add(new ResolutionOption { Value = 512 });
            comboBoxResolution.Items.Add(new ResolutionOption { Value = 1024 });
            comboBoxResolution.Items.Add(new ResolutionOption { Value = 2048 });

            if (selected.AllowsHighResolution)
            {
                comboBoxResolution.Items.Add(new ResolutionOption { Value = 4096 });
            }
            if (selected is WmtsMapSource)
            {
                comboBoxResolution.Items.Add(new ResolutionOption { Value = 8192 });
            }

            if (currentSelection.HasValue)
            {
                var match = System.Linq.Enumerable.FirstOrDefault(System.Linq.Enumerable.Cast<ResolutionOption>(comboBoxResolution.Items), r => r.Value == currentSelection.Value);
                if (match != null)
                {
                    comboBoxResolution.SelectedItem = match;
                    return;
                }
            }
            
            comboBoxResolution.SelectedItem = System.Linq.Enumerable.FirstOrDefault(System.Linq.Enumerable.Cast<ResolutionOption>(comboBoxResolution.Items), r => r.Value == 2048);
        }
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

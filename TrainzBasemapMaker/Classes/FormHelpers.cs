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

        public static void ComboBoxMapType_DrawItem(object? sender, DrawItemEventArgs e)
        {
            if (e.Index < 0 || sender is not ComboBox combo) return;

            string text = combo.Items[e.Index] is IMapSource mapSource ? mapSource.Name : combo.Items[e.Index]?.ToString() ?? string.Empty;

            e.DrawBackground();

            Font font = e.Font ?? combo.Font;
            using (Brush textBrush = new SolidBrush(e.ForeColor))
            {
                e.Graphics.DrawString(text, font, textBrush, e.Bounds);
            }

            e.DrawFocusRectangle();
        }

        public static string RemoveDiacritics(string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;

            // Handle specific Polish characters that FormD may not decompose as expected (e.g. ł/Ł)
            var sb = new System.Text.StringBuilder(text.Length);
            foreach (char c in text)
            {
                switch (c)
                {
                    case 'ą': sb.Append('a'); break;
                    case 'ć': sb.Append('c'); break;
                    case 'ę': sb.Append('e'); break;
                    case 'ł': sb.Append('l'); break;
                    case 'ń': sb.Append('n'); break;
                    case 'ó': sb.Append('o'); break;
                    case 'ś': sb.Append('s'); break;
                    case 'ź': sb.Append('z'); break;
                    case 'ż': sb.Append('z'); break;
                    case 'Ą': sb.Append('A'); break;
                    case 'Ć': sb.Append('C'); break;
                    case 'Ę': sb.Append('E'); break;
                    case 'Ł': sb.Append('L'); break;
                    case 'Ń': sb.Append('N'); break;
                    case 'Ó': sb.Append('O'); break;
                    case 'Ś': sb.Append('S'); break;
                    case 'Ź': sb.Append('Z'); break;
                    case 'Ż': sb.Append('Z'); break;
                    default:
                        sb.Append(c);
                        break;
                }
            }

            string normalizedString = sb.ToString().Normalize(System.Text.NormalizationForm.FormD);
            var result = new System.Text.StringBuilder(normalizedString.Length);

            foreach (char c in normalizedString)
            {
                var unicodeCategory = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    result.Append(c);
                }
            }

            return result.ToString().Normalize(System.Text.NormalizationForm.FormC);
        }
    }
}

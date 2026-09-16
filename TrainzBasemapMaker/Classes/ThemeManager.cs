using System;
using System.Drawing;
using System.Windows.Forms;
using TrainzBasemapMaker.Properties;

namespace TrainzBasemapMaker.Classes
{
    public static class ThemeManager
    {
        public static void ApplyTheme(Form form)
        {
            if (Settings.Default.DarkMode)
            {
                ApplyDarkTheme(form);
                
                // Handle menu strips and status strips specifically for the form
                foreach (Control control in form.Controls)
                {
                    if (control is MenuStrip menuStrip)
                    {
                        menuStrip.BackColor = Color.FromArgb(45, 45, 48);
                        menuStrip.ForeColor = Color.White;
                        menuStrip.Renderer = new DarkModeRenderer();
                        foreach (ToolStripItem item in menuStrip.Items)
                        {
                            ApplyDarkThemeToolStripItem(item);
                        }
                    }
                    else if (control is StatusStrip statusStrip)
                    {
                        statusStrip.BackColor = Color.FromArgb(45, 45, 48);
                        statusStrip.ForeColor = Color.White;
                    }
                }
            }
            else
            {
                ApplyLightTheme(form);
            }
        }

        private static void ApplyDarkThemeToolStripItem(ToolStripItem item)
        {
            item.BackColor = Color.FromArgb(45, 45, 48);
            item.ForeColor = Color.White;
            
            if (item is ToolStripMenuItem menuItem)
            {
                foreach (ToolStripItem dropDownItem in menuItem.DropDownItems)
                {
                    ApplyDarkThemeToolStripItem(dropDownItem);
                }
            }
        }

        private static void ApplyDarkTheme(Control control)
        {
            if (control is Label || control is CheckBox || control is RadioButton || control is GroupBox)
            {
                control.BackColor = Color.Transparent;
            }
            else
            {
                control.BackColor = Color.FromArgb(45, 45, 48);
            }
            control.ForeColor = Color.White;

            if (control is TextBox || control is ListBox || control is ComboBox)
            {
                control.BackColor = Color.FromArgb(30, 30, 30);
                control.ForeColor = Color.White;
            }
            
            if (control is Button button)
            {
                button.FlatStyle = FlatStyle.Flat;
                button.FlatAppearance.BorderColor = Color.FromArgb(85, 85, 85);
                button.BackColor = Color.FromArgb(60, 60, 60);
                button.ForeColor = Color.White;
            }
            
            if (control is GroupBox groupBox)
            {
                groupBox.ForeColor = Color.White;
            }

            foreach (Control child in control.Controls)
            {
                ApplyDarkTheme(child);
            }
        }

        private static void ApplyLightTheme(Control control)
        {
            // By default, WinForms controls return to their default colors
            // if we don't do anything, but since forms are created fresh, 
            // if DarkMode is false, we don't strictly need to traverse them.
            // But if we toggle on the fly, we would. For this app, settings take effect immediately?
            // Actually, usually changing theme requires restart or re-applying on the fly.
            
            control.BackColor = SystemColors.Control;
            control.ForeColor = SystemColors.ControlText;

            if (control is TextBox || control is ListBox || control is ComboBox)
            {
                control.BackColor = SystemColors.Window;
                control.ForeColor = SystemColors.WindowText;
            }

            if (control is Button button)
            {
                button.FlatStyle = FlatStyle.Standard;
                button.UseVisualStyleBackColor = true;
            }
            
            if (control is GroupBox groupBox)
            {
                groupBox.ForeColor = SystemColors.ControlText;
            }
            
            if (control is MenuStrip menuStrip)
            {
                menuStrip.BackColor = SystemColors.Control;
                menuStrip.ForeColor = SystemColors.ControlText;
                menuStrip.RenderMode = ToolStripRenderMode.ManagerRenderMode;
                foreach (ToolStripItem item in menuStrip.Items)
                {
                    ApplyLightThemeToolStripItem(item);
                }
            }
            else if (control is StatusStrip statusStrip)
            {
                statusStrip.BackColor = SystemColors.Control;
                statusStrip.ForeColor = SystemColors.ControlText;
            }

            foreach (Control child in control.Controls)
            {
                ApplyLightTheme(child);
            }
        }
        
        private static void ApplyLightThemeToolStripItem(ToolStripItem item)
        {
            item.BackColor = SystemColors.Control;
            item.ForeColor = SystemColors.ControlText;
            
            if (item is ToolStripMenuItem menuItem)
            {
                foreach (ToolStripItem dropDownItem in menuItem.DropDownItems)
                {
                    ApplyLightThemeToolStripItem(dropDownItem);
                }
            }
        }
    }

    public class DarkModeRenderer : ToolStripProfessionalRenderer
    {
        public DarkModeRenderer() : base(new DarkModeColorTable()) { }
    }

    public class DarkModeColorTable : ProfessionalColorTable
    {
        public override Color ToolStripDropDownBackground => Color.FromArgb(45, 45, 48);
        public override Color ImageMarginGradientBegin => Color.FromArgb(45, 45, 48);
        public override Color ImageMarginGradientMiddle => Color.FromArgb(45, 45, 48);
        public override Color ImageMarginGradientEnd => Color.FromArgb(45, 45, 48);
        public override Color MenuBorder => Color.FromArgb(85, 85, 85);
        public override Color MenuItemBorder => Color.FromArgb(85, 85, 85);
        public override Color MenuItemSelected => Color.FromArgb(60, 60, 60);
        public override Color MenuStripGradientBegin => Color.FromArgb(45, 45, 48);
        public override Color MenuStripGradientEnd => Color.FromArgb(45, 45, 48);
        public override Color MenuItemSelectedGradientBegin => Color.FromArgb(60, 60, 60);
        public override Color MenuItemSelectedGradientEnd => Color.FromArgb(60, 60, 60);
        public override Color MenuItemPressedGradientBegin => Color.FromArgb(45, 45, 48);
        public override Color MenuItemPressedGradientEnd => Color.FromArgb(45, 45, 48);
    }
}

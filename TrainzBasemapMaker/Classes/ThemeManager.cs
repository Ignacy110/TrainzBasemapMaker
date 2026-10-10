// Trainz Basemap Maker
// https://github.com/Ignacy110/TrainzBasemapMaker
//
// Copyright (C) 2026 Ignacy110 (http://github.com/Ignacy110)
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, see (http://www.gnu.org/licenses/).

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using TrainzBasemapMaker.Properties;

namespace TrainzBasemapMaker.Classes
{
    public static class ThemeManager
    {
        [DllImport("dwmapi.dll", PreserveSig = true)]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        private const int DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1 = 19;
        private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;

        public static void UpdateTitleBarTheme(Form form, bool isDark)
        {
            if (!OperatingSystem.IsWindows()) return;

            if (!form.IsHandleCreated)
            {
                form.HandleCreated += (s, e) => UpdateTitleBarTheme(form, isDark);
                return;
            }

            int useDarkMode = isDark ? 1 : 0;
            if (DwmSetWindowAttribute(form.Handle, DWMWA_USE_IMMERSIVE_DARK_MODE, ref useDarkMode, sizeof(int)) != 0)
            {
                DwmSetWindowAttribute(form.Handle, DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1, ref useDarkMode, sizeof(int));
            }
        }

        public static void ApplyTheme(Form form)
        {
            bool isDark = Settings.Default.DarkMode;
            UpdateTitleBarTheme(form, isDark);

            if (isDark)
            {
                ApplyDarkTheme(form);
            }
            else
            {
                ApplyLightTheme(form);
            }

            form.Invalidate(true);
        }

        private static void ApplyDarkThemeToolStripItem(ToolStripItem item)
        {
            item.BackColor = Color.FromArgb(45, 45, 48);
            item.ForeColor = Color.White;

            if (item is ToolStripStatusLabel statusLabel && statusLabel.IsLink)
            {
                statusLabel.LinkColor = Color.FromArgb(100, 180, 246);
                statusLabel.ActiveLinkColor = Color.FromArgb(144, 202, 249);
                statusLabel.VisitedLinkColor = Color.FromArgb(180, 140, 230);
            }

            if (item is ToolStripDropDownItem dropDownItem)
            {
                dropDownItem.DropDown.BackColor = Color.FromArgb(45, 45, 48);
                dropDownItem.DropDown.ForeColor = Color.White;
                dropDownItem.DropDown.Renderer = new DarkModeRenderer();

                foreach (ToolStripItem subItem in dropDownItem.DropDownItems)
                {
                    ApplyDarkThemeToolStripItem(subItem);
                }
            }
        }

        private static void ApplyDarkTheme(Control control)
        {
            if (control is Form formControl)
            {
                UpdateTitleBarTheme(formControl, true);
            }

            if (control is Label || control is CheckBox || control is RadioButton || control is GroupBox)
            {
                control.BackColor = Color.Transparent;
            }
            else
            {
                control.BackColor = Color.FromArgb(45, 45, 48);
            }
            control.ForeColor = Color.White;

            if (control is LinkLabel linkLabel)
            {
                linkLabel.BackColor = Color.Transparent;
                linkLabel.LinkColor = Color.FromArgb(100, 180, 246);
                linkLabel.ActiveLinkColor = Color.FromArgb(144, 202, 249);
                linkLabel.VisitedLinkColor = Color.FromArgb(180, 140, 230);
            }

            if (control is TextBox || control is ListBox)
            {
                control.BackColor = Color.FromArgb(30, 30, 30);
                control.ForeColor = Color.White;
            }

            if (control is ComboBox comboBox)
            {
                comboBox.BackColor = Color.FromArgb(30, 30, 30);
                comboBox.ForeColor = Color.White;
                comboBox.FlatStyle = FlatStyle.Flat;

                if (comboBox.DrawMode == DrawMode.Normal)
                {
                    comboBox.Tag = "AutoDarkOwnerDraw";
                    comboBox.DrawMode = DrawMode.OwnerDrawFixed;
                }

                if (comboBox.Tag as string == "AutoDarkOwnerDraw")
                {
                    comboBox.DrawItem -= ComboBox_DefaultDrawItem;
                    comboBox.DrawItem += ComboBox_DefaultDrawItem;
                }

                comboBox.Invalidate(true);
                comboBox.Update();
            }

            if (control is Button button)
            {
                button.FlatStyle = FlatStyle.Flat;
                button.FlatAppearance.BorderColor = Color.FromArgb(85, 85, 85);
                button.FlatAppearance.MouseOverBackColor = Color.FromArgb(75, 75, 75);
                button.FlatAppearance.MouseDownBackColor = Color.FromArgb(90, 90, 90);
                button.BackColor = Color.FromArgb(60, 60, 60);
                button.ForeColor = Color.White;
            }

            if (control is GroupBox groupBox)
            {
                groupBox.ForeColor = Color.White;
            }

            if (control is TabPage tabPage)
            {
                tabPage.UseVisualStyleBackColor = false;
                tabPage.BackColor = Color.FromArgb(45, 45, 48);
                tabPage.ForeColor = Color.White;
            }

            if (control is DarkTabControl darkTabControl)
            {
                darkTabControl.ApplyThemeLayout();
            }
            else if (control is TabControl tabControl)
            {
                tabControl.DrawMode = TabDrawMode.OwnerDrawFixed;
                tabControl.DrawItem -= TabControl_DrawItem;
                tabControl.DrawItem += TabControl_DrawItem;
                tabControl.Invalidate();
            }

            if (control is ToolStrip toolStrip)
            {
                toolStrip.BackColor = Color.FromArgb(45, 45, 48);
                toolStrip.ForeColor = Color.White;
                toolStrip.Renderer = new DarkModeRenderer();
                foreach (ToolStripItem item in toolStrip.Items)
                {
                    ApplyDarkThemeToolStripItem(item);
                }
            }

            if (control is Microsoft.Web.WebView2.WinForms.WebView2 webView)
            {
                webView.DefaultBackgroundColor = Color.FromArgb(45, 45, 48);
                if (webView.CoreWebView2 != null)
                {
                    try
                    {
                        webView.CoreWebView2.Profile.PreferredColorScheme = Microsoft.Web.WebView2.Core.CoreWebView2PreferredColorScheme.Dark;
                    }
                    catch { }
                }
            }

            if (control.ContextMenuStrip != null)
            {
                control.ContextMenuStrip.BackColor = Color.FromArgb(45, 45, 48);
                control.ContextMenuStrip.ForeColor = Color.White;
                control.ContextMenuStrip.Renderer = new DarkModeRenderer();
                foreach (ToolStripItem item in control.ContextMenuStrip.Items)
                {
                    ApplyDarkThemeToolStripItem(item);
                }
            }

            foreach (Control child in control.Controls)
            {
                if (child is Form childForm)
                {
                    ApplyTheme(childForm);
                }
                else
                {
                    ApplyDarkTheme(child);
                }
            }
        }

        private static void ComboBox_DefaultDrawItem(object? sender, DrawItemEventArgs e)
        {
            if (sender is not ComboBox combo || e.Index < 0 || e.Index >= combo.Items.Count) return;

            bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
            bool isEnabled = combo.Enabled;

            Color backColor = isSelected ? SystemColors.Highlight : Color.FromArgb(30, 30, 30);
            Color foreColor = !isEnabled
                ? Color.FromArgb(120, 120, 120)
                : (isSelected ? SystemColors.HighlightText : Color.White);

            using (SolidBrush backBrush = new SolidBrush(backColor))
            {
                e.Graphics.FillRectangle(backBrush, e.Bounds);
            }

            string text = combo.GetItemText(combo.Items[e.Index]) ?? string.Empty;
            Font font = e.Font ?? combo.Font;

            using (SolidBrush textBrush = new SolidBrush(foreColor))
            using (StringFormat sf = new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Near })
            {
                e.Graphics.DrawString(text, font, textBrush, e.Bounds, sf);
            }

            e.DrawFocusRectangle();
        }

        private static void TabControl_DrawItem(object? sender, DrawItemEventArgs e)
        {
            if (sender is not TabControl tc || e.Index < 0 || e.Index >= tc.TabPages.Count) return;

            TabPage page = tc.TabPages[e.Index];
            bool isSelected = tc.SelectedIndex == e.Index;

            Color backColor = isSelected ? Color.FromArgb(45, 45, 48) : Color.FromArgb(30, 30, 30);
            Color foreColor = isSelected ? Color.White : Color.FromArgb(200, 200, 200);

            // Extend selected tab downwards by 2px to seamlessly merge with the tab page
            Rectangle tabRect = isSelected
                ? new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height + 2)
                : e.Bounds;

            using (SolidBrush brush = new SolidBrush(backColor))
            {
                e.Graphics.FillRectangle(brush, tabRect);
            }

            using (Pen pen = new Pen(Color.FromArgb(65, 65, 68)))
            {
                if (isSelected)
                {
                    e.Graphics.DrawLine(pen, tabRect.Left, tabRect.Bottom, tabRect.Left, tabRect.Top);
                    e.Graphics.DrawLine(pen, tabRect.Left, tabRect.Top, tabRect.Right - 1, tabRect.Top);
                    e.Graphics.DrawLine(pen, tabRect.Right - 1, tabRect.Top, tabRect.Right - 1, tabRect.Bottom);
                }
                else
                {
                    e.Graphics.DrawRectangle(pen, tabRect.X, tabRect.Y, tabRect.Width - 1, tabRect.Height - 1);
                }
            }

            TextRenderer.DrawText(
                e.Graphics,
                page.Text,
                tc.Font,
                e.Bounds,
                foreColor,
                backColor,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
            );
        }

        private static void ApplyLightTheme(Control control)
        {
            if (control is Form formControl)
            {
                UpdateTitleBarTheme(formControl, false);
            }

            control.BackColor = SystemColors.Control;
            control.ForeColor = SystemColors.ControlText;

            if (control is LinkLabel linkLabel)
            {
                linkLabel.BackColor = Color.Transparent;
                linkLabel.LinkColor = Color.FromArgb(0, 102, 204);
                linkLabel.ActiveLinkColor = Color.Red;
                linkLabel.VisitedLinkColor = Color.FromArgb(128, 0, 128);
            }

            if (control is TextBox || control is ListBox)
            {
                control.BackColor = SystemColors.Window;
                control.ForeColor = SystemColors.WindowText;
            }

            if (control is ComboBox comboBox)
            {
                comboBox.BackColor = SystemColors.Window;
                comboBox.ForeColor = SystemColors.WindowText;
                comboBox.FlatStyle = FlatStyle.Standard;

                if (comboBox.Tag as string == "AutoDarkOwnerDraw")
                {
                    comboBox.DrawItem -= ComboBox_DefaultDrawItem;
                    comboBox.DrawMode = DrawMode.Normal;
                    comboBox.Tag = null;
                }

                comboBox.Invalidate(true);
                comboBox.Update();
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

            if (control is TabPage tabPage)
            {
                tabPage.UseVisualStyleBackColor = true;
                tabPage.BackColor = SystemColors.Control;
                tabPage.ForeColor = SystemColors.ControlText;
            }

            if (control is DarkTabControl darkTabControl)
            {
                darkTabControl.ApplyThemeLayout();
            }
            else if (control is TabControl tabControl)
            {
                tabControl.DrawMode = TabDrawMode.Normal;
                tabControl.DrawItem -= TabControl_DrawItem;
                tabControl.Invalidate();
            }

            if (control is ToolStrip toolStrip)
            {
                toolStrip.BackColor = SystemColors.Control;
                toolStrip.ForeColor = SystemColors.ControlText;
                toolStrip.RenderMode = ToolStripRenderMode.ManagerRenderMode;
                foreach (ToolStripItem item in toolStrip.Items)
                {
                    ApplyLightThemeToolStripItem(item);
                }
            }

            if (control is Microsoft.Web.WebView2.WinForms.WebView2 webView)
            {
                webView.DefaultBackgroundColor = Color.White;
                if (webView.CoreWebView2 != null)
                {
                    try
                    {
                        webView.CoreWebView2.Profile.PreferredColorScheme = Microsoft.Web.WebView2.Core.CoreWebView2PreferredColorScheme.Light;
                    }
                    catch { }
                }
            }

            if (control.ContextMenuStrip != null)
            {
                control.ContextMenuStrip.BackColor = SystemColors.Control;
                control.ContextMenuStrip.ForeColor = SystemColors.ControlText;
                control.ContextMenuStrip.RenderMode = ToolStripRenderMode.ManagerRenderMode;
                foreach (ToolStripItem item in control.ContextMenuStrip.Items)
                {
                    ApplyLightThemeToolStripItem(item);
                }
            }

            foreach (Control child in control.Controls)
            {
                if (child is Form childForm)
                {
                    ApplyTheme(childForm);
                }
                else
                {
                    ApplyLightTheme(child);
                }
            }
        }

        private static void ApplyLightThemeToolStripItem(ToolStripItem item)
        {
            item.BackColor = SystemColors.Control;
            item.ForeColor = SystemColors.ControlText;

            if (item is ToolStripStatusLabel statusLabel && statusLabel.IsLink)
            {
                statusLabel.LinkColor = Color.FromArgb(0, 102, 204);
                statusLabel.ActiveLinkColor = Color.Red;
                statusLabel.VisitedLinkColor = Color.FromArgb(128, 0, 128);
            }

            if (item is ToolStripDropDownItem dropDownItem)
            {
                dropDownItem.DropDown.BackColor = SystemColors.Control;
                dropDownItem.DropDown.ForeColor = SystemColors.ControlText;
                dropDownItem.DropDown.RenderMode = ToolStripRenderMode.ManagerRenderMode;

                foreach (ToolStripItem subItem in dropDownItem.DropDownItems)
                {
                    ApplyLightThemeToolStripItem(subItem);
                }
            }
        }

        private class DarkModeRenderer : ToolStripProfessionalRenderer
        {
            public DarkModeRenderer() : base(new DarkModeColorTable()) { }
        }

        private class DarkModeColorTable : ProfessionalColorTable
        {
            public override Color ToolStripDropDownBackground => Color.FromArgb(45, 45, 48);
            public override Color ImageMarginGradientBegin => Color.FromArgb(45, 45, 48);
            public override Color ImageMarginGradientMiddle => Color.FromArgb(45, 45, 48);
            public override Color ImageMarginGradientEnd => Color.FromArgb(45, 45, 48);
            public override Color MenuBorder => Color.FromArgb(65, 65, 68);
            public override Color MenuItemBorder => Color.FromArgb(75, 75, 80);
            public override Color MenuItemSelected => Color.FromArgb(60, 60, 60);
            public override Color MenuStripGradientBegin => Color.FromArgb(45, 45, 48);
            public override Color MenuStripGradientEnd => Color.FromArgb(45, 45, 48);
            public override Color MenuItemSelectedGradientBegin => Color.FromArgb(60, 60, 60);
            public override Color MenuItemSelectedGradientEnd => Color.FromArgb(60, 60, 60);
            public override Color MenuItemPressedGradientBegin => Color.FromArgb(45, 45, 48);
            public override Color MenuItemPressedGradientEnd => Color.FromArgb(45, 45, 48);
            public override Color StatusStripGradientBegin => Color.FromArgb(45, 45, 48);
            public override Color StatusStripGradientEnd => Color.FromArgb(45, 45, 48);
            public override Color SeparatorDark => Color.FromArgb(70, 70, 74);
            public override Color SeparatorLight => Color.FromArgb(45, 45, 48);
            public override Color GripDark => Color.FromArgb(70, 70, 74);
            public override Color GripLight => Color.FromArgb(45, 45, 48);
            public override Color CheckBackground => Color.FromArgb(60, 60, 60);
            public override Color CheckSelectedBackground => Color.FromArgb(75, 75, 75);
            public override Color CheckPressedBackground => Color.FromArgb(90, 90, 90);
            public override Color ToolStripBorder => Color.FromArgb(60, 60, 64);
        }
    }
}

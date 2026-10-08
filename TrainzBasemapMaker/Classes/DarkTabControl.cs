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
using System.Windows.Forms;
using TrainzBasemapMaker.Properties;

namespace TrainzBasemapMaker.Classes
{
    public class DarkTabControl : TabControl
    {
        private const int WM_ERASEBKGND = 0x0014;
        private int _hoverIndex = -1;

        public DarkTabControl()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw, true);

            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.ItemSize = new Size(0, 25);

            UpdateUserPaint();
        }

        private void UpdateUserPaint()
        {
            SetStyle(ControlStyles.UserPaint, Settings.Default.DarkMode);
        }

        public override Rectangle DisplayRectangle
        {
            get
            {
                Rectangle rect = base.DisplayRectangle;
                if (!Settings.Default.DarkMode)
                    return rect;

                return new Rectangle(0, rect.Top, Width, Math.Max(0, Height - rect.Top));
            }
        }

        public void ApplyThemeLayout()
        {
            UpdateUserPaint();
            Rectangle rect = DisplayRectangle;
            foreach (TabPage page in TabPages)
            {
                page.Bounds = rect;
            }
            Invalidate();
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            if (Settings.Default.DarkMode) return;
            base.OnPaintBackground(pevent);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (!Settings.Default.DarkMode)
            {
                base.OnPaint(e);
                return;
            }

            int topStripHeight = DisplayRectangle.Top > 0 ? DisplayRectangle.Top : (TabCount > 0 ? GetTabRect(0).Bottom + 2 : 27);

            Color stripColor = Color.FromArgb(30, 30, 30);
            Color contentBgColor = Color.FromArgb(45, 45, 48);
            Color borderColor = Color.FromArgb(65, 65, 68);
            Color hoverBgColor = Color.FromArgb(40, 40, 42);

            // 1. Fill the ENTIRE tab header strip
            using (SolidBrush stripBrush = new SolidBrush(stripColor))
            {
                e.Graphics.FillRectangle(stripBrush, new Rectangle(0, 0, Width, topStripHeight));
            }

            // 2. Base line under the strip
            using (Pen pen = new Pen(borderColor))
            {
                e.Graphics.DrawLine(pen, 0, topStripHeight - 1, Width, topStripHeight - 1);
            }

            // 3. Draw each tab on top of the dark strip
            for (int i = 0; i < TabCount; i++)
            {
                Rectangle tabRect = GetTabRect(i);
                bool isSelected = (SelectedIndex == i);
                bool isHovered = (i == _hoverIndex && !isSelected);
                TabPage page = TabPages[i];

                Color tabBg = isSelected ? contentBgColor : (isHovered ? hoverBgColor : stripColor);
                Color tabFg = isSelected ? Color.White : (isHovered ? Color.White : Color.FromArgb(200, 200, 200));

                // If selected, extend down to topStripHeight to cover the strip line and merge with page
                Rectangle r = isSelected
                    ? new Rectangle(tabRect.X, tabRect.Y, tabRect.Width, topStripHeight - tabRect.Y)
                    : new Rectangle(tabRect.X, tabRect.Y, tabRect.Width, Math.Max(0, topStripHeight - 1 - tabRect.Y));

                using (SolidBrush tabBrush = new SolidBrush(tabBg))
                {
                    e.Graphics.FillRectangle(tabBrush, r);
                }

                using (Pen pen = new Pen(borderColor))
                {
                    if (isSelected)
                    {
                        e.Graphics.DrawLine(pen, r.Left, topStripHeight - 1, r.Left, r.Top);
                        e.Graphics.DrawLine(pen, r.Left, r.Top, r.Right - 1, r.Top);
                        e.Graphics.DrawLine(pen, r.Right - 1, r.Top, r.Right - 1, topStripHeight - 1);
                    }
                    else
                    {
                        // Subtle vertical separator between inactive tabs
                        if (i < TabCount - 1 && SelectedIndex != i + 1)
                        {
                            e.Graphics.DrawLine(pen, tabRect.Right, tabRect.Y + 4, tabRect.Right, topStripHeight - 5);
                        }
                    }
                }

                // Centered text with matching backColor for perfect subpixel ClearType antialiasing
                Rectangle textRect = new Rectangle(tabRect.X, 0, tabRect.Width, topStripHeight - 1);
                TextRenderer.DrawText(
                    e.Graphics,
                    page.Text,
                    this.Font,
                    textRect,
                    tabFg,
                    tabBg,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine
                );
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (!Settings.Default.DarkMode) return;

            int topStripHeight = DisplayRectangle.Top > 0 ? DisplayRectangle.Top : (TabCount > 0 ? GetTabRect(0).Bottom + 2 : 27);
            int newHover = -1;

            if (e.Y < topStripHeight)
            {
                for (int i = 0; i < TabCount; i++)
                {
                    Rectangle tabRect = GetTabRect(i);
                    Rectangle hoverRect = new Rectangle(tabRect.X, 0, tabRect.Width, topStripHeight);
                    if (hoverRect.Contains(e.Location))
                    {
                        newHover = i;
                        break;
                    }
                }
            }

            if (newHover != _hoverIndex)
            {
                _hoverIndex = newHover;
                Invalidate(new Rectangle(0, 0, Width, topStripHeight));
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (_hoverIndex != -1)
            {
                _hoverIndex = -1;
                int topStripHeight = DisplayRectangle.Top > 0 ? DisplayRectangle.Top : (TabCount > 0 ? GetTabRect(0).Bottom + 2 : 27);
                Invalidate(new Rectangle(0, 0, Width, topStripHeight));
            }
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e);
            Invalidate();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            Invalidate();
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_ERASEBKGND && Settings.Default.DarkMode)
            {
                m.Result = (IntPtr)1;
                return;
            }

            base.WndProc(ref m);
        }
    }
}

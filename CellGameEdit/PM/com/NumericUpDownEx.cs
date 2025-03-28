﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CellGameEdit.PM.com
{
    public partial class NumericUpDownEx : NumericUpDown
    {
        public NumericUpDownEx()
        {
            InitializeComponent();
        }

        public NumericUpDownEx(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
        
        private const int WM_MOUSEWHEEL = 0x020A;
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_MOUSEWHEEL)
            {
                int direct = m.WParam.ToInt32();

                if (direct == 0)
                {
                    base.WndProc(ref m);
                }
                else if (direct > 0)
                {
                    try
                    {
                        Value += Increment;
                    }
                    catch (Exception err) { }
                }
                else if (direct < 0)
                {
                    try
                    {
                        Value -= Increment;
                    }
                    catch (Exception err) { }
                }
            }
            else
            {
                base.WndProc(ref m);
            }
        }
    }
}

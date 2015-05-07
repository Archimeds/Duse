using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Duse
{
    class MyNumericUpDown : System.Windows.Forms.NumericUpDown
    {
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                if (value.Length < 2)
                {
                    value = "000" + value;
                }
                else
                {
                    value = "00" + value;
                }

                base.Text = value;
            }
        }
    }
}

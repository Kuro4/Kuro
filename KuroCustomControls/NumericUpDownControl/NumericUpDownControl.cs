using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace KuroCustomControls
{
    public class NumericUpDownControl : Control
    {
        static NumericUpDownControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NumericUpDownControl),
                new FrameworkPropertyMetadata(typeof(NumericUpDownControl)));
        }
    }
}

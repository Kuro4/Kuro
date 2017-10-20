using KuroCustomControls;
using KuroUtilities.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KuroCustomControlsTests
{
    public class VM_MainWindow
    {
        public TimePickerControl tp { get; set; } = new TimePickerControl();

        public com testCom { get; set; }

        public VM_MainWindow()
        {
            tp.TimeChanged += Tp_TimeChanged;
            testCom = new com(add);
        }

        private void add()
        {
            tp.Hours += 5;
            System.Diagnostics.Debug.WriteLine("add");
        }


        private void Tp_TimeChanged(object sender, TimeChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("NewTime : " + e.NewTime);
            System.Diagnostics.Debug.WriteLine("OldTime : " + e.OldTime);
            System.Diagnostics.Debug.WriteLine("SubtractedTime : " + e.SubtractedTime);
        }


        public class com : ICommand
        {
            public Action _Action;
            public com(Action action)
            {
                _Action = action;
            }

            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                _Action();
            }
        }
    }
}

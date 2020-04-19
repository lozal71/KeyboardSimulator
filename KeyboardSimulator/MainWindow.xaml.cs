using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KeyboardSimulator
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Brush tempColor;
        public bool flagRepeat = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private Button GetButton(KeyEventArgs e)
        {
            Button tempBtn = null;
            foreach (DockPanel dock in ButtonSet.Children)
            {
                foreach (Button btn in dock.Children)
                {
                    if (e.Key.ToString() == btn.Name.ToString())
                    {
                        tempBtn = btn;
                        break;
                    }
                }
            }
            return tempBtn;
        }
        private void WritePanel_KeyDown(object sender, KeyEventArgs e)
        {
            Button tempButton = GetButton(e);
            if (!e.IsRepeat)
            {
                WritePanel.Text += e.Key.ToString();
                if (tempButton != null)
                {
                    tempColor = tempButton.Background;
                }
            }
            if (tempButton != null)
            {
                tempButton.Background = Brushes.White;
            }
            if (e.IsRepeat)
            {
                flagRepeat = true;
            }

        }
        private void WritePanel_KeyUp(object sender, KeyEventArgs e)
        {
            Button tempButton = GetButton(e);
            if (tempButton != null)
            {
                tempButton.Background = tempColor;
                flagRepeat = false;
            }
            //foreach (DockPanel dock in ButtonSet.Children)
            //{
            //    foreach (Button btn in dock.Children)
            //    {
            //        if (e.Key.ToString() == btn.Name.ToString())
            //        {
            //            btn.Background = tempColor;
            //            flagRepeat = false;
            //            break;
            //        }
            //    }
            //}
        }
        private void WritePanel_TextInpt(object sender, TextCompositionEventArgs e)
        {
            if (!flagRepeat)
            {
                WritePanel.Text += e.Text + "\n";
                flagRepeat = true;
            }
        }
    }
}

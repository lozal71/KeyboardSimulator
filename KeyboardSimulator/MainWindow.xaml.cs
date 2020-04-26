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
    public static class ResourceHelper
    {
        static public string FindNameFromResource(ResourceDictionary dictionary, object resourceItem)
        {
            foreach (object key in dictionary.Keys)
            {
                if (dictionary[key] == resourceItem)
                {
                    return key.ToString();
                }
            }

            return null;
        }
    }
    public partial class MainWindow : Window
    {
        public Brush tempColor;
        public bool flagRepeat = false;
        public bool flagTab = false;
        //Button tempButton;
        public MainWindow()
        {
            InitializeComponent();
        }

        private Button GetButton(KeyEventArgs e)
        {
            Button tempBtn = null;
            bool flagBreak = false;
            foreach (DockPanel dock in ButtonSet.Children)
            {
                foreach (Button btn in dock.Children)
                {
                    if (e.Key.ToString() == btn.Name.ToString())
                    {
                        //if (btn.Background.ToString() != "#FF808080")
                        //{
                    //    if (btn.Name.ToString() == "Tab")
                    //    {
                    //        flagTab = true;
                    //        flagBreak = true;
                    //        break;
                    //    }
                    ////}
                    //    else
                    //    {
                            tempBtn = btn;
                            flagBreak = true;
                            break;
                        //}
                    }
                }
                if (flagBreak)
                {
                    break;
                }
            }
            return tempBtn;
        }
        private void WritePanel_KeyDown(object sender, KeyEventArgs e)
        {
            Button tempButton = GetButton(e);
            if (!e.IsRepeat)
            {
                //WritePanel.Text += tempButton.Background.ToString(); // e.Key.ToString();
                if (tempButton != null)
                {
                    //string name = ResourceHelper.FindNameFromResource(this.Resources, tempButton);
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
            //if (flagTab)
            //{
            //    WritePanel.Focus();
            //    WritePanel.Select(WritePanel.Text.Length, 1);
            //}

        }
        private void WritePanel_KeyUp(object sender, KeyEventArgs e)
        {
            Button tempButton = GetButton(e);
            if (tempButton != null && tempButton.Name.ToString() != "Back")
            {
                //tempButton.Background = tempColor;
                tempButton.Background = tempButton.BorderBrush;
                flagRepeat = false;
            }
            //tempButton = null;
        }
        private void WritePanel_TextInpt(object sender, TextCompositionEventArgs e)
        {
            //string tempSrtring;
            //string str;
            //int len;
            if (!flagRepeat)
            {
                //if (!flagBack)
                //{
                    WritePanel.Text += e.Text;
                    flagRepeat = true;
                //}
                //else
                //{
                //    str = WritePanel.Text.ToString();
                //    len = str.Length;
                //    tempSrtring = str.Remove(len - 1);
                //    WritePanel.Text = tempSrtring;
                //    flagBack = false;
                //}
            }
        }
    }
}

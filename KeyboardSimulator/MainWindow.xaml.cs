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
        //public bool flagRepeat = false;
        //public bool flagTab = false;
        //Button tempButton;
        public MainWindow()
        {
            InitializeComponent();
            if (Keyboard.IsKeyToggled(Key.CapsLock)) Switch_Big();
            else Switch_Small();
        }

        private Button GetButton(KeyEventArgs e)
        {
            Button tempBtn = null;
            bool flagBreak = false;
            string str;
            foreach (DockPanel dock in ButtonSet.Children)
            {
                if (dock.Visibility == Visibility.Visible)
                {
                    foreach (Button btn in dock.Children)
                    {
                        str = btn.Name.ToString().Split('_')[0];
                        if (e.Key.ToString() == btn.Name.ToString() ||
                            e.Key.ToString() + "_shift" == btn.Name.ToString())
                        {
                            tempBtn = btn;
                            flagBreak = true;
                            break;
                        }
                    }
                }
                if (flagBreak)
                {
                    break;
                }
            }
            return tempBtn;
        }
        private void Switch_Big()
        {
            Dock1.Visibility = Visibility.Hidden;
            Dock11.Visibility = Visibility.Visible;
            Dock2.Visibility = Visibility.Hidden;
            Dock22.Visibility = Visibility.Visible;
            Dock3.Visibility = Visibility.Hidden;
            Dock33.Visibility = Visibility.Visible;
        }
        private void Switch_Big_Shift()
        {
            Dock0.Visibility = Visibility.Hidden;
            Dock00.Visibility = Visibility.Visible;
            Dock1.Visibility = Visibility.Hidden;
            Dock11.Visibility = Visibility.Visible;
            Dock2.Visibility = Visibility.Hidden;
            Dock22.Visibility = Visibility.Visible;
            Dock3.Visibility = Visibility.Hidden;
            Dock33.Visibility = Visibility.Visible;
        }
        private void Switch_Big_Shift_Digit_Small()
        {
            Dock00.Visibility = Visibility.Hidden;
            Dock0.Visibility = Visibility.Visible;
            Dock1.Visibility = Visibility.Hidden;
            Dock11.Visibility = Visibility.Visible;
            Dock2.Visibility = Visibility.Hidden;
            Dock22.Visibility = Visibility.Visible;
            Dock3.Visibility = Visibility.Hidden;
            Dock33.Visibility = Visibility.Visible;
        }
        private void Switch_Small()
        {
            Dock11.Visibility = Visibility.Hidden;
            Dock1.Visibility = Visibility.Visible;
            Dock22.Visibility = Visibility.Hidden;
            Dock2.Visibility = Visibility.Visible;
            Dock33.Visibility = Visibility.Hidden;
            Dock3.Visibility = Visibility.Visible;
        }
        private void Switch_Small_Shift()
        {
            Dock00.Visibility = Visibility.Hidden;
            Dock0.Visibility = Visibility.Visible;
            Dock11.Visibility = Visibility.Hidden;
            Dock1.Visibility = Visibility.Visible;
            Dock22.Visibility = Visibility.Hidden;
            Dock2.Visibility = Visibility.Visible;
            Dock33.Visibility = Visibility.Hidden;
            Dock3.Visibility = Visibility.Visible;
        }
        private void Switch_Small_Shift_Digit_Big()
        {
            Dock0.Visibility = Visibility.Hidden;
            Dock00.Visibility = Visibility.Visible;
            Dock11.Visibility = Visibility.Hidden;
            Dock1.Visibility = Visibility.Visible;
            Dock22.Visibility = Visibility.Hidden;
            Dock2.Visibility = Visibility.Visible;
            Dock33.Visibility = Visibility.Hidden;
            Dock3.Visibility = Visibility.Visible;
        }

        private void Switch_Case()
        {
            // если включен CapsLock
            if (Keyboard.IsKeyToggled(Key.CapsLock))
            {
                // показывем кнопки верхнего регистра для всех, кроме кнопок цифрового ряда 
                Switch_Big();
                // если нажат Shift
                if (Keyboard.IsKeyDown(Key.LeftShift) ||
                    Keyboard.IsKeyDown(Key.RightShift))
                {
                    // переключаем регистр кнопок цифрового ряда
                    //if (Dock0.Visibility == Visibility.Visible)
                    //{
                        Dock0.Visibility = Visibility.Hidden;
                        Dock00.Visibility = Visibility.Visible;
                    //}
                    //else
                    //{
                    //    Dock00.Visibility = Visibility.Hidden;
                    //    Dock0.Visibility = Visibility.Visible;
                    //}
                    // переключаем на верхний регистр все остальные кнопки
                    Switch_Small();
                }
            }
            else
            {
                Switch_Small();
                if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                {
                    // переключаем регистр кнопки цифрового ряда
                    //if (Dock0.Visibility == Visibility.Visible)
                    //{
                    //    Dock0.Visibility = Visibility.Hidden;
                    //    Dock00.Visibility = Visibility.Visible;
                    //}
                    //else
                    //{
                        Dock00.Visibility = Visibility.Hidden;
                        Dock0.Visibility = Visibility.Visible;
                    //}
                    Switch_Big();
                }
            }

        }
        private void WritePanel_KeyDown(object sender, KeyEventArgs e)
        {
            // определяем кнопку по клавише
            Button tempButton = GetButton(e);
            // если кнопка определилась
            if (tempButton != null)
            {
                // меняем цвет фона на белый
                if (e.Key.ToString() != "Capital")
                {
                    tempButton.Background = Brushes.White;
                }
                // если включен CapsLock
                if (Keyboard.IsKeyToggled(Key.CapsLock))
                {
                    // если нажат Shift
                    if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                    {
                        Switch_Small_Shift_Digit_Big(); //регистр всех кнопок-нижний, цифровой ряд - верхний
                    }
                    else
                    {
                        Switch_Big(); // регистр всех кнопок, кроме цифрового ряда - верхний
                    }
                }
                else // если CapsLock выключен
                {
                    // если нажат Shift
                    if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                    {
                        Switch_Big_Shift(); // регистр всех кнопок - верхний
                    }
                    else
                    {
                        Switch_Small_Shift(); // регистр всех кнопок - нижний
                    }
                }
            }
        }
        private void WritePanel_KeyUp(object sender, KeyEventArgs e)
        {
            Button tempButton = GetButton(e);
            if (tempButton != null) 
            {
                tempButton.Background = tempButton.BorderBrush;
                // если включен CapsLock
                if (Keyboard.IsKeyToggled(Key.CapsLock))
                {
                    // если отжат Shift
                    if (Keyboard.IsKeyUp(Key.LeftShift) || Keyboard.IsKeyUp(Key.RightShift))
                    {
                        Switch_Big_Shift_Digit_Small(); // регистр всех кнопок - верхний, цифровой - нижний
                    }
                    else
                    {
                        Switch_Small_Shift(); // регистр всех кнопок - нижний
                    }
                }
                else // если CapsLock выключен
                {
                    // если отжат Shift
                    if (Keyboard.IsKeyUp(Key.LeftShift) || Keyboard.IsKeyUp(Key.RightShift))
                    {
                        Switch_Small_Shift(); // регистр всех кнопок - нижний
                    }
                    else
                    {
                        Switch_Big_Shift(); // регистр всех кнопок - верхний
                    }
                }
            }
            tempButton = null;
        }
        private void WritePanel_TextInpt(object sender, TextCompositionEventArgs e)
        {
            WritePanel.Text += e.Text;   
        }
    }
}

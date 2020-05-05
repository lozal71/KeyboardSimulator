﻿using System;
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
using System.Diagnostics;
using System.Threading;
using System.Security.Cryptography.X509Certificates;

namespace KeyboardSimulator
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public struct ButtonDouble
        {
            public Button self;
            public Button shift;
        }

        public bool flagStart;
        public bool flagError;
        public Stopwatch stopWatch;
        public TimeSpan ts;
        public int symbolsCounter;
        public int errCounter;
        public MainWindow()
        {
            InitializeComponent();
            if (Keyboard.IsKeyToggled(Key.CapsLock)) Switch_Big();
            else Switch_Small();
            flagStart = false;
            WritePanel.IsEnabled = false;
            btnStart.IsEnabled = true;
            btnStart.Opacity = 1.0;
            btnStop.IsEnabled = false;
            btnStop.Opacity = 0.5;
            stopWatch = new Stopwatch();
        }
        private ButtonDouble GetButtons(KeyEventArgs e)
        {
            ButtonDouble tempBtn;
            tempBtn.self = null;
            tempBtn.shift = null;
            bool flagBreak = false;
            string str;
            foreach (DockPanel dock in ButtonSet.Children)
            {
                foreach (Button btn in dock.Children)
                {
                    str = btn.Name.ToString().Split('_')[0];
                    if (e.Key.ToString() == btn.Name.ToString() ||
                        e.Key.ToString() + "_shift" == btn.Name.ToString())
                    {
                        if (!btn.Name.ToString().Contains("shift"))
                        {
                            tempBtn.self = btn;
                        }
                        else
                        {
                            tempBtn.shift = btn;
                        }
                        if (tempBtn.self != null && tempBtn.shift != null)
                        {
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
            Dock1_shift.Visibility = Visibility.Visible;
            Dock2.Visibility = Visibility.Hidden;
            Dock2_shift.Visibility = Visibility.Visible;
            Dock3.Visibility = Visibility.Hidden;
            Dock3_shift.Visibility = Visibility.Visible;
        }
        private void Switch_Big_Shift()
        {
            Dock0.Visibility = Visibility.Hidden;
            Dock0_shift.Visibility = Visibility.Visible;
            Dock1.Visibility = Visibility.Hidden;
            Dock1_shift.Visibility = Visibility.Visible;
            Dock2.Visibility = Visibility.Hidden;
            Dock2_shift.Visibility = Visibility.Visible;
            Dock3.Visibility = Visibility.Hidden;
            Dock3_shift.Visibility = Visibility.Visible;
        }
        private void Switch_Big_Shift_Digit_Small()
        {
            Dock0_shift.Visibility = Visibility.Hidden;
            Dock0.Visibility = Visibility.Visible;
            Dock1.Visibility = Visibility.Hidden;
            Dock1_shift.Visibility = Visibility.Visible;
            Dock2.Visibility = Visibility.Hidden;
            Dock2_shift.Visibility = Visibility.Visible;
            Dock3.Visibility = Visibility.Hidden;
            Dock3_shift.Visibility = Visibility.Visible;
        }
        private void Switch_Small()
        {
            Dock1_shift.Visibility = Visibility.Hidden;
            Dock1.Visibility = Visibility.Visible;
            Dock2_shift.Visibility = Visibility.Hidden;
            Dock2.Visibility = Visibility.Visible;
            Dock3_shift.Visibility = Visibility.Hidden;
            Dock3.Visibility = Visibility.Visible;
        }
        private void Switch_Small_Shift()
        {
            Dock0_shift.Visibility = Visibility.Hidden;
            Dock0.Visibility = Visibility.Visible;
            Dock1_shift.Visibility = Visibility.Hidden;
            Dock1.Visibility = Visibility.Visible;
            Dock2_shift.Visibility = Visibility.Hidden;
            Dock2.Visibility = Visibility.Visible;
            Dock3_shift.Visibility = Visibility.Hidden;
            Dock3.Visibility = Visibility.Visible;
        }
        private void Switch_Small_Shift_Digit_Big()
        {
            Dock0.Visibility = Visibility.Hidden;
            Dock0_shift.Visibility = Visibility.Visible;
            Dock1_shift.Visibility = Visibility.Hidden;
            Dock1.Visibility = Visibility.Visible;
            Dock2_shift.Visibility = Visibility.Hidden;
            Dock2.Visibility = Visibility.Visible;
            Dock3_shift.Visibility = Visibility.Hidden;
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
                    Dock0_shift.Visibility = Visibility.Visible;
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
                    Dock0_shift.Visibility = Visibility.Hidden;
                    Dock0.Visibility = Visibility.Visible;
                    //}
                    Switch_Big();
                }
            }

        }
        private void WritePanel_KeyDown(object sender, KeyEventArgs e)
        {
            if (flagStart)
            {
                // определяем кнопку по клавише
                Button tempButton = GetButton(e);
                // если кнопка определилась
                if (tempButton != null)
                {
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
                    // если это не кнопка CapsLock
                    if (e.Key.ToString() != "Capital" &&
                        !e.Key.ToString().Contains("Ctrl") &&
                        !e.Key.ToString().Contains("Win") &&
                        !e.Key.ToString().Contains("Alt"))
                    {
                        // меняем цвет фона на белый
                        tempButton.Background = Brushes.White;
                    }

                }
            }
        }
        private void WritePanel_KeyUp(object sender, KeyEventArgs e)
        {
            if (flagStart)
            {
                ButtonDouble tempButton = GetButtons(e);
                if (tempButton.self != null && tempButton.shift != null)
                {
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
                    // возвращаем цвет фона
                    tempButton.self.Background = tempButton.self.BorderBrush;
                    tempButton.shift.Background = tempButton.shift.BorderBrush;
                }
                tempButton.self = null;
                tempButton.shift = null;
            }
        }

        private void WritePanel_PreviwTextInput(object sender, TextCompositionEventArgs e)
        {
            if (flagStart)
            {
                //x = ReadPanel.Text[symbolsCounter];
                //y = e.Text[0];
                if (ReadPanel.Text.Length > symbolsCounter)
                {
                    if (ReadPanel.Text[symbolsCounter] != e.Text[0])
                    {
                        WritePanel.Foreground = Brushes.Orange;
                        //WritePanel.FontStyle = FontStyles.Italic;
                        errCounter++;
                    }
                    else
                    {
                        WritePanel.Foreground = Brushes.Black;
                        //WritePanel.FontStyle = FontStyles.Normal;
                    }
                    symbolsCounter++;
                }
                else
                {
                    btnStop.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                }

            }
        }

        private void WritePanel_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (ReadPanel.Text.Length > symbolsCounter)
            {
                if (e.Key == Key.Space && ReadPanel.Text[symbolsCounter] != ' ')
                {
                    WritePanel.Foreground = Brushes.Orange;
                    symbolsCounter++;
                    errCounter++;
                }
                else if (e.Key == Key.Space && ReadPanel.Text[symbolsCounter] == ' ')
                {
                    WritePanel.Foreground = Brushes.Black;
                    symbolsCounter++;
                }
            }
            else
            {
                btnStop.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            flagStart = true;
            WritePanel.Text = "";
            ReadPanel.Text = "I love you I love you I love you I love you I love you I love you";
            btnStart.IsEnabled = false;
            btnStart.Opacity = 0.5;
            btnStop.IsEnabled = true;
            btnStop.Opacity = 1.0;
            WritePanel.IsEnabled = true;
            WritePanel.Focus();
            stopWatch.Start();
            symbolsCounter = 0;
            errCounter = 0;
        }
        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            int totalTime;
            flagStart = false;
            btnStart.IsEnabled = true;
            btnStart.Opacity = 1.0;
            btnStop.IsEnabled = false;
            btnStop.Opacity = 0.5;
            WritePanel.IsEnabled = false;
            WritePanel.Focus();
            stopWatch.Stop();
            ts = stopWatch.Elapsed;
            Fails.Text = errCounter.ToString();
            totalTime = ts.Hours * 60 + ts.Minutes;
            if (totalTime != 0)
            {
                Speed.Text = ((int)symbolsCounter / totalTime).ToString();
            }
            else
            {
                Speed.Text = symbolsCounter.ToString();
            }
        }
    }
}

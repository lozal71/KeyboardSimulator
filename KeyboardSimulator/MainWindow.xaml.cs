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
        string[] exercises;
        /// <summary>
        /// составная кнопка нижнего и верхнего регистров
        /// </summary>
        public struct ButtonDouble  
        {
            /// <summary>
            ///  кнопка нижнего регистра
            /// </summary>
            public Button self;
            /// <summary>
            /// кнопка верхнего регистра
            /// </summary>
            public Button shift;  
        }
        /// <summary>
        /// признак нажатия на кнопку "Старт"
        /// </summary>
        public bool flagStart;
        /// <summary>
        /// признак нажатия на клавишу клавиатуры Backspace
        /// </summary>
        public bool flagBack;   
        /// <summary>
        ///  счетчик времени работы тренажера
        /// </summary>
        public Stopwatch stopWatch; 
        public TimeSpan ts;  // таймер
        /// <summary>
        /// счетчик введенных символов
        /// </summary>
        public int symbolsCounter;
        /// <summary>
        /// счетчик ошибок
        /// </summary>
        public int errCounter; 
        /// <summary>
        /// начальная инициализация
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            exercises = new string[] { "i love you.", "i like it.", "hu are you?",
                                       "i am traveling down the river"};
            // если нажата клавиша CapsLock
            if (Keyboard.IsKeyToggled(Key.CapsLock))
            {
                // показываем кнопки в режиме CapsLock (все буквы в верхнем регистре и цифры)
                Letters_Big_Digit(); 

            }
            else
            {
                // показывем кнопки в обычном режиме (все буквы в нижнем регистре и цифры)
                Letters_Small_Digit(); 
            }
            // при запуске
            flagStart = false; // кнопка "Старт" ненажата
            flagBack = false; // кнопка "Backspace" ненажата
            WritePanel.IsEnabled = false; // панель ввода текста - недоступна
            btnStart.IsEnabled = true; // кнопка "Старт" доступна
            btnStart.Opacity = 1.0;    // кнопка "Старт" - непрозрачна
            btnStop.IsEnabled = false; // кнопка "Стоп" - недоступна
            btnStop.Opacity = 0.5;     // кнопка "Стоп" - полупрозрачна 
            stopWatch = new Stopwatch();  // объект для отсчета времени работы тренажера
        }
        /// <summary>
        /// определяет какие кнопки соответствуют нажатой клавише на клавиатуре
        /// </summary>
        /// <param name="e"> объект события нажатия клавиши на клавиатуре </param>
        /// <returns> возвращает кнопки нижнего и верхнего регистра </returns>
        private ButtonDouble GetButtons(KeyEventArgs e)
        {
            ButtonDouble tempBtn;
            tempBtn.self = null;
            tempBtn.shift = null;
            bool flagBreak = false; // признак прерывания цикла
            // перебираем все dockpanel из ButtonSet
            foreach (DockPanel dock in ButtonSet.Children)
            {
                // перебираем все кнопки на каждой панели
                foreach (Button btn in dock.Children)
                {
                    // если название клавиши на клавиатуре совпадает 
                    // с названием кнопки верхнего или нижнего регистров
                    if (e.Key.ToString() == btn.Name.ToString() ||
                        e.Key.ToString() + "_shift" == btn.Name.ToString())
                    {
                        // если это кнопка нижнего регистра (в названии нет признака "_shift")
                        if (!btn.Name.ToString().Contains("shift"))
                        {
                            tempBtn.self = btn; // фиксируем кнопку нижнего регистра
                        }
                        else // если это кнопка верхнего регистра (в названии есть признак "_shift")
                        {
                            tempBtn.shift = btn; // фиксируем кнопку верхнего регистра
                        }
                        // если кнопки в обоих регистрах определены
                        if (tempBtn.self != null && tempBtn.shift != null)
                        {
                            // прерываем циклы перебора
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
        /// <summary>
        /// параметры видимости dock - панелей установливаются так,
        /// чтобы буквенные ряды были видны в верхнем регистре,
        /// в цифровом ряду были видны символы
        /// </summary>
        private void Letters_Big_Symbols()
        {
            Dock0.Visibility = Visibility.Hidden;
            Dock1.Visibility = Visibility.Hidden;
            Dock2.Visibility = Visibility.Hidden;
            Dock3.Visibility = Visibility.Hidden;

            Dock0_shift.Visibility = Visibility.Visible;
            Dock1_shift.Visibility = Visibility.Visible;
            Dock2_shift.Visibility = Visibility.Visible;
            Dock3_shift.Visibility = Visibility.Visible;
        }
        /// <summary>
        /// параметры видимости dock - панелей установливаются так,
        /// чтобы буквенные ряды были видны в верхнем регистре,
        /// в цифровом ряду были видны цифры
        /// </summary>
        private void Letters_Big_Digit()
        {
            Dock0_shift.Visibility = Visibility.Hidden;
            Dock1_shift.Visibility = Visibility.Visible;
            Dock2_shift.Visibility = Visibility.Visible;
            Dock3_shift.Visibility = Visibility.Visible;

            Dock0.Visibility = Visibility.Visible;
            Dock1.Visibility = Visibility.Hidden;
            Dock2.Visibility = Visibility.Hidden;
            Dock3.Visibility = Visibility.Hidden;

        }
        /// <summary>
        /// параметры видимости dock - панелей установливаются так,
        /// чтобы буквенные ряды были видны в нижнем регистре,
        /// в цифровом ряду были видны цифры
        /// </summary>
        private void Letters_Small_Digit()
        {
            Dock0_shift.Visibility = Visibility.Hidden;
            Dock1_shift.Visibility = Visibility.Hidden;
            Dock2_shift.Visibility = Visibility.Hidden;
            Dock3_shift.Visibility = Visibility.Hidden;

            Dock0.Visibility = Visibility.Visible; 
            Dock1.Visibility = Visibility.Visible;
            Dock2.Visibility = Visibility.Visible;
            Dock3.Visibility = Visibility.Visible;
        }
        /// <summary>
        /// параметры видимости dock - панелей установливаются так,
        /// чтобы буквенные ряды были видны в верхнем регистре,
        /// в цифровом ряду были видны символы
        /// </summary>
        private void Letters_Small_Symbols()
        {
            Dock0.Visibility = Visibility.Hidden;
            Dock1.Visibility = Visibility.Visible;
            Dock2.Visibility = Visibility.Visible;
            Dock3.Visibility = Visibility.Visible;

            Dock0_shift.Visibility = Visibility.Visible;
            Dock1_shift.Visibility = Visibility.Hidden;
            Dock2_shift.Visibility = Visibility.Hidden;
            Dock3_shift.Visibility = Visibility.Hidden;
 
        }
        /// <summary>
        /// обработка события KeyDown дла панели ввода WritePanel
        /// для отслеживания нажатия на клавиши CapsLock и Shift
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WritePanel_KeyDown(object sender, KeyEventArgs e)
        {
            if (flagStart) // если нажата кнопка "Старт"
            {
                // определяем кнопку по клавише
                ButtonDouble tempButton = GetButtons(e);
                // если кнопка определилась
                if (tempButton.self != null && tempButton.shift != null)
                {
                    // если включен CapsLock
                    if (Keyboard.IsKeyToggled(Key.CapsLock))
                    {
                        // если нажат Shift
                        if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                        {
                            Letters_Small_Symbols(); //регистр букв - нижний, цифровой ряд - символы
                        }
                        else
                        {
                            Letters_Big_Digit(); // регистр букв - верхний, цифровой ряд - цифры
                        }
                    }
                    else // если CapsLock выключен
                    {
                        // если нажат Shift
                        if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                        {
                            Letters_Big_Symbols(); // регистр букв - верхний, цифровой ряд - символы
                        }
                        else
                        {
                            Letters_Small_Digit(); // регистр букв - нижний, цифровой ряд - цифры
                        }
                    }
                    // если это не клавиши CapsLock, Ctrl, Win, Alt
                    if (e.Key.ToString() != "Capital" &&
                        !e.Key.ToString().Contains("Ctrl") &&
                        !e.Key.ToString().Contains("Win") &&
                        !e.Key.ToString().Contains("Alt"))
                    {
                        // меняем цвет фона на белый
                        tempButton.self.Background = Brushes.White;
                        tempButton.shift.Background = Brushes.White;
                    }

                }
            }
        }
        /// <summary>
        /// обработка события KeyUp для панели ввода WritePanel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WritePanel_KeyUp(object sender, KeyEventArgs e)
        {
            if (flagStart) // если нажата кнопка "Старт"
            {   
                // определяем кнопку по клавише
                ButtonDouble tempButton = GetButtons(e);
                // если кнопка определилась
                if (tempButton.self != null && tempButton.shift != null)
                {
                    // если включен CapsLock
                    if (Keyboard.IsKeyToggled(Key.CapsLock))
                    {
                        // если отжат Shift
                        if (Keyboard.IsKeyUp(Key.LeftShift) || Keyboard.IsKeyUp(Key.RightShift))
                        {
                            Letters_Big_Digit(); // регистр букв  - верхний, цифровой ряд - цифры
                        }
                        else
                        {
                            Letters_Small_Digit(); // регистр букв - нижний, цифровой ряд - цифры
                        }
                    }
                    else // если CapsLock выключен
                    {
                        // если отжат Shift
                        if (Keyboard.IsKeyUp(Key.LeftShift) || Keyboard.IsKeyUp(Key.RightShift))
                        {
                            Letters_Small_Digit(); // регистр букв - нижний, цифровой ряд - цифры
                        }
                        else
                        {
                            Letters_Big_Symbols(); // регистр букв - верхний, цифровой ряд - сиволы
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
        /// <summary>
        /// обработка события PreviwTextInput для панели ввода WritePanel
        /// для определения правильности ввода и подсчета введенных символов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WritePanel_PreviwTextInput(object sender, TextCompositionEventArgs e)
        {
            // если была нажа кнопка "Старт"
            if (flagStart)
            {
                // если счетчик введенных символов не превысил длину вводимой строки
                if (ReadPanel.Text.Length > symbolsCounter)
                {
                    // если не была нажата клавиша клавиатуры "Backspace"
                    if (!flagBack)
                    {
                        // если ввод неверный
                        if (ReadPanel.Text[symbolsCounter] != e.Text[0])
                        {
                            // устанавливаем цвет режима неверного ввода
                            WritePanel.Foreground = Brushes.Orange;
                            errCounter++; // считаем ошибку
                        }
                        else 
                        {
                            // устанавливаем цвет режима правильного ввода
                            WritePanel.Foreground = Brushes.Black;
                        }
                        // считаем введенные символы
                        symbolsCounter++;
                    }
                }
                // если счетчик введенных символов равен длине вводимой строки
                else
                {
                    // эмулируем событие нажатия на кнопку "Стоп"
                    btnStop.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                }

            }
        }
        /// <summary>
        /// обработка события PreviewKeyDown для панели ввода WritePanel
        /// для отслеживания нажатия клавиш "Space" и "Backspace"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WritePanel_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // если не нажата клавиша клавиатуры "Backspace"
            if (e.Key != Key.Back)
            {
                flagBack = false;
                // если счетчик введенных символов не превысил длину вводимой строки
                if (ReadPanel.Text.Length > symbolsCounter)
                {
                    // если нажата клавиша клавиатуры "Space", но нужно было нажать не "Space"
                    if (e.Key == Key.Space && ReadPanel.Text[symbolsCounter] != ' ')
                    {
                        // устанавливаем цвет режима неверного ввода
                        WritePanel.Foreground = Brushes.Orange;
                        // считаем ошибку
                        errCounter++;
                        // считаем "Space" как введенный символ
                        symbolsCounter++;
                    }
                    // если нажата клавиша клавиатуры "Space", и нужно было нажать "Space"
                    else if (e.Key == Key.Space && ReadPanel.Text[symbolsCounter] == ' ')
                    {
                        // устанавливаем цвет режима правильного ввода
                        WritePanel.Foreground = Brushes.Black;
                        // считаем "Space" как введенный символ
                        symbolsCounter++;
                    }
                }
                // если счетчик введенных символов равен длине вводимой строки
                else
                {
                    // эмулируем событие нажатия на кнопку "Стоп"
                    btnStop.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                }
            }
            // если нажата клавиша клавиатуры "Backspace"
            else
            {
                // уменьшаем счетчик нажатия клавиши и счетчик ошибок
                flagBack = true;
                if (symbolsCounter !=0) symbolsCounter--;
                if (errCounter != 0) errCounter--;
            }
        }
        /// <summary>
        /// обработка события нажатия на кнопку "Старт"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            var rand = new Random();
            int n = rand.Next(4);
            string phrase = exercises[n];
            string nPhrase = "";
            if (Registr_cbx.IsChecked == true)
            {
                phrase = phrase.Replace(phrase.Substring(0, 1), phrase.Substring(0, 1).ToUpper());
            }
            flagStart = true;  // включаем признак нажатия кнопки "Старт"
            // панель ввода: очищаем, делаем доступной и даем фокус
            WritePanel.Text = ""; 
            WritePanel.IsEnabled = true;
            WritePanel.Focus();
            // заполняем панель чтения в зависимости от уровня сложности
            for (int i = 1; i <= Difficulty.Value; i++)
            {
                nPhrase += " " + phrase;
            }
            ReadPanel.Text = nPhrase;
            // переключаем кнопку "Старт" - недоступна и полупрозрачна
            btnStart.IsEnabled = false;
            btnStart.Opacity = 0.5;
            // переключаем кнопку "Стоп" - доступна и непрозрачна
            btnStop.IsEnabled = true;
            btnStop.Opacity = 1.0;
            // стартуем таймер
            stopWatch.Start();
            // обнуляем счетчики введенных символов и ошибок
            symbolsCounter = 0;
            errCounter = 0;
            // очищаем поля скорости и ошибок
            Speed.Text = "";
            Fails.Text = "";
        }
        /// <summary>
        /// обработка события нажатия на кнопку "Стоп"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            int totalTime; // общее время работы между нажатем кнопок "Старт" и "Стоп"
            flagStart = false; // выключаем признак нажатия кнопки "Старт"
            // переключаем кнопку "Старт" - доступна и непрозрачна
            btnStart.IsEnabled = true;
            btnStart.Opacity = 1.0;
            // переключаем кнопку "Стоп" - недоступна и полупрозрачна
            btnStop.IsEnabled = false;
            btnStop.Opacity = 0.5;
            // панель ввода: делаем недоступной и даем фокус
            WritePanel.IsEnabled = false;
            WritePanel.Focus();
            // остановка таймера и фиксация времени в минутах
            stopWatch.Stop();
            ts = stopWatch.Elapsed;
             totalTime = ts.Hours * 60 + ts.Minutes;
            // вычисление скорости набора символов и запись этой информации в поле "Speed"
            if (totalTime != 0)
            {
                Speed.Text = ((int)symbolsCounter / totalTime).ToString();
            }
            else
            {
                Speed.Text = symbolsCounter.ToString();
            }
            // запись значения счетчика ошибок в поле "Fails"
            Fails.Text = errCounter.ToString();
        }
    }
}

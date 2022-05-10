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
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace MasterMindWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Button> solutionColors = new List<Button>();
        private Dictionary<CheckBox, Button> keyValues;
        private bool[] isFilledOut;
        private Button[][] buttons = new Button[10][];
        private Button[][] responses = new Button[10][];
        private int counter = 0;
        private Dictionary<Brush, Stim> buttonStimPairs = new Dictionary<Brush, Stim>();
        public MainWindow()
        {
            InitializeComponent();

            keyValues = new Dictionary<CheckBox, Button>()
            {
                { cb_blue, b_blue },
                { cb_green, b_green },
                { cb_purple, b_purple },
                { cb_orange, b_orange },
                { cb_red, b_red },
                { cb_pink, b_pink  }
            };

            isFilledOut = new bool[10];
            for (int i = 0; i < isFilledOut.Length; i++)
                isFilledOut[i] = false;

            GenerateColors();

            solution_1.Background = solutionColors[0].Background;
            solution_2.Background = solutionColors[1].Background;
            solution_3.Background = solutionColors[2].Background;
            solution_4.Background = solutionColors[3].Background;
        }

        //Ez biztosítja, hogyha a színre kattintunk, akkor is pipálódjon be csakis a megfelelő CheckBox
        private void b_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            keyValues.Keys.Where(_ => _.Name.Split('_')[1] == b.Name.Split('_')[1]).ToList().ForEach(_ => _.IsChecked = true);
            keyValues.Keys.Where(_ => _.Name.Split('_')[1] != b.Name.Split('_')[1]).ToList().ForEach(_ => _.IsChecked = false);
        }

        //Ez biztosítja, hogy egyszerre csak 1 CheckBox legyen bepipálva
        private void cb_Click(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            keyValues.Keys.Where(_ => _.Name.Split('_')[1] != cb.Name.Split('_')[1]).ToList().ForEach(_ => _.IsChecked = false);
        }

        private void b_Tip_Click(object sender, RoutedEventArgs e)
        {
            //Ha nincs kiválasztva semmi, akkor return
            if (!keyValues.Keys.Any(_ => _.IsChecked == true))
                return;

            if(buttons.Any(_ => _ is null))
            {
                IEnumerable<Button> tmp = FindVisualChildren<Button>(Application.Current.MainWindow);
                tmp = tmp.Where(_ => _.Name.Contains('_')).Where(_ => _.Name.Split('_')[1] == 1.ToString());

                //Tömb feltöltése a valós buttonokkal
                for (int i = 0; i < buttons.Length; i++)
                {
                    buttons[i] = FindVisualChildren<Button>(Application.Current.MainWindow).Where(_ => _.Name.Contains('_') && _.Name.StartsWith("b")).Where(_ => _.Name.Split('_')[1].Equals((i + 1).ToString())).ToArray();
                }
            }

            Button b = sender as Button;
            //Gomb kiszinezése a kiválasztott színra
            foreach (var item in keyValues)
            {
                if (item.Key.IsChecked == true)
                    b.Background = item.Value.Background;
            }

            for (int i = 0; i < buttons.Length; i++)
            {
                for (int j = 0; j < buttons[i].Length; j++)
                {
                    if (buttons[i][j].Background == Brushes.White)
                        return;
                }
                counter = 0;
                //Hogyha már egy sorban lévő összes gomb kapott színt, akkor a következő sorra lépés
                for (int j = 0; j < buttons[i].Length; j++)
                {
                    buttons[i][j].IsEnabled = false;
                    buttons[i + 1][j].IsEnabled = true;
                    React(buttons[i][j]);
                }

                foreach (var item in buttonStimPairs.Values)
                {
                    if(item == Stim.Perfect)
                    {
                        responses[i][counter].Visibility = Visibility.Visible;
                        responses[i][counter].Background = Brushes.Black;
                        counter++;
                    }
                    else if (item == Stim.JustColor)
                    {
                        responses[i][counter].Visibility = Visibility.Visible;
                        responses[i][counter].Background = Brushes.White;
                        counter++;
                    }
                }
                buttonStimPairs.Clear();
            }
        }

        private void React(Button button)
        {
            if (responses.Any(_ => _ is null))
            {
                IEnumerable<Button> tmp = FindVisualChildren<Button>(Application.Current.MainWindow);
                tmp = tmp.Where(_ => _.Name.Contains('_') && _.Name.StartsWith("v")).Where(_ => _.Name.Split('_')[1].Equals((1).ToString())).ToArray();
                for (int i = 0; i < responses.Length; i++)
                {
                    responses[i] = FindVisualChildren<Button>(Application.Current.MainWindow).Where(_ => _.Name.Contains('_') && _.Name.StartsWith("v")).Where(_ => _.Name.Split('_')[1].Equals((i + 1).ToString())).ToArray();
                }
            }

            //Van többszörös előfordulás
            //if (buttons[Convert.ToInt32(button.Name.Split('_')[1]) - 1].Select(_ => _.Background).Distinct().Count() != 4)
            //{
            //    for (int i = 0; i < 4; i++)
            //    {
            //        //Logic to put black (amikor van benne és jó helyen is van)
            //        if (button.Background == solutionColors[i].Background && (i + 1) == Convert.ToInt32(button.Name.Split('_')[2]) && !markBlack)
            //        {
            //            markBlack = true;
            //            break;
            //        }
            //        //Logic to put white
            //        else if (button.Background == solutionColors[i].Background && !markWhite)
            //        {
            //            markWhite = true;
            //            break;
            //        }
            //    }
            //    if (markBlack)
            //    {
            //        responses[Convert.ToInt32(button.Name.Split('_')[1]) - 1][counter].Visibility = Visibility.Visible;
            //        responses[Convert.ToInt32(button.Name.Split('_')[1]) - 1][counter].Background = Brushes.Black;
            //        counter++;
            //    }
            //    else if (markWhite)
            //    {
            //        responses[Convert.ToInt32(button.Name.Split('_')[1]) - 1][counter].Visibility = Visibility.Visible;
            //        responses[Convert.ToInt32(button.Name.Split('_')[1]) - 1][counter].Background = Brushes.White;
            //        counter++;
            //    }

            //}
            ////A solutionben nincs többszörös előfordulás de a sorban van
            //else if(solutionColors.Select(_ => _.Background).Distinct().Count() != 4)
            //{

            //}
            ////A sorban nincs többszörös előfordulás de a solutionben van
            //else if (buttons[Convert.ToInt32(button.Name.Split('_')[1]) - 1].Select(_ => _.Background).Distinct().Count() != 4)
            //{

            //}
            ////Egyikben sincs többszörös előfordulás
            //else
            //{
            //    for (int i = 0; i < 4; i++)
            //    {
            //        //Logic to put black (amikor van benne és jó helyen is van)
            //        if (button.Background == solutionColors[i].Background && (i + 1) == Convert.ToInt32(button.Name.Split('_')[2]))
            //        {
            //            responses[Convert.ToInt32(button.Name.Split('_')[1]) - 1][counter].Visibility = Visibility.Visible;
            //            responses[Convert.ToInt32(button.Name.Split('_')[1]) - 1][counter].Background = Brushes.Black;
            //            counter++;
            //            break;
            //        }
            //        //Logic to put white
            //        else if (button.Background == solutionColors[i].Background)
            //        {
            //            responses[Convert.ToInt32(button.Name.Split('_')[1]) - 1][counter].Visibility = Visibility.Visible;
            //            responses[Convert.ToInt32(button.Name.Split('_')[1]) - 1][counter].Background = Brushes.White;
            //            counter++;
            //            break;
            //        }
            //    }
            //}

            
            for (int i = 0; i < 4; i++)
            {
                if (button.Background == solutionColors[i].Background)
                {
                    if (!buttonStimPairs.Keys.Contains(button.Background))
                    {
                        buttonStimPairs.Add(button.Background, Stim.JustColor);

                        if ((i + 1) == Convert.ToInt32(button.Name.Split('_')[2]))
                            buttonStimPairs[button.Background] = Stim.Perfect;
                    }
                    else if((i + 1) == Convert.ToInt32(button.Name.Split('_')[2]))
                        buttonStimPairs[button.Background] = Stim.Perfect;
                    break;
                }
            }
            //}
        }

        //A valós obejktumokat(mint pl. gombok) adja vissza a paraméterként átadott objektumbol
        private static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj == null)
                yield break;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                var child = VisualTreeHelper.GetChild(depObj, i);

                if (child != null && child is T)
                    yield return (T)child;

                foreach (T childOfChild in FindVisualChildren<T>(child))
                    yield return childOfChild;
            }
        }

        //Random színek generálása
        private void GenerateColors()
        {
            Random random = new Random();
            List<int> rndNums = new List<int>();

            for (int i = 0; i < 4; i++)
            {
                int x;
                do
                {
                    x = random.Next(1, 7);
                } while (rndNums.Contains(x));
                rndNums.Add(x);

                switch (x)
                {
                    case 1:
                        solutionColors.Add(b_blue);
                        break;
                    case 2:
                        solutionColors.Add(b_green);
                        break;
                    case 3:
                        solutionColors.Add(b_purple);
                        break;
                    case 4:
                        solutionColors.Add(b_orange);
                        break;
                    case 5:
                        solutionColors.Add(b_red);
                        break;
                    case 6:
                        solutionColors.Add(b_pink);
                        break;
                }
            }
        }
    }

    public enum Stim
    {
        Perfect,
        JustColor,
        No
    }

    //Adatbeli változások azonnali megjelenítése
    //public class User : INotifyPropertyChanged
    //{
    //    private string name;

    //    public string Name
    //    {
    //        get { return name; }
    //        set {
    //                if(name != value)
    //                {
    //                    name = value;
    //                    NotifyPropertyChanged("Name");
    //                }
    //             }
    //    }


    //    public event PropertyChangedEventHandler PropertyChanged;

    //    public void NotifyPropertyChanged(string propName)
    //    {
    //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    //    }
    //}
}

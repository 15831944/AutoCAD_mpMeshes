using AcApp = Autodesk.AutoCAD.ApplicationServices.Core.Application;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Autodesk.AutoCAD.Internal;
using Autodesk.AutoCAD.Runtime;
using ModPlus.Helpers;
using ModPlusAPI;
using ModPlusAPI.Windows;
using Exception = System.Exception;

namespace mpMeshes
{
    public partial class MpMeshes
    {
        private const string LangItem = "mpMeshes";

        private double _ep = 0.001;
        /// <summary>Инициализация окна</summary>
        public MpMeshes()
        {
            InitializeComponent();
            Title = ModPlusAPI.Language.GetItem(LangItem, "h1");
            //
            LabelName.Text = ModPlusAPI.Language.GetItem(LangItem, "h35");
            // Заполняем значения первой вкладки
            FillFirstTab();
            // second
            SecondFill();
            // third
            ThirdFill();
        }
        #region MainWindow
        private void TabItem_GotFocus(object sender, RoutedEventArgs e)
        {
            LabelName.Text = ModPlusAPI.Language.GetItem(LangItem, "h35");
        }

        private void TabItem_GotFocus_1(object sender, RoutedEventArgs e)
        {
            LabelName.Text = ModPlusAPI.Language.GetItem(LangItem, "h36");
        }

        private void TabItem_GotFocus_2(object sender, RoutedEventArgs e)
        {
            LabelName.Text = ModPlusAPI.Language.GetItem(LangItem, "h37");
        }

        private void Window_MouseLeave(object sender, MouseEventArgs e)
        {
            Utils.SetFocusToDwgView();
        }

        private void Window_MouseEnter(object sender, MouseEventArgs e)
        {
            Focus();
        }
        // Запрет нажатия пробела в текстовом поле
        public void TbNoSpace_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }
        // Ввод в текстовое поле только чисел
        public void TextBoxOnlyNumbers_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!short.TryParse(e.Text, out _))
            {
                e.Handled = true;
            }
        }
        
        private void MpMeshes_OnLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                const UserConfigFile.ConfigFileZone zone = UserConfigFile.ConfigFileZone.Settings;
                const string f = "mpMeshes";
                // first
                CbFirstMeshType.SelectedIndex = int.TryParse(UserConfigFile.GetValue(zone, f, nameof(CbFirstMeshType)), out var i) ? i : 0;
                TbFirstMeshWidth.Text = UserConfigFile.GetValue(zone, f, nameof(TbFirstMeshWidth));
                TbFirstMeshLength.Text = UserConfigFile.GetValue(zone, f, nameof(TbFirstMeshLength));
                CbFirstLongitudinal.SelectedItem = UserConfigFile.GetValue(zone, f, nameof(CbFirstLongitudinal));
                CbFirstLongitudinalClass.SelectedItem = UserConfigFile.GetValue(zone, f, nameof(CbFirstLongitudinalClass));
                CbFirstTransverse.SelectedItem = UserConfigFile.GetValue(zone, f, nameof(CbFirstTransverse));
                CbFirstTransverseCalss.SelectedItem = UserConfigFile.GetValue(zone, f, nameof(CbFirstTransverseCalss));
                CbFirstLongitudinalStep.SelectedItem = UserConfigFile.GetValue(zone, f, nameof(CbFirstLongitudinalStep));
                CbFirstTransverseStep.SelectedItem = UserConfigFile.GetValue(zone, f, nameof(CbFirstTransverseStep));
                slider1.Value = double.TryParse(UserConfigFile.GetValue(zone, f, nameof(slider1)), out var d) ? d : 3.0;
                // second
                CbSecondMeshType.SelectedIndex = int.TryParse(UserConfigFile.GetValue(zone, f, nameof(CbSecondMeshType)), out i) ? i : 0;
                TbSecondMeshLength.Text = UserConfigFile.GetValue(zone, f, nameof(TbSecondMeshLength));
                CbSecondMainStep.SelectedItem = UserConfigFile.GetValue(zone, f, nameof(CbSecondMainStep));
                slider2.Value = double.TryParse(UserConfigFile.GetValue(zone, f, nameof(slider2)), out d) ? d : 3.0;
                // third
                CbThirdMeshType.SelectedIndex = int.TryParse(UserConfigFile.GetValue(zone, f, nameof(CbThirdMeshType)), out i) ? i : 0;
                CbThirdMeshLength.SelectedIndex = int.TryParse(UserConfigFile.GetValue(zone, f, nameof(CbThirdMeshLength)), out i) ? i : 0;
                CbThirdMeshWidth.SelectedIndex = int.TryParse(UserConfigFile.GetValue(zone, f, nameof(CbThirdMeshWidth)), out i) ? i : 0;
                CbThirdDiamsRatio.SelectedIndex = int.TryParse(UserConfigFile.GetValue(zone, f, nameof(CbThirdDiamsRatio)), out i) ? i : 0;
                slider3.Value = double.TryParse(UserConfigFile.GetValue(zone, f, nameof(slider3)), out d) ? d : 3.0;
            }
            catch (Exception exception)
            {
                ExceptionBox.Show(exception);
            }
        }

        private void MpMeshes_OnClosed(object sender, EventArgs e)
        {
            try
            {
                const UserConfigFile.ConfigFileZone zone = UserConfigFile.ConfigFileZone.Settings;
                const string f = "mpMeshes";
                // first
                UserConfigFile.SetValue(zone, f, nameof(CbFirstMeshType), CbFirstMeshType.SelectedIndex.ToString(), false);
                UserConfigFile.SetValue(zone, f, nameof(TbFirstMeshWidth), TbFirstMeshWidth.Text, false);
                UserConfigFile.SetValue(zone, f, nameof(TbFirstMeshLength), TbFirstMeshLength.Text, false);
                UserConfigFile.SetValue(zone, f, nameof(CbFirstLongitudinal), CbFirstLongitudinal.SelectedItem.ToString(), false);
                UserConfigFile.SetValue(zone, f, nameof(CbFirstLongitudinalClass), CbFirstLongitudinalClass.SelectedItem.ToString(), false);
                UserConfigFile.SetValue(zone, f, nameof(CbFirstTransverse), CbFirstTransverse.SelectedItem.ToString(), false);
                UserConfigFile.SetValue(zone, f, nameof(CbFirstTransverseCalss), CbFirstTransverseCalss.SelectedItem.ToString(), false);
                UserConfigFile.SetValue(zone, f, nameof(CbFirstLongitudinalStep), CbFirstLongitudinalStep.SelectedItem.ToString(), false);
                UserConfigFile.SetValue(zone, f, nameof(CbFirstTransverseStep), CbFirstTransverseStep.SelectedItem.ToString(), false);
                UserConfigFile.SetValue(zone, f, nameof(slider1), slider1.Value.ToString(CultureInfo.InvariantCulture), false);
                // second
                UserConfigFile.SetValue(zone, f, nameof(CbSecondMeshType), CbSecondMeshType.SelectedIndex.ToString(), false);
                UserConfigFile.SetValue(zone, f, nameof(TbSecondMeshLength), TbSecondMeshLength.Text, false);
                UserConfigFile.SetValue(zone, f, nameof(CbSecondMainStep), CbSecondMainStep.SelectedItem.ToString(), false);
                UserConfigFile.SetValue(zone, f, nameof(slider2), slider2.Value.ToString(CultureInfo.InvariantCulture), false);
                // third
                UserConfigFile.SetValue(zone, f, nameof(CbThirdMeshType), CbThirdMeshType.SelectedIndex.ToString(), false);
                UserConfigFile.SetValue(zone, f, nameof(CbThirdMeshLength), CbThirdMeshLength.SelectedIndex.ToString(), false);
                UserConfigFile.SetValue(zone, f, nameof(CbThirdMeshWidth), CbThirdMeshWidth.SelectedIndex.ToString(), false);
                UserConfigFile.SetValue(zone, f, nameof(CbThirdDiamsRatio), CbThirdDiamsRatio.SelectedIndex.ToString(), false);
                UserConfigFile.SetValue(zone, f, nameof(slider3), slider3.Value.ToString(CultureInfo.InvariantCulture), false);
                // save
                UserConfigFile.SaveConfigFile();
            }
            catch (Exception exception)
            {
                ExceptionBox.Show(exception);
            }
        }

        #endregion
        #region First
        readonly MpMeshesHelpFunc _helpFunc = new MpMeshesHelpFunc();
        // Делаем специальные переменные для обозначения ограничения
        // вводимых значений        
        ////////////////////
        // Минимальная ширина сетки
        private double _minWidthLimit;
        // Максимальная ширина сетки
        private double _maxWidthLimit;
        // Минимальная длина сетки
        private double _minLengthLimit;
        // Максимальная длина сетки
        private double _maxLengthLimit;
        // Список диаметров арматуры
        private readonly List<string> _firstArmature = new List<string> { "3", "4", "5", "6", "8", "10", "12", "14", "16", "18", "20", "22", "25", "28", "32", "36", "40" };
        // Масса арматуры
        private readonly List<string> _firstArmatureMass = new List<string>
        { "0.052", "0.092", "0.144", "0.222", "0.395", "0.617", "0.888", "1.21", "1.58",
            "2.0", "2.47", "2.98", "3.85", "4.83", "6.31", "7.99", "9.87" };
        // Выбор типа сетки
        private void CbFirstMeshType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0) return;
            // Обозначаем вид сетки и требуемые ограничения
            if (e.AddedItems[0].Equals("1"))
            {
                ChFirstRoll.IsChecked = false;
                ChFirstRoll.Visibility = Visibility.Collapsed;
                MeshKind.Text = ModPlusAPI.Language.GetItem(LangItem, "h38");
                _minWidthLimit = 650; _maxWidthLimit = 3050;
                FirstWidthLimit.Text = "(От 650 до 3050)";
                _minLengthLimit = 850; _maxLengthLimit = 9000;
                FirstLengthLimit.Text = "(От 850 до 9000)";
                FillComboBoxWithStringList(CbFirstLongitudinal, _firstArmature.GetRange(6, 11));
                FillComboBoxWithStringList(CbFirstTransverse, _firstArmature.GetRange(3, 6));
                FillComboBoxWithStringList(CbFirstLongitudinalStep, new List<string> { "200" });
                FillComboBoxWithStringList(CbFirstTransverseStep, new List<string> { "600" });
                TbFirstTransverseOutput.Text = "25";
                TbFirstLongitudinalOutput.Text = "25";
            }
            else if (e.AddedItems[0].Equals("2"))
            {
                ChFirstRoll.IsChecked = false;
                ChFirstRoll.Visibility = Visibility.Collapsed;
                MeshKind.Text = ModPlusAPI.Language.GetItem(LangItem, "h38");
                _minWidthLimit = 650; _maxWidthLimit = 3050;
                FirstWidthLimit.Text = "(От 650 до 3050)";
                _minLengthLimit = 850; _maxLengthLimit = 5950;
                FirstLengthLimit.Text = "(От 850 до 5950)";
                FillComboBoxWithStringList(CbFirstLongitudinal, _firstArmature.GetRange(6, 7));
                FillComboBoxWithStringList(CbFirstTransverse, _firstArmature.GetRange(3, 6));
                FillComboBoxWithStringList(CbFirstLongitudinalStep, new List<string> { "200" });
                FillComboBoxWithStringList(CbFirstTransverseStep, new List<string> { "200" });
                TbFirstTransverseOutput.Text = "25";
                TbFirstLongitudinalOutput.Text = "25";
            }
            else if (e.AddedItems[0].Equals("3"))
            {
                ChFirstRoll.IsChecked = false;
                ChFirstRoll.Visibility = Visibility.Collapsed;
                MeshKind.Text = ModPlusAPI.Language.GetItem(LangItem, "h38");
                _minWidthLimit = 850; _maxWidthLimit = 3050;
                FirstWidthLimit.Text = "(От 650 до 3050)";
                _minLengthLimit = 850; _maxLengthLimit = 6250;
                FirstLengthLimit.Text = "(От 850 до 6250)";
                FillComboBoxWithStringList(CbFirstLongitudinal, _firstArmature.GetRange(3, 6));
                FillComboBoxWithStringList(CbFirstTransverse, _firstArmature.GetRange(6, 7));
                FillComboBoxWithStringList(CbFirstLongitudinalStep, new List<string> { "200", "400" });
                FillComboBoxWithStringList(CbFirstTransverseStep, new List<string> { "200" });
                TbFirstTransverseOutput.Text = "25";
                TbFirstLongitudinalOutput.Text = "25";
            }
            else if (e.AddedItems[0].Equals("4"))
            {
                ChFirstRoll.Visibility = Visibility.Visible;
                MeshKind.Text = ModPlusAPI.Language.GetItem(LangItem, "h39");
                _minWidthLimit = 650; _maxWidthLimit = 3800;
                FirstWidthLimit.Text = "(От 650 до 3800)";
                _minLengthLimit = 850; _maxLengthLimit = 9000;
                FirstLengthLimit.Text = "(От 850 до 9000)";
                FillComboBoxWithStringList(CbFirstLongitudinal, _firstArmature.GetRange(0, 6));
                FillComboBoxWithStringList(CbFirstTransverse, _firstArmature.GetRange(0, 6));
                FillComboBoxWithStringList(CbFirstLongitudinalStep, new List<string> { "100", "150", "200", "300", "400", "500" });
                FillComboBoxWithStringList(CbFirstTransverseStep, new List<string> { "75", "100", "150", "125", "175", "200", "250", "300", "400" });
                TbFirstTransverseOutput.Text = "25";
                TbFirstLongitudinalOutput.Text = "25";
            }
            else if (e.AddedItems[0].Equals("5"))
            {
                ChFirstRoll.Visibility = Visibility.Visible;
                MeshKind.Text = ModPlusAPI.Language.GetItem(LangItem, "h39");
                _minWidthLimit = 650; _maxWidthLimit = 3800;
                FirstWidthLimit.Text = "(От 650 до 3800)";
                _minLengthLimit = 3950; _maxLengthLimit = 9000;
                FirstLengthLimit.Text = "(От 3950 до 9000)";
                FillComboBoxWithStringList(CbFirstLongitudinal, _firstArmature.GetRange(0, 3));
                FillComboBoxWithStringList(CbFirstTransverse, _firstArmature.GetRange(2, 4));
                FillComboBoxWithStringList(CbFirstLongitudinalStep, new List<string> { "100", "150", "200", "300", "400", "500" });
                FillComboBoxWithStringList(CbFirstTransverseStep, new List<string> { "75", "100", "150", "125", "175", "200", "250", "300", "400" });
                TbFirstTransverseOutput.Text = "25";
                TbFirstLongitudinalOutput.Text = "25";
            }
            /////////////////////////////////////////////////
            // Записываем тип сетки в первое текстовое поле результата
            TbxC.Text = e.AddedItems[0] + "C";
        }
        // Выбор значения "Рулонная"
        private void ChFirstRoll_Checked(object sender, RoutedEventArgs e)
        {
            if (CbFirstMeshType.SelectedItem.Equals("4"))
            {
                TbxC.Text = TbxC.Text + "р";
                _maxLengthLimit = GetMaxLength(CbFirstLongitudinal.SelectedItem.ToString());
                FirstLengthLimit.Text = ModPlusAPI.Language.GetItem(LangItem, "h40");
                LengthChange();
            }
            else if (CbFirstMeshType.SelectedItem.Equals("5"))
            {
                TbxC.Text = TbxC.Text + "р";
                _maxLengthLimit = GetMaxLength(CbFirstLongitudinal.SelectedItem.ToString());
                FirstLengthLimit.Text = ModPlusAPI.Language.GetItem(LangItem, "h41");
                LengthChange();
            }
        }
        // Отмена выбора "Рулонная"
        private void ChFirstRoll_Unchecked(object sender, RoutedEventArgs e)
        {
            if (CbFirstMeshType.SelectedItem.Equals("4"))
            {
                TbxC.Text = TbxC.Text.TrimEnd('р');
                _minLengthLimit = 850; _maxLengthLimit = 9000;
                FirstLengthLimit.Text = ModPlusAPI.Language.GetItem(LangItem, "h42");
                LengthChange();
            }
            if (CbFirstMeshType.SelectedItem.Equals("5"))
            {
                TbxC.Text = TbxC.Text.TrimEnd('р');
                _minLengthLimit = 3950; _maxLengthLimit = 9000;
                FirstLengthLimit.Text = ModPlusAPI.Language.GetItem(LangItem, "h43");
                LengthChange();
            }
        }
        // Изменение текста в поле "Ширина сетки"
        private void TbFirstMeshWidth_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((TextBox)sender).Text))
            {
                // вывод результата
                if (int.Parse(((TextBox)sender).Text) < _minWidthLimit)
                    TbMessage.Text = _helpFunc.Message(1, _minWidthLimit.ToString(CultureInfo.InvariantCulture));
                else if (int.Parse(((TextBox)sender).Text) > _maxWidthLimit)
                    TbMessage.Text = _helpFunc.Message(2, _maxWidthLimit.ToString(CultureInfo.InvariantCulture));
                else
                {
                    TbMessage.Text = _helpFunc.Message(0, string.Empty);
                    TbFirstb.Text = (int.Parse(((TextBox)sender).Text) / 10).ToString(CultureInfo.InvariantCulture) + "x";
                }
                // Доборный шаг
                AdditionallyStep();
                // масса
                FirstGetMass();
            }
            else TbFirstb.Text = string.Empty;
        }
        // Изменение текста в поле "Длина сетки"
        private void TbFirstMeshLength_TextChanged(object sender, TextChangedEventArgs e)
        {
            LengthChange();
        }
        private void LengthChange()
        {
            if (!string.IsNullOrEmpty(TbFirstMeshLength.Text))
            {
                if (int.Parse(TbFirstMeshLength.Text) < _minLengthLimit)
                    TbMessage.Text = _helpFunc.Message(3, _minLengthLimit.ToString(CultureInfo.InvariantCulture));
                else if (int.Parse(TbFirstMeshLength.Text) > _maxLengthLimit)
                    TbMessage.Text = _helpFunc.Message(4, _maxLengthLimit.ToString(CultureInfo.InvariantCulture));
                else
                {
                    TbMessage.Text = _helpFunc.Message(0, string.Empty);
                    TbFirstl.Text = (int.Parse(TbFirstMeshLength.Text) / 10).ToString(CultureInfo.InvariantCulture);
                }
                // Доборный шаг
                AdditionallyStep();
                // масса
                FirstGetMass();
            }
            else TbFirstl.Text = string.Empty;
        }

        // Заполнение первоночальных значений первой вкладки
        private void FillFirstTab()
        {
            // Заполняем список "Тип сетки"
            FillComboBoxWithStringList(CbFirstMeshType, new List<string> { "1", "2", "3", "4", "5" });
        }
        // Выбор диаметра продольных стержней
        private void CbFirstLongitudinal_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0) return;

            TbFirstd.Text = e.AddedItems[0].ToString();
            // Заполняем список классов арматуры
            if (e.AddedItems[0].ToString().Equals("3") ||
                e.AddedItems[0].ToString().Equals("4") ||
                e.AddedItems[0].ToString().Equals("5"))
            {
                ChFirstRoll.Visibility = Visibility.Visible;
                FillComboBoxWithStringList(CbFirstLongitudinalClass, new List<string> { "B500C", "Bp-I" });
            }
            else
            {
                ChFirstRoll.IsChecked = false;
                ChFirstRoll.Visibility = Visibility.Collapsed;
                if (CbFirstMeshType.SelectedItem.ToString() == "1" ||
                   CbFirstMeshType.SelectedItem.ToString() == "2" ||
                   CbFirstMeshType.SelectedItem.ToString() == "3")
                    FillComboBoxWithStringList(CbFirstLongitudinalClass, new List<string> { "A400", "A500C", "A600C" });
                else
                    FillComboBoxWithStringList(CbFirstLongitudinalClass, new List<string> { "A240", "A400", "A500C", "B500C" });
            }
            // Отношение меньшего диаметра к большему
            if (CbFirstTransverse.Items.Count > 0)
            {
                if (double.Parse(e.AddedItems[0].ToString()) >
                    double.Parse(CbFirstTransverse.SelectedItem.ToString()))
                {
                    TbMessage.Text = double.Parse(CbFirstTransverse.SelectedItem.ToString())
                                     / double.Parse(e.AddedItems[0].ToString()) < 0.25
                                     ? ModPlusAPI.Language.GetItem(LangItem, "h44")
                                     : string.Empty;
                }
                else
                {
                    TbMessage.Text = double.Parse(e.AddedItems[0].ToString())
                                     / double.Parse(CbFirstTransverse.SelectedItem.ToString()) < 0.25
                                     ? ModPlusAPI.Language.GetItem(LangItem, "h44")
                                     : string.Empty;
                }
            }
            // Максимальная длина рулона
            if (CbFirstMeshType.SelectedItem.ToString().Equals("4") ||
                CbFirstMeshType.SelectedItem.ToString().Equals("5"))
            {
                if (ChFirstRoll.IsChecked != null && ChFirstRoll.IsChecked.Value)
                    _maxLengthLimit = GetMaxLength(e.AddedItems[0].ToString());
                else _maxLengthLimit = 9000;
            }
            LengthChange();
            // масса
            FirstGetMass();
        }
        // Выбор диаметра поперечных стержней
        private void CbFirstTransverse_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0) return;

            TbFirstdOne.Text = e.AddedItems[0].ToString();
            // Заполняем список классов арматуры
            if (e.AddedItems[0].ToString().Equals("3") ||
                e.AddedItems[0].ToString().Equals("4") ||
                e.AddedItems[0].ToString().Equals("5"))
            {
                FillComboBoxWithStringList(CbFirstTransverseCalss, new List<string> { "Bp-I", "B500C" });
            }
            else
            {
                if (CbFirstMeshType.SelectedItem.ToString() == "1" ||
                    CbFirstMeshType.SelectedItem.ToString() == "2" ||
                    CbFirstMeshType.SelectedItem.ToString() == "3")
                    FillComboBoxWithStringList(CbFirstTransverseCalss, new List<string> { "A400", "A500C", "B500C", "A600C" });
                else
                    FillComboBoxWithStringList(CbFirstTransverseCalss, new List<string> { "B500C", "B-I" });
            }
            // Отношение меньшего диаметра к большему
            if (CbFirstLongitudinal.Items.Count > 0)
            {
                if (double.Parse(e.AddedItems[0].ToString()) >
                    double.Parse(CbFirstLongitudinal.SelectedItem.ToString()))
                {
                    TbMessage.Text = (double.Parse(CbFirstLongitudinal.SelectedItem.ToString())
                                           / double.Parse(e.AddedItems[0].ToString())) < 0.25
                                           ? ModPlusAPI.Language.GetItem(LangItem, "h44")
                                           : string.Empty;
                }
                else
                {
                    TbMessage.Text = (double.Parse(e.AddedItems[0].ToString())
                                           / double.Parse(CbFirstLongitudinal.SelectedItem.ToString())) < 0.25
                                           ? ModPlusAPI.Language.GetItem(LangItem, "h44")
                                           : string.Empty;
                }
            }
            // масса
            FirstGetMass();
        }
        // Выбор класса арматуры продольных стержней
        private void CbFirstLongitudinalClass_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0) return;

            TbFirstdClass.Text = e.AddedItems[0].ToString();
        }
        // Выбор класса арматуры поперченых стержней
        private void CbFirstTransverseCalss_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0) return;

            TbFirstdOneClass.Text = e.AddedItems[0].ToString();
        }
        // Выбор шага продольных стержней
        private void CbFirstLongitudinalStep_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0) return;

            // Если тип сетки 1, 2 или 3 с шагом 200, то шаг не указывается
            if (CbFirstMeshType.SelectedItem.ToString().Equals("1") ||
                CbFirstMeshType.SelectedItem.ToString().Equals("2") ||
                (CbFirstMeshType.SelectedItem.ToString().Equals("3") &
                e.AddedItems[0].ToString().Equals("200"))
                )
                TbFirstdStep.Text = string.Empty;
            else
                TbFirstdStep.Text = "-" + e.AddedItems[0];
            // Доборный шаг
            AdditionallyStep();
            // масса
            FirstGetMass();
        }
        // Выбор шага поперечных стержней
        private void CbFirstTransverseStep_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0) return;

            // Если тип сетки 1, 2 или 3 с шагом 200, то шаг не указывается
            if (CbFirstMeshType.SelectedItem.ToString().Equals("1") ||
                CbFirstMeshType.SelectedItem.ToString().Equals("2") ||
                (CbFirstMeshType.SelectedItem.ToString().Equals("3") &
                e.AddedItems[0].ToString().Equals("200"))
                )
                TbFirstdOneStep.Text = string.Empty;
            else
                TbFirstdOneStep.Text = "-" + e.AddedItems[0];
            // Доборный шаг
            AdditionallyStep();
            // масса
            FirstGetMass();
        }
        // Изменение текста в поле "Выпуск продольных стержней"
        private void TbFirstLongitudinalOutput_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Если текст вводится во второе окошко когда в первом пусто
            // то стираем этот ввод и выходим
            if (((TextBox)sender).Name.Equals("TbFirstLongitudinalOutputTwo"))
            {
                if (string.IsNullOrEmpty(TbFirstLongitudinalOutput.Text))
                {
                    TbFirstLongitudinalOutputTwo.Text = string.Empty;
                    return;
                }
            }
            if (string.IsNullOrEmpty(((TextBox)sender).Text))
            {
                TbFirstaOne.Text = string.Empty; TbFirstaTwo.Text = string.Empty;
                TbMessage.Text = string.Empty;
                FillTransverseOutput();
                // Доборный шаг
                AdditionallyStep();
                return;
            }

            switch (CbFirstMeshType.SelectedItem.ToString())
            {
                // Для типа 1 - кратно 25
                case "1":
                    TbMessage.Text = Math.Abs(Math.IEEERemainder(int.Parse(((TextBox)sender).Text), 25)) > _ep
                        ? ModPlusAPI.Language.GetItem(LangItem, "h45")
                        : string.Empty;
                    break;
                // Для типа 2,3 - кратно 25
                case "2":
                    TbMessage.Text = Math.Abs(Math.IEEERemainder(int.Parse(((TextBox)sender).Text), 25)) > _ep
                        ? ModPlusAPI.Language.GetItem(LangItem, "h45")
                        : string.Empty;
                    break;
                case "3":
                    TbMessage.Text = Math.Abs(Math.IEEERemainder(int.Parse(((TextBox)sender).Text), 25)) > _ep
                        ? ModPlusAPI.Language.GetItem(LangItem, "h45")
                        : string.Empty;
                    break;
                // Для 4,5 - от 30 до 200 кратно 5
                case "4":
                    if (int.Parse(((TextBox)sender).Text) != 25)
                    {
                        if (int.Parse(((TextBox)sender).Text) < 30 ||
                            int.Parse(((TextBox)sender).Text) > 200)
                            TbMessage.Text = ModPlusAPI.Language.GetItem(LangItem, "h46");
                        else
                            if (Math.Abs(Math.IEEERemainder(int.Parse(((TextBox)sender).Text), 5)) > _ep)
                            TbMessage.Text = ModPlusAPI.Language.GetItem(LangItem, "h46");
                        else TbMessage.Text = string.Empty;
                    }
                    else TbMessage.Text = string.Empty;
                    break;
                case "5":
                    if (int.Parse(((TextBox)sender).Text) != 25)
                    {
                        if (int.Parse(((TextBox)sender).Text) < 30 ||
                            int.Parse(((TextBox)sender).Text) > 200)
                            TbMessage.Text = ModPlusAPI.Language.GetItem(LangItem, "h46");
                        else
                            if (Math.Abs(Math.IEEERemainder(int.Parse(((TextBox)sender).Text), 5)) > _ep)
                            TbMessage.Text = ModPlusAPI.Language.GetItem(LangItem, "h46");
                        else TbMessage.Text = string.Empty;
                    }
                    else TbMessage.Text = string.Empty;
                    break;
            }
            FillTransverseOutput();
            // Доборный шаг
            AdditionallyStep();
        }
        // Изменение в поле "Выпуск продолных стержней"
        private void TbFirstTransverseOutput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(((TextBox)sender).Text))
            {
                TbFirsta.Text = string.Empty;
                TbMessage.Text = string.Empty;
                FillLongitudinalOutput();
                // Доборный шаг
                AdditionallyStep();
                return;
            }

            switch (CbFirstMeshType.SelectedItem.ToString())
            {
                // Для типа 1 - 25
                case "1":
                    TbFirstTransverseOutput.Text = "25";
                    break;
                // Для типа 2,3 - кратно 25
                case "2":
                    TbMessage.Text = Math.Abs(Math.IEEERemainder(int.Parse(((TextBox)sender).Text), 25)) > _ep
                        ? ModPlusAPI.Language.GetItem(LangItem, "h45")
                        : string.Empty;
                    break;
                case "3":
                    TbMessage.Text = Math.Abs(Math.IEEERemainder(int.Parse(((TextBox)sender).Text), 25)) > _ep
                        ? ModPlusAPI.Language.GetItem(LangItem, "h45")
                        : string.Empty;
                    break;
                // Для 4,5 - от 15, 20, 30, а также от 25 до 100 кратно 100
                case "4":
                    if (int.Parse(((TextBox)sender).Text) != 15 ||
                        int.Parse(((TextBox)sender).Text) != 20 ||
                        int.Parse(((TextBox)sender).Text) != 30)
                    {
                        if (int.Parse(((TextBox)sender).Text) < 25 ||
                            int.Parse(((TextBox)sender).Text) > 100)
                            TbMessage.Text = ModPlusAPI.Language.GetItem(LangItem, "h47");
                        else
                            if (Math.Abs(Math.IEEERemainder(int.Parse(((TextBox)sender).Text), 25)) > _ep)
                            TbMessage.Text = ModPlusAPI.Language.GetItem(LangItem, "h47");
                        else TbMessage.Text = string.Empty;
                    }
                    else TbMessage.Text = string.Empty;
                    break;
                case "5":
                    if (int.Parse(((TextBox)sender).Text) != 15 ||
                        int.Parse(((TextBox)sender).Text) != 20 ||
                        int.Parse(((TextBox)sender).Text) != 30)
                    {
                        if (int.Parse(((TextBox)sender).Text) < 25 ||
                            int.Parse(((TextBox)sender).Text) > 100)
                            TbMessage.Text = ModPlusAPI.Language.GetItem(LangItem, "h47");
                        else
                            if (Math.Abs(Math.IEEERemainder(int.Parse(((TextBox)sender).Text), 25)) > _ep)
                            TbMessage.Text = ModPlusAPI.Language.GetItem(LangItem, "h47");
                        else TbMessage.Text = string.Empty;
                    }
                    else TbMessage.Text = string.Empty;
                    break;
            }
            FillLongitudinalOutput();
            // Доборный шаг
            AdditionallyStep();
        }
        // Заполнение значения выпусков продольных стержней
        private void FillTransverseOutput()
        {
            string a1 = TbFirstLongitudinalOutput.Text;
            string a2 = TbFirstLongitudinalOutputTwo.Text;
            // Если значения одинаковые
            if (a1.Equals(a2))
            {
                if (!a1.Equals("25") & !TbFirstTransverseOutput.Text.Equals("25"))
                {
                    TbFirstaOne.Text = a1;
                    TbFirsta.Text = TbFirstTransverseOutput.Text;
                }
                else
                {
                    TbFirstaOne.Text = string.Empty;
                    TbFirstaTwo.Text = string.Empty;
                    TbFirsta.Text = string.Empty;
                }
            }
            // Если значения не равны
            if (!a1.Equals(a2))
            {
                // Если второе пустое, первое 25 и нижнее 25 - не пишем
                if (a1.Equals("25") & string.IsNullOrEmpty(a2) & TbFirstTransverseOutput.Text.Equals("25"))
                {
                    TbFirstaOne.Text = string.Empty;
                    TbFirstaTwo.Text = string.Empty;
                    TbFirsta.Text = string.Empty;
                }
                // Иначе - пишем все
                else
                {
                    TbFirstaOne.Text = a1;
                    if (!string.IsNullOrEmpty(a2))
                        TbFirstaTwo.Text = "+" + a2;
                    TbFirsta.Text = TbFirstTransverseOutput.Text;
                }
            }
            // масса
            FirstGetMass();
        }
        // Заполнение значения выпусков поперченых стержней
        private void FillLongitudinalOutput()
        {
            string a = TbFirstTransverseOutput.Text;
            // Если значение равно 25
            if (a.Equals("25"))
            {
                // Если верхнее левое - 25, правое - пусто
                // тогда все стираем
                if (TbFirstLongitudinalOutput.Text.Equals("25") & string.IsNullOrEmpty(TbFirstLongitudinalOutputTwo.Text))
                {
                    TbFirstaOne.Text = string.Empty;
                    TbFirstaTwo.Text = string.Empty;
                    TbFirsta.Text = string.Empty;
                }
                // Иначе - пишем
                else
                {
                    TbFirstaOne.Text = TbFirstLongitudinalOutput.Text;
                    TbFirstaTwo.Text = "+" + TbFirstLongitudinalOutputTwo.Text;
                    TbFirsta.Text = a;
                }
            }
            // Если не равно 25
            else
            {
                // Если верхнее левое пустое - не пишем
                if (string.IsNullOrEmpty(TbFirstLongitudinalOutput.Text))
                    TbFirsta.Text = string.Empty;
                else
                {
                    TbFirstaOne.Text = TbFirstLongitudinalOutput.Text;
                    TbFirstaTwo.Text = "+" + TbFirstLongitudinalOutputTwo.Text;
                    TbFirsta.Text = a;
                }
            }
            // масса
            FirstGetMass();
        }
        // Добавление доборного шага
        private void AdditionallyStep()
        {
            // Для продольных стержней
            if (!string.IsNullOrEmpty(TbFirstMeshWidth.Text))
            {
                var b = double.Parse(TbFirstMeshWidth.Text); // Ширина
                var s = double.Parse(CbFirstLongitudinalStep.SelectedItem.ToString()); // Шаг
                var a = double.Parse(TbFirstTransverseOutput.Text); // Выпуск арматуры
                var snum = Math.Truncate((b - a - a) / s); // Количество стержней
                var addstep = (b - (snum * s) - a - a);
                if (Math.Abs(addstep) > _ep)
                    TbFirstdAddStep.Text = "(" + addstep.ToString(CultureInfo.InvariantCulture) + ")";
                else TbFirstdAddStep.Text = string.Empty;
            }
            // Для поперечных стержней
            if (!string.IsNullOrEmpty(TbFirstMeshLength.Text))
            {
                var l = double.Parse(TbFirstMeshLength.Text); // Длина
                var s = double.Parse(CbFirstTransverseStep.SelectedItem.ToString()); // шаг
                double a = 0; double a2 = 0; double addstep; double snum;
                if (!string.IsNullOrEmpty(TbFirstLongitudinalOutput.Text))
                    a = double.Parse(TbFirstLongitudinalOutput.Text); // Выпуск 1
                if (!string.IsNullOrEmpty(TbFirstLongitudinalOutputTwo.Text))
                    a2 = double.Parse(TbFirstLongitudinalOutputTwo.Text); // Выпуск 2
                if (Math.Abs(a) < _ep)
                {
                    snum = Math.Truncate((l - a2 - a2) / s);
                    addstep = (l - (snum * s) - a2 - a2);
                }
                else if (Math.Abs(a2) < _ep)
                {
                    snum = Math.Truncate((l - a - a) / s);
                    addstep = (l - (snum * s) - a - a);
                }
                else
                {
                    snum = Math.Truncate((l - a - a2) / s);
                    addstep = (l - (snum * s) - a - a2);
                }
                if (Math.Abs(addstep) > _ep)
                    TbFirstdOneAddStep.Text = "(" + addstep.ToString(CultureInfo.InvariantCulture) + ")";
                else TbFirstdOneAddStep.Text = string.Empty;
            }
            TbdAddStep_TextInput();
            TbdOneAddStep_TextInput();
            // масса
            FirstGetMass();
        }
        // Ввод текста в поле "Доборный шаг продольных стержней"
        private void TbdAddStep_TextInput()
        {
            if (!string.IsNullOrEmpty(TbFirstdAddStep.Text))
            {

                if (CbFirstMeshType.SelectedItem.ToString().Equals("1") ||
                    CbFirstMeshType.SelectedItem.ToString().Equals("2") ||
                    CbFirstMeshType.SelectedItem.ToString().Equals("3")
                    )
                {
                    TbFirstOutputOneMessage.Visibility = Visibility.Visible;
                    TbFirstOutputOneMessage.Text = ModPlusAPI.Language.GetItem(LangItem, "h48");
                }
                else if (CbFirstMeshType.SelectedItem.ToString().Equals("4") ||
                    CbFirstMeshType.SelectedItem.ToString().Equals("5"))
                {
                    //  Доборный шаг
                    double addstep = double.Parse(
                        TbFirstdAddStep.Text.Substring(0, TbFirstdAddStep.Text.Length - 1).Remove(0, 1));
                    // Основной шаг 
                    double step = double.Parse(CbFirstLongitudinalStep.SelectedItem.ToString());
                    if (addstep < 50 || addstep > step)
                    {
                        TbFirstOutputOneMessage.Visibility = Visibility.Visible;
                        TbFirstOutputOneMessage.Text = ModPlusAPI.Language.GetItem(LangItem, "h49") + " ("
                            + step + ") " + ModPlusAPI.Language.GetItem(LangItem, "h50");
                    }
                    else if (Math.Abs(Math.IEEERemainder(addstep, 10)) > _ep)
                    {
                        TbFirstOutputOneMessage.Visibility = Visibility.Visible;
                        TbFirstOutputOneMessage.Text = ModPlusAPI.Language.GetItem(LangItem, "h49") + " ("
                            + step + ") " + ModPlusAPI.Language.GetItem(LangItem, "h50");
                    }
                    else
                    {
                        TbFirstOutputOneMessage.Visibility = Visibility.Collapsed;
                        TbFirstOutputOneMessage.Text = string.Empty;
                    }
                }
                else
                {
                    TbFirstOutputOneMessage.Visibility = Visibility.Collapsed;
                    TbFirstOutputOneMessage.Text = string.Empty;
                }
            }
            else
            {
                TbFirstOutputOneMessage.Visibility = Visibility.Collapsed;
                TbFirstOutputOneMessage.Text = string.Empty;
            }
            // масса
            FirstGetMass();
        }
        // Ввод текста в поле "Доборный шаг поперечных стержней"
        private void TbdOneAddStep_TextInput()
        {
            if (!string.IsNullOrEmpty(TbFirstdOneAddStep.Text))
            {

                if (CbFirstMeshType.SelectedItem.ToString().Equals("1"))
                {
                    // Доборный шаг
                    var addstep = double.Parse(
                        TbFirstdOneAddStep.Text.Substring(0, TbFirstdOneAddStep.Text.Length - 1).Remove(0, 1));
                    if (Math.Abs(addstep - 100.0) > _ep &&
                        Math.Abs(addstep - 200.0) > _ep &&
                        Math.Abs(addstep - 300.0) > _ep)
                    {
                        TbFirstOutputTwoMessage.Visibility = Visibility.Visible;
                        TbFirstOutputTwoMessage.Text = ModPlusAPI.Language.GetItem(LangItem, "h51");
                    }
                    else
                    {
                        TbFirstOutputTwoMessage.Visibility = Visibility.Collapsed;
                        TbFirstOutputTwoMessage.Text = string.Empty;
                    }
                }
                else if (CbFirstMeshType.SelectedItem.ToString().Equals("2") ||
                    CbFirstMeshType.SelectedItem.ToString().Equals("3"))
                {
                    TbFirstOutputTwoMessage.Visibility = Visibility.Visible;
                    TbFirstOutputTwoMessage.Text = ModPlusAPI.Language.GetItem(LangItem, "h52");
                }
                else if (CbFirstMeshType.SelectedItem.ToString().Equals("4") ||
                    CbFirstMeshType.SelectedItem.ToString().Equals("5"))
                {
                    // Доборный шаг
                    double addstep = double.Parse(
                        TbFirstdOneAddStep.Text.Substring(0, TbFirstdOneAddStep.Text.Length - 1).Remove(0, 1));
                    if (addstep < 50 || addstep > 250)
                    {
                        TbFirstOutputTwoMessage.Visibility = Visibility.Visible;
                        TbFirstOutputTwoMessage.Text = ModPlusAPI.Language.GetItem(LangItem, "h53");
                    }
                    else if (Math.Abs(Math.IEEERemainder(addstep, 10)) > _ep)
                    {
                        TbFirstOutputTwoMessage.Visibility = Visibility.Visible;
                        TbFirstOutputTwoMessage.Text = ModPlusAPI.Language.GetItem(LangItem, "h53");
                    }
                    else
                    {
                        TbFirstOutputTwoMessage.Visibility = Visibility.Collapsed;
                        TbFirstOutputTwoMessage.Text = string.Empty;
                    }
                }
                else
                {
                    TbFirstOutputTwoMessage.Visibility = Visibility.Collapsed;
                    TbFirstOutputTwoMessage.Text = string.Empty;
                }
            }
            else
            {
                TbFirstOutputTwoMessage.Visibility = Visibility.Collapsed;
                TbFirstOutputTwoMessage.Text = string.Empty;
            }
            // масса
            FirstGetMass();
        }
        // Подсчет массы
        private void FirstGetMass()
        {
            double l, b, s, s1, sadd = 0, s1Add = 0, a = 0, a1 = 0, a2 = 0;
            // Выпуск поперечных стержней
            if (!string.IsNullOrEmpty(TbFirstTransverseOutput.Text))
                a = double.Parse(TbFirstTransverseOutput.Text);
            // Выпуски продольных стержней
            if (!string.IsNullOrEmpty(TbFirstLongitudinalOutput.Text))
                a1 = double.Parse(TbFirstLongitudinalOutput.Text); // Выпуск 1
            if (!string.IsNullOrEmpty(TbFirstLongitudinalOutputTwo.Text))
                a2 = double.Parse(TbFirstLongitudinalOutputTwo.Text); // Выпуск 2
            // Диаметр продольных стержней
            if (CbFirstLongitudinal.SelectedIndex == -1) return;
            var d = CbFirstLongitudinal.SelectedItem.ToString();
            // Диаметр поперечных стержней
            if (CbFirstTransverse.SelectedIndex == -1) return;
            var d1 = CbFirstTransverse.SelectedItem.ToString();
            // Ширина сетки
            if (!double.TryParse(TbFirstMeshLength.Text, out l)) return;
            // Длина сетки
            if (!double.TryParse(TbFirstMeshWidth.Text, out b)) return;
            // Шаг продольных стержней
            if (!double.TryParse(CbFirstLongitudinalStep.SelectedItem.ToString(), out s)) return;
            // Шаг поперечных стержней
            if (!double.TryParse(CbFirstTransverseStep.SelectedItem.ToString(), out s1)) return;
            // Доборный шаг продолных стержней
            if (!string.IsNullOrEmpty(TbFirstdAddStep.Text))
                if (!double.TryParse(TbFirstdAddStep.Text.Substring(0, TbFirstdAddStep.Text.Length - 1).Remove(0, 1)
                    , out sadd)) return;
            // Доборный шаг поперечных стержней
            if (!string.IsNullOrEmpty(TbFirstdOneAddStep.Text))
                if (!double.TryParse(TbFirstdOneAddStep.Text.Substring(0, TbFirstdOneAddStep.Text.Length - 1).Remove(0, 1)
                    , out s1Add)) return;
            // Находим количество продольных стержней
            var snumLong = Math.Truncate((b - a - a) / s) + 1;
            // Если есть добор, то +1
            if (sadd > 0) snumLong += 1;
            // Находим количество поперечных стержней
            double snumTrans;
            if (Math.Abs(a1) < _ep)
            {
                snumTrans = Math.Truncate((l - a2 - a2) / s1) + 1;
            }
            else if (Math.Abs(a2) < _ep)
            {
                snumTrans = Math.Truncate((l - a1 - a1) / s1) + 1;
            }
            else
            {
                snumTrans = Math.Truncate((l - a1 - a2) / s1) + 1;
            }
            // Если есть добор, то +1
            if (s1Add > 0) snumTrans += 1;
            // Считаем массу
            var mass = (snumLong * double.Parse(_firstArmatureMass[_firstArmature.IndexOf(d)]) * l / 1000) +
                (snumTrans * double.Parse(_firstArmatureMass[_firstArmature.IndexOf(d1)]) * b / 1000);
            TbFirstMassa.Text = (Math.Round(mass, int.Parse(TbFirstRound.Text))).ToString(CultureInfo.InvariantCulture);
        }
        // Возврат максимальной длины рулона в зависимости от диаметра продольной арматуры
        private static double GetMaxLength(string diam)
        {
            double result = 10417000;
            switch (diam)
            {
                case "3": result = 28846000; break;
                case "4": result = 16304000; break;
                case "5": result = 10417000; break;
            }
            return result;
        }
        // вставка значения в таблицу
        public void SetResultToTable()
        {
            if (!string.IsNullOrEmpty(TbMessage.Text) ||
                !string.IsNullOrEmpty(TbFirstOutputOneMessage.Text) ||
                !string.IsNullOrEmpty(TbFirstOutputTwoMessage.Text) ||
                string.IsNullOrEmpty(TbFirstMeshLength.Text) ||
                string.IsNullOrEmpty(TbFirstMeshWidth.Text)
            )
            {
                if (!ModPlusAPI.Windows.MessageBox.ShowYesNo(ModPlusAPI.Language.GetItem(LangItem, "h54"),
                    MessageBoxIcon.Question))
                    return;
            }
            Hide();
            try
            {
                if (string.IsNullOrEmpty(TbFirsta.Text) & string.IsNullOrEmpty(TbFirstaOne.Text) &
                    string.IsNullOrEmpty(TbFirstaTwo.Text))
                {
                    InsertToAutoCad.AddSpecificationItemToTableRow(new InsertToAutoCad.SpecificationItemForTable(
                        "",
                        "ГОСТ 23279-2012",
                        "\\A1;" + TbxC.Text + "{\\H1x; \\H0.9x;\\S" + TbFirstd.Text + TbFirstdClass.Text + TbFirstdStep.Text + TbFirstdAddStep.Text +
                        "/" + TbFirstdOne.Text + TbFirstdOneClass.Text + TbFirstdOneStep.Text + TbFirstdOneAddStep.Text + ";\\H1x; }" + TbFirstb.Text + TbFirstl.Text,
                        TbFirstMassa.Text.Replace(',', '.').Replace('.', char.Parse(ModPlusAPI.Variables.Separator)),
                        "", ""));
                }
                else
                {
                    InsertToAutoCad.AddSpecificationItemToTableRow(new InsertToAutoCad.SpecificationItemForTable(
                        "",
                        "ГОСТ 23279-2012",
                        "\\A1;" + TbxC.Text +
                        "{\\H1x; \\H0.9x;\\S" + TbFirstd.Text + TbFirstdClass.Text + TbFirstdStep.Text +
                        TbFirstdAddStep.Text +
                        "/" + TbFirstdOne.Text + TbFirstdOneClass.Text + TbFirstdOneStep.Text +
                        TbFirstdOneAddStep.Text +
                        ";\\H1x; }" + TbFirstb.Text + TbFirstl.Text + " {\\H0.9x;\\S" +
                        TbFirstaOne.Text + TbFirstaTwo.Text + "/" + TbFirsta.Text + ";}",
                        TbFirstMassa.Text.Replace(',', '.').Replace('.', char.Parse(ModPlusAPI.Variables.Separator)),
                        "", ""));
                }
            }
            catch (Exception exception)
            {
                ExceptionBox.Show(exception);
            }
            finally
            {
                Show();
            }
        }

        private void BtFirstAdd_Click(object sender, RoutedEventArgs e)
        {
            SetResultToTable();
        }
        // Передвижение ползунка
        private void slider1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                TbFirstRound.Text = slider1.Value.ToString(CultureInfo.InvariantCulture);
                // Масса
                FirstGetMass();
            }
            catch
            {
                // ignored
            }
        }
        // Включение изображения
        private void BtFirstImage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var imagename = string.Empty;

                if (CbFirstMeshType.SelectedIndex == 0)
                    imagename = "ГОСТ 23279-85_1";
                else if (CbFirstMeshType.SelectedIndex == 1)
                    imagename = "ГОСТ 23279-85_2";
                else if (CbFirstMeshType.SelectedIndex == 2)
                    imagename = "ГОСТ 23279-85_3";
                else if (CbFirstMeshType.SelectedIndex == 3)
                    imagename = "ГОСТ 23279-85_4";
                else if (CbFirstMeshType.SelectedIndex == 4)
                    imagename = "ГОСТ 23279-85_5";


                var imgWin = new ShowImageWindow
                {
                    Img =
                    {
                        Source =
                            new BitmapImage(
                                new Uri(
                                    @"pack://application:,,,/mpMeshes_" + new Interface().AvailProductExternalVersion +
                                    ";component/Resources/" + imagename + ".png", UriKind.Absolute))
                    }
                };
                imgWin.ShowDialog();
            }
            catch (Exception ex)
            {
                ExceptionBox.Show(ex);
            }
        }
        /// <summary>
        /// Заполнение ComboBox строковыми значениями
        /// </summary>
        /// <param name="source">Заполняемый ComboBox</param>
        /// <param name="items">Список значений</param>
        public static void FillComboBoxWithStringList(ComboBox source, List<string> items)
        {
            // Запоминаем старое значение
            // если список не пуст и что-то выбрано
            var oldSelection = string.Empty;
            if (source.Items.Count > 0)
                if (source.SelectedIndex != -1)
                    oldSelection = source.SelectedItem.ToString();
            // Очищаем список
            source.ItemsSource = null;
            // Заполняем по новой
            source.ItemsSource = items;
            // Если новый список содержит старое значение, то выбираем его
            if (!string.IsNullOrEmpty(oldSelection))
                source.SelectedIndex = items.Contains(oldSelection) ? items.IndexOf(oldSelection) : 0;
            else
                source.SelectedIndex = 0;
        }
        #endregion

        #region Second
        // Первоначальное заполнение значений
        private void SecondFill()
        {
            // Тип сетки по точности
            CbSecondMeshType.Items.Add(ModPlusAPI.Language.GetItem(LangItem, "h55"));
            CbSecondMeshType.Items.Add(ModPlusAPI.Language.GetItem(LangItem, "h56"));
            CbSecondMeshType.SelectedIndex = 0;
            // Основной шаг сетки
            CbSecondMainStep.Items.Add("100");
            CbSecondMainStep.Items.Add("150");
            CbSecondMainStep.Items.Add("200");
            CbSecondMainStep.SelectedIndex = 0;
        }
        // Выбор типа сетки по точности размеров
        private void CbSecondMeshType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TbSecondType.Text = e.AddedItems[0].Equals(ModPlusAPI.Language.GetItem(LangItem, "h55")) ? "5BpI" : "5ПBpI";
        }
        // Выбор основного шага сетки
        private void CbSecondMainStep_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems[0].Equals("100"))
            {
                TbSecondS.Text = TbSecondSOne.Text = "100";
                TbSecondLongitudinalOutput.Text = "50";
                TbSecondTransverseSum.Text = "24";
                TbSecondTransverseBreak.Text = "100x23";
            }
            else if (e.AddedItems[0].Equals("150"))
            {
                TbSecondS.Text = TbSecondSOne.Text = "150";
                TbSecondLongitudinalOutput.Text = "75";
                TbSecondTransverseSum.Text = "16";
                TbSecondTransverseBreak.Text = "150x7+200+150x7";
            }
            else if (e.AddedItems[0].Equals("200"))
            {
                TbSecondS.Text = TbSecondSOne.Text = "200";
                TbSecondLongitudinalOutput.Text = "100";
                TbSecondTransverseSum.Text = "14";
                TbSecondTransverseBreak.Text = "100+200x5+100+200x5+100";
            }
            SetMeshLength();
        }
        // Подсчет массы сетки
        private void Massa()
        {
            if (!string.IsNullOrEmpty(TbMessage.Text))
            { TbSecondMassa.Text = string.Empty; return; }
            // Масса продольных стержней
            var massaLong = double.Parse(TbSecondTransverseSum.Text) * 0.144 *
                double.Parse(TbSecondMeshLength.Text) / 1000;
            ///////////////////////////////////////////
            var length = double.Parse(TbSecondMeshLength.Text);
            var twoOutputs = double.Parse(TbSecondLongitudinalOutput.Text) * 2;
            var step = double.Parse(CbSecondMainStep.SelectedItem.ToString());
            var summ = Math.Truncate((length - twoOutputs) / step) + 1;
            // Масса поперечных стержней
            var massaTrans = summ * 0.144 * 2.350;
            // Результат
            TbSecondMassa.Text = Math.Round(massaLong + massaTrans, int.Parse(TbSecondRound.Text)).ToString(CultureInfo.InvariantCulture);
        }
        // Вставка результата
        private void BtSecondAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(TbMessage.Text) ||
                string.IsNullOrEmpty(TbSecondMeshLength.Text) ||
                string.IsNullOrEmpty(TbSecondMassa.Text)
            )
            {
                if (!ModPlusAPI.Windows.MessageBox.ShowYesNo(ModPlusAPI.Language.GetItem(LangItem, "h54"),
                    MessageBoxIcon.Question))
                    return;
            }
            Hide();
            try
            {
                InsertToAutoCad.AddSpecificationItemToTableRow(new InsertToAutoCad.SpecificationItemForTable(
                    "",
                    "ГОСТ 8478-81",
                    "\\A1;" + TbSecondType.Text +
                    "{\\H1x; \\H0.9x;\\S" + TbSecondS.Text +
                    "/" + TbSecondSOne.Text +
                    ";\\H1x; }" + "2350  L=" + TbSecondMeshLength.Text,
                    TbSecondMassa.Text.Replace(',', '.').Replace('.', char.Parse(ModPlusAPI.Variables.Separator)),
                    "", ""));
            }
            catch (Exception exception)
            {
                ExceptionBox.Show(exception);
            }
            finally
            {
                Show();
            }
        }

        // Ввод длины сетки
        private void TbSecondMeshLength_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((TextBox)sender).Text))
            {
                SetMeshLength();
            }
            else TbSecondMeshLengthResult.Text = string.Empty;
        }
        private void SetMeshLength()
        {
            // Проверка длины сетки
            // Проверяем на остаток от деления за минусом выпусков
            if (!string.IsNullOrEmpty(TbSecondMeshLength.Text))
            {
                if (double.Parse(TbSecondMeshLength.Text) >
                    (double.Parse(TbSecondLongitudinalOutput.Text) * 2) +
                    double.Parse(CbSecondMainStep.SelectedItem.ToString()))
                {
                    var length = double.Parse(TbSecondMeshLength.Text);
                    var twoOutputs = double.Parse(TbSecondLongitudinalOutput.Text) * 2;
                    var step = double.Parse(CbSecondMainStep.SelectedItem.ToString());
                    var ieeereminder = Math.IEEERemainder(length - twoOutputs, step);
                    TbMessage.Text = Math.Abs(ieeereminder) > _ep ? ModPlusAPI.Language.GetItem(LangItem, "h57") : string.Empty;
                }
                else
                    TbMessage.Text = ModPlusAPI.Language.GetItem(LangItem, "h57");
                TbSecondMeshLengthResult.Text = TbSecondMeshLength.Text;
                Massa();
            }
        }
        // Передвижение ползунка
        private void slider2_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                TbSecondRound.Text = slider2.Value.ToString(CultureInfo.InvariantCulture);
                // Масса
                Massa();
            }
            catch
            {
                //ignored
            }
        }
        // Включение изображения
        private void BtSecondImage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                const string imagename = "ГОСТ 8478-81";

                var imgWin = new ShowImageWindow
                {
                    Img =
                    {
                        Source =
                            new BitmapImage(
                                new Uri(
                                    @"pack://application:,,,/mpMeshes_" + new Interface().AvailProductExternalVersion +
                                    ";component/Resources/" + imagename + ".png", UriKind.Absolute))
                    }
                };
                imgWin.ShowDialog();
            }
            catch (Exception ex)
            {
                ExceptionBox.Show(ex);
            }
        }
        #endregion

        #region Third
        // Список диаметров арматуры
        private readonly List<string> _thirdArmature = new List<string> { "6", "8", "10", "12", "14", "16", "18", "20", "22", "25", "28", "32" };
        // Масса арматуры
        private readonly List<string> _thirdArmatureMass = new List<string> { "0.222", "0.395", "0.617", "0.888", "1.21", "1.58", "2.0", "2.47", "2.98", "3.85", "4.83", "6.31" };
        // Список с длиннами
        private readonly List<string> _lengths = new List<string> { "1150", "1450", "1750", "2050", "2350", "2650", "2950",
            "3250", "3550", "3850", "4150", "4450", "4750", "5050", "5350", "5650", "5950", "6250", "6550", "6850",
            "7150", "7450", "7750", "8050", "8350", "8650", "8950" };
        // Список выпусков продольных стержней в соответствии с длиной (для типа 1)
        private readonly List<string> _outputs = new List<string>{"no","125","275","125","275","125","275","125","275","125","275",
            "125","275","125","275","125","275","125","275","125","275","125","275","125","275","125","275"};
        // Список с шириной
        private readonly List<string> _widths = new List<string> { "850", "1050", "1250", "1450", "1650", "1850", "2050", "2250", "2450", "2650", "2850" };
        // Первоначальное заполнение значений
        private void ThirdFill()
        {
            FillComboBoxWithStringList(CbThirdMeshType, new List<string> {
                ModPlusAPI.Language.GetItem(LangItem, "h58"),
                ModPlusAPI.Language.GetItem(LangItem, "h59")
            });
        }
        // Выбор типа сетки
        private void CbThirdMeshType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbThirdMeshType.SelectedIndex == 0)
            {
                TbThirdType.Text = "1C";
                // Заполняем значения ширины, т.к. от нее плясать будем
                FillComboBoxWithStringList(CbThirdMeshWidth, _widths);
            }
            else if (CbThirdMeshType.SelectedIndex == 1)
            {
                TbThirdType.Text = "2C";
                // Значения будем заносить "вручную", т.к. их немного
                FillComboBoxWithStringList(CbThirdMeshLength, _lengths.Take(11).ToList());
            }
        }
        // Подсчет массы сетки
        private void ThirdMassa()
        {
            try
            {
                string
                    longDiam =
                        CbThirdDiamsRatio.SelectedItem.ToString()
                            .Split('/')
                            .GetValue(0)
                            .ToString(), // Диаметр продольных стержней
                    transDiam =
                        CbThirdDiamsRatio.SelectedItem.ToString()
                            .Split('/')
                            .GetValue(1)
                            .ToString(), // Диаметр поперечных стержней
                    width = CbThirdMeshWidth.SelectedItem.ToString(), // Ширина сетки
                    length = CbThirdMeshLength.SelectedItem.ToString(), // Длина сетки
                    longOutput = TbThirdLongitudinalOutput.Text, // Выпуск продольных стержней
                    transOutput = TbThirdTransverseOutput.Text, // Выпуск поперечных стержней
                    n = "200", // Шаг продольных стержней
                    m = "200" // Шаг поперечных стержней
                    ;
                if (CbThirdMeshType.SelectedIndex == 0) // с рабочей арм. в 1 направл.
                    m = "600";
                TbThirdLongitudinalStep.Text = n;
                TbThirdTransverseStep.Text = m;
                // Количество продольных стержней
                var longSum = Math.Truncate((double.Parse(width) - (double.Parse(transOutput) * 2)) / double.Parse(n)) +
                              1;
                // Количество поперечных стержней стержней
                var transSum =
                    Math.Truncate((double.Parse(length) - (double.Parse(longOutput) * 2)) / double.Parse(m)) + 1;
                // Масса продольных стержней
                var longMassa = double.Parse(length) * longSum *
                                double.Parse(_thirdArmatureMass[_thirdArmature.IndexOf(longDiam)]) / 1000;
                // Масса поперечных стержней
                var transMassa = double.Parse(width) * transSum *
                                 double.Parse(_thirdArmatureMass[_thirdArmature.IndexOf(transDiam)]) / 1000;
                // Общая масса
                TbThirdMassa.Text = Math.Round(longMassa + transMassa, int.Parse(TbThirdRound.Text))
                    .ToString(CultureInfo.InvariantCulture);
            }
            catch
            {
                //ignored
            }
        }
        // Вставка результата
        private void BtThirdAdd_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            try
            {
                InsertToAutoCad.AddSpecificationItemToTableRow(new InsertToAutoCad.SpecificationItemForTable(
                    "",
                    "Серия 1.410-3 выпуск 1",
                    "\\A1;" + TbThirdType.Text +
                    "{\\H1x; \\H0.9x;\\S" + TbThirdLongDiam.Text +
                    "/" + TbThirdTransDiam.Text +
                    ";\\H1x; }" + TbThirdLength.Text + "x" + TbThirdWidth.Text,
                    TbThirdMassa.Text.Replace(',', '.').Replace('.', char.Parse(ModPlusAPI.Variables.Separator)),
                    "", ""));
            }
            catch (Exception exception)
            {
                ExceptionBox.Show(exception);
            }
            finally
            {
                Show();
            }
        }
        // Обработка выбора длины
        private void CbThirdMeshLength_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbThirdMeshLength.Items.Count == 0) return;

            var length = e.AddedItems[0].ToString();
            if (CbThirdMeshType.SelectedIndex == 0)// с рабочей арм. в 1 направл.
            {
                TbThirdLongitudinalOutput.Text = _outputs[_lengths.IndexOf(length)];
            }
            else // с рабочей арм. в 2 напр.
            {
                // Значения заполняем "вручную"
                switch (length)
                {
                    case "1150":
                        FillComboBoxWithStringList(CbThirdMeshWidth, new List<string> { "1150" });
                        TbThirdLongitudinalOutput.Text = "75";
                        break;
                    case "1450":
                        FillComboBoxWithStringList(CbThirdMeshWidth, new List<string> { "1150", "1450" });
                        TbThirdLongitudinalOutput.Text = "25";
                        break;
                    case "1750":
                        FillComboBoxWithStringList(CbThirdMeshWidth, new List<string> { "1450", "1750" });
                        TbThirdLongitudinalOutput.Text = "75";
                        break;
                    case "2050":
                        FillComboBoxWithStringList(CbThirdMeshWidth, new List<string> { "1750", "2050" });
                        TbThirdLongitudinalOutput.Text = "25";
                        break;
                    case "2350":
                        FillComboBoxWithStringList(CbThirdMeshWidth, new List<string> { "1750", "2050", "2350" });
                        TbThirdLongitudinalOutput.Text = "75";
                        break;
                    case "2650":
                        FillComboBoxWithStringList(CbThirdMeshWidth, new List<string> { "2350", "2650" });
                        TbThirdLongitudinalOutput.Text = "25";
                        break;
                    case "2950":
                        FillComboBoxWithStringList(CbThirdMeshWidth, new List<string> { "2350", "2650", "2950" });
                        TbThirdLongitudinalOutput.Text = "75";
                        break;
                    case "3250":
                        FillComboBoxWithStringList(CbThirdMeshWidth, new List<string> { "2650", "2950" });
                        TbThirdLongitudinalOutput.Text = "25";
                        break;
                    case "3550":
                        FillComboBoxWithStringList(CbThirdMeshWidth, new List<string> { "2950" });
                        TbThirdLongitudinalOutput.Text = "75";
                        break;
                    case "3850":
                        FillComboBoxWithStringList(CbThirdMeshWidth, new List<string> { "2950" });
                        TbThirdLongitudinalOutput.Text = "25";
                        break;
                    case "4150":
                        FillComboBoxWithStringList(CbThirdMeshWidth, new List<string> { "2950" });
                        TbThirdLongitudinalOutput.Text = "75";
                        break;
                }
            }
            // Записываем выбранное значение длины, поделив на 10
            TbThirdLength.Text = length.Substring(0, length.Length - 1);
            // Диаметры стержней
            ThirdFillDiams();
        }
        // Обработка выбора ширины
        private void CbThirdMeshWidth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbThirdMeshWidth.Items.Count == 0) return;

            string width = e.AddedItems[0].ToString();
            if (CbThirdMeshType.SelectedIndex == 0)// с рабочей арм. в 1 направл.
            {
                TbThirdTransverseOutput.Text = "25";

                if (width.Equals("850") || width.Equals("1050") ||
                    width.Equals("1250") || width.Equals("1450"))
                    FillComboBoxWithStringList(CbThirdMeshLength, _lengths.GetRange(1, 20));
                else
                    FillComboBoxWithStringList(CbThirdMeshLength, _lengths.GetRange(1, 26));
            }
            else // с рабочей арм. в 2 напр.
            {
                switch (width)
                {
                    case "1150": TbThirdTransverseOutput.Text = "75"; break;
                    case "1450": TbThirdTransverseOutput.Text = "25"; break;
                    case "1750": TbThirdTransverseOutput.Text = "75"; break;
                    case "2050": TbThirdTransverseOutput.Text = "25"; break;
                    case "2350": TbThirdTransverseOutput.Text = "75"; break;
                    case "2650": TbThirdTransverseOutput.Text = "25"; break;
                    case "2950": TbThirdTransverseOutput.Text = "75"; break;
                }
            }
            // Записываем выбранное значение длины, поделив на 10
            TbThirdWidth.Text = width.Substring(0, width.Length - 1);
            // Диаметры стержней
            ThirdFillDiams();
            // Масса
            ThirdMassa();
        }
        // Заполнение списков диаметров
        private void ThirdFillDiams()
        {
            if (CbThirdMeshLength.Items.Count == 0 & CbThirdMeshWidth.Items.Count == 0) return;
            string length = CbThirdMeshLength.SelectedItem.ToString(),
                width = CbThirdMeshWidth.SelectedItem.ToString();

            if (CbThirdMeshType.SelectedIndex == 0)// с рабочей арм. в 1 направл.
            {
                switch (width)
                {
                    case "850":
                        FillComboBoxWithStringList(CbThirdDiamsRatio,
                new List<string> { "10/6", "12/6", "14/6", "16/6", "18/6", "20/6", "22/6", "25/8", "28/8", "32/8" }); break;
                    case "1050":
                        FillComboBoxWithStringList(CbThirdDiamsRatio,
               new List<string> { "10/6", "12/6", "14/6", "16/6", "18/6", "20/8", "22/8", "25/8", "28/8", "32/10" }); break;
                    case "1250":
                        FillComboBoxWithStringList(CbThirdDiamsRatio,
               new List<string> { "10/6", "12/6", "14/6", "16/6", "18/8", "20/8", "22/8", "25/8", "28/10", "32/10" }); break;
                    case "1450":
                        FillComboBoxWithStringList(CbThirdDiamsRatio,
               new List<string> { "10/6", "12/6", "14/6", "16/8", "18/8", "20/8", "22/10", "25/10", "28/10", "32/12" }); break;
                    case "1650":
                        FillComboBoxWithStringList(CbThirdDiamsRatio,
               new List<string> { "10/6", "12/6", "14/8", "16/8", "18/8", "20/10", "22/10", "25/10", "28/12", "32/12" }); break;
                    case "1850":
                        FillComboBoxWithStringList(CbThirdDiamsRatio,
               new List<string> { "10/6", "12/8", "14/8", "16/8", "18/10", "20/10", "22/10", "25/12", "28/12", "32/14" }); break;
                    case "2050":
                        FillComboBoxWithStringList(CbThirdDiamsRatio,
               new List<string> { "10/8", "12/8", "14/8", "16/10", "18/10", "20/10", "22/12", "25/12", "28/14", "32/14" }); break;
                    case "2250":
                        FillComboBoxWithStringList(CbThirdDiamsRatio,
               new List<string> { "10/8", "12/8", "14/8", "16/10", "18/10", "20/10", "22/12", "25/12", "28/14", "32/14" }); break;
                    case "2450":
                        FillComboBoxWithStringList(CbThirdDiamsRatio,
               new List<string> { "10/8", "12/8", "14/8", "16/10", "18/10", "20/10", "22/12", "25/12", "28/14", "32/14" }); break;
                    case "2650":
                        FillComboBoxWithStringList(CbThirdDiamsRatio,
               new List<string> { "10/8", "12/8", "14/8", "16/10", "18/10", "20/10", "22/12", "25/12", "28/14", "32/14" }); break;
                    case "2850":
                        FillComboBoxWithStringList(CbThirdDiamsRatio,
           new List<string> { "10/8", "12/8", "14/8", "16/10", "18/10", "20/10", "22/12", "25/12", "28/14", "32/14" }); break;
                }
            }
            else // с рабочей арм. в 2 напр.
            {
                if (length.Equals("1150") & width.Equals("1150"))
                    FillComboBoxWithStringList(CbThirdDiamsRatio, new List<string> { "10/10", "12/12" });
                else if (length.Equals("1450") & width.Equals("1150"))
                    FillComboBoxWithStringList(CbThirdDiamsRatio, new List<string> { "10/10", "12/12", "14/12" });
                else if (length.Equals("1450") & width.Equals("1450"))
                    FillComboBoxWithStringList(CbThirdDiamsRatio, new List<string> { "10/10", "12/12" });
                else if (length.Equals("1750") & width.Equals("1450"))
                    FillComboBoxWithStringList(CbThirdDiamsRatio, new List<string> { "10/10", "12/12", "14/12" });
                else if (length.Equals("1750") & width.Equals("1750"))
                    FillComboBoxWithStringList(CbThirdDiamsRatio, new List<string> { "10/10", "12/12", "14/12", "14/14" });
                else if (length.Equals("2050") & width.Equals("1750"))
                    FillComboBoxWithStringList(CbThirdDiamsRatio, new List<string> { "10/10", "12/12", "14/12", "14/14", "16/14" });
                else if (length.Equals("2350") & width.Equals("1750"))
                    FillComboBoxWithStringList(CbThirdDiamsRatio, new List<string> { "10/10", "12/12", "14/12", "14/14", "16/14" });
                else if (length.Equals("2050") & width.Equals("2050"))
                    FillComboBoxWithStringList(CbThirdDiamsRatio, new List<string> { "10/10", "12/12", "14/12", "14/14" });
                else if (length.Equals("2350") & width.Equals("2050"))
                    FillComboBoxWithStringList(CbThirdDiamsRatio, new List<string> { "10/10", "12/12", "14/12", "14/14", "16/14" });
                else if (length.Equals("2650") & width.Equals("2350") & TbThirdTransverseOutput.Text.Equals("25"))
                    FillComboBoxWithStringList(CbThirdDiamsRatio, new List<string> { "10/10", "12/12", "14/12", "14/14", "16/14", "18/14" });
                else if (length.Equals("2350") & width.Equals("2350"))
                    FillComboBoxWithStringList(CbThirdDiamsRatio, new List<string> { "10/10", "12/12", "14/12", "14/14" });
                else if (length.Equals("2650") & width.Equals("2350") & TbThirdTransverseOutput.Text.Equals("75"))
                    FillComboBoxWithStringList(CbThirdDiamsRatio, new List<string> { "10/10", "12/12", "14/12", "14/14", "16/14", "18/14", "20/14" });
                else if (length.Equals("2950") & width.Equals("2350"))
                    FillComboBoxWithStringList(CbThirdDiamsRatio, new List<string> { "10/10", "12/12", "14/12", "14/14", "16/14", "18/14", "20/14" });
                else if (length.Equals("2650") & width.Equals("2650"))
                    FillComboBoxWithStringList(CbThirdDiamsRatio, new List<string> { "10/10", "12/12", "14/12", "14/14" });
                else if (length.Equals("2950") & width.Equals("2650"))
                    FillComboBoxWithStringList(CbThirdDiamsRatio, new List<string> { "10/10", "12/12", "14/12", "14/14", "16/14", "18/14", "20/14" });
                else if (length.Equals("3250") & width.Equals("2650"))
                    FillComboBoxWithStringList(CbThirdDiamsRatio, new List<string> { "10/10", "12/12", "14/12", "14/14", "16/14", "18/14", "20/14" });
                else if (length.Equals("2950") & width.Equals("2950"))
                    FillComboBoxWithStringList(CbThirdDiamsRatio, new List<string> { "10/10", "12/12", "14/12", "14/14", "16/14" });
                else if (length.Equals("3250") & width.Equals("2950") ||
                    length.Equals("3550") & width.Equals("2950") ||
                    length.Equals("3850") & width.Equals("2950") ||
                    length.Equals("4150") & width.Equals("2950"))
                    FillComboBoxWithStringList(CbThirdDiamsRatio, new List<string> { "10/10", "12/12", "14/12", "14/14", "16/14", "18/14", "20/14" });
            }
            // Масса
            ThirdMassa();
        }
        // Выбор отношения диаметров
        private void CbThirdDiamsRatio_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbThirdDiamsRatio.Items.Count == 0) return;
            string ratio = e.AddedItems[0].ToString();
            TbThirdLongDiam.Text = ratio.Split('/').GetValue(0) + "AIII";
            TbThirdTransDiam.Text = ratio.Split('/').GetValue(1) + "AIII";
            // Масса
            ThirdMassa();
        }
        // Передвижение ползунка
        private void slider3_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                TbThirdRound.Text = slider3.Value.ToString(CultureInfo.InvariantCulture);
                // Масса
                ThirdMassa();
            }
            catch
            {
                // ignored
            }
        }
        // Включение изображения
        private void BtThirdImage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var imagename = string.Empty;
                if (CbThirdMeshType.SelectedIndex == 0)
                    imagename = "Серия 1.410-3 в.1_1";
                else if (CbThirdMeshType.SelectedIndex == 1)
                    imagename = "Серия 1.410-3 в.1_2";

                var imgWin = new ShowImageWindow
                {
                    Img =
                    {
                        Source =
                            new BitmapImage(
                                new Uri(
                                    @"pack://application:,,,/mpMeshes_" + new Interface().AvailProductExternalVersion +
                                    ";component/Resources/" + imagename + ".png", UriKind.Absolute))
                    }
                };
                imgWin.ShowDialog();
            }
            catch (Exception ex)
            {
                ExceptionBox.Show(ex);
            }
        }
        #endregion
    }
    public class MpMeshesHelpFunc
    {
        private const string LangItem = "mpMeshes";

        // Вспомогательные и рабочие функции
        /// <summary>
        /// Заполнение окна вывода сообщений
        /// </summary>
        /// <param name="number">Номер сообщения</param>
        /// <param name="str">Дополнительное текстовое значение</param>
        public string Message(int number, string str)
        {
            var messages = new List<string>
            {
                string.Empty,
                Language.GetItem(LangItem, "h60") + " " + str + " " + Language.GetItem(LangItem, "mm"),
                Language.GetItem(LangItem, "h61") + " " + str + " " + Language.GetItem(LangItem, "mm"),
                Language.GetItem(LangItem, "h62") + " " + str + " " + Language.GetItem(LangItem, "mm"),
                Language.GetItem(LangItem, "h63") + " " + str + " " + Language.GetItem(LangItem, "mm")
            };
            return messages[number];
        }
    }
    /// <summary>
    /// Запуск функции
    /// </summary>
    public class MpMeshesFunc
    {
        private MpMeshes _window;

        [CommandMethod("ModPlus", "mpMeshes", CommandFlags.Modal)]
        public void Main()
        {
            Statistic.SendCommandStarting(new Interface());

            if (_window == null)
            {
                _window = new MpMeshes();
                _window.Closed += win_Closed;
            }
            if (_window.IsLoaded)
                _window.Activate();
            else
                AcApp.ShowModelessWindow(AcApp.MainWindow.Handle, _window);
        }

        private void win_Closed(object sender, EventArgs e)
        {
            _window = null;
            Utils.SetFocusToDwgView();
        }
    }
}

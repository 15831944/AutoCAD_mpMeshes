using System.Windows;
using System.Windows.Input;
using mpSettings;
using ModPlus;

namespace mpMeshes
{
    /// <summary>
    /// Логика взаимодействия для ShowImageWindow.xaml
    /// </summary>
    public partial class ShowImageWindow
    {
        public ShowImageWindow()
        {
            InitializeComponent();
            MpWindowHelpers.OnWindowStartUp(
                this,
                MpSettings.GetValue("Settings", "MainSet", "Theme"),
                MpSettings.GetValue("Settings", "MainSet", "AccentColor"),
                MpSettings.GetValue("Settings", "MainSet", "BordersType")
                );
        }
        // Нажатие кнопки Ок
        private void BtOk_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ShowImageWindow_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}

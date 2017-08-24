using System.Windows;
using System.Windows.Input;

namespace mpMeshesSA
{
    /// <summary>
    /// Логика взаимодействия для ShowImageWindow.xaml
    /// </summary>
    public partial class ShowImageWindow
    {
        public ShowImageWindow()
        {
            InitializeComponent();
        }
        // Нажатие кнопки Ок
        private void BtOk_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ShowImageWindow_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}

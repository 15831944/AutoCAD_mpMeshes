using System.Windows.Input;
using ModPlusAPI.Windows.Helpers;

namespace mpMeshes
{
    public partial class ShowImageWindow
    {
        public ShowImageWindow()
        {
            InitializeComponent();
            this.OnWindowStartUp();
        }
        private void ShowImageWindow_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}

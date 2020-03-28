namespace mpMeshes
{
    using System;
    using Autodesk.AutoCAD.ApplicationServices.Core;
    using Autodesk.AutoCAD.Internal;
    using Autodesk.AutoCAD.Runtime;
    using ModPlusAPI;

    /// <summary>
    /// Запуск функции
    /// </summary>
    public class MpMeshesFunc
    {
        private MpMeshes _window;

        [CommandMethod("ModPlus", "mpMeshes", CommandFlags.Modal)]
        public void Main()
        {
            Statistic.SendCommandStarting(new ModPlusConnector());

            if (_window == null)
            {
                _window = new MpMeshes();
                _window.Closed += win_Closed;
            }

            if (_window.IsLoaded)
            {
                _window.Activate();
            }
            else
            {
                Application.ShowModelessWindow(Application.MainWindow.Handle, _window);
            }
        }

        private void win_Closed(object sender, EventArgs e)
        {
            _window = null;
            Utils.SetFocusToDwgView();
        }
    }
}
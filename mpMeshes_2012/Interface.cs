using mpPInterface;

namespace mpMeshes
{
    public class Interface : IPluginInterface
    {
        private const string _Name = "mpMeshes";
        private const string _AvailCad = "2012";
        private const string _LName = "Сетки";
        private const string _Description = "Подбор и расчет массы арматурных сеток согласно нормативных документов";
        private const string _Author = "Пекшев Александр aka Modis";
        private const string _Price = "0";
        public string Name { get { return _Name; } }
        public string AvailCad { get { return _AvailCad; } }
        public string LName { get { return _LName; } }
        public string Description { get { return _Description; } }
        public string Author { get { return _Author; } }
        public string Price { get { return _Price; } }
    }
    public class VersionData
    {
        public const string FuncVersion = "2012";
    }
}

using mpPInterface;

namespace mpMeshes
{
    public class Interface : IPluginInterface
    {
        public string Name => "mpMeshes";
        public string AvailCad => "2018";
        public string LName => "Сетки";
        public string Description => "Подбор и расчет массы арматурных сеток согласно нормативных документов";
        public string Author => "Пекшев Александр aka Modis";
        public string Price => "0";
    }
    public class VersionData
    {
        public const string FuncVersion = "2018";
    }
}

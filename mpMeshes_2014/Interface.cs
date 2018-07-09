using System;
using System.Collections.Generic;
using ModPlusAPI.Interfaces;

namespace mpMeshes
{
    public class Interface : IModPlusFunctionInterface
    {
        public SupportedProduct SupportedProduct => SupportedProduct.AutoCAD;
        public string Name => "mpMeshes";
        public string AvailProductExternalVersion => "2014";
        public string FullClassName => string.Empty;
        public string AppFullClassName => string.Empty;
        public Guid AddInId => Guid.Empty;
        public string LName => "Сетки";
        public string Description => "Подбор и расчет массы арматурных сеток согласно нормативных документов";
        public string Author => "Пекшев Александр aka Modis";
        public string Price => "0";
        public bool CanAddToRibbon => true;
        public string FullDescription => string.Empty;
        public string ToolTipHelpImage => string.Empty;
        public List<string> SubFunctionsNames => new List<string>();
        public List<string> SubFunctionsLames => new List<string>();
        public List<string> SubDescriptions => new List<string>();
        public List<string> SubFullDescriptions => new List<string>();
        public List<string> SubHelpImages => new List<string>();
        public List<string> SubClassNames => new List<string>();
    }

}

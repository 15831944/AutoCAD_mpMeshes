namespace mpMeshes
{
    using System.Collections.Generic;
    using ModPlusAPI;

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
}
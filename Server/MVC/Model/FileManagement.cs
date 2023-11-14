namespace Server.MVC.Model
{
    public static class FileManagement
    {
        public static bool CheckFile(string path)
        {
            return File.Exists(path);
        }

        public static string[][] GetTableStr(string path, string sep = ";")
        {
            var strLines = FileReader.ReadLines(path);
            if (strLines.Count == 0) return Array.Empty<string[]>();
            string[][] tableStr = new string[strLines.Count][];
            for (int i = 0; i < strLines.Count; i++)
            {
                tableStr[i] = strLines[i].Split(sep);
            }
            return tableStr;
        }

        public static void SaveTable(string path, List<string[]> table)
        {
            FileWriter.OverwriteFile(path, table.Select(x => string.Join(";", x)).ToList());
        }
        
    }
}

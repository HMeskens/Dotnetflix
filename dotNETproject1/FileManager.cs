﻿using System.Collections.Generic;
using System.IO;

namespace dotNETproject1
{
    internal class FileManager
    {
        public void WriteDataToFile(string textToWriteFile, string path)
        {
            using StreamWriter write = new StreamWriter(path, true);
            write.WriteLine(textToWriteFile);
        }

        public List<string> ReadDataFromFile(string path)
        {
            using StreamReader reader = new StreamReader(path);
            string line = string.Empty;

            List<string> lines = new List<string>();

            while ((line = reader.ReadLine()) != null)
            {
                lines.Add(line);
            }
            return lines;
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SerializeProj
{
    internal class ScanerFolders
    {
        private DirectoryInfo directoryesInfo;
        private FileInfo[] filesInfo;
        private string[] files;
        private List<byte[]> binLines;
        private byte[] byteString;
        private string pathBinFile;
        private string[] oldFiles;
        List<string> newFiles;
        public void InitFolder() //Сюда бы конструктор запилить
        {
            directoryesInfo = new DirectoryInfo("/home/sergey/Projects/SerializeProj/MainFolder");
            filesInfo = directoryesInfo.GetFiles();
            files = Directory.GetFiles("/home/sergey/Projects/SerializeProj/MainFolder");
            binLines = new List<byte[]>();
            pathBinFile = "/home/sergey/Projects/SerializeProj/binarnik";
        }
        public void FormatStringFiles()
        {
            for (int path = 0; path < files.Length; path++)
            {
                files[path] = files[path] + "_" + filesInfo[path].Length + "\n";
            }
        }
        public void FormatStringsToArray() // После выполнения заполняется Лист массивами байтовыми.
        {
            for (int i = 0; i < files.Length; i++)
            {
                byteString = new byte[files[i].Length];
                byteString = Encoding.Unicode.GetBytes(files[i]);
                binLines.Add(byteString);
            }
        }
        public void CreateBinFile()
        {
            FileStream fs = new FileStream(pathBinFile, FileMode.OpenOrCreate);
            using (BinaryWriter bw = new BinaryWriter(fs))
            {
                for (int i = 0; i < binLines.Count; i++)
                {
                    bw.Write(binLines[i]);
                }

            }
            if (fs != null)
            {
                fs.Dispose();
            }
        }
        public void UnpackBinFile()
        {
            FileStream fs = new FileStream(pathBinFile, FileMode.OpenOrCreate);
            using (BinaryReader br = new BinaryReader(fs))
            {
                byte[] allstrInFile = br.ReadBytes((int)fs.Length);
                char[] ReadCH = br.ReadChars((int)fs.Length);
                char[] stringIn1252 = new char[Encoding.Unicode.GetCharCount(allstrInFile, 0, allstrInFile.Length)];
                Encoding.Unicode.GetChars(allstrInFile, 0, allstrInFile.Length, stringIn1252, 0);
                files = new string(stringIn1252).Split("\n");
                Array.Resize(ref files, files.Length - 1);
            }
            if (fs != null)
            {
                fs.Dispose();
            }
        }
        public bool CheckChanges()
        {
            newFiles = null;
            oldFiles = files;
            InitFolder();
            FormatStringFiles();
            newFiles = new List<string>();
            for (int i = 0; i < files.Length; i++)
            {
                bool flag =false;
                for (int j = 0; j < oldFiles.Length; j++)
                {
                    if (files[i].Equals(oldFiles[j]))
                    {
                        flag = true;
                        break;
                    }
                    else continue;
                }
                if(flag)
                {
                    flag = false;
                    continue;
                }
                else newFiles.Add(files[i]);

            }
            if (newFiles.Count != 0)
            {
                return true;
            }
            else return false;
        }
    }
}
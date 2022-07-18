using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Ultima
{
    public sealed class RadarCol
    {
        static RadarCol()
        {
            Initialize();
        }

        public static short[] Colors { get; private set; }

        public static short GetItemColor(int index)
        {
            if (index + 0x4000 < Colors.Length)
                return Colors[index + 0x4000];
            return 0;
        }
        public static short GetLandColor(int index)
        {
            if (index < Colors.Length)
                return Colors[index];
            return 0;
        }

        public static void SetItemColor(int index, short value)
        {
            Colors[index + 0x4000] = value;
        }
        public static void SetLandColor(int index, short value)
        {
            Colors[index] = value;
        }

        public static void Initialize()
        {
            string path = Files.GetFilePath("radarcol.mul");
            if (path != null)
            {
                using FileStream fs = new(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                Colors = new short[fs.Length / 2];
                GCHandle gc = GCHandle.Alloc(Colors, GCHandleType.Pinned);
                byte[] buffer = new byte[(int)fs.Length];
                fs.Read(buffer, 0, (int)fs.Length);
                Marshal.Copy(buffer, 0, gc.AddrOfPinnedObject(), (int)fs.Length);
                gc.Free();
            }
            else
                Colors = new short[0x8000];
        }

        public static void Save(string FileName)
        {
            using FileStream fs = new(FileName, FileMode.Create, FileAccess.Write, FileShare.Write);
            using BinaryWriter bin = new(fs);
            for (int i = 0; i < Colors.Length; ++i)
            {
                bin.Write(Colors[i]);
            }
        }

        public static void ExportToCSV(string FileName)
        {
            using StreamWriter Tex = new(new FileStream(FileName, FileMode.Create, FileAccess.ReadWrite), System.Text.Encoding.GetEncoding(1252));
            Tex.WriteLine("ID;Color");

            for (int i = 0; i < Colors.Length; ++i)
            {
                Tex.WriteLine(String.Format("0x{0:X4};{1}", i, Colors[i]));
            }
        }

        public static void ImportFromCSV(string FileName)
        {
            if (!File.Exists(FileName))
                return;
            using (StreamReader sr = new(FileName))
            {
                string line;
                int count = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    if ((line = line.Trim()).Length == 0 || line.StartsWith("#"))
                        continue;
                    if (line.StartsWith("ID;"))
                        continue;
                    ++count;
                }
                Colors = new short[count];
            }
            using (StreamReader sr = new(FileName))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if ((line = line.Trim()).Length == 0 || line.StartsWith("#"))
                        continue;
                    if (line.StartsWith("ID;"))
                        continue;
                    try
                    {
                        string[] split = line.Split(';');
                        if (split.Length < 2)
                            continue;

                        int id = ConvertStringToInt(split[0]);
                        int color = ConvertStringToInt(split[1]);
                        Colors[id] = (short)color;

                    }
                    catch { }
                }
            }
        }

        private static int ConvertStringToInt(string text)
        {
            int result;
            if (text.Contains("0x"))
            {
                string convert = text.Replace("0x", "");
                int.TryParse(convert, System.Globalization.NumberStyles.HexNumber, null, out result);
            }
            else
                int.TryParse(text, System.Globalization.NumberStyles.Integer, null, out result);

            return result;
        }
    }
}

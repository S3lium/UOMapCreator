using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace Transition
{
    public class RandomStatics : CollectionBase
    {
        private readonly Collection m_Random;
        public int Freq { get; set; }
        public RandomStaticCollection this[int index]
        {
            get => (RandomStaticCollection)this.List[index];
            set => this.List[index] = value;
        }
        public void Add(RandomStaticCollection Value)
        {
            this.InnerList.Add(Value);
            byte arg_17_0 = 0;
            byte b = checked((byte)Value.Count);
            for (byte b2 = arg_17_0; b2 <= b; b2 += 1)
            {
                this.m_Random.Add(Value, null, null, null);
            }
        }
        public void Remove(RandomStaticCollection Value)
        {
            this.InnerList.Remove(Value);
        }
        public RandomStatics()
        {
            this.m_Random = new Collection();
        }
        public RandomStatics(string iFileName)
        {
            this.m_Random = new Collection();
            XmlDocument xmlDocument = new();
            try
            {
                string filename = string.Format("Data/Statics/{0}", iFileName);
                xmlDocument.Load(filename);
                XmlElement xmlElement = (XmlElement)xmlDocument.SelectSingleNode("//RandomStatics");
                this.Freq = XmlConvert.ToInt16(xmlElement.GetAttribute("Chance"));

                IEnumerator enumerator = xmlElement.SelectNodes("Statics").GetEnumerator();

                try
                {
                    while (enumerator.MoveNext())
                    {
                        XmlElement xmlInfo = (XmlElement)enumerator.Current;
                        RandomStaticCollection randomStaticCollection = new(xmlInfo);
                        this.InnerList.Add(randomStaticCollection);
                        if (randomStaticCollection.Freq > 0)
                        {
                            byte arg_AC_0 = 1;
                            byte b = checked((byte)randomStaticCollection.Freq);
                            for (byte b2 = arg_AC_0; b2 <= b; b2 += 1)
                            {
                                this.m_Random.Add(randomStaticCollection, null, null, null);
                            }
                        }
                    }
                }
                finally
                {
                    if (enumerator is IDisposable)
                    {
                        ((IDisposable)enumerator).Dispose();
                    }
                }
            }
            catch (Exception expr_F8)
            {
                ProjectData.SetProjectError(expr_F8);
                Interaction.MsgBox("Can not find:" + iFileName, MsgBoxStyle.OkOnly, null);
                ProjectData.ClearProjectError();
            }
        }
        public void Save(string iFileName)
        {
            XmlTextWriter xmlTextWriter = new(iFileName, Encoding.UTF8);
            xmlTextWriter.Indentation = 2;
            xmlTextWriter.Formatting = Formatting.Indented;
            xmlTextWriter.WriteStartDocument();
            xmlTextWriter.WriteStartElement("RandomStatics");
            xmlTextWriter.WriteAttributeString("Chance", StringType.FromInteger(this.Freq));

            IEnumerator enumerator = this.InnerList.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    RandomStaticCollection randomStaticCollection = (RandomStaticCollection)enumerator.Current;
                    randomStaticCollection.Save(xmlTextWriter);
                }
            }
            finally
            {
                if (enumerator is IDisposable)
                {
                    ((IDisposable)enumerator).Dispose();
                }
            }
            xmlTextWriter.WriteEndElement();
            xmlTextWriter.WriteEndDocument();
            xmlTextWriter.Close();
        }
        public void Display(ListBox iList)
        {
            iList.Items.Clear();

            IEnumerator enumerator = this.InnerList.GetEnumerator();

            try
            {
                while (enumerator.MoveNext())
                {
                    RandomStaticCollection item = (RandomStaticCollection)enumerator.Current;
                    iList.Items.Add(item);
                }
            }
            finally
            {
                if (enumerator is IDisposable)
                {
                    ((IDisposable)enumerator).Dispose();
                }
            }
        }
        public void GetRandomStatic(short X, short Y, short Z, Collection[,] StaticMap)
        {
            checked
            {
                if (this.m_Random.Count != 0)
                {
                    VBMath.Randomize();
                    if ((int)Math.Round((double)Conversion.Int(unchecked(100f * VBMath.Rnd()))) <= this.Freq)
                    {
                        int index = (int)Math.Round((double)unchecked(Conversion.Int(checked(this.m_Random.Count - 1)) * VBMath.Rnd())) + 1;
                        ((RandomStaticCollection)this.m_Random[index]).RandomStatic(X, Y, Z, StaticMap);
                    }
                }
            }
        }
    }
}

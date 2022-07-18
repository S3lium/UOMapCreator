using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;
using System.Windows.Forms;
using System.Xml;

namespace Transition
{
    public class RandomStaticCollection : CollectionBase
    {
        public Collection iCollection => (Collection)this.List;
        public string Description { get; set; }
        public int Freq { get; set; }
        public RandomStatic this[int index]
        {
            get => (RandomStatic)this.List[index];
            set => this.List[index] = value;
        }
        public void Add(RandomStatic Value)
        {
            this.InnerList.Add(Value);
        }
        public void Remove(RandomStatic Value)
        {
            this.InnerList.Remove(Value);
        }
        public void Save(XmlTextWriter xmlInfo)
        {
            xmlInfo.WriteStartElement("Statics");
            xmlInfo.WriteAttributeString("Description", this.Description);
            xmlInfo.WriteAttributeString("Freq", StringType.FromInteger(this.Freq));

            IEnumerator enumerator = this.InnerList.GetEnumerator();

            try
            {
                while (enumerator.MoveNext())
                {
                    RandomStatic randomStatic = (RandomStatic)enumerator.Current;
                    randomStatic.Save(xmlInfo);
                }
            }
            finally
            {
                if (enumerator is IDisposable)
                {
                    ((IDisposable)enumerator).Dispose();
                }
            }
            xmlInfo.WriteEndElement();
        }
        public void Display(ListBox iList)
        {
            iList.Items.Clear();
            try
            {
                foreach (RandomStatic item in InnerList)
                {
                    iList.Items.Add(item);
                }
            }
            catch
            {

            }
        }
        public void RandomStatic(short X, short Y, short Z, Collection[,] StaticMap)
        {
            IEnumerator enumerator = this.InnerList.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    RandomStatic randomStatic = (RandomStatic)enumerator.Current;
                    StaticCell item = new(randomStatic.TileID, checked((byte)(unchecked(X + randomStatic.X) % 8)), checked((byte)(unchecked(Y + randomStatic.Y) % 8)), (short)(Z + randomStatic.Z));
                    StaticMap[(short)(X + randomStatic.X >> 3), (short)(Y + randomStatic.Y >> 3)].Add(item, null, null, null);
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
        public override string ToString()
        {
            return string.Format("{0} Freq:{1}", this.Description, this.Freq);
        }
        public RandomStaticCollection()
        {
        }
        public RandomStaticCollection(string iDescription, int iFreq)
        {
            this.Description = iDescription;
            this.Freq = iFreq;
        }
        public RandomStaticCollection(XmlElement xmlInfo)
        {
            this.Description = xmlInfo.GetAttribute("Description");
            this.Freq = XmlConvert.ToInt16(xmlInfo.GetAttribute("Freq"));

            IEnumerator enumerator = xmlInfo.SelectNodes("Static").GetEnumerator();

            try
            {
                while (enumerator.MoveNext())
                {
                    XmlElement xmlInfo2 = (XmlElement)enumerator.Current;
                    this.InnerList.Add(new RandomStatic(xmlInfo2));
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
    }
}

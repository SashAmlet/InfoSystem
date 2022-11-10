using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace InfoSystem.AnalysingMethods
{
    internal class XmlSAXAnalysing: IAnalysing
    {
        public void AnalisingMethod(string[] restrictions, string filePath, DataGridView dataGridView1)
        {
            int columnCount = dataGridView1.ColumnCount;
            int rowCount = dataGridView1.RowCount;
            var sb = new string[columnCount];
            var xmlReader = new XmlTextReader(filePath);
            int col = 0, row = 0;
            while (xmlReader.Read())
            {
                switch (xmlReader.NodeType)
                {
                    case XmlNodeType.XmlDeclaration:
                        break;
                    case XmlNodeType.Element:
                        if (xmlReader.Name == "specialty")
                        {
                            if ((xmlReader.GetAttribute(0) == (restrictions[0] == String.Empty ? xmlReader.GetAttribute(0) : restrictions[0])))
                            {
                                col = 0;
                                sb[col++] = xmlReader.GetAttribute(0);
                            }
                            else
                            {
                                xmlReader.Read();
                                while (xmlReader.Name != "specialty")
                                {
                                    xmlReader.Read();
                                }
                            }
                        }
                        else if (xmlReader.Name == "group")
                        {
                            col = 1;
                            if ((xmlReader.GetAttribute(0) == (restrictions[1] == String.Empty ? xmlReader.GetAttribute(0) : restrictions[1])))
                            {
                                sb[col++] = xmlReader.GetAttribute(0);
                            }
                            else
                            {
                                xmlReader.Read();
                                while (xmlReader.Name != "group")// || xmlReader.Name != "specialty")
                                {
                                    xmlReader.Read();
                                }
                            }
                        }
                        else if (xmlReader.Name == "student")
                        {
                            col = 2;
                            while (xmlReader.Read())
                            {
                                if (xmlReader.Name == "student")
                                    break;
                                else if (xmlReader.NodeType == XmlNodeType.Text)
                                {
                                    if (xmlReader.Value == (restrictions[col] == String.Empty ? xmlReader.Value : restrictions[col]))
                                    {
                                        sb[col++] = xmlReader.Value;
                                    }
                                    else
                                    {
                                        while (xmlReader.Name != "student")
                                        {
                                            xmlReader.Read();
                                        }
                                        break;

                                    }
                                    if (col == columnCount)
                                    {
                                        int i = 0;
                                        foreach (string cell in sb)
                                        {
                                            dataGridView1[i++, row].Value = cell;
                                        }
                                        if (rowCount - row == 1)
                                        {
                                            rowCount += 10;
                                            dataGridView1.RowCount = rowCount;
                                        }
                                        ++row;
                                    }
                                }
                            }

                        }
                        break;
                    case XmlNodeType.Comment:
                        break;
                    case XmlNodeType.Text:
                        break;
                }
            }
            xmlReader.Close();
        }
    }
}

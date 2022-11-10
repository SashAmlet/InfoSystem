using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace InfoSystem.AnalysingMethods
{
    internal class XmlDOMAnalysing : IAnalysing
    {
        public void AnalisingMethod(string[] restrictions, string filePath, DataGridView dataGridView1)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(filePath);
            XmlElement dataBase = xml.DocumentElement;
            int rowCount = dataGridView1.Rows.Count;

            int col = 0, row = 0;
            foreach (XmlNode _specialty in dataBase) // specialty
            {
                if (restrictions[0] == _specialty.Attributes.GetNamedItem("SPECIALTY").Value || restrictions[0] == string.Empty)
                {
                    foreach (XmlNode _group in _specialty.ChildNodes) //group
                    {
                        if (restrictions[1] == _group.Attributes.GetNamedItem("GROUP").Value || restrictions[1] == string.Empty)
                        {
                            foreach (XmlNode _student in _group.ChildNodes) //student
                            {
                                if ((restrictions[2] == _student.ChildNodes[0].InnerText || restrictions[2] == string.Empty) && (restrictions[3] == _student.ChildNodes[1].InnerText || restrictions[3] == string.Empty) && (restrictions[4] == _student.ChildNodes[2].InnerText || restrictions[4] == string.Empty) && (restrictions[5] == _student.ChildNodes[3].InnerText || restrictions[5] == string.Empty))
                                {
                                    dataGridView1[col++, row].Value = _specialty.Attributes.GetNamedItem("SPECIALTY").Value;
                                    dataGridView1[col++, row].Value = _group.Attributes.GetNamedItem("GROUP").Value;
                                    dataGridView1[col++, row].Value = _student.ChildNodes[0].InnerText;
                                    dataGridView1[col++, row].Value = _student.ChildNodes[1].InnerText;
                                    dataGridView1[col++, row].Value = _student.ChildNodes[2].InnerText;
                                    dataGridView1[col++, row].Value = _student.ChildNodes[3].InnerText;

                                    if (rowCount - row == 1)
                                    {
                                        rowCount += 10;
                                        dataGridView1.RowCount = rowCount;
                                    }
                                    ++row;
                                    col = 0;
                                }

                            }
                        }
                    }
                }
            }
        }
    }
}

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
            // // // Знаходжу усі моди, що відповідають відповідним restrictions і запихую їх у students // // //
            string[] res3 = { "specialty[@SPECIALTY", "group[@GROUP", "[name", "[surname", "[phone", "[registration" };
            var xpath = string.Empty; // той самий магічний стрінг, що містить шлях до студента, що відповідає усім параметрам
            int sw = 2;
            for (int i = 0; i < sw; i++) 
            {
                xpath += (restrictions[i] != string.Empty ? "//" + res3[i] + " = '" + restrictions[i] + "']" : "");
            }
            xpath += "//student";
            for (int i = sw; i < res3.Length; i++)
            {
                xpath += (restrictions[i] != string.Empty ? /*"//" +*/ res3[i] + " = '" + restrictions[i] + "']" : "");
            }
            var students = xml.SelectNodes(xpath);
            // // // Заповнюю табличку // // //
            int rowCount = dataGridView1.Rows.Count;
            int col = 0, row = 0;
            foreach (XmlNode student in students)
            {
                dataGridView1[col++, row].Value = student.ParentNode.ParentNode.Attributes.GetNamedItem("SPECIALTY").Value;
                dataGridView1[col++, row].Value = student.ParentNode.Attributes.GetNamedItem("GROUP").Value;
                dataGridView1[col++, row].Value = student.ChildNodes[0].InnerText;
                dataGridView1[col++, row].Value = student.ChildNodes[1].InnerText;
                dataGridView1[col++, row].Value = student.ChildNodes[2].InnerText;
                dataGridView1[col++, row].Value = student.ChildNodes[3].InnerText;

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

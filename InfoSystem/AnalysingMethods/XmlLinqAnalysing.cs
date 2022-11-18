using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace InfoSystem.AnalysingMethods
{
    internal class XmlLinqAnalysing : IAnalysing
    {
        public void AnalisingMethod(string[] restrictions, string filePath, DataGridView dataGridView1)
        {
            int rowCount = dataGridView1.RowCount;
            XDocument xdoc = XDocument.Load(filePath);
            // // // За допомогою Linq зберігаю у list myStudents усих студентів, що відповідають фільтрам// // //
            var myStudents =
                (
                from mySpecialty in xdoc.Descendants("specialty")
                where (mySpecialty.Attribute("SPECIALTY").Value == (restrictions[0] == string.Empty ? mySpecialty.Attribute("SPECIALTY").Value : restrictions[0]))
                from myGroup in mySpecialty.Descendants("group")
                where (myGroup.Attribute("GROUP").Value == (restrictions[1] == string.Empty ? myGroup.Attribute("GROUP").Value : restrictions[1]))
                from mySt in myGroup.Descendants("student")
                where ((mySt.Elements("name").Single().Value == (restrictions[2] == String.Empty ? mySt.Elements("name").Single().Value : restrictions[2])) && (mySt.Elements("surname").Single().Value == (restrictions[3] == String.Empty ? mySt.Elements("surname").Single().Value : restrictions[3])) && (mySt.Elements("phone").Single().Value == (restrictions[4] == String.Empty ? mySt.Elements("phone").Single().Value : restrictions[4])) && (mySt.Elements("registration").Single().Value == (restrictions[5] == String.Empty ? mySt.Elements("registration").Single().Value : restrictions[5])))
                select new
                {
                    _spec = (string)mySpecialty.Attribute("SPECIALTY").Value,
                    _group = (string)myGroup.Attribute("GROUP").Value,
                    _name = (string)mySt.Elements("name").Single().Value,
                    _surname = (string)mySt.Elements("surname").Single().Value,
                    _phone = (string)mySt.Elements("phone").Single().Value,
                    _registration = (string)mySt.Elements("registration").Single().Value
                }
                ).ToList();
            // // // Виводжу свій лист // // //
            int col = 0, row = 0;
            foreach (var st in myStudents)
            {
                dataGridView1[col++, row].Value = st._spec;
                dataGridView1[col++, row].Value = st._group;
                dataGridView1[col++, row].Value = st._name;
                dataGridView1[col++, row].Value = st._surname;
                dataGridView1[col++, row].Value = st._phone;
                dataGridView1[col++, row].Value = st._registration;
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

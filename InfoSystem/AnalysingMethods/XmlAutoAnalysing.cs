using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace InfoSystem.AnalysingMethods
{
    internal class XmlAutoAnalysing: IAnalysing
    {
        public dataBase XmlAutoReading(string filePath) // Заповнює класи у XMLClasses інфою із нашого dataBase.xml
        {
            return (dataBase)new XmlSerializer(typeof(dataBase)).Deserialize(new StreamReader(filePath));
        }
        public void AnalisingMethod(string[] restrictions, string filePath, DataGridView dataGridView1) // Роблю теж саме, що й у XmlLinqAnalising, але працюю не з XML файлом, а з класами XMLClasses
        {
            int rowCount = dataGridView1.RowCount;
            dataBase dataBaseClass = XmlAutoReading(filePath); // закидую інфу з мого xml файла у класи XMLClasses
            // // // За допомогою Linq зберігаю у mySpecialty усі спеціальності, що задані фільтром (або якась конкретна, або усі разом) // // //
            var myStudents =
                (
                from mySpecialty in dataBaseClass.specialty
                where (mySpecialty.SPECIALTY == (restrictions[0] == string.Empty ? mySpecialty.SPECIALTY : restrictions[0]))
                from myGroup in mySpecialty.@group
                where (myGroup.GROUP == (restrictions[1] == String.Empty ? myGroup.GROUP : restrictions[1]))
                from mySt in myGroup.student
                where ((mySt.name == (restrictions[2] == String.Empty ? mySt.name : restrictions[2])) && (mySt.surname == (restrictions[3] == String.Empty ? mySt.surname : restrictions[3])) && (mySt.phone == (restrictions[4] == String.Empty ? mySt.phone : ulong.Parse(restrictions[4]))) && (mySt.registration == (restrictions[5] == String.Empty ? mySt.registration : restrictions[5])))
                select new
                {
                    _spec = (string)mySpecialty.SPECIALTY,
                    _group = (string)myGroup.GROUP,
                    _name = (string)mySt.name,
                    _surname = (string)mySt.surname,
                    _phone = (ulong) mySt.phone,
                    _registration = (string)mySt.registration
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

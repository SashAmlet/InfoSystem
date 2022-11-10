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
            var mySpecialty =
                from specialty in dataBaseClass.specialty
                where (specialty.SPECIALTY == (restrictions[0] == string.Empty ? specialty.SPECIALTY : restrictions[0]))
                select specialty;
            // // // 

            int col = 0, row = 0;
            foreach (var sp in mySpecialty)
            {
                // // // До кожної спеціальності підбираю список груп (myGroup), що відповідають фільтрам // // //
                var myGroup =
                    from spGroup in sp.@group
                    where (spGroup.GROUP == (restrictions[1] == String.Empty ? spGroup.GROUP : restrictions[1]))
                    select spGroup;
                // // // 
                foreach (var gr in myGroup)
                {
                    // // // У кожній групі вибираю студентів, що проходят по усім фільтрам (ім'я, призвище, номер телефону, прописка) // // //)
                    var myStudent =
                        from mySt in gr.student
                        where ((mySt.name == (restrictions[2] == String.Empty ? mySt.name : restrictions[2])) && (mySt.surname == (restrictions[3] == String.Empty ? mySt.surname : restrictions[3])) && (mySt.phone == (restrictions[4] == String.Empty ? mySt.phone : ulong.Parse(restrictions[4]))) && (mySt.registration == (restrictions[5] == String.Empty ? mySt.registration : restrictions[5])))
                        select mySt;
                    // // // Виводжу усе, що відфільтрувалось // // //
                    foreach (var st in myStudent)
                    {
                        dataGridView1[col++, row].Value = sp.SPECIALTY;
                        dataGridView1[col++, row].Value = gr.GROUP;
                        dataGridView1[col++, row].Value = st.name;
                        dataGridView1[col++, row].Value = st.surname;
                        dataGridView1[col++, row].Value = st.phone;
                        dataGridView1[col++, row].Value = st.registration;
                        col = 0;
                        if (rowCount - row == 1)
                        {
                            rowCount += 10;
                            dataGridView1.RowCount = rowCount;
                        }
                        ++row;
                    }
                    // // //
                }
            }

        }
    }
}

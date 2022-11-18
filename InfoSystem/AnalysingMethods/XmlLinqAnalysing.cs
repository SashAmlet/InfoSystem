using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace InfoSystem.AnalysingMethods
{
    internal class XmlLinqAnalysing: IAnalysing
    {
        public void AnalisingMethod(string[] restrictions, string filePath, DataGridView dataGridView1)
        {
            int rowCount = dataGridView1.RowCount;
            XDocument xdoc = XDocument.Load(filePath);
            // // // За допомогою Linq зберігаю у mySpecialty усі спеціальності, що задані фільтром (або якась конкретна, або усі разом) // // //
            var mySpecialty =
                from specialty in xdoc.Descendants("specialty")
                where (specialty.Attribute("SPECIALTY").Value == (restrictions[0] == string.Empty ? specialty.Attribute("SPECIALTY").Value : restrictions[0]))
                select specialty;
            // // // 

            int col = 0, row = 0;
            foreach (var sp in mySpecialty)
            {
                // // // До кожної спеціальності підбираю список груп (myGroup), що відповідають фільтрам // // //
                var myGroup =
                    from spGroup in sp.Descendants("group")
                    where (spGroup.Attribute("GROUP").Value == (restrictions[1] == string.Empty ? spGroup.Attribute("GROUP").Value : restrictions[1]))
                    select spGroup;
                // // // 
                foreach (var gr in myGroup)
                {
                    // // // У кожній групі вибираю студентів, що проходят по усім фільтрам (ім'я, призвище, номер телефону, прописка) // // //)
                    var myStudent =
                        from mySt in gr.Descendants("student")
                        where ((mySt.Elements("name").Single().Value == (restrictions[2] == String.Empty ? mySt.Elements("name").Single().Value : restrictions[2])) && (mySt.Elements("surname").Single().Value == (restrictions[3] == String.Empty ? mySt.Elements("surname").Single().Value : restrictions[3])) && (mySt.Elements("phone").Single().Value == (restrictions[4] == String.Empty ? mySt.Elements("phone").Single().Value : restrictions[4])) && (mySt.Elements("registration").Single().Value == (restrictions[5] == String.Empty ? mySt.Elements("registration").Single().Value : restrictions[5])))
                        select mySt;
                    // // // Виводжу усе, що відфільтрувалось // // //
                    foreach (var st in myStudent)
                    {
                        dataGridView1[col++, row].Value = sp.Attribute("SPECIALTY").Value;
                        dataGridView1[col++, row].Value = gr.Attribute("GROUP").Value;
                        dataGridView1[col++, row].Value = st.Elements("name").Single().Value;
                        dataGridView1[col++, row].Value = st.Elements("surname").Single().Value;
                        dataGridView1[col++, row].Value = st.Elements("phone").Single().Value;
                        dataGridView1[col++, row].Value = st.Elements("registration").Single().Value;
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

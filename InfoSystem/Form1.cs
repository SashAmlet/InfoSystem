using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Xml.Serialization;
using System.Data.SqlTypes;
using System.Text;
using System.Xml.Xsl;

namespace InfoSystem
{
    public partial class Form1 : Form
    {
        //private dataBase dataBaseClass;
        private const int columnCount = 6, checkBoxCount = 6;
        private int rowCount;
        private const string filePath = "dataBase.xml";

        // // // Initializing part // // //
        private void InitializeDataGridView()
        {

            string[] headers = {"Specialty","Group","Name","Surname","Phone number","Registration"};
            rowCount = 10;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.ColumnCount = columnCount;
            dataGridView1.RowCount = rowCount;
            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                col.HeaderText = headers[col.Index];
            }
        }
        private void InitializeComboBox(dataBase dataBaseClass)
        {
            // // //Заповнення ComboBox-ів без повтор. елементів // // //
            foreach (dataBaseSpecialty DBS in dataBaseClass.specialty)
            {
                specialtyComboBox.Items.Add(DBS.SPECIALTY);
            }
            foreach (dataBaseSpecialty DBS in dataBaseClass.specialty)
            {
                foreach (dataBaseSpecialtyGroup DBSG in DBS.group)
                {
                    groupComboBox.Items.Add(DBSG.GROUP);
                }
            }
            // // // Заповнення ComboBox-ів з повтор. елементами// // // 
            List<string> nameList = new List<string>();
            List<string> surnameList = new List<string>();
            List<ulong> phoneList = new List<ulong>();
            List<string> registrationList = new List<string>();
            // Заповнюю відповідні листи
            foreach (dataBaseSpecialty DBS in dataBaseClass.specialty)
            {
                foreach (dataBaseSpecialtyGroup DBSG in DBS.group)
                {
                    foreach (dataBaseSpecialtyGroupStudent DBSGS in DBSG.student)
                    {
                        nameList.Add(DBSGS.name);
                        surnameList.Add(DBSGS.surname);
                        phoneList.Add(DBSGS.phone);
                        registrationList.Add(DBSGS.registration);
                    }
                }
            }
            // Видаляю з листів дублюючі елементи
            var newNameList = new HashSet<string>(nameList).ToList();
            var newSurnameList = new HashSet<string>(surnameList).ToList();
            var newPhoneList = new HashSet<ulong>(phoneList).ToList();
            var newRegistrationList = new HashSet<string>(registrationList).ToList();
            // Заповнюю ComboBox-си
            foreach (string el in newNameList)
            {
                nameComboBox.Items.Add(el);
            }
            foreach (string el in newSurnameList)
            {
                surnameComboBox.Items.Add(el);
            }
            foreach (ulong el in newPhoneList)
            {
                phoneComboBox.Items.Add(el);
            }
            foreach (string el in newRegistrationList)
            {
                registrationComboBox.Items.Add(el);
            }
        }
        private void InitializeRadioButton()
        {
            DomRadioButton.Checked = true;
        }
        // // //
        private dataBase XmlAutoReading() // Заповнює класи у XMLClasses інфою із нашого dataBase.xml
        {
            return (dataBase)new XmlSerializer(typeof(dataBase)).Deserialize(new StreamReader(filePath));
        }
        public Form1()
        {

            InitializeComponent();
            InitializeRadioButton();
            InitializeComboBox(XmlAutoReading());
            InitializeDataGridView();


        }
        // // // Restriction methods // // //
        private bool[] checkBoxCheck()
        {
            var checkBoxes = new bool[checkBoxCount]; 
            checkBoxes[0] = checkBoxSpecialty.Checked ? true:false;
            checkBoxes[1] = checkBoxGroup.Checked ? true : false;
            checkBoxes[2] = checkBoxName.Checked ? true : false;
            checkBoxes[3] = checkBoxSurname.Checked ? true : false;
            checkBoxes[4] = checkBoxPhone.Checked ? true : false;
            checkBoxes[5] = checkBoxReg.Checked ? true : false;
            return checkBoxes;
        }
        private string[] Restriction(bool[] checkBoxes)
        {
            var restrictions = new string[checkBoxCount];
            restrictions[0] = checkBoxes[0] == true ? (specialtyComboBox.SelectedItem != null ? specialtyComboBox.SelectedItem.ToString() : String.Empty) : string.Empty;
            restrictions[1] = checkBoxes[1] == true ? (groupComboBox.SelectedItem != null ? groupComboBox.SelectedItem.ToString() : String.Empty) : string.Empty;
            restrictions[2] = checkBoxes[2] == true ? (nameComboBox.SelectedItem != null ? nameComboBox.SelectedItem.ToString() : String.Empty) : string.Empty;
            restrictions[3] = checkBoxes[3] == true ? (surnameComboBox.SelectedItem != null ? surnameComboBox.SelectedItem.ToString() : String.Empty) : string.Empty;
            restrictions[4] = checkBoxes[4] == true ? (phoneComboBox.SelectedItem != null ? phoneComboBox.SelectedItem.ToString() : String.Empty) : string.Empty;
            restrictions[5] = checkBoxes[5] == true ? (registrationComboBox.SelectedItem != null ? registrationComboBox.SelectedItem.ToString() : String.Empty) : string.Empty;
            return restrictions;
        }

        // // // Analising (output) methods // // //
        private void XmlDOMAnalising(string[] restrictions)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(filePath);
            XmlElement dataBase = xml.DocumentElement;

            int col = 0, row = 0;
            foreach (XmlNode _specialty in dataBase) // specialty
            {
                if ((restrictions[0] == _specialty.Attributes.GetNamedItem("SPECIALTY").Value) || (restrictions[0] == string.Empty))
                {
                    foreach (XmlNode _group in _specialty.ChildNodes) //group
                    {
                        if ((restrictions[1] == _group.Attributes.GetNamedItem("GROUP").Value) || (restrictions[1] == string.Empty))
                        {
                            foreach (XmlNode _student in _group.ChildNodes) //student
                            {
                                if ((restrictions[2] == _student.ChildNodes[0].InnerText || (restrictions[2] == string.Empty)) &&(restrictions[3] == _student.ChildNodes[1].InnerText || (restrictions[3] == string.Empty)) &&(restrictions[4] == _student.ChildNodes[2].InnerText || (restrictions[4] == string.Empty)) &&(restrictions[5] == _student.ChildNodes[3].InnerText || (restrictions[5] == string.Empty)))
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
        private void XmlSAXAnalising(string[] restrictions)
        {
            var sb = new string[columnCount];
            var xmlReader = new XmlTextReader(filePath);
            int col = 0, row = 0;
            while (xmlReader.Read())
            {
                switch(xmlReader.NodeType)
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
                                else if(xmlReader.NodeType == XmlNodeType.Text)
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
                                        foreach(string cell in sb)
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
        private void XmlAutoAnalising(string[] restrictions) // Роблю теж саме, що й у XmlLinqAnalising, але працюю не з XML файлом, а з класами XMLClasses
        {
            dataBase dataBaseClass = XmlAutoReading(); // закидую інфу з мого xml файла у класи XMLClasses
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
        private void XmlLinqAnalising(string[] restrictions)
        {
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
                    // // //
                }
            }
        }
        private void AnaliseFile(string[] restrictions)
        {
            if (File.Exists(filePath))
            {
                if (DomRadioButton.Checked)
                    XmlDOMAnalising(restrictions);
                else if (SaxRadioButton.Checked)
                    XmlSAXAnalising(restrictions);
                else if (LinqRadioButton.Checked)
                    XmlLinqAnalising(restrictions);
                else if (autoRadioButton.Checked)
                    XmlAutoAnalising(restrictions);
            }
            else
                MessageBox.Show("FilePath_ERROR");
        }

        // // // Form's elements // // //
        private void searchButton_Click(object sender, EventArgs e)
        {
            // Updating our grid //
            dataGridView1.Columns.Clear();
            InitializeDataGridView();
            // Cheacking all the restrictions//
            var checkBoxes = checkBoxCheck();
            var restrictions = Restriction(checkBoxes);
            //
            AnaliseFile(restrictions);

        }
        private void transformToHTMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            XslCompiledTransform xslt = new XslCompiledTransform();
            xslt.Load("dataBaseXSL.xsl");
            xslt.Transform("dataBase.xml", "dataBase.html");
        }
        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Творець: Остренко Олександр, група: К-27 \n\n" +
                            "Щоб вивести дані у табличку, достатньо клацнути на кнопку 'Search'\n" +
                            "P.S. у тебе вийде, я в тебе вірю 😊\n" +
                            "Якщо же в тебе глаза розбігаються від кількості інфи на екрані, то\n" +
                            "можна застосувати фільтри (вибираєш у потрібній менюшечці ключове \n" +
                            "слово, за яким хочеш зробити фільтрацію, а потім тицяєш на пустий \n" +
                            "квадратик поруч\n" +
                            "P.S. ти же не хочеш, щоб він так і залишився пустим🥺\n" +
                            "Ну а щодо трьох крапок унизу, то на них можеш не зважати\n" +
                            "Але якщо тобі і вправду цікаво, то я би рекомендував Linq😁"
                            );
        }
    }
}
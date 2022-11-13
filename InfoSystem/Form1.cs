using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Xml.Serialization;
using System.Data.SqlTypes;
using System.Text;
using System.Xml.Xsl;
using InfoSystem.AnalysingMethods;

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
        public Form1()
        {

            InitializeComponent();
            InitializeRadioButton();
            XmlAutoAnalysing analysing = new XmlAutoAnalysing();
            InitializeComboBox(analysing.XmlAutoReading(filePath));
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
        private void AnaliseFile(string[] restrictions)
        {
            if (File.Exists(filePath))
            {
                IAnalysing Analysing; // Задаю свій об'єкт Analysing

                // Визначаю об'єкт Analysing
                if (DomRadioButton.Checked)
                    Analysing = new XmlDOMAnalysing();
                else if (SaxRadioButton.Checked)
                    Analysing = new XmlSAXAnalysing();
                else if (LinqRadioButton.Checked)
                    Analysing = new XmlLinqAnalysing();
                else //if (autoRadioButton.Checked)
                    Analysing = new XmlAutoAnalysing();
                
                //Викликаю відповідний метод
                Analysing.AnalisingMethod(restrictions, filePath, dataGridView1);
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
            if (File.Exists(filePath))
            {
                xslt.Load("dataBaseXSL.xsl");
                xslt.Transform("dataBase.xml", "dataBase.html");
            }
            else
                MessageBox.Show("FilePath_ERROR");
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
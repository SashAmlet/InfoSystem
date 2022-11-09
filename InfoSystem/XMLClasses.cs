using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoSystem
{
    internal class XMLClasses
    {
    }


    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class dataBase
    {
        private int i = 0;
        private dataBaseSpecialty[] specialtyField;// = new dataBaseSpecialty[100];

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("specialty")]
        public dataBaseSpecialty[] specialty
        {
            get
            {
                return this.specialtyField;
            }
            set
            {
                this.specialtyField = value;
            }
        }
        public void addMem()
        {
            dataBaseSpecialty[] sF = new dataBaseSpecialty[++i];
            if (specialtyField != null)
            {
                for (int j = 0; j < specialtyField.Length; j++)
                    sF[j] = specialtyField[j];
            }
            specialtyField = sF;
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class dataBaseSpecialty
    {
        int i = 0;

        private dataBaseSpecialtyGroup[] groupField;// = new dataBaseSpecialtyGroup[100];

        private string sPECIALTYField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("group")]
        public dataBaseSpecialtyGroup[] group
        {
            get
            {
                return this.groupField;
            }
            set
            {
                this.groupField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string SPECIALTY
        {
            get
            {
                return this.sPECIALTYField;
            }
            set
            {
                this.sPECIALTYField = value;
            }
        }

        public void addMem()
        {
            dataBaseSpecialtyGroup[] gF = new dataBaseSpecialtyGroup[++i];
            if (groupField != null)
            {
                for (int j = 0; j < groupField.Length; j++)
                    gF[j] = groupField[j];
            }
            groupField = gF;
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class dataBaseSpecialtyGroup
    {
        int i = 0;
        private dataBaseSpecialtyGroupStudent[] studentField;// = new dataBaseSpecialtyGroupStudent[100];

        private string gROUPField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("student")]
        public dataBaseSpecialtyGroupStudent[] student
        {
            get
            {
                return this.studentField;
            }
            set
            {
                this.studentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string GROUP
        {
            get
            {
                return this.gROUPField;
            }
            set
            {
                this.gROUPField = value;
            }
        }
        public void addMem()
        {
            dataBaseSpecialtyGroupStudent[] sF = new dataBaseSpecialtyGroupStudent[++i];
            if (studentField != null)
            {
                for (int j = 0; j < studentField.Length; j++)
                    sF[j] = studentField[j];
            }
            studentField = sF;
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class dataBaseSpecialtyGroupStudent
    {

        private string nameField;

        private string surnameField;

        private ulong phoneField;

        private string registrationField;

        private byte iDENTField;

        /// <remarks/>
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public string surname
        {
            get
            {
                return this.surnameField;
            }
            set
            {
                this.surnameField = value;
            }
        }

        /// <remarks/>
        public ulong phone
        {
            get
            {
                return this.phoneField;
            }
            set
            {
                this.phoneField = value;
            }
        }

        /// <remarks/>
        public string registration
        {
            get
            {
                return this.registrationField;
            }
            set
            {
                this.registrationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte IDENT
        {
            get
            {
                return this.iDENTField;
            }
            set
            {
                this.iDENTField = value;
            }
        }
    }



}

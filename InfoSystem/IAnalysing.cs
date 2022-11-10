using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoSystem
{
    interface IAnalysing
    {
        abstract void AnalisingMethod(string[] restrictions, string filePath, DataGridView dataGridView1);
    }
}

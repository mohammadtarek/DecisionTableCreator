using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QAT_Project
{
    public partial class Form2 : Form
    {
        Dictionary<String, List<String>> Conditions;
        Dictionary<String, List<String>> Actions;
        Dictionary<String, String> CondAct = new Dictionary<string, string>();
        String Cond = "";
        String Act = "";
        int tableCount;



        public Form2()
        {
            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form3 new_form = new Form3();
            CondAct.Add("TypeCash*Pe,AmountCheck*<=75", "Cash*X");
            CondAct.Add("TypeCash*Pa,Company*Y", "Cash*X");
            CondAct.Add("TypeCash*Pe,AmountCheck*>75", "Don'tCash*X");
            CondAct.Add("TypeCash*Pa,Company*N", "Don'tCash*X");


            /*
            CondAct.Add("TypeCash*Pe,AmountCheck*<=75,Company*N", "Cash*X");
            CondAct.Add("TypeCash*Pe,AmountCheck*>75,Company*Y", "Don'tCash*X");
            CondAct.Add("TypeCash*Pe,AmountCheck*>75,Company*N", "Don'tCash*X");
            */

            new_form.CopyData(CondAct, Conditions, Actions, tableCount);
            this.Hide();
            new_form.ShowDialog();
            this.Close();
        }

        public void CopyData(Dictionary<String, List<String>> Conditions, Dictionary<String, List<String>> Actions, int tableCount)
        {
            this.Conditions = Conditions;
            this.Actions = Actions;
            this.tableCount = tableCount;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            for(int i = 0; i < Conditions.Count; i++)
            {
                comboBox1.Items.Add(Conditions.ElementAt(i).Key);
            }
            for (int i = 0; i < Actions.Count; i++)
            {
                comboBox3.Items.Add(Actions.ElementAt(i).Key);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            List<String> condVal = new List<string>();
            condVal = Conditions[comboBox1.Text];
            //comboBox4.Text = condVal.Count.ToString();
            for (int i = 0; i < condVal.Count; i++)
            {
                comboBox2.Items.Add(condVal.ElementAt(i));
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox4.Items.Clear();
            List<String> actVal = new List<string>();
            actVal = Actions[comboBox3.Text];
            for (int i = 0; i < actVal.Count; i++)
            {
                comboBox4.Items.Add(actVal.ElementAt(i));
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Cond.Equals(""))
            {
                Cond += comboBox1.Text + "*" + comboBox2.Text;
            }
            else
            {
               Cond += "," + comboBox1.Text + "*" + comboBox2.Text;
            }
            comboBox1.Text = "";
            comboBox2.Text = "";
            comboBox2.Items.Clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (Act.Equals(""))
            {
                Act += comboBox3.Text + "*" + comboBox4.Text;
            }
            else
            {
                Act += "," + comboBox3.Text + "*" + comboBox4.Text;
            }
            comboBox3.Text = "";
            comboBox4.Text = "";
            comboBox4.Items.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CondAct.Add(Cond, Act);
            Cond = "";
            Act = "";
        }
    }

   
}

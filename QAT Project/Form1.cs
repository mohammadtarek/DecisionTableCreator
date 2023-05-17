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
    public partial class Form1 : Form
    {
        Dictionary<String, List<String>> Conditions = new Dictionary<string, List<string>>();
        Dictionary<String, List<String>> Actions = new Dictionary<string, List<string>>();
        List<String> CondValues = new List<string>();
        List<String> ActValues = new List<string>();
        int valCount = 0;
        int total = 1;

        public Form1()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form2 new_form = new Form2();
            
            ////////////Helper////////////
            Conditions.Add("TypeCash", new List<string> { "Pe","Pa"});
            Conditions.Add("AmountCheck", new List<string> { "<=75", ">75" });
            Conditions.Add("Company", new List<string> { "Y", "N" });
            

            Actions.Add("Cash", new List<string> { "X" });
            Actions.Add("Don'tCash", new List<string> { "X" });
            total = 8;
            //////////////////////////////
            new_form.CopyData(Conditions, Actions, total);
            this.Hide();
            new_form.ShowDialog();
            this.Close();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            CondValues.Add(textBox2.Text);
            comboBox1.Items.Add(textBox2.Text);
            textBox2.Text = "";
            valCount++;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ActValues.Add(textBox4.Text);
            comboBox2.Items.Add(textBox4.Text);
            textBox4.Text = "";
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            total = total * valCount;
            valCount = 0;
            Conditions[textBox1.Text] = CondValues;
            CondValues = new List<string>();
            comboBox1.Items.Clear();
            textBox2.Text = "";
            textBox1.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Actions.Add(textBox3.Text, ActValues);
            ActValues = new List<string>();
            comboBox2.Items.Clear();
            textBox4.Text = "";
            textBox3.Clear();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

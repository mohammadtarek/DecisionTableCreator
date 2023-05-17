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
    public partial class Form3 : Form
    {
        Dictionary<String, List<String>> Conditions;
        Dictionary<String, List<String>> Actions;
        Dictionary<String,String> Conditions_Actions;
        Dictionary<String, int> ConditionRF;
        int tableCount;
        public Form3()
        {
            InitializeComponent();
        }

        public void CopyData(Dictionary<string,string> Conditions_Actions, Dictionary<String, List<String>> Conditions, Dictionary<String, List<String>> Actions, int tableCount)
        {
            this.Conditions_Actions = Conditions_Actions;
            this.Conditions = Conditions;
            this.Actions = Actions;
            this.tableCount = tableCount;
            this.label2.Text = this.label2.Text + tableCount.ToString();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            TableFill(this.tableCount);
            Reduce_Table(this.tableCount);
        }

        void TableFill(int tableCount)
        {
            DataTable dt = new DataTable();
            ConditionRF = new Dictionary<string, int>();
            int repeatingFactor = tableCount;
            dt.Columns.Add("Conditions", typeof(String));
            for(int i = 1; i < tableCount + 1; i++)
            {
                dt.Columns.Add(i.ToString(), typeof(String));
            }

            for (int i = 0; i < Conditions.Count; i++)
            {
                dt.Rows.Add(Conditions.ElementAt(i).Key);
            }

            dt.Rows.Add("");

            for (int i = 0; i < Actions.Count; i++)
            {
                dt.Rows.Add(Actions.ElementAt(i).Key);
            }

            dataGridView1.DataSource = dt;

            for (int i = 0; i < Conditions.Count; i++)
            {
                repeatingFactor = repeatingFactor / Conditions.ElementAt(i).Value.Count;
                ConditionRF.Add(Conditions.ElementAt(i).Key, repeatingFactor);
                if(repeatingFactor < 1)
                {
                    repeatingFactor = 1;
                }
                List<String> l = Conditions.ElementAt(i).Value;
                int j = repeatingFactor;
                int val = 0;
                for(int num = 1 ; num < tableCount + 1; num++)
                {
                    if (j == 0)
                    {
                        j = repeatingFactor;
                        val++;
                        if(val == l.Count)
                        {
                            val = 0;
                        }
                    }
                    dataGridView1.Rows[i].Cells[num].Value = l.ElementAt(val);
                    j--;
                }
            }

           for( int i = 1; i < tableCount+1; i++)
           {
                for (int j = 0; j < Conditions_Actions.Count; j++)
                {
                    String s1 = Conditions_Actions.ElementAt(j).Key;
                    String s2 = Conditions_Actions.ElementAt(j).Value;

                    ///Name*Value,Name*Value
                    String[] ThisConditions = s1.Split(',');
                    String[] ThisActions = s2.Split(',');
                    
                    /// Name*Value
                    Dictionary<String, String> CondVal = new Dictionary<string, string>();
                    for(int z = 0; z < ThisConditions.Length; z++)
                    {
                        String[] MyS = ThisConditions[z].Split('*');
                        CondVal.Add(MyS[0], MyS[1]);
                    }

                    Dictionary<String, String> ActVal = new Dictionary<string, string>();
                    for (int z = 0; z < ThisActions.Length; z++)
                    {
                        String[] MyS = ThisActions[z].Split('*');
                        ActVal.Add(MyS[0], MyS[1]);
                    }


                    int count = 0;
                    for (int k = 0; k < CondVal.Count; k++)
                    {
                        for(int a = 0; a < Conditions.Count; a++)
                        {
                            if(dataGridView1.Rows[a].Cells[0].Value.ToString().Equals(CondVal.ElementAt(k).Key))
                            {
                                if(dataGridView1.Rows[a].Cells[i].Value.ToString().Equals(CondVal.ElementAt(k).Value))
                                {
                                    count++;
                                }
                            }
                        }
                    }
                    if(count == CondVal.Count)
                    {
                        for(int x = Conditions.Count + 1; x < Conditions.Count + 1 + Actions.Count; x++)
                        {
                            for(int b = 0; b < ActVal.Count; b++)
                            {
                                if(dataGridView1.Rows[x].Cells[0].Value.ToString().Equals(ActVal.ElementAt(b).Key))
                                {
                                    dataGridView1.Rows[x].Cells[i].Value = ActVal.ElementAt(b).Value;
                                }
                            }
                        }
                    }
                }
           }


        }

        void Reduce_Table(int tableCount)
        {
           
            Dictionary<int, String> TableActions = new Dictionary<int, string>();
            List<int> ColsAdded = new List<int>();
            Dictionary<List<int>, List<int>> ColsToRed = new Dictionary<List<int>, List<int>>();
            
            for (int j = 1; j < tableCount+1; j++)
            {
                String A = "";
                for (int i = Conditions.Count + 1; i < Conditions.Count + 1 + Actions.Count; i++)
                {
                    A += dataGridView1.Rows[i].Cells[j].Value.ToString() + ",";
                }

                TableActions.Add(j,A);
                A = "";

                
            }

            List<int> MyCom = new List<int>();
            for (int i = 0; i < Conditions.Count; i++)
            {
                MyCom.Add(i);
            }
            List<List<int>> combos = GetAllCombos(MyCom);


            List<List<int>> LoopCombos = new List<List<int>>();
            for(int CombCount = 1; CombCount <= Conditions.Count; CombCount++)
            {
                for(int i = 0; i < combos.Count; i++)
                {
                    if(combos[i].Count == CombCount)
                    {
                        LoopCombos.Add(combos[i]);
                    }
                }

                //Try_Each_Combo of # elements loop
                for(int i = 0; i < LoopCombos.Count; i++)
                {
                    Dictionary<int, String> TableConditions = new Dictionary<int, string>();

                    //for each column get the combo value
                    for (int j = 1; j < tableCount+1; j++)
                    {
                        String MyS = "";
                        for (int cnum = 0; cnum < LoopCombos[i].Count; cnum++)
                        {
                            //add cond. values to string
                            MyS += dataGridView1.Rows[LoopCombos[i][cnum]].Cells[j].Value;
                        }
                        TableConditions.Add(j, MyS);
                    }

                    //A dic. to save cols which have the same cond.
                    //The loop divides the cols into groups
                    Dictionary<String, List<int>> IDCols = new Dictionary<string, List<int>>();
                    for (int j = 1; j < tableCount+1; j++)
                    {
                        if(!IDCols.ContainsKey(TableConditions[j]))
                        {
                            IDCols.Add(TableConditions[j], new List<int> { j });
                        }
                        else
                        {
                            IDCols[TableConditions[j]].Add(j);
                        }
                    }

                    //Loop to check on each group
                    for(int j = 0; j <IDCols.Count; j++)
                    {
                        //loop to check whithin each group
                        String Act = TableActions[IDCols.ElementAt(j).Value[0]];
                        int count = 0;
                        for (int k = 0; k < IDCols.ElementAt(j).Value.Count; k++)
                        {
                            if(TableActions[IDCols.ElementAt(j).Value[k]].Equals(Act))
                            {
                                count++;
                            }
                        }
                        if(count == IDCols.ElementAt(j).Value.Count)
                        {
                            //Check if added before
                            List<int> temp = new List<int>();
                            for (int k = 0; k < count; k++)
                            {
                                if(!ColsAdded.Contains(IDCols.ElementAt(j).Value[k]))
                                {
                                    temp.Add(IDCols.ElementAt(j).Value[k]);
                                    ColsAdded.Add(IDCols.ElementAt(j).Value[k]);
                                }
                            }
                            //Add to pending reduction
                            ColsToRed.Add(temp, LoopCombos[i]);
                        }
                        
                    }

                }
            }
            //MessageBox.Show(ColsToRed.Count.ToString());

            for(int i = 0; i < ColsToRed.Count; i++)
            {
                //Loopto set neglected fields to "    -"
                for(int j = 0;  j < ColsToRed.ElementAt(i).Value.Count; j++)
                {
                    if (ColsToRed.ElementAt(i).Key.Count > 1)
                    {
                        for (int k = 0; k < Conditions.Count; k++)
                        {
                            if (!ColsToRed.ElementAt(i).Value.Contains(k))
                            {
                                dataGridView1.Rows[k].Cells[ColsToRed.ElementAt(i).Key[0]].Value = "      -";
                            }
                        }
                    }
                }
                if (ColsToRed.ElementAt(i).Key.Count> 1)
                {
                    for (int j = 1; j < ColsToRed.ElementAt(i).Key.Count; j++)
                    {
                        dataGridView1.Columns[ColsToRed.ElementAt(i).Key[j]].Visible = false;
                    }
                }
            }

            int c = 0;
            for(int i = 1; i < tableCount+1; i++)
            {
                if(dataGridView1.Columns[i].Visible)
                {
                    c++;
                }
            }

            label3.Text += c.ToString();
        }

        public static List<List<T>> GetAllCombos<T>(List<T> list)
        {
            int comboCount = (int)Math.Pow(2, list.Count) - 1;
            List<List<T>> result = new List<List<T>>();
            for (int i = 1; i < comboCount + 1; i++)
            {
                // make each combo here
                result.Add(new List<T>());
                for (int j = 0; j < list.Count; j++)
                {
                    // i / 2^j
                    if ((i >> j) % 2 != 0)
                        result.Last().Add(list[j]);
                }
            }
            return result;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}


/// compare every possible compination and see if the action value is the same at all
/// columns that contain this specific compination , then it's reduced to 1 col.
/*
 * #    1     2      3      4
 * C1   Y     Y      N      N
 * C2   M     F      M      F
 * 
 * Dic ---> (Y,[1,2]), (N,[3,4])
 */




using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NLua;
using ReferenceFunctions;
using System.Collections;

namespace LuaJITNet
{
    public partial class Form1 : Form
    {
        private Lua globalState;

        public Form1()
        {
            InitializeComponent();
            this.globalState = new Lua();
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            try
            {
                int iterations = (int)udInput.Value;

                Lua state = new Lua();

                state["Funcs"] = new ReferenceFunctions.References(state);
                //state.LoadCLRPackage();
                //state.DoString(@" import ('ReferenceFunctions.dll', 'Funcs') 
                //    import ('System.Web') ");
                this.txtOutput.Text = "";

                DateTime startTime = DateTime.Now;

                for(int i=0; i < iterations; i++)
                {
                    var res = state.DoString(luaInput.Text)[0];

                    if(i == 0)
                    {
                        if (res.ToString() == "table")
                        {
                            this.txtOutput.Text += TableToString((LuaTable)res);
                        }
                        else
                        {
                            this.txtOutput.Text += res.ToString();
                        }
                    }
                }
                
                double diff = (int)(DateTime.Now - startTime).TotalMilliseconds;
                this.txtOutput.Text += "\nCompleted in: " + diff + " ms";
                this.txtOutput.Text += "\n(~" + Math.Round(diff / iterations, 3) + " ms per iteration)";
            }
            catch(Exception ex)
            {
                this.txtOutput.Text = ex.ToString();
            }
        }

        private string TableToString(LuaTable table)
        {
            StringBuilder sb = new StringBuilder();

            List<object> keys = table.Keys.Cast<object>().ToList();
            List<object> value = table.Values.Cast<object>().ToList();

            sb.Append("{");
            for (int j = 0; j < keys.Count; j++)
            {
                if (j > 0) sb.Append(", ");
                if(value[j].ToString() == "table") sb.Append(keys[j] + " = " + TableToString((LuaTable)value[j]));
                else sb.Append(keys[j] + " = " + value[j]);
            }

            sb.Append("}");

            return sb.ToString();
        }
    }
}

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
using KeraLua;
using System.Threading;

namespace LuaJITNet
{
    public partial class Form1 : Form
    {
        public Boolean running = false;
        AutoResetEvent _continuing = new AutoResetEvent(false);

        public Form1()
        {
            InitializeComponent();
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            try
            {
                int iterations = (int)udInput.Value;

                NLua.Lua state = new NLua.Lua();

                state["Funcs"] = new ReferenceFunctions.References(state);
                state.DebugHook += this.luaBreakHook;
                state.SetDebugHook(NLua.Event.EventMasks.LUA_MASKLINE, 0);

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

        public void luaBreakHook(object sender, NLua.Event.DebugHookEventArgs e)
        {
            _continuing.Reset();
            this.btnCont.Enabled = true;

            NLua.Lua state = (NLua.Lua)sender;
            LuaDebug debugger = e.LuaDebug;
            
            return;
        }

        public void displayBreakPoint(string data)
        {

        }

        private void btnCont_Click(object sender, EventArgs e)
        {
            this.btnCont.Enabled = false;
            _continuing.Set();
        }
    }
}

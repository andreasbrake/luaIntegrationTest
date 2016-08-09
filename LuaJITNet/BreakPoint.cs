using NLua;
using KeraLua;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaJITNet
{
    class BreakPoint
    {
        private NLua.Lua instance { get; set; }
        private Form1 form { get; set; }
        public BreakPoint(NLua.Lua instance, Form1 form)
        {
            this.instance = instance;
            this.form = form;
        }

        public void Break()
        {
            LuaDebug debugger = new LuaDebug();

            instance.GetInfo("input", ref debugger);
            form.displayBreakPoint("break");
        }
    }
}

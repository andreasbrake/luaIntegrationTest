using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLua;

namespace ReferenceFunctions
{
    public class References
    {
        private Lua _state;

        public References(Lua state)
        {
            this._state = state;
        }

        public int getValue()
        {
            return 100;
        }

        public Modifer getModifier(double input)
        {
            return new Modifer()
            {
                Response = "data from input",
                Data = input * (new Random()).NextDouble()
            };
        }

        public LuaTable getModifier_Lua(double input)
        {
            LuaTable table = CreateTable();
            Modifer mod = getModifier(input);
            table["Response"] = mod.Response;
            table["Data"] = mod.Data;
            return table;
        }

        public TestObject getObject()
        {
            return new TestObject()
            {
                Value1 = "a",
                Value2 = 1,
                Data = 491.1256
            };
        }
        
        public TestData getData()
        {
            return new TestData()
            {
                Headers = new string[] { "Field_1", "Field_2", "Field_3", "Field_4", "Field_5", "Field_6" },
                Data = new object[] {
                    new object[] { 1, "b", 3, "D", 7, 4 },
                    new object[] { 2, "k", 8, "G", 5, 3 },
                    new object[] { 7, "h", 3, "X", 2, 2 },
                    new object[] { 4, "b", 4, "N", 3, 5 },
                    new object[] { 2, "w", 3, "R", 7, 6 },
                    new object[] { 4, "k", 1, "T", 7, 4 },
                }
            };
        }

        public LuaTable getData_Lua()
        {
            TestData data = getData();
            LuaTable table = CreateTable();

            table["Headers"] = ArrayToTable(data.Headers);
            table["Data"] = ArrayToTable(data.Data);

            return table;
        }

        public Dictionary<string, object> getDbRow()
        {
            return new Dictionary<string, object>()
            {
                { "Field_1", 1},
                { "Field_2", "b"},
                { "Field_3", 3},
                { "Field_4", "D"},
                { "Field_5", 7},
                { "Field_6", 4}
            };
        }
        public LuaTable getDbRow_Lua()
        {
            return DictToTable(this.getDbRow());
        }

        public LuaTable DictToTable(Dictionary<string, object> input)
        {
            LuaTable table = CreateTable();

            foreach(KeyValuePair<string, object> kv in input)
            {
                table[kv.Key] = kv.Value;
            }

            return table;
        }

        public LuaTable ArrayToTable(object[] input)
        {
            LuaTable table = CreateTable();
            for (int i=0; i < input.Length; i++)
            {
                if(input[i].GetType().BaseType.Name == "Array")
                {
                    table[i + 1] = ArrayToTable((object[])input[i]);
                }
                else
                {
                    table[i + 1] = input[i];
                }
            }
            return table;
        }

        public LuaTable CreateTable()
        {
            return (LuaTable)_state.DoString("return {}")[0];
        }
    }
}

public class Modifer
{
    public string Response { get; set; }
    public double Data { get; set; }
}

public class TestObject
{
    public object Value1 { get; set; }
    public object Value2 { get; set; }
    public double Data { get; set; }
}

public class TestData
{
    public string[] Headers { get; set; }
    public object[] Data{ get; set; }
    public int Length { get { return this.Headers.Length; } }
}
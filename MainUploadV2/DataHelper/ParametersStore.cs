using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainUploadV2.DataHelper
{
    public class ParametersStore
    {
        public string Text { get; set; }
        public object Value { get; set; }

        public ParametersStore() { }
        public ParametersStore(string text, string value)
        {
            this.Text = text;
            this.Value = Value;
        }
    }
    public enum ExecuteType
    {
        ExecuteReader,
        ExecuteNonQuery,
        ExecuteScalar
    };

    public class DbParameter
    {
       
        public string Name { get; set; }
        public ParameterDirection Direction { get; set; }
        public object Value { get; set; }

        public DbParameter(string paramName, ParameterDirection paramDirection, object paramValue)
        {
            Name = paramName;
            Direction = paramDirection;
            Value = paramValue;
        }

        public DbParameter()
        {
        }
    }
}

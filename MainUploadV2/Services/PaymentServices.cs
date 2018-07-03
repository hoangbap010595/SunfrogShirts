using MainUploadV2.DataHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MainUploadV2.Services
{
    public class PaymentServices
    {
        private ConnectionStoreDB con = new ConnectionStoreDB();


        public DataTable GetListPayMent()
        {
            var lsData = con.GetExecuteProcedure("spGetPaymenTool", null);
            return lsData;
        }


        public string PaymenTool(string UserID, int ToolID, string DateExpires, string Note)
        {
            List<ParametersStore> param = new List<ParametersStore>();
            param.Add(new ParametersStore() { Text = "UserID", Value = UserID });
            param.Add(new ParametersStore() { Text = "ToolID", Value = ToolID });
            param.Add(new ParametersStore() { Text = "DateExpires", Value = DateExpires });
            param.Add(new ParametersStore() { Text = "Note", Value = Note });
            var data = con.ConvertDataTableToList("sp_PayMentToolForUser", param);
            string rs = data[0]["Status"].ToString();
            return rs;
        }
    }
}

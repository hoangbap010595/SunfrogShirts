using MainUploadV2.DataHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainUploadV2.Services
{
    public class AccountServices
    {
        private ConnectionStoreDB con = new ConnectionStoreDB();

        public int CheckLogin(string username, string password)
        {
            List<ParametersStore> param = new List<ParametersStore>();
            param.Add(new ParametersStore() { Text = "Username", Value = username });
            param.Add(new ParametersStore() { Text = "Password", Value = password });
            var data = con.ConvertDataTableToList("spCheckLogin", param);
            int rs = int.Parse(data[0]["result"].ToString());
            return rs;
        }
    }
}

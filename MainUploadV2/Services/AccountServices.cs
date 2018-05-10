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
    public class AccountServices
    {
        private ConnectionStoreDB con = new ConnectionStoreDB();

        public string CheckLogin(string username, string password)
        {
            List<ParametersStore> param = new List<ParametersStore>();
            param.Add(new ParametersStore() { Text = "Username", Value = username });
            param.Add(new ParametersStore() { Text = "Password", Value = GetMd5Hash(password) });
            var data = con.ConvertDataTableToList("spCheckLogin", param);
            string rs = data[0]["UserID"].ToString();
            return rs;
        }
        public Dictionary<string, object> GetInfoMember(string userID)
        {
            List<Dictionary<string, object>> lsData = new List<Dictionary<string, object>>();
            List<ParametersStore> param = new List<ParametersStore>();
            param.Add(new ParametersStore() { Text = "UserID", Value = userID });
            lsData = con.ConvertDataTableToList("getMembersByID", param);

            return lsData[0];
        }

        public List<Dictionary<string, object>> GetToolForUser(string userID)
        {
            List<Dictionary<string, object>> lsData = new List<Dictionary<string, object>>();
            List<ParametersStore> param = new List<ParametersStore>();
            param.Add(new ParametersStore() { Text = "UserID", Value = userID });
            lsData = con.ConvertDataTableToList("Members_GetTools", param);
            return lsData;
        }
        public DataTable GetListMember()
        {
            var lsData = con.GetExecuteProcedure("getMemberAccount", null);
            return lsData;
        }
        public int InsertNewAccount(string username, string password, string fullName, string dateExpires, string acctype)
        {
            List<ParametersStore> param = new List<ParametersStore>();
            param.Add(new ParametersStore() { Text = "ID", Value = GetMd5Hash(username) });
            param.Add(new ParametersStore() { Text = "Username", Value = username });
            param.Add(new ParametersStore() { Text = "Password", Value = GetMd5Hash(password) });
            param.Add(new ParametersStore() { Text = "FullName", Value = fullName });
            param.Add(new ParametersStore() { Text = "DateExpires", Value = dateExpires });
            param.Add(new ParametersStore() { Text = "AccType", Value = acctype });
            var data = con.ConvertDataTableToList("spInsertNewAccount", param);
            int rs = int.Parse(data[0]["Status"].ToString());
            return rs;
        }
        public static string GetMd5Hash(string value)
        {
            var md5Hasher = MD5.Create();
            var data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(value));
            var sBuilder = new StringBuilder();
            for (var i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
    }
}

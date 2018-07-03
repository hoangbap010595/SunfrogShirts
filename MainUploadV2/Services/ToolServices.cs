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
    public class ToolServices
    {
        private ConnectionStoreDB con = new ConnectionStoreDB();


        public DataTable GetListTool()
        {
            var lsData = con.GetExecuteProcedure("spGetListTool", null);
            return lsData;
        }

        public DataTable GetListDataTool()
        {
            var lsData = con.GetExecuteProcedure("getDataTool", null);
            return lsData;
        }
       
    }
}

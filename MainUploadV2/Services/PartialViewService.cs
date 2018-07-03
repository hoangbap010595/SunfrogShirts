using MainUploadV2.DataHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainUploadV2.Services
{
    public class PartialViewService
    {
        private ConnectionStoreDB con = new ConnectionStoreDB();
        public DataTable GetListMember()
        {
            var lsData = con.GetExecuteProcedure("getDataTool", null);
            return lsData;
        }

        public static string Username
        {
            get { return MainUploadV2.Properties.Settings.Default.Username.ToString(); }
            set
            {
                MainUploadV2.Properties.Settings.Default.Username = value;
                MainUploadV2.Properties.Settings.Default.Save();
            }
        }

        public static string Password
        {
            get { return MainUploadV2.Properties.Settings.Default.Password.ToString(); }
            set
            {
                MainUploadV2.Properties.Settings.Default.Password = value;
                MainUploadV2.Properties.Settings.Default.Save();
            }
        }

        public static bool Remember
        {
            get { return MainUploadV2.Properties.Settings.Default.Remember; }
            set
            {
                MainUploadV2.Properties.Settings.Default.Remember = value;
                MainUploadV2.Properties.Settings.Default.Save();
            }
        }
    }
}

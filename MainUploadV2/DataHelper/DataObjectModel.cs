using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainUploadV2.DataHelper
{
    public static class DataObjectModel
    {
        public static List<AccType> getListAccType()
        {
            List<AccType> ls = new List<AccType>();
            ls.Add(new AccType() { Id = 1, Name = "Dùng thử" });
            ls.Add(new AccType() { Id = 2, Name = "Không giới hạn" });
            return ls;
        }
    }


    public class AccType
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}

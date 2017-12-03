﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCProShirts
{
    public class ApplicationUser
    {
        public string UserID { get; set; }
        public string Email { get; set; }
        public string Code { get; set; }
        public string ApiKey { get; set; }
        public string ViewOnlyApiKey { get; set; }
        public string GroupID { get; set; }
        public string EntityID { get; set; }
        public string PayableId { get; set; }
    }
}

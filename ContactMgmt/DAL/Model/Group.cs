using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContactMgmtApp.DAL.interfaces;

namespace ContactMgmtApp.DAL.Model
{
    public class Group : IDalEntity
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
    }
}

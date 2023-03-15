using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordResumeParser
{
    public class Resume
    {
        public string FirstName { get; set; } = String.Empty;
        public string LastName { get; set; } = String.Empty;
        public string LinkedIn { get; set; } = String.Empty;

        public string Git { get; set; } = String.Empty;
        public string Phone { get; set; } = String.Empty;
        public string Website { get; set; } = String.Empty;
        public string Summary { get; set; } = String.Empty;
        public List<Education> Academics { get; set; } = new List<Education>();

        public List<Project> Career { get; set; } = new List<Project>();        
    }
}

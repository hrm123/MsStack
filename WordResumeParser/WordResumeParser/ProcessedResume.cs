using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordResumeParser
{
    public class ItemProperty
    {
        public string Style { get; set; }
        public string Lang { get; set; }
    }

    internal class ProcessedResume
    {
        public string Text { get; set; }

        private ItemProperty _itemProperty = new ItemProperty();

        public ItemProperty ItemProperty { get; set; }
    }
}

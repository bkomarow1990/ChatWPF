using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatLibrary
{
    [Serializable]
    public class MessageInfo
    {
        public string Message { get; set; }
        public string UserName { get; set; }
        public DateTime Time { get; set; }
    }
}

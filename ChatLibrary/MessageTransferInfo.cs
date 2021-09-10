using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Media;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ChatLibrary
{
    [Serializable]
    public enum Actions { JOIN = 0, LEAVE, SEND};
    [Serializable]
    public class MessageTransferInfo
    {
        public string Username { get; set; }
        public string Message { get; set; }
        public Actions Action { get; set; }
        //public Brush Color { get; set; }
    }
}

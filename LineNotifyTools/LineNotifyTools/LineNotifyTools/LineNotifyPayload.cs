using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace LineNotifyTools
{
    public class LineNotifyPayload
    {
        public string Message { get; set; }
        public Image ImageFile { get; set; }

        public LineNotifyPayload(string message = null)
        {
            Message = message;
        }
    }
}

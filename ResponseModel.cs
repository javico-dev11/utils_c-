using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace logic_hawks.logic
{
    public class ResponseModel
    {
        public dynamic result { get; set; }
        public bool response { get; set; }
        public string message { get; set; }
        public string title { get; set; }
        public string href { get; set; }
        public string function { get; set; }

        public ResponseModel()
        {
            response = false;
        }

        public void SetResponse(bool r, string m = "", dynamic _result = null)
        {
            response = r;
            message = m;
            result = _result;

            
        }
    }
}

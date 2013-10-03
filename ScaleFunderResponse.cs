using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace ScaleFunder
{
    public class ScaleFunderResponse
    {
        private String _response_code;
        public String ResponseCode
        {
            get { return _response_code; }
            set { _response_code = value; }
        }
        private String _msg;
        public String Msg
        {
            get { return _msg; }
            set { _msg = value; }
        }
    }
}

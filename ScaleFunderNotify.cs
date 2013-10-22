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
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Security.Cryptography.X509Certificates;

namespace ScaleFunder
{
    public class ScaleFunderNotify
    {
        public ScaleFunderNotify()
        {
            _params = new NameValueCollection() ;
        }
        private string _post_url = String.Empty;
        public string PostURL { get { return _post_url; } set { _post_url = value; } }
        private string _app_key = String.Empty;
        public string AppKey { get { return _app_key; } set { _app_key = value; } }
        private string _amount = String.Empty;
        public string Amount { get { return _amount; } set { _amount = value; } }
        private string _donation_id = String.Empty;
        public string DonId { get { return _donation_id; } set { _donation_id = value; } }
        
        private NameValueCollection _params = null;
        public void AddParam(string sKey, string sValue)
        {
            _params.Set(sKey, sValue);
        }
        public ScaleFunderResponse Post() 
        {
            if (this._post_url == String.Empty || this._app_key == String.Empty || this._params.Count == 0 || this._amount == String.Empty || this._donation_id == String.Empty)
            {
                throw new ScaleFunderArgsNotSet("ARGS Not Set: AppKey and PostURL must be set, and AddParams must be called at least once");
            }
            ScaleFunderAuth oScaleFunderAuth = new ScaleFunderAuth();
            //string sDigest = oScaleFunderAuth.CollToDigest(this._params, this._app_key);
            string sDigest = oScaleFunderAuth.ValsToDigest(this.Amount, this.DonId, this.AppKey);
            this._params.Set("sf_sig", sDigest);
            this._params.Set("sf_amount", this.Amount);
            this._params.Set("sf_don_id",this.DonId);
            string sResponseBody;
            //ServicePointManager.CertificatePolicy = new MyCertPolicy();

            try
            {
                using (WebClient client = new WebClient())
                {
                    byte[] responsebytes = client.UploadValues(this.PostURL, "POST", this._params);
                    sResponseBody = Encoding.UTF8.GetString(responsebytes);
                }
            }
            catch (WebException e)
            {
                throw e;
            }
            var jss = new JavaScriptSerializer();
            var dResponse = jss.Deserialize<Dictionary<string, string>>(sResponseBody);
            string sCode;
            string sMsg;
            dResponse.TryGetValue("response_code", out sCode);
            dResponse.TryGetValue("msg", out sMsg);
            var oScaleResponse = new ScaleFunderResponse();
            oScaleResponse.Msg = sMsg;
            oScaleResponse.ResponseCode = sCode;
            return oScaleResponse;

        }

        public string GetFinalPing()
        {
            if (this._post_url == String.Empty || this._app_key == String.Empty || this._params.Count == 0 || this._amount == String.Empty || this._donation_id == String.Empty)
            {
                throw new ScaleFunderArgsNotSet("ARGS Not Set: AppKey and PostURL must be set, and AddParams must be called at least once");
            }
            ScaleFunderAuth oScaleFunderAuth = new ScaleFunderAuth();
            //string sDigest = oScaleFunderAuth.CollToDigest(this._params, this._app_key);
            string sDigest = oScaleFunderAuth.ValsToDigest(this.Amount, this.DonId, this.AppKey);
            var oQueryDict = HttpUtility.ParseQueryString("");
            foreach (string key in this._params)
            {
                oQueryDict.Set(key, this._params[key]);
            }

            oQueryDict.Set("sf_sig", sDigest);


            return this._post_url + "?" + oQueryDict.ToString();
        }
    }
    [Serializable()]    
    public class ScaleFunderArgsNotSet : System.Exception
    {
        public ScaleFunderArgsNotSet() : base() { }
        public ScaleFunderArgsNotSet(string message) : base(message) { }
        public ScaleFunderArgsNotSet(string message, System.Exception inner) : base(message, inner) { }

        // A constructor is needed for serialization when an 
        // exception propagates from a remoting server to the client.  
        protected ScaleFunderArgsNotSet(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) { }
    }
    
    
    /*
    public class MyCertPolicy : System.Net.ICertificatePolicy
    {
        public MyCertPolicy()
        { }

        public bool CheckValidationResult(ServicePoint sp,
        X509Certificate cert, WebRequest req, int problem)
        {

            return true;
        }
    }
    */
      
}


/*


ScaleFunder obj;

obj.PostURL = XXXXX
obj.AppKey = XXXXXXX
obj.AddParam("CF_Donation_ID","")
obj.AddParam("Client_Ref","")
sUrl = obj.GetRequestUrl()

try:
   oResponse = obj.Post()
catch {

}

ConnectionTimeout

NoResponseException

ErrorProcessingTransaction
*/
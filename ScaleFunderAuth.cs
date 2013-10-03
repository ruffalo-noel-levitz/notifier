using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
 
namespace ScaleFunder
{
    public class ScaleFunderAuth
    {
        public bool VerifyRequest(NameValueCollection dFormParams, string sApiKey, string sSig)
        {
            NameValueCollection dParams = new NameValueCollection(dFormParams);
            dParams.Remove("sig");
            string sToValidate = this.CollToStr(dParams);
            string sDigest = this.CalculateDigest(sToValidate, sApiKey);
            Int32 nCompare = System.String.Compare(sDigest, sSig);
            return (nCompare == 0);
        }
        public string CollToDigest(NameValueCollection oColl, string sApiKey)
        {
            string sConcat = this.CollToStr(oColl);
            string sDigest = this.CalculateDigest(sConcat, sApiKey);
            return sDigest.ToLower();
        }
        public string CollToStr(NameValueCollection oColl)
        {
            string[] lKeys;
            lKeys = oColl.AllKeys;
            System.Array.Sort(lKeys);
            string sToValidate = "";
            foreach (string sKey in lKeys)
            {
                sToValidate += sKey;
                sToValidate += oColl[sKey];
            }
            return sToValidate;
        }
 
        public string CalculateDigest (string sToSign, string sApiKey)
        {
            HMACSHA256 hmac;
            Encoding enc = new UTF8Encoding(true,true);
            hmac = new HMACSHA256(enc.GetBytes(sApiKey));
            byte[] sComputedHash = hmac.ComputeHash(enc.GetBytes(sToSign));
            return this.bytes_to_hex(sComputedHash).ToLower();           
        }
 
        public string bytes_to_hex(byte[] bytes)
        {
            string str = BitConverter.ToString(bytes);
            return str.Replace("-","");
        }

	}
        
   }

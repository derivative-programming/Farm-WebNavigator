using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace FS.Common.Authentication
{
    public class AuthenticationToken
    {
        public string UserName = string.Empty;

        public string UserID = string.Empty;

        public string ApiSessionCode = string.Empty;  

        public string ContextCode = string.Empty; //CustomerCode

        public string DriverCode = string.Empty; //DriverCode //ObjDataSetCode

        public string HelperCode = string.Empty; //HelperCode //LaborerCode

        public string CustomerCode = string.Empty; //CustomerCode

        public int CustomerID = 0;

        public string IPAddress = string.Empty;

        public string Domain = string.Empty;

        public DateTime ExpirationUTCDateTime = DateTime.UtcNow;

        public string CustomerRoleListCSV = string.Empty;

        public string ObjDataSetCode = string.Empty;
         
        public string LaborerCode
        {
            get { return this.DriverCode; }
        }


        public AuthenticationToken(string objDataSetCode, string userName, string userID, string contextCode, 
            string HelperCode,   string DriverCode,
              string customerCode, int customerID, 
            string IPAddress, string domain, DateTime expirationUTCDateTime, string customerRoleListCSV)
        {
            this.UserName = userName;
            this.UserID = userID;
            this.ContextCode = contextCode;
            this.DriverCode = DriverCode; 
            this.HelperCode = HelperCode; 
            this.CustomerCode = customerCode;
            this.CustomerID = customerID;
            this.IPAddress = IPAddress;
            this.Domain = domain;
            this.ExpirationUTCDateTime = expirationUTCDateTime;
            this.CustomerRoleListCSV = customerRoleListCSV;
            this.ApiSessionCode = Guid.NewGuid().ToString();
            this.ObjDataSetCode = objDataSetCode;
        }
         
        public AuthenticationToken(string encryptedTokenData)
        {
            FS.Common.Encryption.EncryptionServices encService = new Encryption.EncryptionServices();
            string data = encService.Decrypt(encryptedTokenData);

            List<string> dataItems = new List<string>();
            while(data.Contains("|RM|"))
            {
                dataItems.Add(data.Substring(0, data.IndexOf("|RM|")));
                data = data.Remove(0, data.IndexOf("|RM|") + "|RM|".Length);
            }
            for (int i = 0; i < dataItems.Count; i++)
            {
                if (!dataItems[i].Contains(":"))
                    continue;
                string key = dataItems[i].Split(':')[0];
                string value = dataItems[i].Split(':')[1];
                switch (key)
                {
                    case "UserName":
                        this.UserName = value;
                        break;
                    case "UserID":
                        this.UserID = value;
                        break;
                    case "ContextCode":
                        this.ContextCode = value;
                        break;
                    case "ApiSessionCode":
                        this.ApiSessionCode = value;
                        break;
                    case "DriverCode":
                        this.DriverCode = value;
                        break; 
                    case "HelperCode":
                        this.HelperCode = value;
                        break; 
                    case "CustomerCode":
                        this.CustomerCode = value;
                        break;
                    case "CustomerID":
                        this.CustomerID = int.Parse(value);
                        break;
                    case "IPAddress":
                        this.IPAddress = value;
                        break;
                    case "ObjDataSetCode":
                        this.ObjDataSetCode = value;
                        break;
                    case "ExpirationUTCDateTime":
                        DateTime testDate = DateTime.Now;
                        if (DateTime.TryParse(value, out testDate))
                        {
                            this.ExpirationUTCDateTime = testDate; 
                        }
                        break;
                    case "Domain":
                        this.Domain = value;
                        break;
                    case "CustomerRoleListCSV":
                        this.CustomerRoleListCSV = value;
                        break; 
                    default:
                        break;
                }
            }
        }
         
        public bool IsValid()
        {
            bool result = false;

            if (this.UserID.Length > 0 &&
                this.ContextCode.Length > 0 &&
                this.UserName.Length > 0 &&
                this.IPAddress.Length > 0 &&
                this.Domain.Length > 0 )
                result = true;

            return result;
        }

        public bool IsExpired()
        {
            bool result = false;

            if (this.ExpirationUTCDateTime < DateTime.UtcNow)
                result = true;

            return result;
        }


        public string BuildEncryptedTokenData()
        {
            string result = string.Empty;
            string data = string.Empty;
            data = data + "UserName:" + this.UserName + "|RM|";
            data = data + "UserID:" + this.UserID + "|RM|";
            data = data + "ApiSessionCode:" + this.ApiSessionCode + "|RM|";
            data = data + "ContextCode:" + this.ContextCode + "|RM|";
            data = data + "DriverCode:" + this.DriverCode + "|RM|"; 
            data = data + "HelperCode:" + this.HelperCode + "|RM|"; 
            data = data + "CustomerCode:" + this.CustomerCode + "|RM|";
            data = data + "CustomerID:" + this.CustomerID.ToString() + "|RM|";
            data = data + "IPAddress:" + this.IPAddress + "|RM|";
            data = data + "ExpirationUTCDateTime:" + this.ExpirationUTCDateTime.ToString() + "|RM|";
            data = data + "Domain:" + this.Domain + "|RM|";
            data = data + "CustomerRoleListCSV:" + this.CustomerRoleListCSV + "|RM|";
            data = data + "ObjDataSetCode:" + this.ObjDataSetCode + "|RM|";
            FS.Common.Encryption.EncryptionServices encService = new Encryption.EncryptionServices();
            result = encService.Encrypt(data);
            return result;
        }
    }

}

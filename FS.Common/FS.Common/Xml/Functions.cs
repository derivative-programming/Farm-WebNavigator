using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Net;


namespace HM.Common.Xml
{
    public class Functions
    {


        public static string Transform(string xmlInput, string xslPath, string paramName, string paramValue)
        {

            MemoryStream ms;
            System.Xml.XmlDocument xd = new XmlDocument();
            System.Xml.Xsl.XsltArgumentList xslArgs;
            System.Xml.Xsl.XslTransform xslTransform;
            System.Xml.XPath.XPathDocument xPath;
            StringWriter stringWrite;
            System.Xml.XmlUrlResolver urlResolve;
            System.Xml.XmlDocument doc = new XmlDocument(); 

            try
            {

                xd.LoadXml(xmlInput);
                ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(xd.OuterXml));
                xPath = new System.Xml.XPath.XPathDocument(ms);
                xslTransform = new System.Xml.Xsl.XslTransform();
                xslTransform.Load(xslPath);

                // Load up response for transformation with argument directing the output for the entity
                xslArgs = new System.Xml.Xsl.XsltArgumentList();
                if (paramName.Length > 0)
                {
                    xslArgs.AddParam(paramName, "", paramValue);
                }


                stringWrite = new StringWriter();
                urlResolve = new XmlUrlResolver();

                xslTransform.Transform(xPath, xslArgs, stringWrite, urlResolve);
                return stringWrite.ToString();


            }
            catch (System.Xml.Xsl.XsltException ex)
            {
                throw new System.Exception(ex.Message);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public static string TransformText(string xmlInput, string xslText, string paramName, string paramValue)
        {

            MemoryStream ms;
            System.Xml.XmlDocument xd = new XmlDocument();
            System.Xml.Xsl.XsltArgumentList xslArgs;
            System.Xml.Xsl.XslTransform xslTransform;
            System.Xml.XPath.XPathDocument xPath;
            StringWriter stringWrite;
            System.Xml.XmlUrlResolver urlResolve;
            System.Xml.XmlDocument doc = new XmlDocument();


            try
            {

                xd.LoadXml(xmlInput);
                ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(xd.OuterXml));
                xPath = new System.Xml.XPath.XPathDocument(ms);
                xslTransform = new System.Xml.Xsl.XslTransform();
                string tempfile  = System.IO.Path.GetTempFileName();
                System.IO.File.WriteAllText(tempfile,xslText);
                xslTransform.Load(tempfile);

                // Load up response for transformation with argument directing the output for the entity
                xslArgs = new System.Xml.Xsl.XsltArgumentList();
                if (paramName.Length > 0)
                {
                    xslArgs.AddParam(paramName, "", paramValue);
                }


                stringWrite = new StringWriter();
                urlResolve = new XmlUrlResolver();

                xslTransform.Transform(xPath, xslArgs, stringWrite, urlResolve);
                System.IO.File.Delete(tempfile);
                return stringWrite.ToString();


            }
            catch (System.Xml.Xsl.XsltException ex)
            {
                throw new System.Exception(ex.Message);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public static int CountChildren(XmlNode node)
        {
            int total = 0;

            foreach (XmlNode child in node.ChildNodes)
            {
                total++;
            }
            return total;
        }

        /// <summary>
        /// Makes a WebRequest to the Interspire API by querying the address : 
        /// http://emailmarketing.boostctr.com/xml.php
        /// return an XML output string.
        /// </summary>
        public static string XMLResultFromXmlRequest(string xmlData)
        {
            string result = "";
            WebRequest request = WebRequest.Create("http://emailmarketing.boostctr.com/xml.php");
            // Set the Method property of the request to POST.
            request.Method = "POST";
            //convert the xml data into a byte array
            byte[] byteArray = Encoding.UTF8.GetBytes(xmlData);
            // Set the ContentType property of the WebRequest.
            request.ContentType = "text/xml";
            // Set the ContentLength property of the WebRequest.
            request.ContentLength = byteArray.Length;
            // Get the request stream.
            Stream dataStream = request.GetRequestStream();
            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.
            dataStream.Close();
            // Get the response.
            WebResponse response = request.GetResponse();
            // Get the stream containing content returned by the server.
            dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            result = reader.ReadToEnd();
            return result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Collections;
using System.Xml;

namespace HM.Common.Providers
{

    public class Provider
    {
        string name;
        string providerType;
        NameValueCollection providerAttributes = new NameValueCollection();

        public Provider(XmlAttributeCollection attributes)
        {

            // Set the name of the provider
            //
            name = attributes["name"].Value;

            // Set the type of the provider
            //
            providerType = attributes["type"].Value;

            // Store all the attributes in the attributes bucket
            //
            foreach (XmlAttribute attribute in attributes)
            {

                if ((attribute.Name != "name") && (attribute.Name != "type"))
                    providerAttributes.Add(attribute.Name, attribute.Value);

            }

        }


        /// <summary>
        /// display name
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
        }

        public string Type
        {
            get
            {
                return providerType;
            }
        }

        public NameValueCollection Attributes
        {
            get
            {
                return providerAttributes;
            }
        }

    }
}

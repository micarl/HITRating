using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Xml;
using System.Collections;

namespace HitRating.Utilities
{
    public static class StringUtility
    {
        public static string TrimAll(string original) 
        {
            if (string.IsNullOrEmpty(original))
            {
                return null;    
            }
            
            string ns = "";
            string[] sa = original.Trim().Split(new char[] { ' ' });

            foreach (var s in sa)
            {
                ns += s;
            }

            return ns;
        }

        public static string EnGroup(string original, char groupBy = '#', bool preGroup = true)
        {
            if (string.IsNullOrEmpty(original))
            {
                return null;
            }
            
            original = TrimAll(groupBy + original);

            string[] groups = original.Split(new char[] { groupBy }, StringSplitOptions.RemoveEmptyEntries);

            string s = "";
            if (preGroup)
            {
                foreach (string g in groups)
                {
                    s += groupBy + g;
                }
            }
            else { 
                foreach (string g in groups)
                {
                    s += g + groupBy;
                }
            }

            return s;
        }

        public static string EnGroup(string[] originals, char groupBy = '#', bool preGrouped = true)
        {
            if (originals == null)
            {
                return null;
            }

            string s = "";
            if (preGrouped)
            {
                foreach (string g in originals)
                {
                    s += groupBy + g;
                }
            }
            else
            {
                foreach (string g in originals)
                {
                    s += g + groupBy;
                }
            }

            return s;
        }

        public static string[] DeGroup(string original, char groupBy = '#')
        {
            if (string.IsNullOrEmpty(original))
            {
                return null;
            }

            original = TrimAll(original);

            return original.Split(new char[] { groupBy }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static string EscapeXml(string xml) {
            return xml.Replace("&", "&amp;");
        }
    }

    public class RandString
    {
        private string framerStr = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        private static Random myRandom = new Random();

        /// <summary>
        /// 如未提供参数构造,则默认由数字+小写字母构成
        /// </summary>
        public RandString()
        {
            
        }

        public string Generate(int length = 20)
        {
            // 获取的长度不能为0个或者负数个
            if (length < 1)
            {
                throw new ArgumentException("字符长度不能为0或者负数!");
            }
            else
            {
                // 如果只是获取少量随机字符串,
                // 这样没有问题.
                // 但如果需要短时间获取大量随机字符串的话,
                // 这样可能性能不高.
                // 可以改用StringBuilder类来提高性能,
                // 需要的可以自己改一下 ^o^
                string tempStr = null;
                for (int i = 0; i < length; i++)
                {
                    int randNum = myRandom.Next(framerStr.Length);
                    tempStr += framerStr[randNum];
                }

                return tempStr;
            }
        }
    }

    public static class XmlToJson
    {
        public static object Parse(XmlDocument xmlDoc)
        {
            StringBuilder sbJSON = new StringBuilder();
            sbJSON.Append("{ ");
            XmlToJSONnode(sbJSON, xmlDoc.DocumentElement, true);
            sbJSON.Append("}");
            return (new System.Web.Script.Serialization.JavaScriptSerializer()).DeserializeObject(sbJSON.ToString());
        }

        //  XmlToJSONnode:  Output an XmlElement, possibly as part of a higher array
        private static void XmlToJSONnode(StringBuilder sbJSON, XmlElement node, bool showNodeName)
        {
            if (showNodeName)
                sbJSON.Append("\"" + SafeJSON(node.Name) + "\": ");
            sbJSON.Append("{");
            // Build a sorted list of key-value pairs
            //  where   key is case-sensitive nodeName
            //          value is an ArrayList of string or XmlElement
            //  so that we know whether the nodeName is an array or not.
            SortedList childNodeNames = new SortedList();

            //  Add in all node attributes
            if (node.Attributes != null)
                foreach (XmlAttribute attr in node.Attributes)
                    StoreChildNode(childNodeNames, attr.Name, attr.InnerText);

            //  Add in all nodes
            foreach (XmlNode cnode in node.ChildNodes)
            {
                if (cnode is XmlText)
                    StoreChildNode(childNodeNames, "value", cnode.InnerText);
                else if (cnode is XmlElement)
                    StoreChildNode(childNodeNames, cnode.Name, cnode);
            }

            // Now output all stored info
            foreach (string childname in childNodeNames.Keys)
            {
                ArrayList alChild = (ArrayList)childNodeNames[childname];
                if (alChild.Count == 1)
                    OutputNode(childname, alChild[0], sbJSON, true);
                else
                {
                    sbJSON.Append(" \"" + SafeJSON(childname) + "\": [ ");
                    foreach (object Child in alChild)
                        OutputNode(childname, Child, sbJSON, false);
                    sbJSON.Remove(sbJSON.Length - 2, 2);
                    sbJSON.Append(" ], ");
                }
            }
            sbJSON.Remove(sbJSON.Length - 2, 2);
            sbJSON.Append(" }");
        }

        //  StoreChildNode: Store data associated with each nodeName
        //                  so that we know whether the nodeName is an array or not.
        private static void StoreChildNode(SortedList childNodeNames, string nodeName, object nodeValue)
        {
            // Pre-process contraction of XmlElement-s
            if (nodeValue is XmlElement)
            {
                // Convert  <aa></aa> into "aa":null
                //          <aa>xx</aa> into "aa":"xx"
                XmlNode cnode = (XmlNode)nodeValue;
                if (cnode.Attributes.Count == 0)
                {
                    XmlNodeList children = cnode.ChildNodes;
                    if (children.Count == 0)
                        nodeValue = null;
                    else if (children.Count == 1 && (children[0] is XmlText))
                        nodeValue = ((XmlText)(children[0])).InnerText;
                }
            }
            // Add nodeValue to ArrayList associated with each nodeName
            // If nodeName doesn't exist then add it
            object oValuesAL = childNodeNames[nodeName];
            ArrayList ValuesAL;
            if (oValuesAL == null)
            {
                ValuesAL = new ArrayList();
                childNodeNames[nodeName] = ValuesAL;
            }
            else
                ValuesAL = (ArrayList)oValuesAL;
            ValuesAL.Add(nodeValue);
        }

        private static void OutputNode(string childname, object alChild, StringBuilder sbJSON, bool showNodeName)
        {
            if (alChild == null)
            {
                if (showNodeName)
                    sbJSON.Append("\"" + SafeJSON(childname) + "\": ");
                sbJSON.Append("null");
            }
            else if (alChild is string)
            {
                if (showNodeName)
                    sbJSON.Append("\"" + SafeJSON(childname) + "\": ");
                string sChild = (string)alChild;
                sChild = sChild.Trim();
                sbJSON.Append("\"" + SafeJSON(sChild) + "\"");
            }
            else
                XmlToJSONnode(sbJSON, (XmlElement)alChild, showNodeName);
            sbJSON.Append(", ");
        }

        // Make a string safe for JSON
        private static string SafeJSON(string sIn)
        {
            StringBuilder sbOut = new StringBuilder(sIn.Length);
            foreach (char ch in sIn)
            {
                if (Char.IsControl(ch) || ch == '\'')
                {
                    int ich = (int)ch;
                    sbOut.Append(@"\u" + ich.ToString("x4"));
                    continue;
                }
                else if (ch == '\"' || ch == '\\' || ch == '/')
                {
                    sbOut.Append('\\');
                }
                sbOut.Append(ch);
            }
            return sbOut.ToString();
        }
    }
}

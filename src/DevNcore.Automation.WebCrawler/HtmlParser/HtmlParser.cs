using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DevNcore.Automation.WebCrawler
{
    public class HtmlParser
    {
        public string HtmlText { get; set; }
        public HtmlDocument hdoc { get; set; }

        public HtmlParser(string HtmlText)
        {
            HtmlText = HtmlText;
            hdoc = new HtmlDocument();
            hdoc.LoadHtml(HtmlText);
        }



        public HtmlNode GetNodeXpath(HtmlNode parent, string xpath)
        {
            try
            {
                if (string.IsNullOrEmpty(xpath))
                    return null;

                return parent.SelectSingleNode(xpath);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[{MethodBase.GetCurrentMethod().Name}] 예외발생 => {ex.Message}");
            }

            return null;
        }


    


        public HtmlNode GetNodeXpath(string xpath)
        {
            try
            {
                if (string.IsNullOrEmpty(xpath))
                    return null;

                return hdoc.DocumentNode.SelectSingleNode(xpath);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("GetNode: exception: " + ex.Message);
            }
            return null;
        }

    
        public HtmlNode GetNodeId(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return null;

                return hdoc.GetElementbyId(id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[{MethodBase.GetCurrentMethod().Name}] 예외발생 => {ex.Message}");
            }
            return null;
        }

        public HtmlNodeCollection GetNodesXpath(HtmlNode parent, string xpath)
        {
            try
            {
                if (string.IsNullOrEmpty(xpath))
                    return null;

                return parent.SelectNodes(xpath);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[{MethodBase.GetCurrentMethod().Name}] 예외발생 => {ex.Message}");
            }
            return null;
        }

     
        public string GetAttValueXpath(HtmlNode parent, string xpath, string attribute)
        {
            try
            {
                if (string.IsNullOrEmpty(xpath))
                    return null;

                if (string.IsNullOrEmpty(attribute))
                    return null;

                HtmlNode node = parent.SelectSingleNode(xpath);
                if (node == null)
                    return null;

                return node.Attributes[attribute].Value;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[{MethodBase.GetCurrentMethod().Name}] 예외발생 => {ex.Message}");
            }
            return null;
        }

        public HtmlNodeCollection GetNodesXpath(string xpath)
        {
            try
            {
                if (string.IsNullOrEmpty(xpath))
                    return null;

                return hdoc.DocumentNode.SelectNodes(xpath);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[{MethodBase.GetCurrentMethod().Name}] 예외발생 => {ex.Message}");
            }
            return null;
        }

    
        public string GetAttValueXpath(string xpath, string attribute)
        {
            try
            {
                if (string.IsNullOrEmpty(xpath))
                    return null;

                if (string.IsNullOrEmpty(attribute))
                    return null;

                HtmlNode node = hdoc.DocumentNode.SelectSingleNode(xpath);
                if (node == null)
                    return null;

                return node.Attributes[attribute].Value;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[{MethodBase.GetCurrentMethod().Name}] 예외발생 => {ex.Message}");
            }
            return null;
        }
      
        public string GetAttValueId(string id, string attribute)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return null;

                if (string.IsNullOrEmpty(attribute))
                    return null;

                HtmlNode node = hdoc.GetElementbyId(id);
                if (node == null)
                    return null;

                return node.Attributes[attribute].Value;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[{MethodBase.GetCurrentMethod().Name}] 예외발생 => {ex.Message}");
            }
            return null;
        }


    }
}

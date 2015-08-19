using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetMVCEnumerator.source
{
    public class Result
    {
        public string MethodName;
        public List<String> HttpMethods = new List<String>();
        public string Route;
        public List<String> Attributes = new List<String>();


        private Boolean containsMethod(List<String> methodAttributes, String httpMethod)
        {
            String attrname = methodAttributes.FirstOrDefault(s => s.Contains(httpMethod));
            return !String.IsNullOrEmpty(attrname);
        }

        public void setRoute(List<String> methodAttributes)
        {
            String attribute = methodAttributes.FirstOrDefault(s => s.Contains("Route"));
            
            if( attribute != null)
            {
                Route = attribute;
            }
        }

        public void setSupportedHTTPMethods(List<String> methodAttributes)
        {
            if( containsMethod(methodAttributes, "HttpPost") || 
                    containsMethod( methodAttributes, "Http.Post") )
            {
                HttpMethods.Add("POST");
            }
            if( containsMethod(methodAttributes, "HttpGet") || 
                    containsMethod( methodAttributes, "Http.Get") )
            {
                HttpMethods.Add("GET");
            }
            if( containsMethod(methodAttributes, "HttpPut") || 
                    containsMethod( methodAttributes, "Http.Put") )
            {
                HttpMethods.Add("PUT");
            }
            if( containsMethod(methodAttributes, "HttpPatch") || 
                    containsMethod( methodAttributes, "Http.Patch") )
            {
                HttpMethods.Add("PATCH");
            }
            if( containsMethod(methodAttributes, "HttpHead") || 
                    containsMethod( methodAttributes, "Http.Head") )
            {
                HttpMethods.Add("HEAD");
            }
            if( containsMethod(methodAttributes, "HttpDelete") || 
                    containsMethod( methodAttributes, "Http.Delete") )
            {
                HttpMethods.Add("DELETE");
            }
            if( containsMethod(methodAttributes, "HttpOptions") || 
                    containsMethod( methodAttributes, "Http.Options") )
            {
                HttpMethods.Add("OPTIONS");
            }
        }
    }

}

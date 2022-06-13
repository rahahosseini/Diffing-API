using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using static DifferingAPI2.Models.Model;

namespace DifferingAPI2.Controllers
{

    public class HomeController : Controller
    {
        //Because we do not have data base here to store data there and fetch them I declare a static property for this
        public static Dictionary<int, (string, string)> keyValuePairs = new Dictionary<int, (string, string)>();


        //controller method for the endpoint ---> <host>/v1/diff/<ID>/left
        [HttpPut]
        public string PutLeft(int id, string data)
        {
            if (keyValuePairs.ContainsKey(id))
            {
                var newValue = (data, keyValuePairs[id].Item2);
                keyValuePairs[id] = newValue;
            }
            else
            {
                keyValuePairs.Add(id, (data, null));
            }

            return "201 Created";
        }


        //controller method for the endpoint ---> <host>/v1/diff/<ID>/right
        [HttpPut]
        public string PutRight(int id, string data)
        {
            if (keyValuePairs.ContainsKey(id))
            {
                var newValue = (keyValuePairs[id].Item1, data);
                keyValuePairs[id] = newValue;
            }
            else
            {
                keyValuePairs.Add(id, (null, data));
            }

            return "201 Created";
        }

        //controller method for the endpoint -----> <host>/v1/diff/<ID>
        [HttpGet]
        public string FetchResult(int id)
        {
            if (!keyValuePairs.ContainsKey(id))
                return "404 Not Found";
            if (keyValuePairs[id].Item1 == null || keyValuePairs[id].Item2 == null)
                return "404 Not Found";

            return Difference(keyValuePairs[id].Item1, keyValuePairs[id].Item2);

        }
      
        public string Difference(string item1, string item2)
        {
            var result = new Result();
            if (item1.Length == item2.Length)
            {
                if (item1 == item2)
                    result.diffResultType = "Equals";

                else
                {
                    byte[] bytesItem1 = System.Convert.FromBase64String(item1);
                    byte[] bytesItem2 = System.Convert.FromBase64String(item2);

                    List<Different> different = new List<Different>();

                    for (int i = 0; i < bytesItem1.Length; i++)
                    {
                        if (bytesItem1[i] == bytesItem2[i])
                            continue;
                        else
                        {
                            int startIndex = i;
                            while (i < bytesItem1.Length && bytesItem1[i] != bytesItem2[i])
                            {
                                if (different.Where(x => x.offset == startIndex).Count() == 0)
                                    different.Add(new Different() { offset = startIndex, length = 1 });
                                else
                                    different.Where(x => x.offset == startIndex).First().length++;
                                    
                                i++;

                            }
                        }
                    }
                    result.diffs = different;
                    result.diffResultType = "ContentDoNotMatch";
                }
            }
            else
            {
                result.diffResultType = "SizeDoNotMatch";

            }

            return JsonConvert.SerializeObject(result);
        }

    }


}

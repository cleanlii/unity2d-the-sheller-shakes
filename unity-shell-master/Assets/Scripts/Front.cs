using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniJSON;

using System;
using System.IO;
using System.Text;
using System.Net;
using Newtonsoft.Json;





/*
 * Copyright (c) 2013 Calvin Rien
 *
 * Based on the JSON parser by Patrick van Bergen
 * http://techblog.procurios.nl/k/618/news/view/14605/14863/How-do-I-write-my-own-parser-for-JSON.html
 *
 * Simplified it so that it doesn't throw exceptions
 * and can be used in Unity iPhone with maximum code stripping.
 *
 * Permission is hereby granted, free of charge, to any person obtaining
 * a copy of this software and associated documentation files (the
 * "Software"), to deal in the Software without restriction, including
 * without limitation the rights to use, copy, modify, merge, publish,
 * distribute, sublicense, and/or sell copies of the Software, and to
 * permit persons to whom the Software is furnished to do so, subject to
 * the following conditions:
 *
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
 * MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
 * IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
 * CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
 * TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
 * SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

namespace MiniJSON
{
    // Example usage:
    //
    //  using UnityEngine;
    //  using System.Collections;
    //  using System.Collections.Generic;
    //  using MiniJSON;
    //
    //  public class MiniJSONTest : MonoBehaviour {
    //      void Start () {
    //          var jsonString = "{ \"array\": [1.44,2,3], " +
    //                          "\"object\": {\"key1\":\"value1\", \"key2\":256}, " +
    //                          "\"string\": \"The quick brown fox \\\"jumps\\\" over the lazy dog \", " +
    //                          "\"unicode\": \"\\u3041 Men\u00fa sesi\u00f3n\", " +
    //                          "\"int\": 65536, " +
    //                          "\"float\": 3.1415926, " +
    //                          "\"bool\": true, " +
    //                          "\"null\": null }";
    //
    //          var dict = Json.Deserialize(jsonString) as Dictionary<string,object>;
    //
    //          Debug.Log("deserialized: " + dict.GetType());
    //          Debug.Log("dict['array'][0]: " + ((List<object>) dict["array"])[0]);
    //          Debug.Log("dict['string']: " + (string) dict["string"]);
    //          Debug.Log("dict['float']: " + (double) dict["float"]); // floats come out as doubles
    //          Debug.Log("dict['int']: " + (long) dict["int"]); // ints come out as longs
    //          Debug.Log("dict['unicode']: " + (string) dict["unicode"]);
    //
    //          var str = Json.Serialize(dict);
    //
    //          Debug.Log("serialized: " + str);
    //      }
    //  }

    /// <summary>
    /// This class encodes and decodes JSON strings.
    /// Spec. details, see http://www.json.org/
    ///
    /// JSON uses Arrays and Objects. These correspond here to the datatypes IList and IDictionary.
    /// All numbers are parsed to doubles.
    /// </summary>
    public static class Json
    {
        /// <summary>
        /// Parses the string json into a value
        /// </summary>
        /// <param name="json">A JSON string.</param>
        /// <returns>An List<object>, a Dictionary<string, object>, a double, an integer,a string, null, true, or false</returns>
        public static object Deserialize(string json)
        {
            // save the string for debug information
            if (json == null)
            {
                return null;
            }

            return Parser.Parse(json);
        }

        sealed class Parser : IDisposable
        {
            const string WORD_BREAK = "{}[],:\"";

            public static bool IsWordBreak(char c)
            {
                return Char.IsWhiteSpace(c) || WORD_BREAK.IndexOf(c) != -1;
            }

            enum TOKEN
            {
                NONE,
                CURLY_OPEN,
                CURLY_CLOSE,
                SQUARED_OPEN,
                SQUARED_CLOSE,
                COLON,
                COMMA,
                STRING,
                NUMBER,
                TRUE,
                FALSE,
                NULL
            };

            StringReader json;

            Parser(string jsonString)
            {
                json = new StringReader(jsonString);
            }

            public static object Parse(string jsonString)
            {
                using (var instance = new Parser(jsonString))
                {
                    return instance.ParseValue();
                }
            }

            public void Dispose()
            {
                json.Dispose();
                json = null;
            }

            Dictionary<string, object> ParseObject()
            {
                Dictionary<string, object> table = new Dictionary<string, object>();

                // ditch opening brace
                json.Read();

                // {
                while (true)
                {
                    switch (NextToken)
                    {
                        case TOKEN.NONE:
                            return null;
                        case TOKEN.COMMA:
                            continue;
                        case TOKEN.CURLY_CLOSE:
                            return table;
                        default:
                            // name
                            string name = ParseString();
                            if (name == null)
                            {
                                return null;
                            }

                            // :
                            if (NextToken != TOKEN.COLON)
                            {
                                return null;
                            }
                            // ditch the colon
                            json.Read();

                            // value
                            table[name] = ParseValue();
                            break;
                    }
                }
            }

            List<object> ParseArray()
            {
                List<object> array = new List<object>();

                // ditch opening bracket
                json.Read();

                // [
                var parsing = true;
                while (parsing)
                {
                    TOKEN nextToken = NextToken;

                    switch (nextToken)
                    {
                        case TOKEN.NONE:
                            return null;
                        case TOKEN.COMMA:
                            continue;
                        case TOKEN.SQUARED_CLOSE:
                            parsing = false;
                            break;
                        default:
                            object value = ParseByToken(nextToken);

                            array.Add(value);
                            break;
                    }
                }

                return array;
            }

            object ParseValue()
            {
                TOKEN nextToken = NextToken;
                return ParseByToken(nextToken);
            }

            object ParseByToken(TOKEN token)
            {
                switch (token)
                {
                    case TOKEN.STRING:
                        return ParseString();
                    case TOKEN.NUMBER:
                        return ParseNumber();
                    case TOKEN.CURLY_OPEN:
                        return ParseObject();
                    case TOKEN.SQUARED_OPEN:
                        return ParseArray();
                    case TOKEN.TRUE:
                        return true;
                    case TOKEN.FALSE:
                        return false;
                    case TOKEN.NULL:
                        return null;
                    default:
                        return null;
                }
            }

            string ParseString()
            {
                StringBuilder s = new StringBuilder();
                char c;

                // ditch opening quote
                json.Read();

                bool parsing = true;
                while (parsing)
                {

                    if (json.Peek() == -1)
                    {
                        parsing = false;
                        break;
                    }

                    c = NextChar;
                    switch (c)
                    {
                        case '"':
                            parsing = false;
                            break;
                        case '\\':
                            if (json.Peek() == -1)
                            {
                                parsing = false;
                                break;
                            }

                            c = NextChar;
                            switch (c)
                            {
                                case '"':
                                case '\\':
                                case '/':
                                    s.Append(c);
                                    break;
                                case 'b':
                                    s.Append('\b');
                                    break;
                                case 'f':
                                    s.Append('\f');
                                    break;
                                case 'n':
                                    s.Append('\n');
                                    break;
                                case 'r':
                                    s.Append('\r');
                                    break;
                                case 't':
                                    s.Append('\t');
                                    break;
                                case 'u':
                                    var hex = new char[4];

                                    for (int i = 0; i < 4; i++)
                                    {
                                        hex[i] = NextChar;
                                    }

                                    s.Append((char)Convert.ToInt32(new string(hex), 16));
                                    break;
                            }
                            break;
                        default:
                            s.Append(c);
                            break;
                    }
                }

                return s.ToString();
            }

            object ParseNumber()
            {
                string number = NextWord;

                if (number.IndexOf('.') == -1)
                {
                    long parsedInt;
                    Int64.TryParse(number, out parsedInt);
                    return parsedInt;
                }

                double parsedDouble;
                Double.TryParse(number, out parsedDouble);
                return parsedDouble;
            }

            void EatWhitespace()
            {
                while (Char.IsWhiteSpace(PeekChar))
                {
                    json.Read();

                    if (json.Peek() == -1)
                    {
                        break;
                    }
                }
            }

            char PeekChar
            {
                get
                {
                    return Convert.ToChar(json.Peek());
                }
            }

            char NextChar
            {
                get
                {
                    return Convert.ToChar(json.Read());
                }
            }

            string NextWord
            {
                get
                {
                    StringBuilder word = new StringBuilder();

                    while (!IsWordBreak(PeekChar))
                    {
                        word.Append(NextChar);

                        if (json.Peek() == -1)
                        {
                            break;
                        }
                    }

                    return word.ToString();
                }
            }

            TOKEN NextToken
            {
                get
                {
                    EatWhitespace();

                    if (json.Peek() == -1)
                    {
                        return TOKEN.NONE;
                    }

                    switch (PeekChar)
                    {
                        case '{':
                            return TOKEN.CURLY_OPEN;
                        case '}':
                            json.Read();
                            return TOKEN.CURLY_CLOSE;
                        case '[':
                            return TOKEN.SQUARED_OPEN;
                        case ']':
                            json.Read();
                            return TOKEN.SQUARED_CLOSE;
                        case ',':
                            json.Read();
                            return TOKEN.COMMA;
                        case '"':
                            return TOKEN.STRING;
                        case ':':
                            return TOKEN.COLON;
                        case '0':
                        case '1':
                        case '2':
                        case '3':
                        case '4':
                        case '5':
                        case '6':
                        case '7':
                        case '8':
                        case '9':
                        case '-':
                            return TOKEN.NUMBER;
                    }

                    switch (NextWord)
                    {
                        case "false":
                            return TOKEN.FALSE;
                        case "true":
                            return TOKEN.TRUE;
                        case "null":
                            return TOKEN.NULL;
                    }

                    return TOKEN.NONE;
                }
            }
        }

        /// <summary>
        /// Converts a IDictionary / IList object or a simple type (string, int, etc.) into a JSON string
        /// </summary>
        /// <param name="json">A Dictionary<string, object> / List<object></param>
        /// <returns>A JSON encoded string, or null if object 'json' is not serializable</returns>
        public static string Serialize(object obj)
        {
            return Serializer.Serialize(obj);
        }

        sealed class Serializer
        {
            StringBuilder builder;

            Serializer()
            {
                builder = new StringBuilder();
            }

            public static string Serialize(object obj)
            {
                var instance = new Serializer();

                instance.SerializeValue(obj);

                return instance.builder.ToString();
            }

            void SerializeValue(object value)
            {
                IList asList;
                IDictionary asDict;
                string asStr;

                if (value == null)
                {
                    builder.Append("null");
                }
                else if ((asStr = value as string) != null)
                {
                    SerializeString(asStr);
                }
                else if (value is bool)
                {
                    builder.Append((bool)value ? "true" : "false");
                }
                else if ((asList = value as IList) != null)
                {
                    SerializeArray(asList);
                }
                else if ((asDict = value as IDictionary) != null)
                {
                    SerializeObject(asDict);
                }
                else if (value is char)
                {
                    SerializeString(new string((char)value, 1));
                }
                else
                {
                    SerializeOther(value);
                }
            }

            void SerializeObject(IDictionary obj)
            {
                bool first = true;

                builder.Append('{');

                foreach (object e in obj.Keys)
                {
                    if (!first)
                    {
                        builder.Append(',');
                    }

                    SerializeString(e.ToString());
                    builder.Append(':');

                    SerializeValue(obj[e]);

                    first = false;
                }

                builder.Append('}');
            }

            void SerializeArray(IList anArray)
            {
                builder.Append('[');

                bool first = true;

                foreach (object obj in anArray)
                {
                    if (!first)
                    {
                        builder.Append(',');
                    }

                    SerializeValue(obj);

                    first = false;
                }

                builder.Append(']');
            }

            void SerializeString(string str)
            {
                builder.Append('\"');

                char[] charArray = str.ToCharArray();
                foreach (var c in charArray)
                {
                    switch (c)
                    {
                        case '"':
                            builder.Append("\\\"");
                            break;
                        case '\\':
                            builder.Append("\\\\");
                            break;
                        case '\b':
                            builder.Append("\\b");
                            break;
                        case '\f':
                            builder.Append("\\f");
                            break;
                        case '\n':
                            builder.Append("\\n");
                            break;
                        case '\r':
                            builder.Append("\\r");
                            break;
                        case '\t':
                            builder.Append("\\t");
                            break;
                        default:
                            int codepoint = Convert.ToInt32(c);
                            if ((codepoint >= 32) && (codepoint <= 126))
                            {
                                builder.Append(c);
                            }
                            else
                            {
                                builder.Append("\\u");
                                builder.Append(codepoint.ToString("x4"));
                            }
                            break;
                    }
                }

                builder.Append('\"');
            }

            void SerializeOther(object value)
            {
                // NOTE: decimals lose precision during serialization.
                // They always have, I'm just letting you know.
                // Previously floats and doubles lost precision too.
                if (value is float)
                {
                    builder.Append(((float)value).ToString("R"));
                }
                else if (value is int
                  || value is uint
                  || value is long
                  || value is sbyte
                  || value is byte
                  || value is short
                  || value is ushort
                  || value is ulong)
                {
                    builder.Append(value);
                }
                else if (value is double
                  || value is decimal)
                {
                    builder.Append(Convert.ToDouble(value).ToString("R"));
                }
                else
                {
                    SerializeString(value.ToString());
                }
            }
        }
    }
}






public class Rootobject
{
    public int code { get; set; }
    public Datum[] data { get; set; }
    public string msg { get; set; }
}

public class Datum
{
    public string prefab { get; set; }
    public string scene { get; set; }
    public Trans transform { get; set; }
    public string text { get; set; }
   // public int time { get; set; }
    public string id { get; set; }
}

public class Trans
{
    public Position position { get; set; }
    public Rotation rotation { get; set; }
    public Scale scale { get; set; }
}

public class Position
{
    public float x { get; set; }
    public float y { get; set; }
    public float z { get; set; }
}

public class Rotation
{
    //public float w { get; set; }
    public float x { get; set; }
    public float y { get; set; }
    public float z { get; set; }
    public float w { get; set; }
}

public class Scale
{
    public float x { get; set; }
    public float y { get; set; }
    public float z { get; set; }
}


public static class Front
{
    // Start is called before the first frame update

    //public static Shells JsonToList(string js)
    //{
    //    Shells shells= new Shells();
    //    Dictionary<string, object> jsObject = MiniJSON.Json.Deserialize(js) as Dictionary<string,object>;
    //    shells.code = jsObject["code"].ToString();
    //    shells.shell = new List<ShellInfo>();

    //    List<object> shellList = jsObject["date"] as List<object>;
    //    foreach(var i in shellList)
    //    {
    //        Dictionary<string, object> shell = i as Dictionary<string, object>;
    //        ShellInfo shellObj = new ShellInfo();
    //        shellObj.prefabName = shell["prefab"].ToString();
    //        shellObj.text = shell["test"].ToString();
    //        shellObj.time = shell["time"].ToString();
    //        shellObj.id = shell["ID"].ToString();
    //        shellObj.sceneName = shell["scene"].ToString();

    //        Dictionary<string, object> tr = MiniJSON.Json.Deserialize(shell["transform"].ToString()) 
    //            as Dictionary<string, object>;
    //        Dictionary<string, object> tra = new Dictionary<string, object>();

    //        foreach(var j in tr)
    //        {
    //            Dictionary<string, object> psc = MiniJSON.Json.Deserialize(tr[j.Key.ToString()].ToString())
    //                    as Dictionary<string, object>;
    //            xyz p = new xyz();
    //            p.x = float.Parse(psc["x"].ToString());
    //            p.y = float.Parse(psc["y"].ToString());
    //            p.z = float.Parse(psc["z"].ToString());
    //            tra.Add(j.Key.ToString(), p);
    //        }
    //        shellObj.transform = tra;
    //        shells.shell.Add(shellObj);
    //    }
    //    return shells;
    //}

    public static T GetData2<T>(string text) where T : class
    {
        //JsonData table = AnalysisJson.Analy<JsonData>(text);
        T t = JsonConvert.DeserializeObject<T>(text);
        return t;
    }

    public static Rootobject HttpGet(string sense, int num)
    {
        //try
        {
            //创建Get请求
            string url = "http://unity.happypie.net:88/shell/query/" + sense + "/" + num.ToString();

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            //接受返回来的数据
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader streamReader = new StreamReader(stream, Encoding.GetEncoding("utf-8"));
            string retString = streamReader.ReadToEnd();
            streamReader.Close();
            stream.Close();
            response.Close();
            Debug.Log("GET::"+retString);
            Rootobject o = GetData2<Rootobject>(retString);
            return o;
        }
        //catch (Exception)
        //{
        //    Debug.Log("NULL");
        //    Rootobject oo = new Rootobject();
        //    return oo;
        //}
    }

    public static IEnumerator IEHttpGetShell(string sense, int num)
    {
        //http://unity.happypie.net:88/shell/?sense=MainLevel&count=6

        WWW www = new WWW("http://unity.happypie.net:88/shell/query/" + sense + "/" + num.ToString());
        //JsonToList(www.text);
        string str = www.text;
        //Shells obj = JsonUtility.FromJson<Shells>(str);
        yield return www;

        //JsonUtility.FromJson<Shells>(www.text);
        if (www.error != null)
        {
            Debug.LogError(www.error);
        }

        Debug.Log(www.text);
    }


    public static IEnumerator IEPostCreateShell(string nick, string text, string sceneName, string prefabName, GameObject shell)
    {

        string fullUrl = "http://unity.happypie.net:88/shell/create";

        //加入http 头信息
        Dictionary<string, string> JsonDic = new Dictionary<string, string>();
        JsonDic.Add("Content-Type", "application/json");

        //请求的Json数据

        Dictionary<string, object> pos = new Dictionary<string, object>();
        pos["x"] = shell.transform.localPosition.x;
        pos["y"] = shell.transform.localPosition.y;
        pos["z"] = shell.transform.localPosition.z;

        Dictionary<string, object> rot = new Dictionary<string, object>();
        rot["x"] = shell.transform.localRotation.x;
        rot["y"] = shell.transform.localRotation.y;
        rot["z"] = shell.transform.localRotation.z;
        rot["w"] = shell.transform.localRotation.w;

        Dictionary<string, object> scal = new Dictionary<string, object>();
        scal["x"] = shell.transform.localScale.x;
        scal["y"] = shell.transform.localScale.y;
        scal["z"] = shell.transform.localScale.z;

        Dictionary<string, object> trans = new Dictionary<string, object>();
        trans["position"] = pos;
        trans["rotation"] = rot;
        trans["scale"] = scal;

        Dictionary<string, object> UserDic = new Dictionary<string, object>();
        UserDic["prefab"] = prefabName;
        UserDic["scene"] = sceneName;
        UserDic["transform"] = trans;
        UserDic["text"] = text;
        UserDic["nick"] = nick;

        UserDic["time"] = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() +
        DateTime.Now.Day.ToString()  + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
        Dictionary<String, object> tw = new Dictionary<string, object>();
        tw["shell"] = UserDic;

        string data = Json.Serialize(tw);
        Debug.Log("Post Shell: " + data);

        //转换为字节
        byte[] post_data;
        post_data = System.Text.UTF8Encoding.UTF8.GetBytes(data);

        WWW www = new WWW(fullUrl, post_data, JsonDic);
        yield return www;

        if (www.error != null)
        {
            Debug.LogError("error:" + www.error);
        }
        Debug.Log("POST::::"+www.text);
    }

}


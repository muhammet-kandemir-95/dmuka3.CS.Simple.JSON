using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;

namespace dmuka3.CS.Simple.JSON
{
    /// <summary>
    /// JSON class.
    /// </summary>
    public static class json
    {
        #region Variables
        public static readonly object undefined = new object();
        #endregion

        #region Methods
        /// <summary>
        /// Create a JObject with properties or a Array with items.
        /// </summary>
        /// <returns></returns>
        public static dynamic dic(object value = null)
        {
            if (value == null)
                return new JObject(value);
            if (value is JObject || value is JArray)
                return value;

            if (value is IEnumerator)
            {
                var arr = json.arr();
                var enumerator = (IEnumerator)value;
                while (enumerator.MoveNext())
                    arr.push(enumerator.Current);
                return arr;
            }
            else if (value is string == false && value is IEnumerable)
            {
                var arr = json.arr();
                var enumerable = (IEnumerable)value;
                foreach (var item in enumerable)
                    arr.push(item);
                return arr;
            }
            else if (
                value == null ||
                value is sbyte ||
                value is byte ||
                value is ushort ||
                value is short ||
                value is uint ||
                value is int ||
                value is ulong ||
                value is long ||
                value is bool ||
                value is float ||
                value is double ||
                value is decimal ||
                value is DateTime ||
                value is string
                )
                return new JObject(value);

            var result = new JObject();
            var type = value.GetType();
            var props = type.GetProperties();
            foreach (var prop in props)
            {
                var propValue = prop.GetValue(value);
                if (
                    propValue == null ||
                    propValue is sbyte ||
                    propValue is byte ||
                    propValue is ushort ||
                    propValue is short ||
                    propValue is uint ||
                    propValue is int ||
                    propValue is ulong ||
                    propValue is long ||
                    propValue is bool ||
                    propValue is float ||
                    propValue is double ||
                    propValue is decimal ||
                    propValue is DateTime ||
                    propValue is string
                    )
                    result.TrySetMemberByName(prop.Name, propValue);
                else if (propValue is JObject)
                    result.TrySetMemberByName(prop.Name, propValue);
                else if (propValue is JArray)
                    result.TrySetMemberByName(prop.Name, propValue);
                else if (propValue == undefined)
                    result.TrySetMemberByName(prop.Name, propValue);
                else
                    result.TrySetMemberByName(prop.Name, dic(propValue));
            }

            return result;
        }

        /// <summary>
        /// Create a JArray.
        /// </summary>
        /// <returns></returns>
        public static JArray arr()
        {
            return new JArray();
        }

        /// <summary>
        /// Create a JObject.
        /// </summary>
        /// <returns></returns>
        public static JObject obj()
        {
            return new JObject();
        }

        /// <summary>
        /// Get JSON from url's json result.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <param name="content"></param>
        /// <param name="contentType"></param>
        /// <param name="cookieContainer"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static (bool success, int statusCode, string err, dynamic data) download(string url, string method, object content = null, string contentType = "application/json", CookieContainer cookieContainer = null, Dictionary<string, string> headers = null)
        {
            return download(url, method, content == null ? null : Encoding.UTF8.GetBytes(content.ToString()), contentType, cookieContainer, headers);
        }

        /// <summary>
        /// Get JSON from url's json result.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <param name="content"></param>
        /// <param name="contentType"></param>
        /// <param name="cookieContainer"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static (bool success, int statusCode, string err, dynamic data) download(string url, string method, string content = null, string contentType = "application/json", CookieContainer cookieContainer = null, Dictionary<string, string> headers = null)
        {
            return download(url, method, content == null ? null : Encoding.UTF8.GetBytes(content), contentType, cookieContainer, headers);
        }

        /// <summary>
        /// Get JSON from url's json result.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <param name="content"></param>
        /// <param name="contentType"></param>
        /// <param name="cookieContainer"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static (bool success, int statusCode, string err, dynamic data) download(string url, string method = "GET", byte[] content = null, string contentType = "application/json", CookieContainer cookieContainer = null, Dictionary<string, string> headers = null)
        {
            var http = (HttpWebRequest)WebRequest.Create(url);
            http.Accept = "application/json";
            http.ContentType = contentType;
            http.Method = method;

            if (cookieContainer != null)
                http.CookieContainer = cookieContainer;
            if (headers != null)
                foreach (var item in headers)
                    http.Headers.Add(item.Key, item.Value);

            if (content != null)
            {
                Stream newStream = http.GetRequestStream();
                newStream.Write(content, 0, content.Length);
                newStream.Close();
            }

            try
            {
                var response = (HttpWebResponse)http.GetResponse();

                var stream = response.GetResponseStream();
                var sr = new StreamReader(stream);
                var responseContent = sr.ReadToEnd();
                var statusCode = (int)response.StatusCode;
                response.Dispose();

                return (success: true, statusCode: statusCode, err: "", data: json.parse(responseContent));
            }
            catch (WebException ex)
            {
                var response = (HttpWebResponse)ex.Response;
                var stream = response.GetResponseStream();
                var sr = new StreamReader(stream);
                var responseContent = sr.ReadToEnd();
                var statusCode = (int)response.StatusCode;
                response.Dispose();

                return (success: false, statusCode: statusCode, err: responseContent, data: null);
            }
            catch
            {
                throw;
            }
        }

        #region JSON Parse
        private static void throwErrByIndex(string j, int index)
        {
            if (index >= j.Length)
                throw new Exception("JSON syntax error (index: " + index + ")!");
        }

        private static JArray parseJArray(string j, ref int index)
        {
            index++;
            throwErrByIndex(j, index);
            JArray arr = new JArray();

            while (j[index] != ']')
            {
                arr.push(parseJSON(j, ref index));
                throwErrByIndex(j, index);
                if (j[index] != ',' && j[index] != ']')
                    throw new Exception("JSON syntax error (index: " + index + ")!");
            }
            index++;

            return arr;
        }

        private static string parseString(string j, ref int index)
        {
            index++;
            string value = "";
            bool escape = false;
            while (escape == true || (escape == false && j[index] != '"'))
            {
                var c = j[index];
                if (c == '\\')
                    escape = true;
                else if (escape)
                {
                    switch (c)
                    {
                        case '"':
                            value += '"';
                            break;
                        case 'n':
                            value += '\n';
                            break;
                        case 'r':
                            value += '\r';
                            break;
                        case 'b':
                            value += '\b';
                            break;
                        case 't':
                            value += '\t';
                            break;
                        default:
                            value += c;
                            break;
                    }
                    escape = false;
                }
                else
                    value += c;
                index++;
                throwErrByIndex(j, index);
            }
            index++;
            return value;
        }

        private static bool parseBoolean(string j, ref int index)
        {
            if (j[index] == 't')
            {
                index += "true".Length;
                return true;
            }
            else
            {
                index += "false".Length;
                return false;
            }
        }

        private static object parseNull(string j, ref int index)
        {
            index += "null".Length;
            return null;
        }

        private static decimal parseNumber(string j, ref int index)
        {
            string number = "";
            while (true)
            {
                if (index >= j.Length)
                    break;

                var c = j[index];
                if (
                    c != '.' &&
                    c != '0' &&
                    c != '1' &&
                    c != '2' &&
                    c != '3' &&
                    c != '4' &&
                    c != '5' &&
                    c != '6' &&
                    c != '7' &&
                    c != '8' &&
                    c != '9'
                    )
                    break;

                number += c;
                index++;
            }
            return Convert.ToDecimal(number, CultureInfo.InvariantCulture);
        }

        private static JObject parseJObject(string j, ref int index)
        {
            index++;
            throwErrByIndex(j, index);
            JObject jobject = new JObject();

            while (j[index] != '}')
            {
                if (j[index] == '"')
                {
                    string name = parseString(j, ref index);

                    while (j[index] != ':')
                    {
                        index++;
                        throwErrByIndex(j, index);
                    }
                    index++;
                    throwErrByIndex(j, index);

                    object value = parseJSON(j, ref index);
                    jobject.TrySetMemberByName(name, value);
                }
                else
                    index++;

                throwErrByIndex(j, index);
            }
            index++;

            return jobject;
        }

        private static object parseJSON(string j, ref int index)
        {
            bool typeJObject = false;
            bool typeJArray = false;
            bool typeNumber = false;
            bool typeString = false;
            bool typeBoolean = false;
            bool typeNull = false;

            bool found = false;
            while (index < j.Length)
            {
                var c = j[index];
                if (c == 'f' || c == 't')
                {
                    typeBoolean = true;
                    found = true;
                    break;
                }
                else if (c == 'n')
                {
                    typeNull = true;
                    found = true;
                    break;
                }
                else if (c == '"')
                {
                    typeString = true;
                    found = true;
                    break;
                }
                else if (
                    c == '0' ||
                    c == '1' ||
                    c == '2' ||
                    c == '3' ||
                    c == '4' ||
                    c == '5' ||
                    c == '6' ||
                    c == '7' ||
                    c == '8' ||
                    c == '9'
                    )
                {
                    typeNumber = true;
                    found = true;
                    break;
                }
                else if (c == '{')
                {
                    typeJObject = true;
                    found = true;
                    break;
                }
                else if (c == '[')
                {
                    typeJArray = true;
                    found = true;
                    break;
                }
                index++;
            }

            if (found == false)
                throw new Exception("JSON syntax error (index: " + index + ")!");

            if (typeJObject)
                return parseJObject(j, ref index);
            else if (typeJArray)
                return parseJArray(j, ref index);
            else if (typeBoolean)
                return parseBoolean(j, ref index);
            else if (typeString)
                return parseString(j, ref index);
            else if (typeNumber)
                return parseNumber(j, ref index);
            else if (typeNull)
                return parseNull(j, ref index);
            return null;
        }

        /// <summary>
        /// Parse JSON text to JSON
        /// </summary>
        /// <param name="j"></param>
        /// <returns></returns>
        public static dynamic parse(string j)
        {
            int index = 0;
            return parseJSON(j, ref index);
        }
        #endregion
        #endregion
    }
}

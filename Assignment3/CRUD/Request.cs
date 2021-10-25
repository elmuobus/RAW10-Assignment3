using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text.Json;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Assignment3
{
    public class Request
    {
        private List<Category> _data = new List<Category>();

        private Response concatError(Response r, string msg)
        {
            if (r.Status != null)
                r.Status = r.Status + ", " + msg;
            else 
                r.Status = Constants.MissingMethod + msg;
            return r;
        }
        
        private Response errorHandling(RequestContent m)
        {
            var resp = new Response();
            if (m.Method == null)
                concatError(resp, Constants.MissingMethod);
            if (m.Body == null && (m.Method != "delete" || m.Method != "read"))
                concatError(resp, Constants.MissingBody);
            if (m.Date == null)
                concatError(resp, Constants.MissingDate);
            if (m.Path == null)
                concatError(resp, Constants.MissingPath);
            if ( m.Method != "echo" && (m.Path != null && m.Path.Contains("/api/categories") == false))
            {
                concatError(resp, Constants.IllegalPath);
            }
            if (resp.Status != null)
                resp.Status = Constants.BadRequestCode + " " + resp.Status;
            resp.Body = null;
            return resp;
        }
        
        public bool generateFirstDatas()
        {
            try
            {
                _data.Add(new Category() {cid = 1, name = "Beverages"});
                _data.Add(new Category() {cid = 2, name = "Condiments"});
                _data.Add(new Category() {cid = 3, name = "Confections"});
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public Response ApiHandling(RequestContent m)
        {
            var resp = new Response();
            if (m == null)
            {
                resp.Body = null;
                resp.Status = Constants.AllNull;
            }
            else
            {
                resp = errorHandling(m);
                switch (m.Method)
                {
                    case "create":
                        resp = Create(m);
                        break;
                    case "echo":
                        resp = Echo(m);
                        break;
                    case "delete":
                        resp = Delete(m);
                        break;
                    case "read":
                        resp = Read(m);
                        break;
                    case "update":
                        resp = Update(m);
                        break;
                    default:
                        resp.Body = String.Empty;
                        if (m.Method != null)
                        {
                            resp = concatError(resp, Constants.IllegalMethod);
                        }

                        break;
                }
            }
            return resp;
        }

        public Response Echo(RequestContent m)
        {
            var resp = new Response();
            resp.Body = m.Body;
            resp.Status = Constants.Ok;
            return resp;
        }

        public Response Update(RequestContent m)
        {
            var resp = new Response();
            string[] buffer = m.Path.Split('/');
            if (m.Body != null && buffer.Length > 2)
            {
                var bodyFromJson = JsonConvert.DeserializeObject<Category>(m.Body);
                try
                {
                    if (_data.Any(a => a.name == bodyFromJson.name))
                    {
                        var obj = _data.First(x => x.cid == Int32.Parse(buffer[3]));
                        if (obj != null) obj.name = bodyFromJson.name;
                        resp.Status = Constants.Updated;
                    } else {
                        resp.Status = Constants.NotFound;
                    }
                }
                catch (Exception)
                {
                    resp.Status = Constants.Error;
                }
            }
            resp.Body = null;
            return resp;
        }
        
        public Response Create(RequestContent m)
        {
            var resp = new Response();
            try
            {
                if (m.Body != null)
                {
                    var bodyFromJson = JsonConvert.DeserializeObject<Category>(m.Body);
                    if (bodyFromJson.name != "" && !_data.Any(a => a.name == bodyFromJson.name))
                    {
                        Category item = new Category();
                        item.name = bodyFromJson.name;
                        item.cid = _data.Count() + 1;
                        _data.Add(item);
                        var responseJson = JsonSerializer.Serialize(item,
                            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
                        resp.Status = Constants.Created;
                        resp.Body = responseJson;
                    }
                    else
                    {
                        resp.Status = Constants.BadRequestCode + Constants.BadRequest;
                    }
                }
            } catch (NullReferenceException e)
            {
                resp = concatError(resp, Constants.MissingResource);
            }
            return resp;
        }

        public Response Read(RequestContent m)
        {
            var resp = new Response();
            try
            {
                string[] buffer = m.Path.Split('/');
                if (buffer.Length > 2)
                {
                    var item = _data.FirstOrDefault(s => s.cid == Int32.Parse(buffer[3]));
                    if (item != null)
                    {
                        var responseJson = JsonSerializer.Serialize(item,
                            new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
                        resp.Status = Constants.Ok;
                        resp.Body = responseJson;
                    }
                    else
                    {
                        resp.Status = Constants.NotFound;
                    }
                }
                else
                {
                    var responseJson = JsonSerializer.Serialize(_data,
                        new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
                    resp.Status = Constants.Ok;
                    resp.Body = responseJson;
                }

                return resp;
            }
            catch (Exception e)
            {
                resp.Status = Constants.BadRequestCode + Constants.BadRequest;
                return resp;
            }
        }

        public Response Delete(RequestContent m)
        {
            var resp = new Response();
            try
            {
                string[] buffer = m.Path.Split('/');
                var itemToRemove = _data.FirstOrDefault(r => r.cid == Int32.Parse(buffer[3]));
                _data.Remove(itemToRemove);
                resp.Status = Constants.Ok;
                return resp;
            }
            catch (NullReferenceException e)
            {
                resp = concatError(resp, Constants.MissingResource);
            }
            return resp;
        }
    }
}
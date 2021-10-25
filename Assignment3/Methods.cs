using System;
using System.Collections.Generic;

namespace Assignment3
{
    public abstract class AMethod
    {
        protected readonly Request Request;
        protected Response _response;

        protected AMethod(Request request)
        {
            Request = request;
            _response = new Response()
            {
                Status = "",
                Body = null,
            };
        }

        public void Launch()
        {
            var errorString = ErrorHandler();
            if (errorString != "")
            {
                _response.Status = errorString;
            }
            else
            {
                DoMethod();
            }
        }

        protected abstract string ErrorHandler();
        protected abstract void DoMethod();
        
        public Response Response() => _response;
    }
    
    public class Create: AMethod
    {
        public Create(Request request) : base(request)
        { }

        protected override string ErrorHandler()
        {
            List<string> result = new();

            if (Request.Date == null)
                result.Add(ReturnStatus.MissingDate);
            else if (!int.TryParse(Request.Date, out _))
                result.Add(ReturnStatus.IllegalDate);
            if (Request.Path == null)
                result.Add(ReturnStatus.MissingPath);
            if (Request.Body == null)
                result.Add(ReturnStatus.MissingBody);
            else if (!Utils.ValidateJson(Request.Body))
                result.Add(ReturnStatus.IllegalBody);
            if (Request.Path == null || Request.Body == null)
                result.Add(ReturnStatus.MissingResource);

            return result.Count > 0 ? $"4 {string.Join(", ", result)}" : "";
        }

        protected override void DoMethod()
        {
            try
            {
                if (Request.Path != null && Request.Path.Contains(Routes.Categories) && Request.Path.Contains(Routes.Category)) throw new Exception();
                var categoryRow = Utils.FromJson<CategoryRow>(Request.Body!);
                if (categoryRow.Name == null) throw new Exception();
                _response.Status = ReturnStatus.Ok;
                _response.Body = Utils.ToJson(Category.AddNewName(categoryRow.Name));
            }
            catch (Exception)
            {
                _response.Status = ReturnStatus.BadRequest;
            }
        }
    }
    
    public class Read: AMethod
    {
        public Read(Request request) : base(request)
        { }

        protected override string ErrorHandler()
        {
            List<string> result = new();

            if (Request.Date == null)
                result.Add(ReturnStatus.MissingDate);
            else if (!int.TryParse(Request.Date, out _))
                result.Add(ReturnStatus.IllegalDate);
            if (Request.Path == null)
                result.Add($"{ReturnStatus.MissingPath}, {ReturnStatus.MissingResource}");
            
            return result.Count > 0 ? $"4 {string.Join(", ", result)}" : "";
        }

        protected override void DoMethod()
        {
            try
            {
                if (Request.Path != null && !Request.Path.Contains(Routes.Categories)) throw new Exception();
                if (Request.Path.Contains(Routes.Category))
                {
                    var id = int.Parse(Request.Path.Replace(Routes.Category, ""));
                    var category = Category.ReadOne(id);
                    if (category == null)
                    {
                        _response.Status = ReturnStatus.NotFound;
                    }
                    else
                    {
                        _response.Status = ReturnStatus.Ok;
                        _response.Body = Utils.ToJson(category);
                    }
                }
                else
                {
                    _response.Status = ReturnStatus.Ok;
                    _response.Body = Utils.ToJson(Category.ReadAll());
                }
            }
            catch (Exception)
            {
                _response.Status = ReturnStatus.BadRequest;
            }
        }
    }
    
    public class Update: AMethod
    {
        public Update(Request request) : base(request)
        { }

        protected override string ErrorHandler()
        {
            List<string> result = new();

            if (Request.Date == null)
                result.Add(ReturnStatus.MissingDate);
            else if (!int.TryParse(Request.Date, out _))
                result.Add(ReturnStatus.IllegalDate);
            if (Request.Path == null)
                result.Add(ReturnStatus.MissingPath);
            if (Request.Body == null)
                result.Add(ReturnStatus.MissingBody);
            else if (!Utils.ValidateJson(Request.Body))
                result.Add(ReturnStatus.IllegalBody);
            if (Request.Path == null || Request.Body == null)
                result.Add(ReturnStatus.MissingResource);
            
            return result.Count > 0 ? $"4 {string.Join(", ", result)}" : "";
        }

        protected override void DoMethod()
        {
            try
            {
                if (Request.Path != null && !Request.Path.Contains(Routes.Category)) throw new Exception();
                var id = int.Parse(Request.Path?.Replace(Routes.Category, "")!);
                _response.Status = Category.Update(id, Utils.FromJson<CategoryRow>(Request.Body!));
            }
            catch (Exception)
            {
                _response.Status = ReturnStatus.BadRequest;
            }
        }
    }
    
    public class Delete: AMethod
    {
        public Delete(Request request) : base(request)
        { }

        protected override string ErrorHandler()
        {
            List<string> result = new();

            if (Request.Date == null)
                result.Add(ReturnStatus.MissingDate);
            else if (!int.TryParse(Request.Date, out _))
                result.Add(ReturnStatus.IllegalDate);
            if (Request.Path == null)
                result.Add($"{ReturnStatus.MissingPath}, {ReturnStatus.MissingResource}");

            return result.Count > 0 ? $"4 {string.Join(", ", result)}" : "";
        }

        protected override void DoMethod()
        {
            try
            {
                if (Request.Path != null && !Request.Path.Contains(Routes.Category)) throw new Exception();
                var id = int.Parse(Request.Path.Replace(Routes.Category, "")); 
                _response.Status = Category.Remove(id);
            }
            catch (Exception)
            {
                _response.Status = ReturnStatus.BadRequest;
            }
        }
    }
    
    public class Echo: AMethod
    {
        public Echo(Request request) : base(request)
        { }

        protected override string ErrorHandler()
        {
            List<string> result = new();

            if (Request.Date == null)
                result.Add(ReturnStatus.MissingDate);
            else if (!int.TryParse(Request.Date, out _))
                result.Add(ReturnStatus.IllegalDate);
            if (Request.Body == null)
                result.Add(ReturnStatus.MissingBody);
            
            return result.Count > 0 ? $"4 {string.Join(", ", result)}" : "";
        }

        protected override void DoMethod()
        {
            _response.Status = ReturnStatus.Ok;
            _response.Body = Request.Body!;
        }
    }
}
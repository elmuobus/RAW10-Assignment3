namespace Assignment3
{
    public static class Routes
    {
        public static string Categories => "/api/categories";
        public static string Category => "/api/categories/";
    }
    public static class ReturnStatus
    {
        public static string Ok => "1 Ok";
        public static string Created => "2 Created";
        public static string Updated => "3 Updated";
        public static string BadRequest => "4 Bad Request";
        public static string NotFound => "5 Not Found";
        public static string Error => "6 Error";
        public static string MissingMethod => "Missing Method";
        public static string IllegalMethod => "Illegal Method";
        public static string MissingDate => "Missing Date";
        public static string IllegalDate => "Illegal Date";
        public static string MissingPath => "Missing Path";
        public static string IllegalPath => "Illegal Path";
        public static string MissingBody => "Missing Body";
        public static string IllegalBody => "Illegal Body";
        public static string MissingResource => "Missing Resource";
    }
}
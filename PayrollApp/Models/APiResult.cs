namespace PayrollApp.Models
{
    public class APiResult<T>
    {

        public bool Success { get; set; }

        public string ErrorMsg { get; set; }

        public string WarningMsg { get; set; }


        public T Data { get; set; }

    }
}

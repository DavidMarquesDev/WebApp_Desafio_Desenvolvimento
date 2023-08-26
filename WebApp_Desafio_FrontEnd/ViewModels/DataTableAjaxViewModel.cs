namespace WebApp_Desafio_FrontEnd.ViewModels
{
    public class DataTableAjaxViewModel
    {
        public DataTableAjaxViewModel()
        {
            start = 0;
            length = 10;
        }

        public int start { get; set; }
        public int length { get; set; }
        public object data { get; set; }
        public int draw { get; internal set; }
        public int recordsTotal { get; internal set; }
        public int recordsFiltered { get; internal set; }
    }
}

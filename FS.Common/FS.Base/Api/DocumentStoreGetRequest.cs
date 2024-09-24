using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FS.Base.Api
{
    public class DocumentStoreGetRequest
    {
        public int PageNumber { get; set; }
        public int ItemCountPerPage { get; set; }
        public string OrderByColumnName { get; set; }
        public bool OrderByDescending { get; set; }
        public string SearchQuery { get; set; }

        public void ParseQueryString(Dictionary<string, string> queryString)
        {
            //Handle JQuery DataTable 1.10 Requests
            int intVal = 0;
            if (this.OrderByColumnName == null &&
                queryString.ContainsKey("order[0].column") &&
                queryString.ContainsKey("columns[" + queryString["order[0].column"].ToString() + "].data"))
            {
                this.OrderByColumnName = queryString["columns[" + queryString["order[0].column"].ToString() + "].data"].ToString();
            }
            if (queryString.ContainsKey("order[0].dir"))
            {
                if (queryString["order[0].dir"].ToString() == "asc")
                    this.OrderByDescending = false;
                else
                    this.OrderByDescending = true;
            }
            if (this.ItemCountPerPage == 0 && 
                queryString.ContainsKey("length") &&
                int.TryParse(queryString["length"].ToString(), out intVal) &&
                intVal > 0)
            { 
                this.ItemCountPerPage = intVal;
            }
            if (this.PageNumber == 0 &&
                queryString.ContainsKey("start") &&
                int.TryParse(queryString["start"].ToString(), out intVal) &&
                intVal > 0)
            {
                this.PageNumber = (int)(intVal / this.ItemCountPerPage) + 1;
            }  

            if (this.ItemCountPerPage == 0)
                this.ItemCountPerPage = 10;
            if (this.PageNumber == 0)
                this.PageNumber = 1;
            if (this.SearchQuery == null)
                this.SearchQuery = string.Empty;
            if (this.OrderByColumnName == null)
                this.OrderByColumnName = string.Empty;
        }
    }
}

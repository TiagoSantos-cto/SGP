using Microsoft.AspNetCore.Http;
using SGP.Util;
using System;
using System.Collections.Generic;
using System.Data;

namespace SGP.Models
{
    public class Dashboard
    {
        public double Total { get; set; }
       
        public string Status { get; set; }

        public IHttpContextAccessor HttpContextAccessor { get; set; }

        public Dashboard() { }

        public Dashboard(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        public List<Dashboard> RetornarDadosGraficoPie()
        {
            var lista = new List<Dashboard>();

            var sql = @$"SELECT count(R.Status) AS TOTAL,
                                R.STATUS AS STATUS
                         FROM requisicao R
                         WHERE R.VbEncerrada = '0'
                           AND R.VbCancelada = '0'
                         GROUP BY R.STATUS";

            var dal = new DAL();
            var dt = new DataTable();
            dt = dal.RetDataTable(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var item = new Dashboard {
                    Total = Convert.ToDouble(dt.Rows[i]["TOTAL"].ToString()),
                    Status = dt.Rows[i]["STATUS"].ToString()
                    };
                lista.Add(item);
            }

            return lista;
        }
    }
}

using SGP.Util;
using System;
using System.ComponentModel.DataAnnotations;

namespace SGP.Models
{
    public class HomeModel
    {
        public string LerNomeUsuario()
        {
            DAL objDAL = new DAL();
            var dt = objDAL.RetDataTable("select * from Pessoa");

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    return dt.Rows[0]["Nome"].ToString();
                }
            }

            return string.Empty;
        }
    }
}

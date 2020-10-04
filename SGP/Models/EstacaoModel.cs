using Microsoft.AspNetCore.Http;
using SGP.Util;
using System;
using System.Collections.Generic;

namespace SGP.Models
{
    public class EstacaoModel
    {
        public int  Id { get; set; }
        public string Descricao { get; set; }
        public string Tipo { get; set; }
 
        public IHttpContextAccessor HttpContextAccessor { get; set; }

        public EstacaoModel(){}

        public EstacaoModel(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        public void GravarEstacao()
        {
            string sql;

            if (Id == 0)
            {
                sql = $"INSERT INTO ESTACAO_EST  (DESCRICAO, TIPO) VALUES ('{Descricao}', {Tipo})";
            }
            else
            {
                sql = $"UPDATE  ESTACAO_EST SET DESCRICAO = '{Descricao}',  TIPO = {Tipo} WHERE ID = '{Id}'";
            }

            var dal = new DAL();
            dal.ExecutarComandoSQL(sql);
        }

        public void ExcluirEstacao(int id)
        {
            string sql = $"DELETE FROM ESTACAO_EST WHERE ID = {id}";
            var dal = new DAL();
            dal.ExecutarComandoSQL(sql);
        }

        public List<EstacaoModel> ListaEstacao()
        {
            var lista = new List<EstacaoModel>();

            var sql = $"SELECT ID, DESCRICAO, TIPO FROM ESTACAO_EST";
            var dal = new DAL();
            var dt = dal.RetDataTable(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var item = new EstacaoModel
                {
                    Id = Convert.ToInt32(dt.Rows[i]["ID"].ToString()),
                    Descricao = dt.Rows[i]["DESCRICAO"].ToString(),  
                    Tipo = dt.Rows[i]["TIPO"].ToString()
                };

                lista.Add(item);
            }

            return lista;
        }
    }
}

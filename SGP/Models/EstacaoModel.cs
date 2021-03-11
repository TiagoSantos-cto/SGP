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
                sql = $"insert into EstacaoTrabalho (Nome, Tipo) values ('{Descricao}', {Tipo})";
            }
            else
            {
                sql = $"update EstacaoTrabalho SET Nome = '{Descricao}',  Tipo = {Tipo} where IdEstacao = '{Id}'";
            }

            var dal = new DAL();
            dal.ExecutarComandoSQL(sql);
        }

        public void ExcluirEstacao(int id)
        {
            string sql = $"delete from EstacaoTrabalho where IdEstacao = {id}";
            var dal = new DAL();
            dal.ExecutarComandoSQL(sql);
        }

        public List<EstacaoModel> ListaEstacao()
        {
            var lista = new List<EstacaoModel>();

            var sql = $"select IdEstacao, Nome, Tipo from EstacaoTrabalho";
            var dal = new DAL();
            var dt = dal.RetDataTable(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var item = new EstacaoModel
                {
                    Id = Convert.ToInt32(dt.Rows[i]["IdEstacao"].ToString()),
                    Descricao = dt.Rows[i]["Nome"].ToString(),  
                    Tipo = dt.Rows[i]["Tipo"].ToString()
                };

                lista.Add(item);
            }

            return lista;
        }
    }
}

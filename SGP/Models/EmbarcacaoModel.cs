using Microsoft.AspNetCore.Http;
using SGP.Util;
using System;
using System.Collections.Generic;

namespace SGP.Models
{
    public class EmbarcacaoModel
    {
        #region PROPRIEDADES
       
        public string Id { get; set; }
        public string Nome { get; set; }
        public double Capacidade { get; set; }
        public double AreaCarga { get; set; }
        public int Acomodacao { get; set; }
        public string Categoria { get; set; }
        public IHttpContextAccessor HttpContextAccessor { get; set; }

        #endregion

        #region CONTRUTORES

        public EmbarcacaoModel() { }

        public EmbarcacaoModel(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region MÉTODOS

        public void GravarEmbarcacao()
        {
            string sql;

            if (!Existe(Id))
            {
                sql = $"insert into Embarcacao (IdEmbarcacao, Nome, Capacidade, AreaCarga, Acomodacao, Categoria) values ('{Id}','{Nome}', '{Capacidade}', '{AreaCarga}' , '{Acomodacao}' , '{Categoria}')";
            }
            else
            {
                sql = $"update  Embarcacao set Nome = '{Nome}', Capacidade = '{Capacidade}', AreaCarga = '{AreaCarga}', Acomodacao = '{Acomodacao}', Categoria = '{Categoria}' where IdEmbarcacao = '{Id}'";
            }

            var dal = new DAL();
            dal.ExecutarComandoSQL(sql);
        }

        public void ExcluirEmbarcacao(int id)
        {
            string sql = $"delete from Embarcacao where IdEmbarcacao = {id}";
            var dal = new DAL();
            dal.ExecutarComandoSQL(sql);
        }

        public List<EmbarcacaoModel> ListaEmbarcacao()
        {
            var lista = new List<EmbarcacaoModel>();

            var sql = $"select IdEmbarcacao, Nome, Capacidade, AreaCarga, Acomodacao, Categoria from Embarcacao";
            var dal = new DAL();
            var dt = dal.RetDataTable(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var item = new EmbarcacaoModel
                {
                    Id = dt.Rows[i]["IdEmbarcacao"].ToString(),
                    Nome = dt.Rows[i]["Nome"].ToString(),
                    Capacidade = Convert.ToDouble(dt.Rows[i]["Capacidade"].ToString()),
                    AreaCarga = Convert.ToDouble(dt.Rows[i]["AreaCarga"].ToString()),
                    Acomodacao = Convert.ToInt32(dt.Rows[i]["Acomodacao"].ToString()),
                    Categoria = dt.Rows[i]["Categoria"].ToString()
                };

                lista.Add(item);
            }

            return lista;
        }

        public bool Existe(string id)
        {
            var sql = $"select IdEmbarcacao from Embarcacao where IdEmbarcacao = '{id}'";
            var dal = new DAL();
            var dt = dal.RetDataTable(sql);

            return dt.Rows.Count > 0;
        }

        #endregion
    }
}

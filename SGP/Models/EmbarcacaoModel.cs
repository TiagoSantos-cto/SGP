using Microsoft.AspNetCore.Http;
using SGP.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace SGP.Models
{
    public class EmbarcacaoModel
    {

        #region ENUM

        public enum EnumCategoria
        {
            [Description("PSV")]
            PSV,
            [Description("AHTS")]
            AHTS,
            [Description("P")]
            P,
            [Description("UT")]
            UT
        }

        #endregion


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


        public EmbarcacaoModel CarregarRegistro(string id)
        {
            var sql = $@"SELECT IdEmbarcacao,
                                Nome,
                                Acomodacao,
                                AreaCarga,
                                Capacidade,
                                Categoria
                         FROM Embarcacao
                         WHERE IdEmbarcacao = '{id}'";

            var dal = new DAL();
            var dt = dal.RetDataTable(sql);

            var entity = new EmbarcacaoModel
            {
                Id = dt.Rows[0]["IdEmbarcacao"] != null ? dt.Rows[0]["IdEmbarcacao"].ToString() : string.Empty,
                Nome = dt.Rows[0]["Nome"] != null ? dt.Rows[0]["Nome"].ToString() : string.Empty,
                Acomodacao = dt.Rows[0]["Acomodacao"] != null ? Convert.ToInt32(dt.Rows[0]["Acomodacao"].ToString()) : 0,
                AreaCarga = dt.Rows[0]["AreaCarga"] != null ? Convert.ToDouble(dt.Rows[0]["AreaCarga"].ToString()) : 0,
                Capacidade = dt.Rows[0]["Capacidade"] != null ? Convert.ToDouble(dt.Rows[0]["Capacidade"].ToString()) : 0,
                Categoria = dt.Rows[0]["Categoria"] != null ? dt.Rows[0]["Categoria"].ToString() : string.Empty
            };

            return entity;
        }

        #endregion
    }
}

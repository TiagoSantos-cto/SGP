using Microsoft.AspNetCore.Http;
using SGP.Util;
using System;
using System.Collections.Generic;

namespace SGP.Models
{
    public class EmbarcacaoModel
    {
        public string  Id { get; set; }
        public string Nome { get; set; }
        public double Capacidade { get; set; }
        public double AreaCarga { get; set; }
        public int Acomodacao { get; set; }
        public string Categoria { get; set; }

        public IHttpContextAccessor HttpContextAccessor { get; set; }

        public EmbarcacaoModel(){}

        public EmbarcacaoModel(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        public void GravarEmbarcacao()
        {
            string sql;

            if (!Existe(Id))
            {
                sql = $"INSERT INTO EMBARCACAO_EMB (ID, NOME, TIPO, CAPACIDADE, AREACARGA, ACOMODACAO, CATEGORIA) VALUES ('{Id}','{Nome}', '{Capacidade}', '{AreaCarga}' , '{Acomodacao}' , '{Categoria}')";
            }
            else
            {
                sql = $"UPDATE  EMBARCACAO_EMB SET NOME = '{Nome}', CAPACIDADE = '{Capacidade}', AREACARGA = '{AreaCarga}', ACOMODACAO = '{Acomodacao}', CATEGORIA = '{Categoria}' WHERE ID = '{Id}'";
            }

            var dal = new DAL();
            dal.ExecutarComandoSQL(sql);
        }

        public void ExcluirEmbarcacao(int id)
        {
            string sql = $"DELETE FROM EMBARCACAO_EMB WHERE ID = {id}";
            var dal = new DAL();
            dal.ExecutarComandoSQL(sql);
        }

        public List<EmbarcacaoModel> ListaEmbarcacao()
        {
            var lista = new List<EmbarcacaoModel>();

            var sql = $"SELECT ID, NOME, CAPACIDADE, AREACARGA, ACOMODACAO, CATEGORIA FROM EMBARCACAO_EMB";
            var dal = new DAL();
            var dt = dal.RetDataTable(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var item = new EmbarcacaoModel
                {
                    Id = dt.Rows[i]["ID"].ToString(),
                    Nome = dt.Rows[i]["NOME"].ToString(),
                    Capacidade = Convert.ToDouble(dt.Rows[i]["CAPACIDADE"].ToString()),
                    AreaCarga = Convert.ToDouble(dt.Rows[i]["AREACARGA"].ToString()), 
                    Acomodacao = Convert.ToInt32(dt.Rows[i]["ACOMODACAO"].ToString()),
                    Categoria = dt.Rows[i]["CATEGORIA"].ToString()                
                };

                lista.Add(item);
            }

            return lista;
        }

        public bool Existe(string id)
        {
            var sql = $"SELECT ID FROM EMBARCACAO_EMB WHERE ID = '{id}'";
            var dal = new DAL();
            var dt = dal.RetDataTable(sql);

            return dt.Rows.Count > 0;
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SGP.Util;
using System;
using System.Collections.Generic;


namespace SGP.Models
{
    public class EquipamentoModel
    {
        public string  Id { get; set; }
        public string Descricao { get; set; }
        public string Status { get; set; }
        public int Estacao { get; set; }
        public int Quantidade { get; set; }
        public IFormFile Imagem { get; set; }
        public string ImagemPath { get; set; }



        public IHttpContextAccessor HttpContextAccessor { get; set; }

        public EquipamentoModel(){}

        public EquipamentoModel(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        public void GravarEquipamento()
        {
            string sql;

            if (!Existe(Id))
            {
                sql = $"INSERT INTO EQUIPAMENTO_EQP (ID, DESCRICAO, STATUS, ESTACAO_ID, QUANTIDADE, IMAGEM_PATH) VALUES ('{Id}','{Descricao}', '{Status}', '{Estacao}', '{ImagemPath}')";
            }
            else
            {
                sql = $"UPDATE EQUIPAMENTO_EQP SET DESCRICAO = '{Descricao}',  STATUS = '{Status}', ESTACAO_ID = '{Estacao}', QUANTIDADE = '{Quantidade}', IMAGEM_PATH = '{ImagemPath}' WHERE ID = '{Id}'";
            }

            var dal = new DAL();
            dal.ExecutarComandoSQL(sql);
        }

        public void ExcluirEquipamento(int id)
        {
            string sql = $"DELETE FROM EQUIPAMENTO_EQP WHERE ID = {id}";
            var dal = new DAL();
            dal.ExecutarComandoSQL(sql);
        }

        public EquipamentoModel CarregarRegistro(string id)
        {
            var sql = $"SELECT ID, DESCRICAO, ESTACAO_ID, STATUS, QUANTIDADE, IMAGEM_PATH FROM EQUIPAMENTO_EQP WHERE ID = '{id}'";
            var dal = new DAL();
            var dt = dal.RetDataTable(sql);

            var entity = new EquipamentoModel
            {
                Id = dt.Rows[0]["ID"] != null ? dt.Rows[0]["ID"].ToString() : string.Empty,
                Descricao = dt.Rows[0]["DESCRICAO"] != null ? dt.Rows[0]["DESCRICAO"].ToString() : string.Empty,
                Estacao = dt.Rows[0]["ESTACAO_ID"] != null ? Convert.ToInt32(dt.Rows[0]["ESTACAO_ID"].ToString()) : 0,
                Status = dt.Rows[0]["STATUS"] != null ? dt.Rows[0]["STATUS"].ToString() : string.Empty,
                Quantidade = dt.Rows[0]["QUANTIDADE"] != null ? Convert.ToInt32(dt.Rows[0]["QUANTIDADE"].ToString()) : 0,
                ImagemPath = dt.Rows[0]["IMAGEM_PATH"] != null ? dt.Rows[0]["IMAGEM_PATH"].ToString() : string.Empty,
            };
            return entity;
        }

        public List<EquipamentoModel> ListaEquipamento()
        {
            var lista = new List<EquipamentoModel>();

            var sql = $"SELECT ID, DESCRICAO, ESTACAO_ID, STATUS, QUANTIDADE, IMAGEM_PATH FROM EQUIPAMENTO_EQP";
            var dal = new DAL();
            var dt = dal.RetDataTable(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var item = new EquipamentoModel
                {
                    Id = dt.Rows[i]["ID"].ToString(),
                    Descricao = dt.Rows[i]["DESCRICAO"].ToString(),  
                    Status = dt.Rows[i]["STATUS"].ToString(),
                    Estacao = Convert.ToInt32(dt.Rows[i]["ESTACAO_ID"].ToString()),
                    Quantidade = Convert.ToInt32(dt.Rows[i]["QUANTIDADE"].ToString()),
                    ImagemPath = dt.Rows[i]["IMAGEM_PATH"].ToString()
                };

                lista.Add(item);
            }
            return lista;
        }

        public bool Existe(string id)
        {
            var sql = $"SELECT ID FROM EQUIPAMENTO_EQP WHERE ID = '{id}'";
            var dal = new DAL();
            var dt = dal.RetDataTable(sql);

            return dt.Rows.Count > 0;
        }
    }
}

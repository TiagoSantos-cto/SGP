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
                sql = $"insert into Equipamento (IdEquipamento, Tipo, Descricao, Preco, Status, Quantidade, Estacao, ImagemPath) values ('{Id}','DEFAULT', '{Descricao}', '0.00', {Quantidade}, '{Status}', '{Estacao}', '{ImagemPath}')";
            }
            else
            {
                sql = $"update Equipamento SET Descricao = '{Descricao}',  Status = '{Status}', Estacao = '{Estacao}', Quantidade = '{Quantidade}', ImagemPath = '{ImagemPath}' where IdEquipamento = '{Id}'";
            }

            var dal = new DAL();
            dal.ExecutarComandoSQL(sql);
        }

        public void ExcluirEquipamento(int id)
        {
            string sql = $"delete from Equipamento where IdEquipamento = {id}";
            var dal = new DAL();
            dal.ExecutarComandoSQL(sql);
        }

        public EquipamentoModel CarregarRegistro(string id)
        {
            var sql = $"select IdEquipamento, Descricao, Status, Quantidade, Estacao, ImagemPath from Equipamento WHERE IdEquipamento = '{id}'";
            var dal = new DAL();
            var dt = dal.RetDataTable(sql);

            var entity = new EquipamentoModel
            {
                Id = dt.Rows[0]["IdEquipamento"] != null ? dt.Rows[0]["IdEquipamento"].ToString() : string.Empty,
                Descricao = dt.Rows[0]["Descricao"] != null ? dt.Rows[0]["Descricao"].ToString() : string.Empty,
                Estacao = dt.Rows[0]["Estacao"] != null ? Convert.ToInt32(dt.Rows[0]["Estacao"].ToString()) : 0,
                Status = dt.Rows[0]["Status"] != null ? dt.Rows[0]["Status"].ToString() : string.Empty,
                Quantidade = dt.Rows[0]["Quantidade"] != null ? Convert.ToInt32(dt.Rows[0]["Quantidade"].ToString()) : 0,
                ImagemPath = dt.Rows[0]["ImagemPath"] != null ? dt.Rows[0]["ImagemPath"].ToString() : string.Empty,
            };
            return entity;
        }

        public List<EquipamentoModel> ListaEquipamento()
        {
            var lista = new List<EquipamentoModel>();

            var sql = $"select IdEquipamento, Descricao, Status, Quantidade, Estacao, ImagemPath from Equipamento";
            var dal = new DAL();
            var dt = dal.RetDataTable(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var item = new EquipamentoModel
                {
                    Id = dt.Rows[i]["IdEquipamento"].ToString(),
                    Descricao = dt.Rows[i]["Descricao"].ToString(),  
                    Status = dt.Rows[i]["Status"].ToString(),
                    Estacao = Convert.ToInt32(dt.Rows[i]["Estacao"].ToString()),
                    Quantidade = Convert.ToInt32(dt.Rows[i]["Quantidade"].ToString()),
                    ImagemPath = dt.Rows[i]["ImagemPath"].ToString()
                };

                lista.Add(item);
            }
            return lista;
        }

        public bool Existe(string id)
        {
            var sql = $"select IdEquipamento from Equipamento where IdEquipamento = '{id}'";
            var dal = new DAL();
            var dt = dal.RetDataTable(sql);

            return dt.Rows.Count > 0;
        }
    }
}

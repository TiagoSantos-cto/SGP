using Microsoft.AspNetCore.Http;
using SGP.Util;
using System;
using System.Collections.Generic;

namespace SGP.Models
{
    public class EquipamentoModel
    {
        public int  Id { get; set; }
        public string Descricao { get; set; }
        public string Status { get; set; }
        public int Estacao { get; set; }
        public IHttpContextAccessor HttpContextAccessor { get; set; }

        public EquipamentoModel(){}

        public EquipamentoModel(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        public void GravarEquipamento()
        {
            string sql;

            if (Id == 0)
            {
                sql = $"INSERT INTO EQUIPAMENTO_EQP (ID, DESCRICAO, STATUS, ESTACAO_ID) VALUES ('{Id}','{Descricao}', '{Status}', '{Estacao}')";
            }
            else
            {
                sql = $"UPDATE  EQUIPAMENTO_EQP SET DESCRICAO = '{Descricao}',  STATUS = '{Status}', ESTACAO_ID = '{Estacao}' WHERE ID = '{Id}'";
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

        public List<EquipamentoModel> ListaEquipamento()
        {
            var lista = new List<EquipamentoModel>();

            var sql = $"SELECT ID, DESCRICAO, STATUS, ESTACAO FROM EQUIPAMENTO_EQP";
            var dal = new DAL();
            var dt = dal.RetDataTable(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var item = new EquipamentoModel
                {
                    Id = Convert.ToInt32(dt.Rows[i]["ID"].ToString()),
                    Descricao = dt.Rows[i]["DESCRICAO"].ToString(),  
                    Status = dt.Rows[i]["STATUS"].ToString(),
                    Estacao = Convert.ToInt32(dt.Rows[i]["ESTACAO"].ToString())
                };

                lista.Add(item);
            }

            return lista;
        }
    }
}

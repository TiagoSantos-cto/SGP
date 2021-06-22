using SGP.Util;
using System;
using System.Collections.Generic;

namespace SGP.Models
{
    public class ItemRequisicaoModel
    {
        public int CodigoRequisicao { get; set; }
        public string CodigoEquipamento { get; set; }
        public int Quantidade { get; set; }

        public List<ItemRequisicaoModel> ObterItensPorRequisicao(int id)
        {
            var sql = $@"SELECT Id_Equipamento, 
                                Quantidade                            
                           FROM Requsicao_item
                         WHERE Id_Requisicao = '{id}'";

            var lista = new List<ItemRequisicaoModel>();

            var dal = new DAL();
            var dt = dal.RetDataTable(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var entity = new ItemRequisicaoModel
                {
                    CodigoEquipamento = dt.Rows[0]["Id_Equipamento"] != null ? dt.Rows[0]["Id_Equipamento"].ToString() : string.Empty,
                    Quantidade = dt.Rows[0]["Quantidade"] != null ? Convert.ToInt32(dt.Rows[0]["Quantidade"].ToString()) : 0,
                    CodigoRequisicao = id
                };

                lista.Add(entity);
            }

            return lista;
        }
    }
}

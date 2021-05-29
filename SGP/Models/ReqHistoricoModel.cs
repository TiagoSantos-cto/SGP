using Microsoft.AspNetCore.Http;
using SGP.Util;
using System;
using System.Collections.Generic;

namespace SGP.Models
{
    public class ReqHistoricoModel
    {
        #region PROPRIEDADES

        public string Id { get; set; }
        public string Descricao { get; set; }
        public int IdUsuario { get; set; }
        public string NomeUsuario { get; set; }
        public string DataAlteracao { get; set; }
        public int IdRequisicao { get; set; }
        public IHttpContextAccessor HttpContextAccessor { get; set; }

        #endregion

        #region CONTRUTORES

        public ReqHistoricoModel() { }

        public ReqHistoricoModel(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region MÉTODOS

        public void GravarReqHistorico()
        {
            string sql = $@"INSERT INTO Historico (Descricao, IdUsuario, DataAlteracao, Id_Requisicao)
                     VALUES('{Descricao}', '{IdUsuario}', '{Convert.ToDateTime(DateTime.Now):yyyy/MM/dd}', '{IdRequisicao}')";

            var dal = new DAL();
            dal.ExecutarComandoSQL(sql);
        }

        public List<ReqHistoricoModel> ListaReqHistorico(int idRequisicao)
        {
            var lista = new List<ReqHistoricoModel>();

            var sql = $@"SELECT H.Descricao,
                                H.IdUsuario,
                                U.Login AS NomeUsuario,
                                H.DataAlteracao
                         FROM Historico H,
                              usuario U
                         WHERE U.IdUsuario = H.IdUsuario
                           AND H.Id_Requisicao ='{idRequisicao}'";

            var dal = new DAL();
            var dt = dal.RetDataTable(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var item = new ReqHistoricoModel();
                item.IdRequisicao = idRequisicao;
                item.Descricao = dt.Rows[i]["Descricao"].ToString();
                item.IdUsuario = Convert.ToInt32(dt.Rows[i]["IdUsuario"].ToString());
                item.NomeUsuario = dt.Rows[i]["NomeUsuario"].ToString();
                item.DataAlteracao = dt.Rows[i]["DataAlteracao"].ToString();

                lista.Add(item);
            }

            return lista;
        }

        #endregion
    }
}

            
using Microsoft.AspNetCore.Http;
using SGP.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace SGP.Models
{
    public class EntregaModel
    {
        #region ENUM

        public enum StatusEntrega
        {
            [Description("Em processamento")]
            Processamento,
            [Description("Em trânsito")]
            Transito,
            [Description("Encerrada")]
            Encerrada
        }

        #endregion

        #region PROPRIEDADES

        public int IdEntrega { get; set; }
        public int IdRequisicao { get; set; }
        public string Status { get; set; }
        public int IdEmbarcacao { get; set; }
        public string NomeEmbarcacao { get; set; }
        public string DataMaisCedo { get; set; }
        public string DataMaisTarde { get; set; }
        public IHttpContextAccessor HttpContextAccessor { get; set; }

        #endregion

        #region CONSTRUTORES

        public EntregaModel() { }

        public EntregaModel(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region MÉTODOS

        public EntregaModel CarregarRegistro(int? idRequisicao)
        {
            var id = idRequisicao != null ? idRequisicao.ToString() : string.Empty;

            var sql = @$"SELECT E.IdEntrega AS ID,
                                E.DataMaisTarde AS DTTARDE,
                                E.Status AS STATUS,
                                ER.Id_Requisicao AS ID_REQUISICAO,
                                EB.IdEmbarcacao AS ID_EMBARCACAO,
                                EB.Nome AS NOME_EMBARCACAO
                         FROM entrega E,
                              requisicao_entrega ER,
                              requisicao r,
                              embarcacao EB
                         WHERE E.IdEntrega = ER.Id_Entrega
                           AND ER.Id_Entrega = R.IdRequisicao
                           AND ER.Id_Requisicao  = '{id}'";

            var dal = new DAL();
            var dt = dal.RetDataTable(sql);

            var entity = new EntregaModel();

            if (dt.Rows.Count > 0)
            {
                var obj = new EntregaModel
                {
                    IdEntrega = dt.Rows[0]["ID"] != null ? Convert.ToInt32(dt.Rows[0]["ID"].ToString()) : 0,
                    IdRequisicao = dt.Rows[0]["ID_REQUISICAO"] != null ? Convert.ToInt32(dt.Rows[0]["ID_REQUISICAO"].ToString()) : (int)idRequisicao,
                    DataMaisCedo = string.Empty,
                    DataMaisTarde = dt.Rows[0]["DTTARDE"] != null ? Convert.ToDateTime(dt.Rows[0]["DTTARDE"].ToString()).ToString("dd/MM/yyyy") : string.Empty,
                    Status = dt.Rows[0]["STATUS"] != null ? dt.Rows[0]["STATUS"].ToString() : string.Empty,
                    IdEmbarcacao = dt.Rows[0]["ID_EMBARCACAO"] != null ? Convert.ToInt32(dt.Rows[0]["ID_EMBARCACAO"].ToString()) : 0,
                    NomeEmbarcacao = dt.Rows[0]["NOME_EMBARCACAO"] != null ? dt.Rows[0]["NOME_EMBARCACAO"].ToString() : string.Empty
                };

                if (obj.Status == StatusEntrega.Processamento.ToString())
                {
                    obj.Status = StatusEntrega.Processamento.GetDescription();
                }
                else if (obj.Status == StatusEntrega.Transito.ToString())
                {
                    obj.Status = StatusEntrega.Transito.GetDescription();
                }
                else
                {
                    obj.Status = StatusEntrega.Encerrada.GetDescription();
                }
                
                entity = obj;
            }
            else
            {
                entity.IdRequisicao = idRequisicao != null ? (int)idRequisicao : 0;
                entity.DataMaisTarde = DateTime.Now.ToString();
            }

            return entity;
        }

        public void GravarEntrega()
        {
            string sqlEntrega = string.Empty;
            string sqlEmbarcacaoEntrega = string.Empty;
            string sqlEntregaRequisicao = string.Empty;

            var dal = new DAL();

            if (Status == StatusEntrega.Processamento.GetDescription())
            {
                Status = StatusEntrega.Processamento.ToString();
            }
            else if (Status == StatusEntrega.Transito.GetDescription())
            {
                Status = StatusEntrega.Transito.ToString();
            }
            else
            {
                Status = StatusEntrega.Encerrada.ToString();
            }


            if (!Existe(IdEntrega, IdRequisicao))
            {
                IdEntrega = GerarSequencial();

                sqlEntrega = $@"INSERT INTO Entrega (IdEntrega, Status, DataMaisTarde, Id_Embarcacao)
                                VALUES('{IdEntrega}', '{Status}', '{Convert.ToDateTime(DataMaisTarde)}', '{IdEmbarcacao}')";

                //sqlEmbarcacaoEntrega = $@"INSERT INTO EmbarcacaoEntrega (Id_Embarcacao, Id_Entrega)
                //                          VALUES('{IdEmbarcacao}', '{IdEntrega}')";

                sqlEntregaRequisicao = $@"INSERT INTO requisicao_entrega (Id_Entrega, Id_Requisicao)
                                          VALUES('{IdEntrega}', '{IdRequisicao}')";

            }
            else
            {
                sqlEntrega = $@"UPDATE Entrega
                                SET Status = '{Status}',                                   
                                    DataMaisTarde = '{Convert.ToDateTime(DataMaisTarde):yyyy/MM/dd}'
                                WHERE IdEntrega = '{IdEntrega}'";

                //sqlEmbarcacaoEntrega = $@"UPDATE embarcacaoentrega
                //                          SET Id_Embarcacao = '{IdEmbarcacao}'
                //                          WHERE Id_Entrega = '{IdEntrega}'";
            }

            dal.ExecutarComandoSQL(sqlEntrega);
            //dal.ExecutarComandoSQL(sqlEmbarcacaoEntrega);

            if (!string.IsNullOrEmpty(sqlEntregaRequisicao))
            {
                dal.ExecutarComandoSQL(sqlEntregaRequisicao);
            }

            var requisicaoModel = new RequisicaoModel();
            requisicaoModel.SincorinizarEntrega(IdRequisicao, IdEntrega, Status);
        }

        private int GerarSequencial()
        {
            var sequencial = 0;

            var entregas = ListaEntrega();

            var listaID = new List<int>();

            if (entregas.Count > 0)
            {
                foreach (var item in entregas)
                {
                    listaID.Add(item.IdEntrega);
                };

                listaID.Sort();

                sequencial = listaID[listaID.Count - 1];
            }

            return sequencial + 1;
        }

        public List<EntregaModel> ListaEntrega()
        {
            var lista = new List<EntregaModel>();

            var sql = $@"SELECT IdEntrega
                         FROM Entrega";

            var dal = new DAL();
            var dt = dal.RetDataTable(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var item = new EntregaModel
                {
                    IdEntrega = Convert.ToInt32(dt.Rows[i]["IdEntrega"].ToString())
                };

                lista.Add(item);
            }
            return lista;
        }

        public bool Existe(int idEntrega, int idRequisicao)
        {
            var sql = $@"SELECT *
                         FROM requisicao_entrega
                         WHERE Id_Entrega = '{idEntrega}'
                           AND Id_Requisicao = '{idRequisicao}'";

            var dal = new DAL();
            var dt = dal.RetDataTable(sql);

            return dt.Rows.Count > 0;
        }

        #endregion
    }
}

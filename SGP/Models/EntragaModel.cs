using Microsoft.AspNetCore.Http;
using SGP.Util;
using System;
using System.Collections.Generic;

namespace SGP.Models
{
    public class EntregaModel
    {
        public int IdEntrega { get; set; }
        public int IdRequisicao { get; set; }
        public string Status { get; set; }
        public int IdEmbarcacao { get; set; }
        public string NomeEmbarcacao { get; set; }
        public string DataMaisCedo { get; set; }
        public string DataMaisTarde { get; set; }

        public IHttpContextAccessor HttpContextAccessor { get; set; }

        public EntregaModel() { }

        public EntregaModel(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        public EntregaModel CarregarRegistro(int? idRequisicao)
        {
            var id = idRequisicao != null ? idRequisicao.ToString() : string.Empty;

            var sql = @$"SELECT E.IdEntrega AS ID,
                               E.DataMaisCedo AS DTCEDO,
                               E.DataMaisTarde AS DTTARDE,
                               E.Status AS STATUS,
                               ER.Id_Requisicao AS ID_REQUISICAO,
                               EE.Id_Embarcacao AS ID_EMBARCACAO
                        FROM entrega E,
                             entregarequisicao ER,
                             embarcacaoentrega EE
                        WHERE E.IdEntrega = ER.Id_Entrega
                          AND EE.Id_Entrega = E.IdEntrega
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
                    DataMaisCedo = dt.Rows[0]["DTCEDO"] != null ? Convert.ToDateTime(dt.Rows[0]["DTCEDO"].ToString()).ToString("dd/MM/yyyy") : string.Empty,
                    DataMaisTarde = dt.Rows[0]["DTTARDE"] != null ? Convert.ToDateTime(dt.Rows[0]["DTTARDE"].ToString()).ToString("dd/MM/yyyy") : string.Empty,
                    Status = dt.Rows[0]["STATUS"] != null ? dt.Rows[0]["STATUS"].ToString() : string.Empty,
                    IdEmbarcacao = dt.Rows[0]["ID_EMBARCACAO"] != null ? Convert.ToInt32(dt.Rows[0]["ID_EMBARCACAO"].ToString()) : 0,
                    NomeEmbarcacao = "NomeEmbarcação"
                };
               
                entity = obj;
            }
            else
            {
                entity.IdRequisicao = idRequisicao != null ? (int)idRequisicao : 0;
            }

            return entity;
        }

        public void GravarEntrega()
        {
            string sqlEntrega;
            string sqlEmbarcacaoEntrega = string.Empty;
            string sqlEntregaRequisicao = string.Empty;
                   
            if (!Existe(IdEntrega))
            {
                IdEntrega = GerarSequencial();
                sqlEntrega = $"insert into Entrega (IdEntrega, Status, DataMaisCedo, DataMaisTarde) values ('{IdEntrega}', '{Status}', '{DataMaisCedo}', '{DataMaisTarde}')";
                sqlEmbarcacaoEntrega = $"insert into EmbarcacaoEntrega (Id_Embarcacao, Id_Entrega) values ('{IdEmbarcacao}', '{IdEntrega}')";
                sqlEntregaRequisicao = $"insert into EntregaRequisicao (Id_Entrega, Id_Requisicao) values ('{IdEntrega}', '{IdRequisicao}')";
            }
            else
            {
                sqlEntrega = $"update Entrega set Status = '{Status}', DataMaisCedo = '{DataMaisCedo}', DataMaisTarde = '{DataMaisTarde}'  where IdEntrega = '{IdEntrega}'";
            }

            var dal = new DAL();
            dal.ExecutarComandoSQL(sqlEntrega);
            dal.ExecutarComandoSQL(sqlEntregaRequisicao);
            dal.ExecutarComandoSQL(sqlEmbarcacaoEntrega);       
        }

        private int GerarSequencial()
        {
            var sequencial = 0;

            var entregas = ListaEntrega();

            var listaID = new List<int>();
            
            if (entregas.Count >0 )
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

            var sql = $"select IdEntrega from Entrega";
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

        public bool Existe(int id)
        {
            var sql = $"select IdEntrega from Entrega where IdEntrega = '{id}'";
            var dal = new DAL();
            var dt = dal.RetDataTable(sql);

            return dt.Rows.Count > 0;
        }
    }
}

using Microsoft.AspNetCore.Http;
using SGP.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SGP.Models
{
    public class RequisicaoModel
    {
        public enum StatusRequisicao
        {
            Solicitado,
            Liberado,
            EmColeta,
            EmProcessamento,
            Cancelado,
            Programado
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "O campo data é obrigatório.")]
        public string Data { get; set; }

        public string DataFinal { get; set; }

        [Required(ErrorMessage = "O campo descrição é obrigatório.")]
        public string Descricao { get; set; }

        public string Tipo { get; set; }

        public string NomeUsuario { get; set; }

        public string Status { get; set; }

        public string Origem { get; set; }

        public string Destino { get; set; }

        public IList<ItemRequisicaoModel> ItensRequisicao { get; set; }


        public IHttpContextAccessor HttpContextAccessor { get; set; }

        public RequisicaoModel() 
        {
            ItensRequisicao = new List<ItemRequisicaoModel>();
        }

        public RequisicaoModel(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        private string IdUsuarioLogado()
        {
            return HttpContextAccessor.HttpContext.Session.GetString("IdUsuarioLogado");
        }

        public List<RequisicaoModel> ListaRequisicao()
        {
            var lista = new List<RequisicaoModel>();

            var filtro = string.Empty;

            if ((Data != null) && (DataFinal != null))
            {
                filtro += $"AND R.DATA_INCLUSAO >='{Convert.ToDateTime(Data):yyyy/MM/dd}' AND R.DATA_INCLUSAO <= '{Convert.ToDateTime(DataFinal):yyyy/MM/dd}'";
            }

            if (Status != null)
            {
                if (Status != "Todos")
                {
                    filtro += $" AND R.STATUS = '{Status}'";
                }
            }

            if (Tipo != null)
            {
                if (Tipo != "A")
                {
                    filtro += $" AND R.TIPO = '{Tipo}'";
                }
            }

            if (NomeUsuario != null)
            {
                if (!string.IsNullOrEmpty(NomeUsuario))
                {
                    filtro += $" AND U.NOME = '{NomeUsuario}'";
                }
            }

            var sql = @"SELECT R.ID, R.DATA_INCLUSAO AS DATA, U.NOME AS USUARIO, R.STATUS, R.DESCRICAO, R.TIPO, R.ORIGEM, R.DESTINO
                        FROM REQUISICAO_REQ  AS R INNER JOIN USUARIO_USO U 
                        ON R.USUARIO_ID = U.ID "
                        + $" WHERE 1 = 1  {filtro} ORDER BY R.DATA_INCLUSAO DESC LIMIT 10 ";


            var dal = new DAL();
            var dt = dal.RetDataTable(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var item = new RequisicaoModel
                {
                    Id = dt.Rows[i]["ID"] != null ? Convert.ToInt32(dt.Rows[i]["ID"].ToString()) : 0,
                    Descricao = dt.Rows[i]["DESCRICAO"] != null ? dt.Rows[i]["DESCRICAO"].ToString() : string.Empty,
                    Tipo = dt.Rows[i]["TIPO"] != null ? dt.Rows[i]["TIPO"].ToString() : string.Empty,
                    NomeUsuario = dt.Rows[i]["USUARIO"] != null ? dt.Rows[i]["USUARIO"].ToString() : string.Empty,
                    Data = dt.Rows[i]["DATA"] != null ? Convert.ToDateTime(dt.Rows[i]["DATA"].ToString()).ToString("dd/MM/yyyy") : string.Empty,
                    Status = dt.Rows[i]["STATUS"] != null ? dt.Rows[i]["STATUS"].ToString() : string.Empty,
                    Origem = dt.Rows[i]["ORIGEM"] != null ? dt.Rows[i]["ORIGEM"].ToString() : string.Empty,
                    Destino = dt.Rows[i]["DESTINO"] != null ? dt.Rows[i]["DESTINO"].ToString() : string.Empty
                };

                lista.Add(item);
            }

            return lista;
        }

        public RequisicaoModel CarregarRegistro(int? id)
        {
            var sql = @"SELECT R.ID, R.DATA_INCLUSAO, U.NOME AS USUARIO, R.STATUS, R.DESCRICAO, R.TIPO, R.ORIGEM, R.DESTINO
                        FROM REQUISICAO_REQ  AS R INNER JOIN USUARIO_USO U 
                        ON R.USUARIO_ID = U.ID "
                      + $"AND R.ID = '{id}'";

            var dal = new DAL();
            var dt = dal.RetDataTable(sql);

            var entity = new RequisicaoModel
            {
                Id = dt.Rows[0]["ID"] != null ? Convert.ToInt32(dt.Rows[0]["ID"].ToString()) : 0,
                Descricao = dt.Rows[0]["DESCRICAO"] != null ? dt.Rows[0]["DESCRICAO"].ToString() : string.Empty,
                Tipo = dt.Rows[0]["TIPO"] != null ? dt.Rows[0]["TIPO"].ToString() : string.Empty,
                NomeUsuario = dt.Rows[0]["USUARIO"] != null ? dt.Rows[0]["USUARIO"].ToString() : string.Empty,
                Data = dt.Rows[0]["DATA_INCLUSAO"] != null ? Convert.ToDateTime(dt.Rows[0]["DATA_INCLUSAO"].ToString()).ToString("dd/MM/yyyy") : string.Empty,
                Status = dt.Rows[0]["STATUS"] != null ? dt.Rows[0]["STATUS"].ToString() : string.Empty,
                Origem = dt.Rows[0]["ORIGEM"] != null ? dt.Rows[0]["ORIGEM"].ToString() : string.Empty,
                Destino = dt.Rows[0]["DESTINO"] != null ? dt.Rows[0]["DESTINO"].ToString() : string.Empty
            };

            return entity;
        }

        public void Gravar()
        {
            string sql;          
            var dal = new DAL();

            if (Id == 0)
            {
                sql = "INSERT INTO REQUISICAO_REQ (DESCRICAO, DATA, TIPO, STATUS, ORIGEM, DESTINO, USUARIO_ID, USUARIO_ALTERACAO, DATA_ALTERACAO) VALUES " +
                    $" ('{Convert.ToDateTime(Data):yyyy/MM/dd}', '{Tipo}', '{Descricao}', '{Tipo}' ,'{Status}', '{Origem}', '{Destino}', '{IdUsuarioLogado()}', '', '')";
            }
            else
            {
                sql = $"UPDATE  REQUISICAO_REQ SET DATA_INCLUSAO = '{Convert.ToDateTime(Data):yyyy/MM/dd}', " +
                      $"DESCRICAO = '{Descricao}',  TIPO = '{Tipo}', STATUS = '{Status}', ORIGEM ='{Origem}'," +
                      $"DESTINO = '{Destino}', USUARIO_ALTERACAO ='{IdUsuarioLogado()}', DATA_ALTERACAO = '{Convert.ToDateTime(DateTime.Now):yyyy/MM/dd}' WHERE ID = '{Id}'";
            }

            GravarLista();

            dal.ExecutarComandoSQL(sql);
        }

        private void GravarLista()
        {
            string sqlListaItens;
            string deletaLista;

            var dal = new DAL();

            deletaLista = $"DELETE FROM ITEMREQUISICAO_ITR WHERE REQUISICAO_ID = '{Id}'";
            dal.ExecutarComandoSQL(deletaLista);

            foreach (var item in ItensRequisicao)
            {
                sqlListaItens = $"INSERT INTO ITEMREQUISICAO_ITR (EQUIPAMETO_ID, REQUISICAO_ID, QUANTIDADE) VALUES ('{item.CodigoEquipamento}','{item.CodigoRequisicao}','{item.Quantidade}')";
                dal.ExecutarComandoSQL(sqlListaItens);
            }
        }

        public void Excluir(int id)
        {
            string sql = $"DELETE FROM REQUISICAO_REQ WHERE ID = {id}";
            var dal = new DAL();
            dal.ExecutarComandoSQL(sql);
        }


        public IList<EquipamentoModel> ListaItem()
        {
            var equipamentoModel = new EquipamentoModel();
            var lista = equipamentoModel.ListaEquipamento();
    
            return lista;
        }

        public IList<ItemRequisicaoModel> CarregarItens(string idEquipamento, int Quantidade)
        {       
            var item = new ItemRequisicaoModel
            {
                CodigoRequisicao = Id,
                CodigoEquipamento = idEquipamento,
                Quantidade = Quantidade
            };

            ItensRequisicao.Add(item);
            return ItensRequisicao;
        }
    }
}

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

        public int UsuarioAtual { get; set; }

        public string NomeUsuarioAtual { get; set; }

        public int UsuarioResponsavel { get; set; }

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
                filtro += $"AND R.DataInclusao >='{Convert.ToDateTime(Data):yyyy/MM/dd}' AND R.DataInclusao <= '{Convert.ToDateTime(DataFinal):yyyy/MM/dd}'";
            }

            if (Status != null)
            {
                if (Status != "Todos")
                {
                    filtro += $" AND R.Status = '{Status}'";
                }
            }

            if (Tipo != null)
            {
                if (Tipo != "A")
                {
                    filtro += $" AND R.Tipo = '{Tipo}'";
                }
            }

            if (UsuarioAtual > 0)
            {
                filtro += $" AND R.UsuarioAtual = '{UsuarioAtual}'";
            }

            var sql = $@"SELECT R.IdRequisicao AS ID,
                                R.DataInclusao AS DATA,
                                R.UsuarioAtual AS USUARIO,
                                (SELECT U.Login  FROM usuario U WHERE U.IdUsuario = R.UsuarioAtual) AS NOME_USUARIO_ATUAL,
                                R.Status AS STATUS,
                                R.Descricao AS DESCRICAO,
                                R.Tipo AS TIPO,
                                R.Origem AS ORIGEM,
                                R.Destino AS DESTINO
                         FROM requisicao R,
                              funcionario F,
                              usuario U
                         WHERE  U.IdUsuario = R.UsuarioAtual
                         AND U.Id_Funcionario = F.IdFuncionario
                         {filtro}
                         ORDER BY DATA DESC
                         LIMIT 10";

            var dal = new DAL();
            var dt = dal.RetDataTable(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var item = new RequisicaoModel
                {
                    Id = dt.Rows[i]["ID"] != null ? Convert.ToInt32(dt.Rows[i]["ID"].ToString()) : 0,
                    Descricao = dt.Rows[i]["DESCRICAO"] != null ? dt.Rows[i]["DESCRICAO"].ToString() : string.Empty,
                    Tipo = dt.Rows[i]["TIPO"] != null ? dt.Rows[i]["TIPO"].ToString() : string.Empty,
                    UsuarioAtual = dt.Rows[i]["USUARIO"] != null ? Convert.ToInt32(dt.Rows[i]["USUARIO"].ToString()) : 0,
                    NomeUsuarioAtual = dt.Rows[i]["NOME_USUARIO_ATUAL"] != null ? dt.Rows[i]["NOME_USUARIO_ATUAL"].ToString() : string.Empty,
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
            var sql = $@"SELECT R.IdRequisicao AS ID,
                                R.DataInclusao AS DATA_INCLUSAO,
                                R.UsuarioAtual AS USUARIO,
                                (SELECT U.Login  FROM usuario U WHERE U.IdUsuario = R.UsuarioAtual) AS NOME_USUARIO_ATUAL,
                                R.Status AS STATUS,
                                R.Descricao AS DESCRICAO,
                                R.Tipo AS TIPO,
                                R.Origem AS ORIGEM,
                                R.Destino AS DESTINO
                         FROM requisicao R,
                              funcionario F,
                              usuario U
                         WHERE  U.IdUsuario = R.UsuarioAtual
                         AND U.Id_Funcionario = F.IdFuncionario
                         AND R.IdRequisicao = '{id}'";

            var dal = new DAL();
            var dt = dal.RetDataTable(sql);

            var entity = new RequisicaoModel
            {
                Id = dt.Rows[0]["ID"] != null ? Convert.ToInt32(dt.Rows[0]["ID"].ToString()) : 0,
                Descricao = dt.Rows[0]["DESCRICAO"] != null ? dt.Rows[0]["DESCRICAO"].ToString() : string.Empty,
                Tipo = dt.Rows[0]["TIPO"] != null ? dt.Rows[0]["TIPO"].ToString() : string.Empty,
                UsuarioAtual = dt.Rows[0]["USUARIO"] != null ? Convert.ToInt32(dt.Rows[0]["USUARIO"].ToString()) : 0,
                NomeUsuarioAtual = dt.Rows[0]["NOME_USUARIO_ATUAL"] != null ? dt.Rows[0]["NOME_USUARIO_ATUAL"].ToString() : string.Empty,
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
                sql = "INSERT INTO Requisicao (Descricao, Tipo, Origem, Status, Destino, UsuarioAtual, DataInclusao, UsuarioInclusao, DataAlteracao, UsuarioAlteracao) VALUES " +
                    $" ('{Descricao}', '{Tipo}' ,'{Origem}', '{Status}', '{Destino}', '{UsuarioAtual}','{Convert.ToDateTime(Data):yyyy/MM/dd}', '{IdUsuarioLogado()}', '', '')";
            }
            else
            {
                sql = $"UPDATE  Requisicao SET DataAlteracao = '{Convert.ToDateTime(Data):yyyy/MM/dd}', " +
                      $"DESCRICAO = '{Descricao}',  TIPO = '{Tipo}', STATUS = '{Status}', ORIGEM ='{Origem}'," +
                      $"DESTINO = '{Destino}', UsuarioAtual = '{UsuarioAtual}', USUARIO_ALTERACAO ='{IdUsuarioLogado()}', WHERE IdRequisicao = '{Id}'";
            }

            GravarLista();

            dal.ExecutarComandoSQL(sql);
        }

        private void GravarLista()
        {
            string sqlListaItens;

            var dal = new DAL();

            if (Existe(Id))
            {
                ExcluirListaItens(dal, Id);
            }

            foreach (var item in ItensRequisicao)
            {
                sqlListaItens = $"INSERT INTO itemrequisicao (Quantidade, Descricao, Id_Equipamento, Id_Requisicao) VALUES ('{item.Quantidade}', '','{item.CodigoEquipamento}','{item.CodigoRequisicao}')";
                dal.ExecutarComandoSQL(sqlListaItens);
            }
        }

        private void ExcluirListaItens(DAL dal, int id)
        {
            string deletaLista = $"DELETE FROM itemrequisicao WHERE Id_Requisicao = '{id}'";
            dal.ExecutarComandoSQL(deletaLista);
        }

        private bool Existe(int id)
        {
            var sql = $"SELECT Id_Requisicao from itemrequisicao where Id_Requisicao = '{id}'";
            var dal = new DAL();
            var dt = dal.RetDataTable(sql);

            return dt.Rows.Count > 0;
        }

        public void Excluir(int id)
        {
            var dal = new DAL();

            if (Existe(id))
            {
                ExcluirListaItens(dal, id);
            }

            string sql = $"DELETE FROM Requisicao WHERE IdRequisicao = {id}";
            dal.ExecutarComandoSQL(sql);
        }


        public IList<EquipamentoModel> ListaItem()
        {
            var equipamentoModel = new EquipamentoModel();
            var lista = equipamentoModel.ListaEquipamento();

            return lista;
        }

        public IList<ItemRequisicaoModel> ListaItens(string idEquipamento, int Quantidade)
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

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

            if (NomeUsuario != null)
            {
                if (!string.IsNullOrEmpty(NomeUsuario))
                {
                    filtro += $" AND U.Nome = '{NomeUsuario}'";
                }
            }

            var sql = $@"SELECT R.IdRequisicao as ID, R.DataInclusao as DATA, P.Nome as USUARIO, R.Status as STATUS, R.Descricao as DESCRICAO, R.Tipo as TIPO, R.Origem as ORIGEM, R.Destino as DESTINO
                        FROM Requisicao R, Funcionario F, Pessoa P
                        WHERE R.Id_Funcionario = F.IdFuncionario
                        AND P.IdPessoa = F.Id_Pessoa
                        {filtro} 
                        ORDER BY DATA DESC LIMIT 10";


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
            var sql = $@"SELECT R.IdRequisicao as ID, R.DataInclusao as DATA, P.Nome as USUARIO, R.Status as STATUS, R.Descricao as DESCRICAO, R.Tipo as TIPO, R.Origem as ORIGEM, R.Destino as DESTINO
                        FROM Requisicao R, Funcionario F, Pessoa P
                        WHERE R.Id_Funcionario = F.IdFuncionario
                        AND P.IdPessoa = F.Id_Pessoa 
                        AND R.IdRequisicao = '{id}'";     
            
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
                sql = "INSERT INTO Requisicao (IdFuncionario, Descricao, Tipo, Origem, Status, Destino, DataInclusao, UsuarioInclusao, DataAlteracao, UsuarioAlteracao) VALUES " +
                    $" ('{IdUsuarioLogado()}', '{Descricao}', '{Tipo}' ,'{Origem}', '{Status}', '{Destino}','{Convert.ToDateTime(Data):yyyy/MM/dd}', '{IdUsuarioLogado()}', '', '')";
            }
            else
            {
                sql = $"UPDATE  Requisicao SET DataAlteracao = '{Convert.ToDateTime(Data):yyyy/MM/dd}', " +
                      $"DESCRICAO = '{Descricao}',  TIPO = '{Tipo}', STATUS = '{Status}', ORIGEM ='{Origem}'," +
                      $"DESTINO = '{Destino}', USUARIO_ALTERACAO ='{IdUsuarioLogado()}', WHERE IdRequisicao = '{Id}'";
            }

            GravarLista();

            dal.ExecutarComandoSQL(sql);
        }

        private void GravarLista()
        {
            string sqlListaItens;
            string deletaLista;

            var dal = new DAL();

            deletaLista = $"DELETE FROM Requsicao_item WHERE IdRequisicao = '{Id}'";
            dal.ExecutarComandoSQL(deletaLista);

            foreach (var item in ItensRequisicao)
            {
                sqlListaItens = $"INSERT INTO Requsicao_item (IdItemRequisicao, IdRequisicao, Quantidade) VALUES ('{item.CodigoEquipamento}','{item.CodigoRequisicao}','{item.Quantidade}')";
                dal.ExecutarComandoSQL(sqlListaItens);
            }
        }

        public void Excluir(int id)
        {
            string sql = $"DELETE FROM Requisicao WHERE IdRequisicao = {id}";
            var dal = new DAL();
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

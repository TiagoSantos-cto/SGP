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
            Solicitar,
            Liberar,
            Coletar,
            Processar,
            Cancelar,
            Programar
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

        public string NomeUsuarioResponsavel { get; set; }

        public string Status { get; set; }

        public string Origem { get; set; }

        public string NomeEstacaoOrigem { get; set; }

        public string Destino { get; set; }

        public string NomeEstacaoDestino { get; set; }

        public IList<ItemRequisicaoModel> ItensRequisicao { get; set; }

        public IList<string> ItensRequisicaoTela { get; set; }

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
                                R.UsuarioAtual AS USUARIO_ATUAL,                    
                           (SELECT U.Login
                            FROM usuario U
                            WHERE U.IdUsuario = R.UsuarioAtual) AS NOME_USUARIO_ATUAL,
                                R.UsuarioInclusao AS USUARIO_INCLUSAO,                        
                           (SELECT U.Login
                            FROM usuario U
                            WHERE U.IdUsuario = R.UsuarioInclusao) AS NOME_USUARIO_INCLUSAO,
                                R.Status AS STATUS,
                                R.Descricao AS DESCRICAO,
                                R.Tipo AS TIPO,
                                R.Origem AS ORIGEM,                       
                           (SELECT CASE e.Tipo
                                       WHEN 0 THEN
                                              (SELECT a.Nome
                                               FROM armazem a
                                               WHERE a.Id_Estacao = R.Origem)
                                       WHEN 1 THEN
                                              (SELECT u.Nome
                                               FROM unidademaritima u
                                               WHERE u.Id_Estacao = R.Origem)
                                       ELSE ('')
                                   END
                            FROM estacaotrabalho e
                            WHERE e.IdEstacao = R.Origem) AS NOME_ESTACAO_ORIGEM,
                                R.Destino AS DESTINO,                        
                           (SELECT CASE e.Tipo
                                       WHEN 0 THEN
                                              (SELECT a.Nome
                                               FROM armazem a
                                               WHERE a.Id_Estacao = R.Destino)
                                       WHEN 1 THEN
                                              (SELECT u.Nome
                                               FROM unidademaritima u
                                               WHERE u.Id_Estacao = R.Destino)
                                       ELSE ('')
                                   END
                            FROM estacaotrabalho e
                            WHERE e.IdEstacao = R.Destino) AS NOME_ESTACAO_DESTINO
                          FROM requisicao R,
                              funcionario F,
                              usuario U
                          WHERE U.IdUsuario = R.UsuarioAtual
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
                    UsuarioAtual = dt.Rows[i]["USUARIO_ATUAL"] != null ? Convert.ToInt32(dt.Rows[i]["USUARIO_ATUAL"].ToString()) : 0,
                    NomeUsuarioAtual = dt.Rows[i]["NOME_USUARIO_ATUAL"] != null ? dt.Rows[i]["NOME_USUARIO_ATUAL"].ToString() : string.Empty,
                    Data = dt.Rows[i]["DATA"] != null ? Convert.ToDateTime(dt.Rows[i]["DATA"].ToString()).ToString("dd/MM/yyyy") : string.Empty,
                    Status = dt.Rows[i]["STATUS"] != null ? dt.Rows[i]["STATUS"].ToString() : string.Empty,
                    Origem = dt.Rows[i]["ORIGEM"] != null ? dt.Rows[i]["ORIGEM"].ToString() : string.Empty,
                    NomeEstacaoOrigem = dt.Rows[i]["NOME_ESTACAO_ORIGEM"] != null ? dt.Rows[i]["NOME_ESTACAO_ORIGEM"].ToString() : string.Empty,
                    Destino = dt.Rows[i]["DESTINO"] != null ? dt.Rows[i]["DESTINO"].ToString() : string.Empty,
                    NomeEstacaoDestino = dt.Rows[i]["NOME_ESTACAO_DESTINO"] != null ? dt.Rows[i]["NOME_ESTACAO_DESTINO"].ToString() : string.Empty,                   
                    NomeUsuarioResponsavel = dt.Rows[i]["NOME_USUARIO_INCLUSAO"] != null ? dt.Rows[i]["NOME_USUARIO_INCLUSAO"].ToString() : string.Empty                 
                };

                lista.Add(item);
            }

            return lista;
        }

        public RequisicaoModel CarregarRegistro(int? id)
        {
            var sql = $@"SELECT R.IdRequisicao AS ID,
                                R.DataInclusao AS DATA,
                                R.UsuarioAtual AS USUARIO_ATUAL,                    
                           (SELECT U.Login
                            FROM usuario U
                            WHERE U.IdUsuario = R.UsuarioAtual) AS NOME_USUARIO_ATUAL,
                                R.UsuarioInclusao AS USUARIO_INCLUSAO,                        
                           (SELECT U.Login
                            FROM usuario U
                            WHERE U.IdUsuario = R.UsuarioInclusao) AS NOME_USUARIO_INCLUSAO,
                                R.Status AS STATUS,
                                R.Descricao AS DESCRICAO,
                                R.Tipo AS TIPO,
                                R.Origem AS ORIGEM,                       
                           (SELECT CASE e.Tipo
                                       WHEN 0 THEN
                                              (SELECT a.Nome
                                               FROM armazem a
                                               WHERE a.Id_Estacao = R.Origem)
                                       WHEN 1 THEN
                                              (SELECT u.Nome
                                               FROM unidademaritima u
                                               WHERE u.Id_Estacao = R.Origem)
                                       ELSE ('')
                                   END
                            FROM estacaotrabalho e
                            WHERE e.IdEstacao = R.Origem) AS NOME_ESTACAO_ORIGEM,
                                R.Destino AS DESTINO,                        
                           (SELECT CASE e.Tipo
                                       WHEN 0 THEN
                                              (SELECT a.Nome
                                               FROM armazem a
                                               WHERE a.Id_Estacao = R.Destino)
                                       WHEN 1 THEN
                                              (SELECT u.Nome
                                               FROM unidademaritima u
                                               WHERE u.Id_Estacao = R.Destino)
                                       ELSE ('')
                                   END
                            FROM estacaotrabalho e
                            WHERE e.IdEstacao = R.Destino) AS NOME_ESTACAO_DESTINO
                          FROM requisicao R,
                              funcionario F,
                              usuario U
                          WHERE U.IdUsuario = R.UsuarioAtual
                           AND U.Id_Funcionario = F.IdFuncionario
                           AND R.IdRequisicao = '{id}'";

            var dal = new DAL();
            var dt = dal.RetDataTable(sql);

            var entity = new RequisicaoModel
            {
                Id = dt.Rows[0]["ID"] != null ? Convert.ToInt32(dt.Rows[0]["ID"].ToString()) : 0,
                Descricao = dt.Rows[0]["DESCRICAO"] != null ? dt.Rows[0]["DESCRICAO"].ToString() : string.Empty,
                Tipo = dt.Rows[0]["TIPO"] != null ? dt.Rows[0]["TIPO"].ToString() : string.Empty,
                UsuarioAtual = dt.Rows[0]["USUARIO_ATUAL"] != null ? Convert.ToInt32(dt.Rows[0]["USUARIO_ATUAL"].ToString()) : 0,
                NomeUsuarioAtual = dt.Rows[0]["NOME_USUARIO_ATUAL"] != null ? dt.Rows[0]["NOME_USUARIO_ATUAL"].ToString() : string.Empty,
                Data = dt.Rows[0]["DATA"] != null ? Convert.ToDateTime(dt.Rows[0]["DATA"].ToString()).ToString("dd/MM/yyyy") : string.Empty,
                Status = dt.Rows[0]["STATUS"] != null ? dt.Rows[0]["STATUS"].ToString() : string.Empty,
                Origem = dt.Rows[0]["ORIGEM"] != null ? dt.Rows[0]["ORIGEM"].ToString() : string.Empty,
                NomeEstacaoOrigem = dt.Rows[0]["NOME_ESTACAO_ORIGEM"] != null ? dt.Rows[0]["NOME_ESTACAO_ORIGEM"].ToString() : string.Empty,
                Destino = dt.Rows[0]["DESTINO"] != null ? dt.Rows[0]["DESTINO"].ToString() : string.Empty,
                NomeEstacaoDestino = dt.Rows[0]["NOME_ESTACAO_DESTINO"] != null ? dt.Rows[0]["NOME_ESTACAO_DESTINO"].ToString() : string.Empty,
                UsuarioResponsavel = dt.Rows[0]["USUARIO_INCLUSAO"] != null ? Convert.ToInt32(dt.Rows[0]["USUARIO_INCLUSAO"].ToString()) : 0,
                NomeUsuarioResponsavel = dt.Rows[0]["NOME_USUARIO_INCLUSAO"] != null ? dt.Rows[0]["NOME_USUARIO_INCLUSAO"].ToString() : string.Empty
            };

            return entity;
        }

        public IList<ItemRequisicaoModel> ObterItensRequisicao(int? id)
        {
            var sql = $"SELECT Id_Equipamento, Quantidade from itemrequisicao where Id_Requisicao = '{id}'";
            
            var dal = new DAL();
            var dt = dal.RetDataTable(sql);

            var lista = new List<ItemRequisicaoModel>();
            
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var item = new ItemRequisicaoModel
                {
                    CodigoEquipamento = !string.IsNullOrEmpty(dt.Rows[i]["Id_Equipamento"].ToString()) ? dt.Rows[i]["Id_Equipamento"].ToString() : string.Empty,
                    Quantidade = !string.IsNullOrEmpty(dt.Rows[i]["Quantidade"].ToString()) ? Convert.ToInt32(dt.Rows[i]["Quantidade"].ToString()) : 0              
                };

                lista.Add(item);
            }

            return lista;
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
                      $"Descricao = '{Descricao}',  Tipo = '{Tipo}', Status = '{Status}', Origem ='{Origem}'," +
                      $"Destino = '{Destino}', UsuarioAtual = '{UsuarioAtual}', UsuarioAlteracao ='{IdUsuarioLogado()}' WHERE IdRequisicao = '{Id}'";
            }

            dal.ExecutarComandoSQL(sql);
           
            GravarLista(); 
        }

        private void GravarLista()
        {
            string sqlListaItens;

            var dal = new DAL();

            if (Existe(Id))
            {
                ExcluirListaItens(dal, Id);
            }

            var listaGravacao = new List<ItemRequisicaoModel>();
           
            
            if (ItensRequisicaoTela != null)
            {
                foreach (var item in ItensRequisicaoTela)
                {
                    string dados = item;
                    string[] dadosDoItem = dados.Split(';');

                    var newItem = new ItemRequisicaoModel
                    {
                        CodigoRequisicao = Id,
                        CodigoEquipamento = dadosDoItem[0],
                        Quantidade = Convert.ToInt32(dadosDoItem[1])
                    };

                    listaGravacao.Add(newItem);
                }
            }
           

            foreach (var item in listaGravacao)
            {
                sqlListaItens = $"INSERT INTO itemrequisicao (Quantidade, Id_Equipamento, Id_Requisicao) VALUES ('{item.Quantidade}', '{item.CodigoEquipamento}', '{item.CodigoRequisicao}')";
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

using Microsoft.AspNetCore.Http;
using SGP.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SGP.Models
{
    public class RequisicaoModel
    {
        #region ENUM

        public enum StatusRequisicao
        {
            [Description("Solicitar")]
            Solicitar,
            [Description("Liberar")]
            Liberar,
            [Description("Coletar")]
            Coletar,
            [Description("Processar")]
            Processar,
            [Description("Cancelar")]
            Cancelar,
            [Description("Programar")]
            Programar,
            [Description("Encerrar")]
            Encerrar
        }

        #endregion

        #region PROPRIEDADES

        public int Id { get; set; }

        public string Data { get; set; }

        public string DataFinal { get; set; }

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

        public int VbCancelada { get; set; }

        public int VbEncerrada { get; set; }

        public IList<ItemRequisicaoModel> ItensRequisicao { get; set; }

        public IList<string> ItensRequisicaoTela { get; set; }

        public IHttpContextAccessor HttpContextAccessor { get; set; }

        #endregion

        #region CONTRUTORES

        public RequisicaoModel()
        {
            ItensRequisicao = new List<ItemRequisicaoModel>();
        }

        public RequisicaoModel(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region MÉTODOS

        private string IdUsuarioLogado()
        {
            return HttpContextAccessor.HttpContext.Session.GetString("IdUsuarioLogado");
        }

        private string NomeUsuarioLogado()
        {
            return HttpContextAccessor.HttpContext.Session.GetString("NomeUsuarioLogado");
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
                           AND R.VbCancelada = 0
                           AND R.VbEncerrada = 0
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
                                R.VbCancelada AS VBCANCELADA,
                                R.VbEncerrada AS VBENCERRADA,
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

            var entity = new RequisicaoModel();

            if (dt.Rows.Count > 0)
            {
                entity.Id = dt.Rows[0]["ID"] != null ? Convert.ToInt32(dt.Rows[0]["ID"].ToString()) : 0;
                entity.Descricao = dt.Rows[0]["DESCRICAO"] != null ? dt.Rows[0]["DESCRICAO"].ToString() : string.Empty;
                entity.Tipo = dt.Rows[0]["TIPO"] != null ? dt.Rows[0]["TIPO"].ToString() : string.Empty;
                entity.UsuarioAtual = dt.Rows[0]["USUARIO_ATUAL"] != null ? Convert.ToInt32(dt.Rows[0]["USUARIO_ATUAL"].ToString()) : 0;
                entity.NomeUsuarioAtual = dt.Rows[0]["NOME_USUARIO_ATUAL"] != null ? dt.Rows[0]["NOME_USUARIO_ATUAL"].ToString() : string.Empty;
                entity.Data = dt.Rows[0]["DATA"] != null ? Convert.ToDateTime(dt.Rows[0]["DATA"].ToString()).ToString("dd/MM/yyyy") : string.Empty;
                entity.Status = dt.Rows[0]["STATUS"] != null ? dt.Rows[0]["STATUS"].ToString() : string.Empty;
                entity.Origem = dt.Rows[0]["ORIGEM"] != null ? dt.Rows[0]["ORIGEM"].ToString() : string.Empty;
                entity.NomeEstacaoOrigem = dt.Rows[0]["NOME_ESTACAO_ORIGEM"] != null ? dt.Rows[0]["NOME_ESTACAO_ORIGEM"].ToString() : string.Empty;
                entity.Destino = dt.Rows[0]["DESTINO"] != null ? dt.Rows[0]["DESTINO"].ToString() : string.Empty;
                entity.NomeEstacaoDestino = dt.Rows[0]["NOME_ESTACAO_DESTINO"] != null ? dt.Rows[0]["NOME_ESTACAO_DESTINO"].ToString() : string.Empty;
                entity.UsuarioResponsavel = dt.Rows[0]["USUARIO_INCLUSAO"] != null ? Convert.ToInt32(dt.Rows[0]["USUARIO_INCLUSAO"].ToString()) : 0;
                entity.NomeUsuarioResponsavel = dt.Rows[0]["NOME_USUARIO_INCLUSAO"] != null ? dt.Rows[0]["NOME_USUARIO_INCLUSAO"].ToString() : string.Empty;
                entity.VbCancelada = dt.Rows[0]["VBCANCELADA"] != null ? Convert.ToInt32(dt.Rows[0]["VBCANCELADA"].ToString()) : 0;
                entity.VbEncerrada = dt.Rows[0]["VBENCERRADA"] != null ? Convert.ToInt32(dt.Rows[0]["VBENCERRADA"].ToString()) : 0;
            }

            return entity;
        }

        public List<ReqHistoricoModel> ObterHistorico(int id)
        {
            var historicoRequisicao = new ReqHistoricoModel();
            return historicoRequisicao.ListaReqHistorico(id);
        }

        public IList<ItemRequisicaoModel> ObterItensRequisicao(int? id)
        {
            var sql = $@"SELECT Id_Equipamento,
                                Quantidade
                         FROM itemrequisicao
                         WHERE Id_Requisicao = '{id}'";

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
                Id = GerarSequencial();

                sql = $@"INSERT INTO Requisicao (IdRequisicao, Descricao, Tipo, Origem, Status, Destino, UsuarioAtual, DataInclusao, UsuarioInclusao, DataAlteracao, UsuarioAlteracao) 
                         VALUES ('{Id}', '{Descricao}', '{Tipo}' ,'{Origem}', '{Status}', '{Destino}', '{UsuarioAtual}','{Convert.ToDateTime(Data):yyyy/MM/dd}', '{IdUsuarioLogado()}', '', '')";
            }
            else
            {
                sql = $@"UPDATE  Requisicao SET DataAlteracao = '{Convert.ToDateTime(Data):yyyy/MM/dd}', 
                         Descricao = '{Descricao}',  Tipo = '{Tipo}', Status = '{Status}', Origem ='{Origem}',
                         Destino = '{Destino}', UsuarioAtual = '{UsuarioAtual}', UsuarioAlteracao ='{IdUsuarioLogado()}' WHERE IdRequisicao = '{Id}'";
            }

            dal.ExecutarComandoSQL(sql);

            RegistrarHistorico();
            GravarLista();
            AtualizarQuantidadeItem(Id);
        }

        private int GerarSequencial()
        {
            var codigos = ListaSequencial();

            if (codigos.Count > 0)
            {
                var sequencial = codigos[codigos.Count - 1];

                return sequencial + 1;
            }

            return 1;
        }

        private void RegistrarHistorico()
        {
            var historicoRequisicao = new ReqHistoricoModel();
            historicoRequisicao.IdRequisicao = Id;
            historicoRequisicao.IdUsuario = UsuarioAtual;
            historicoRequisicao.Descricao = Descricao;
            historicoRequisicao.DataAlteracao = Data;

            historicoRequisicao.GravarReqHistorico();
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
                sqlListaItens = $@"INSERT INTO itemrequisicao (Quantidade, Id_Equipamento, Id_Requisicao) 
                                   VALUES ('{item.Quantidade}', '{item.CodigoEquipamento}', '{item.CodigoRequisicao}')";

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
            var sql = $@"SELECT Id_Requisicao
                         FROM itemrequisicao
                         WHERE Id_Requisicao = '{id}'";

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

        public void Encerrar(int id)
        {
            var dal = new DAL();         
           
            string sql = $@"UPDATE Requisicao SET DataAlteracao = '{Convert.ToDateTime(DateTime.Now):dd/MM/yyyy}', 
                            Descricao = 'Requisição encerrada em {Convert.ToDateTime(DateTime.Now):dd/MM/yyyy} pelo usuário {NomeUsuarioLogado()}' , VbEncerrada = '1', UsuarioAlteracao ='{IdUsuarioLogado()}' WHERE IdRequisicao = '{id}'";

            dal.ExecutarComandoSQL(sql);         
        }

        private void AtualizarQuantidadeItem(int id)
        {
            var equipamento = new EquipamentoModel();

            var itemRequisicao = new ItemRequisicaoModel();

            var listaItens = itemRequisicao.ObterItensPorRequisicao(id);

            var dal = new DAL();

            foreach (var item in listaItens)
            {
                var quantidadeRequisicao = item.Quantidade;
                var equipamentoBanco = equipamento.CarregarRegistro(item.CodigoEquipamento);
                var quantidadeAtualizada = equipamentoBanco.Quantidade - quantidadeRequisicao;

                string sql = $@"UPDATE equipamento SET Quantidade = '{quantidadeAtualizada}' WHERE IdEquipamento = '{equipamentoBanco.Id}'";

                dal.ExecutarComandoSQL(sql);
            }

        }

        public void Cancelar(int id)
        {
            var dal = new DAL();

            string sql = $@"UPDATE Requisicao SET DataAlteracao = '{Convert.ToDateTime(DateTime.Now):dd/MM/yyyy}', 
                            Descricao = 'Requisição cancelada em {Convert.ToDateTime(DateTime.Now):dd/MM/yyyy} pelo usuário {NomeUsuarioLogado()}' , VbCancelada = '1', UsuarioAlteracao ='{IdUsuarioLogado()}' WHERE IdRequisicao = '{id}'";

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

        public List<int> ListaSequencial()
        {
            var lista = new List<int>();

            var sql = $@"SELECT IdRequisicao AS ID FROM requisicao ORDER BY ID;";

            var dal = new DAL();
            var dt = dal.RetDataTable(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var item = Id = dt.Rows[i]["ID"] != null ? Convert.ToInt32(dt.Rows[i]["ID"].ToString()) : 0;
                lista.Add(item);
            }

            return lista;
        }

        public void SincorinizarEntrega(int idRequisicao, int idEntrega)
        {   
            var sql = $@"UPDATE  Requisicao SET DataAlteracao = '{DateTime.Now}', 
                         Descricao = 'Requisição programada para transporte. Número do transporte: {idEntrega}' WHERE IdRequisicao = '{idRequisicao}'";

            var dal = new DAL();
            dal.ExecutarComandoSQL(sql);
        }

        #endregion
    }
}

using Microsoft.AspNetCore.Http;
using SGP.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SGP.Models
{
    public class SinistroModel
    {
        #region ENUM

        public enum StatusSinistro
        {
            [Description("Aberto")]
            Aberto,
            [Description("Em análise")]
            Analise,
            [Description("Finalizar")]
            Finalizar
        }

        #endregion

        #region PROPRIEDADES

        public int Id { get; set; }

        [Required(ErrorMessage = "O código da requisição é obrigatório.")]
        public int IdRequisicao { get; set; }

        public int UsuarioResponsavel { get; set; }

        public string NomeUsuarioResponsavel { get; set; }

        [Required(ErrorMessage = "O campo data é obrigatório.")]
        public string DataAbertura { get; set; }

        [Required(ErrorMessage = "O campo descrição é obrigatório.")]
        public string Descricao { get; set; }

        public int VbFinalizado { get; set; }

        public string Status { get; set; }

        public int UsuarioAtual { get; set; }

        public string NomeUsuarioAtual { get; set; }

        public IHttpContextAccessor HttpContextAccessor { get; set; }

        #endregion

        #region CONSTRUTORES

        public SinistroModel() { }

        public SinistroModel(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region MÉTODOS

        private string IdUsuarioLogado()
        {
            return HttpContextAccessor.HttpContext.Session.GetString("IdUsuarioLogado");
        }

        public SinistroModel CarregarRegistro(int? id)
        {
            var sql = $@"SELECT S.IdSinistro AS ID, 
                                S.Id_Requisicao AS ID_REQUISICAO,
                                S.DataInclusao AS DATA_INCLUSAO,
                                S.Descricao AS DESCRICAO,
                                S.Status AS STATUS,
                                S.UsuarioAtual AS USUARIO_ATUAL,
                           (SELECT U.Login
                            FROM usuario U
                            WHERE U.IdUsuario = S.UsuarioAtual) AS NOME_USUARIO_ATUAL,                    
                           (SELECT U.Login
                            FROM usuario U
                            WHERE U.IdUsuario = S.UsuarioInclusao) AS NOME_USUARIO_INCLUSAO
                         FROM pedidosinistro S
                         WHERE S.Id_Requisicao = '{id}'
                         ORDER BY DATA_INCLUSAO DESC
                         LIMIT 10";

            var dal = new DAL();
            var dt = dal.RetDataTable(sql);
            var entity = new SinistroModel();

            if (dt.Rows.Count > 0)
            {
                entity.Id = dt.Rows[0]["ID"] != null ? Convert.ToInt32(dt.Rows[0]["ID"].ToString()) : 0;
                entity.IdRequisicao = dt.Rows[0]["ID_REQUISICAO"] != null ? Convert.ToInt32(dt.Rows[0]["ID_REQUISICAO"].ToString()) : 0;
                entity.Descricao = dt.Rows[0]["DESCRICAO"] != null ? dt.Rows[0]["DESCRICAO"].ToString() : string.Empty;
                entity.DataAbertura = dt.Rows[0]["DATA_INCLUSAO"] != null ? Convert.ToDateTime(dt.Rows[0]["DATA_INCLUSAO"].ToString()).ToString() : DateTime.Now.ToString();
                var statusBanco = dt.Rows[0]["STATUS"] != null ? Convert.ToInt32(dt.Rows[0]["STATUS"].ToString()) : 0;
                entity.Status = ((StatusSinistro)statusBanco).GetDescription();
                entity.NomeUsuarioResponsavel = dt.Rows[0]["NOME_USUARIO_INCLUSAO"] != null ? dt.Rows[0]["NOME_USUARIO_INCLUSAO"].ToString() : string.Empty;
                entity.NomeUsuarioAtual = dt.Rows[0]["NOME_USUARIO_ATUAL"] != null ? dt.Rows[0]["NOME_USUARIO_ATUAL"].ToString() : string.Empty;
                entity.UsuarioAtual = dt.Rows[0]["USUARIO_ATUAL"] != null ? Convert.ToInt32(dt.Rows[0]["USUARIO_ATUAL"].ToString()) : 0;
            }

            return entity;
        }

        public void Gravar()
        {
            string sql;
            var dal = new DAL();

            if (Id == 0)
            {
                sql = $@"INSERT INTO pedidosinistro (Id_Requisicao, Descricao, Status, VbFinalizado, UsuarioAtual, DataInclusao, UsuarioInclusao, DataAlteracao, UsuarioAlteracao) VALUES 
                         ('{IdRequisicao}', '{Descricao}',  '{(int)Status.GetEnumValue<StatusSinistro>()}', '0','{UsuarioAtual}', '{Convert.ToDateTime(DateTime.Now)}', '{UsuarioResponsavel}', '','' )";
            }
            else
            {
                sql = $@"UPDATE  pedidosinistro SET DataAlteracao = '{DateTime.Now}', 
                         Descricao = '{Descricao}', Status = '{(int)Status.GetEnumValue<StatusSinistro>()}', 
                         UsuarioAtual = '{UsuarioAtual}', UsuarioAlteracao ='{IdUsuarioLogado()}'  WHERE IdSinistro = '{Id}'";
            }

            dal.ExecutarComandoSQL(sql);
        }

        public void Finalizar(int id)
        {
            var dal = new DAL();

            var idUsuarioAtual = IdUsuarioLogado();

            string sql = $"UPDATE pedidosinistro SET DataAlteracao = '{Convert.ToDateTime(DateTime.Now):dd/MM/yyyy}', " +
                         $"Descricao = 'Análise de sinistro finalizada em {Convert.ToDateTime(DateTime.Now):dd/MM/yyyy} pelo usuário {idUsuarioAtual}' ,  VbFinalizado = '1', UsuarioAlteracao ='{IdUsuarioLogado()}' WHERE IdSinistro = '{id}'";

            dal.ExecutarComandoSQL(sql);
        }

        public List<SinistroModel> ListaSinistro()
        {
            var lista = new List<SinistroModel>();

            var filtro = string.Empty;

            if ((DataAbertura != null))
            {
                filtro += $"AND R.DataInclusao >='{Convert.ToDateTime(DataAbertura):yyyy/MM/dd}'";
            }

            if (Status != null)
            {
                if (Status != "Todos")
                {
                    filtro += $" AND S.Status = '{(int)Status.GetEnumValue<StatusSinistro>()}'";
                }
            }

            if (UsuarioAtual > 0)
            {
                filtro += $" AND S.UsuarioAtual = '{UsuarioAtual}'";
            }

            var sql = $@"SELECT S.IdSinistro AS ID, 
                                R.IdRequisicao AS ID_REQUISICAO,
                                S.DataInclusao AS DATA_INCLUSAO,
                                S.Descricao AS DESCRICAO,
                                S.Status AS STATUS,                        
                           (SELECT U.Login
                            FROM usuario U
                            WHERE U.IdUsuario = S.UsuarioAtual) AS NOME_USUARIO_ATUAL,                   
                           (SELECT U.Login
                            FROM usuario U
                            WHERE U.IdUsuario = S.UsuarioInclusao) AS NOME_USUARIO_INCLUSAO
                         FROM requisicao R,
                              pedidosinistro S,
                              funcionario F,
                              usuario U
                         WHERE S.Id_Requisicao = R.IdRequisicao
                           AND U.IdUsuario = R.UsuarioAtual
                           AND U.Id_Funcionario = F.IdFuncionario
                           AND S.VbFinalizado = 0
                           {filtro}
                         ORDER BY DATA_INCLUSAO DESC
                         LIMIT 10";

            var dal = new DAL();
            var dt = dal.RetDataTable(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var item = new SinistroModel
                {
                    Id = dt.Rows[i]["ID"] != null ? Convert.ToInt32(dt.Rows[i]["ID"].ToString()) : 0,
                    IdRequisicao = dt.Rows[i]["ID_REQUISICAO"] != null ? Convert.ToInt32(dt.Rows[i]["ID_REQUISICAO"].ToString()) : 0,
                    Descricao = dt.Rows[i]["DESCRICAO"] != null ? dt.Rows[i]["DESCRICAO"].ToString() : string.Empty,
                    NomeUsuarioAtual = dt.Rows[i]["NOME_USUARIO_ATUAL"] != null ? dt.Rows[i]["NOME_USUARIO_ATUAL"].ToString() : string.Empty,
                    DataAbertura = dt.Rows[i]["DATA_INCLUSAO"] != null ? Convert.ToDateTime(dt.Rows[i]["DATA_INCLUSAO"].ToString()).ToString() : DateTime.MinValue.ToString(),
                    Status = dt.Rows[i]["STATUS"] != null ? dt.Rows[i]["STATUS"].ToString() : string.Empty,
                    NomeUsuarioResponsavel = dt.Rows[i]["NOME_USUARIO_INCLUSAO"] != null ? dt.Rows[i]["NOME_USUARIO_INCLUSAO"].ToString() : string.Empty
                };

                lista.Add(item);
            }

            return lista;
        }

        #endregion
    }
}

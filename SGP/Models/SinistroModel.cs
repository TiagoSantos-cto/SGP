using Microsoft.AspNetCore.Http;
using SGP.Util;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;

namespace SGP.Models
{
    public class SinistroModel
    {
        public enum StatusSinistro
        {
            [Description("Aberto")]
            Aberto,
            [Description("Em analise")]
            Analise,
            [Description("Finalizado")]
            Finalizado
        }

        public int Id { get; set; }

        public int IdRequisicao { get; set; }

        public int UsuarioResponsavel { get; set; }

        public string NomeUsuarioResponsavel { get; set; }

        [Required(ErrorMessage = "O campo data é obrigatório.")]
        public string DataAbertura { get; set; }

        public string DataFechamento { get; set; }

        [Required(ErrorMessage = "O campo descrição é obrigatório.")]
        public string Descricao { get; set; }

        public int VbFinalizado { get; set; }

        public string Status { get; set; }

        public int UsuarioAtual { get; set; }

        public string NomeUsuarioAtual { get; set; }

        public IHttpContextAccessor HttpContextAccessor { get; set; }

        public SinistroModel(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        public SinistroModel() { }

        private string IdUsuarioLogado()
        {
            return HttpContextAccessor.HttpContext.Session.GetString("IdUsuarioLogado");
        }

        public SinistroModel CarregarRegistro(int? id)
        {
            var sql = $@"SELECT * from PedidoSinistro where IdSinistro = '{id}'";

            var dal = new DAL();
            var dt = dal.RetDataTable(sql);

            var entity = new SinistroModel();
            entity.Id = dt.Rows[0]["ID"] != null ? Convert.ToInt32(dt.Rows[0]["ID"].ToString()) : 0;
            entity.Descricao = dt.Rows[0]["DESCRICAO"] != null ? dt.Rows[0]["DESCRICAO"].ToString() : string.Empty;
            entity.DataAbertura = dt.Rows[0]["DATA"] != null ? Convert.ToDateTime(dt.Rows[0]["DATA"].ToString()).ToString("dd/MM/yyyy") : string.Empty;
            entity.Status = dt.Rows[0]["STATUS"] != null ? dt.Rows[0]["STATUS"].ToString() : string.Empty;
            entity.UsuarioResponsavel = dt.Rows[0]["USUARIO_INCLUSAO"] != null ? Convert.ToInt32(dt.Rows[0]["USUARIO_INCLUSAO"].ToString()) : 0;
            entity.NomeUsuarioResponsavel = dt.Rows[0]["NOME_USUARIO_INCLUSAO"] != null ? dt.Rows[0]["NOME_USUARIO_INCLUSAO"].ToString() : string.Empty;

            return entity;
        }

        public void Gravar()
        {
            string sql;
            var dal = new DAL();

            if (Id == 0)
            {
                sql = "INSERT INTO pedidosinistro (Id_Requisicao, Descricao, Status, VbFinalizado, UsuarioAtual, DataInclusao, UsuarioInclusao, DataAlteracao, UsuarioAlteracao) VALUES " +
                    $" ('{IdRequisicao}', '{Descricao}',  '{Status}', '0','{UsuarioAtual}', '{Convert.ToDateTime(DataAbertura):yyyy/MM/dd}', '{UsuarioResponsavel}', {DateTime.Now},'{IdUsuarioLogado()}' )";
            }
            else
            {
                sql = $"UPDATE  pedidosinistro SET DataAlteracao = '{Convert.ToDateTime(DataAbertura):yyyy/MM/dd}', " +
                      $"Descricao = '{Descricao}', Status = '{Status}'," +
                      $"UsuarioAlteracao ='{IdUsuarioLogado()}' WHERE IdSinistro = '{Id}'";
            }

            dal.ExecutarComandoSQL(sql);
        }

        public void Finalizar(int id)
        {
            var dal = new DAL();

            string sql = $"UPDATE Sinistro SET DataAlteracao = '{Convert.ToDateTime(DateTime.Now):dd/MM/yyyy}', " +
                         $"Descricao = 'Análise de sinistro finalizada em {Convert.ToDateTime(DateTime.Now):dd/MM/yyyy} pelo usuário {IdUsuarioLogado()}' ,  VbFinalizado = '1', UsuarioAlteracao ='{IdUsuarioLogado()}' WHERE IdSinistro = '{id}'";

            dal.ExecutarComandoSQL(sql);
        }

       
    }

}

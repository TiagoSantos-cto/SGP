using Microsoft.AspNetCore.Http;
using SGP.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SGP.Models
{
    public class UsuarioModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo funcionário é obrigatório!")]
        public int IdFuncionario { get; set; }

        [Required(ErrorMessage = "O campo nome é obrigatório!")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo e-mail é obrigatório!")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "E-mail inválido!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo senha é obrigatório!")]
        public string Senha { get; set; }

        public int PerfilAcesso { get; set; }

        public bool Inativo { get; set; }

        public IHttpContextAccessor HttpContextAccessor { get; set; }

        public UsuarioModel() { }

        public UsuarioModel(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        public bool ValidarLogin()
        {
            string sql = $@"SELECT DISTINCT U.IdUsuario as IdUsuario, P.Nome as NomeUsuario, U.PerfilAcesso as PerfilAcesso 
                              FROM Usuario U, Pessoa P, Funcionario F 
                                WHERE U.Id_Funcionario = F.IdFuncionario
                                AND F.Id_Pessoa        = P.IdPessoa
                                AND U.Email            = '{Email}' AND U.Senha = '{Senha}'";

            var dal = new DAL();
            var dt = dal.RetDataTable(sql);

            if (dt != null)
            {
                if (dt.Rows.Count == 1)
                {
                    Id = Convert.ToInt32(dt.Rows[0]["IdUsuario"].ToString());
                    Nome = dt.Rows[0]["NomeUsuario"].ToString();
                    PerfilAcesso = Convert.ToInt32(dt.Rows[0]["PerfilAcesso"].ToString());
                    return true;
                }
            }
            return false;
        }

        public void GravarUsuario()
        {
            string sql = $"INSERT INTO Usuario (Email, Senha, PerfilAcesso, Inativo, IdFuncionario) VALUES('{Email}','{Senha}','{PerfilAcesso}', '{Inativo}','{IdFuncionario}')";
            var dal = new DAL();
            dal.ExecutarComandoSQL(sql);
        }

        public UsuarioModel ObterUsuario(string IdFuncionario)
        {

            var sql = $@"SELECT DISTINCT U.IdUsuario as IdUsuario, U.Email as Email, U.Senha as Senha, U.PerfilAcesso, U.Inativo as Inativo as PerfilAcesso 
                            FROM Usuario U, Pessoa P, Funcionario F
                              WHERE U.Id_Funcionario = F.IdFuncionario
                              AND   F.Id_Pessoa      = P.IdPessoa
                              AND   U.Id_Funcionario = '{IdFuncionario}'";

            var dal = new DAL();
            var dt = dal.RetDataTable(sql);

            var usuario = new UsuarioModel
            {
                Id = Convert.ToInt32(dt.Rows[0]["IdUsuario"].ToString()),
                Email = dt.Rows[0]["Email"].ToString(),
                Senha = dt.Rows[0]["Senha"].ToString(),
                PerfilAcesso = Convert.ToInt32(dt.Rows[0]["PerfilAcesso"].ToString()),
                Inativo = Convert.ToBoolean(dt.Rows[0]["Inativo"].ToString())
            };

            return usuario;
        }

        public List<UsuarioModel> ListaUsuario()
        {
            var lista = new List<UsuarioModel>();

            var sql = $@"SELECT DISTINCT U.IdUsuario as IdUsuario, P.Nome as NomeUsuario, U.PerfilAcesso as PerfilAcesso 
                            FROM Usuario U, Pessoa P, Funcionario F
                              WHERE U.Id_Funcionario = F.IdFuncionario
                              AND   F.Id_Pessoa      = P.IdPessoa";

            var dal = new DAL();
            var dt = dal.RetDataTable(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var item = new UsuarioModel
                {
                    Id = Convert.ToInt32(dt.Rows[i]["IdUsuario"].ToString()),
                    Nome = dt.Rows[i]["NomeUsuario"].ToString(),
                    PerfilAcesso = Convert.ToInt32(dt.Rows[i]["PerfilAcesso"].ToString())
                };

                lista.Add(item);
            }

            return lista;
        }

        public List<UsuarioModel> ListaFuncionario()
        {
            var lista = new List<UsuarioModel>();

            var sql = $@"SELECT F.IdFuncionario as Codigo, P.Nome as Nome
                            FROM Funcionario F, Pessoa P
                             WHERE F.Id_Pessoa = P.IdPessoa";

            var dal = new DAL();
            var dt = dal.RetDataTable(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var item = new UsuarioModel
                {
                    IdFuncionario = Convert.ToInt32(dt.Rows[i]["Codigo"].ToString()),
                    Nome = dt.Rows[i]["Nome"].ToString()
                };

                lista.Add(item);
            }

            return lista;
        }
    }
}

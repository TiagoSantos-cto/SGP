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

        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo usuário é obrigatório!")]
        public string Login { get; set; }

        [Required(ErrorMessage = "O campo senha é obrigatório!")]
        public string Senha { get; set; }

        public int PerfilAcesso { get; set; }

        public IHttpContextAccessor HttpContextAccessor { get; set; }

        public UsuarioModel() { }

        public UsuarioModel(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        public bool ValidarLogin()
        {
            string sql = $@"SELECT DISTINCT U.IdUsuario AS IdUsuario,
                                            F.Nome AS NomeUsuario,
                                            U.PerfilAcesso AS PerfilAcesso
                            FROM Usuario U,
                                 Funcionario F
                            WHERE U.Id_Funcionario = F.IdFuncionario
                              AND U.Login = '{Login}'
                              AND U.Senha = '{Senha}'";

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
            string sql;

            if (Id == 0)
            {
                sql = $"INSERT INTO Usuario (Login, Senha, PerfilAcesso, Id_Funcionario) VALUES('{Login}','{Senha}','{PerfilAcesso}','{IdFuncionario}')";
            }
            else
            {
                sql = $"UPDATE  Usuario SET Login = '{Login}',  Senha = '{Senha}', PerfilAcesso = '{PerfilAcesso}' WHERE Id_Funcionario = '{Id}'";
            }


            var dal = new DAL();
            dal.ExecutarComandoSQL(sql);
        }

        public UsuarioModel ObterUsuario(int? IdFuncionario)
        {

            var sql = $@"SELECT DISTINCT U.IdUsuario AS IdUsuario,
                                         U.Login AS Login,
                                         U.Senha AS Senha,
                                         U.PerfilAcesso AS PerfilAcesso,
                                         U.Id_Funcionario AS IdFuncionario,
                                         F.Nome AS Nome
                         FROM Usuario U,
                              Funcionario F
                         WHERE U.Id_Funcionario = F.IdFuncionario
                           AND U.Id_Funcionario = '{IdFuncionario}'";

            var dal = new DAL();
            var dt = dal.RetDataTable(sql);

            var usuario = new UsuarioModel
            {
                Id = dt.Rows[0]["IdUsuario"] != null ? Convert.ToInt32(dt.Rows[0]["IdUsuario"].ToString()) : 0,
                IdFuncionario = dt.Rows[0]["IdFuncionario"] != null ? Convert.ToInt32(dt.Rows[0]["IdFuncionario"].ToString()) : 0,
                Nome  = dt.Rows[0]["Nome"] != null ? dt.Rows[0]["Nome"].ToString() : string.Empty,
                Login = dt.Rows[0]["Login"] != null ? dt.Rows[0]["Login"].ToString() : string.Empty,
                Senha = dt.Rows[0]["Senha"] != null ? dt.Rows[0]["Senha"].ToString() : string.Empty,
                PerfilAcesso = dt.Rows[0]["PerfilAcesso"] != null ? Convert.ToInt32(dt.Rows[0]["PerfilAcesso"].ToString()) : 0,                      
            };

            return usuario;
        }

       

        public List<UsuarioModel> ListaUsuario()
        {
            var lista = new List<UsuarioModel>();

            var sql = $@"SELECT DISTINCT U.IdUsuario AS IdUsuario,
                                         U.Login AS Login,
                                         F.Nome AS NomeUsuario,
                                         U.PerfilAcesso AS PerfilAcesso,
                                         U.Id_Funcionario AS IdFuncionario
                         FROM Usuario U,
                              Funcionario F
                         WHERE U.Id_Funcionario = F.IdFuncionario";

            var dal = new DAL();
            var dt = dal.RetDataTable(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var item = new UsuarioModel
                {
                    Id = dt.Rows[i]["IdUsuario"] != null ? Convert.ToInt32(dt.Rows[i]["IdUsuario"].ToString()) : 0,
                    Login = dt.Rows[i]["Login"] != null ? dt.Rows[i]["Login"].ToString() : string.Empty,
                    IdFuncionario = dt.Rows[i]["IdFuncionario"] != null ? Convert.ToInt32(dt.Rows[i]["IdFuncionario"].ToString()) : 0,
                    Nome = dt.Rows[i]["NomeUsuario"] != null ? dt.Rows[i]["NomeUsuario"].ToString() : string.Empty,
                    PerfilAcesso = dt.Rows[i]["PerfilAcesso"] != null ? Convert.ToInt32(dt.Rows[i]["PerfilAcesso"].ToString()) : 0
                };

                lista.Add(item);
            }

            return lista;
        }

        public List<UsuarioModel> ListaFuncionario()
        {
            var lista = new List<UsuarioModel>();

            var sql = $@"SELECT F.IdFuncionario AS Codigo,
                                F.Nome AS Nome
                         FROM Funcionario F";
                         
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

        public void Excluir(int id)
        {
            string sql = $"DELETE FROM USUARIO WHERE IdUsuario = {id}";
            var dal = new DAL();
            dal.ExecutarComandoSQL(sql);
        }
    }
}

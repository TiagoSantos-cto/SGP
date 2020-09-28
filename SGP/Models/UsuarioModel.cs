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

        [Required(ErrorMessage = "O campo nome é obrigatório!")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo e-mail é obrigatório!")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "E-mail inválido!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo senha é obrigatório!")]
        public string Senha { get; set; }

        public string Cpf { get; set; }
        public string Rg { get; set; }
        public string Telefone { get; set; }

        [Required(ErrorMessage = "O campo nascimento é obrigatório!")]
        public string Data_Nascimento { get; set; }

        public int Funcao { get; set; }
        public IHttpContextAccessor HttpContextAccessor { get; set; }

        public UsuarioModel(){}

        public UsuarioModel(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        public bool ValidarLogin()
        {
            string sql = $"SELECT ID, NOME, FUNCAO FROM USUARIO_USO WHERE EMAIL='{Email}' AND SENHA ='{Senha}'";

            var dal = new DAL();
            var dt = dal.RetDataTable(sql);

            if (dt != null)
            {
                if (dt.Rows.Count == 1)
                {
                    Id = Convert.ToInt32(dt.Rows[0]["ID"].ToString());
                    Nome = dt.Rows[0]["Nome"].ToString();
                    Funcao = Convert.ToInt32(dt.Rows[0]["FUNCAO"].ToString());
                    return true;
                }
            }

            return false;
        }

        public void GravarUsuario()
        {
            string sql = $"INSERT INTO USUARIO_USO (NOME, EMAIL, SENHA, CPF, RG, TELEFONE, DATA_NASCIMENTO, FUNCAO) VALUES('{Nome}', '{Email}', '{Senha}', '{Cpf}', '{Rg}', '{Telefone}', '{Data_Nascimento}', '{Data_Nascimento}')";
            var dal = new DAL();
            dal.ExecutarComandoSQL(sql);
        }

        public List<UsuarioModel> ListaUsuario()
        {
            var lista = new List<UsuarioModel>();

            var sql = $"SELECT ID, NOME FROM USUARIO_USO";
            var dal = new DAL();
            var dt = dal.RetDataTable(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var item = new UsuarioModel
                {
                    Id = Convert.ToInt32(dt.Rows[i]["ID"].ToString()),
                    Nome = dt.Rows[i]["NOME"].ToString(),               
                };

                lista.Add(item);
            }

            return lista;
        }
    }
}

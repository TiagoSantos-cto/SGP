using Microsoft.AspNetCore.Http;
using SGP.Util;
using System;
using System.Collections.Generic;

namespace SGP.Models
{
    public class EstacaoModel
    {
        #region PROPRIEDADES

        public int Id { get; set; }
        public string Nome { get; set; }
        public int Tipo { get; set; }
        public IHttpContextAccessor HttpContextAccessor { get; set; }

        #endregion

        #region CONTRUTORES

        public EstacaoModel() { }

        public EstacaoModel(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region MÉTODOS

        public void GravarEstacao()
        {
            Id = GerarSequencial();

            string sqlEstacao = string.Empty;
            string sqlTipo = string.Empty;

            if (Tipo == 0)
            {
                if (!Existe(Nome))
                {
                    sqlEstacao = $@"INSERT INTO EstacaoTrabalho (IdEstacao, Tipo)
                                    VALUES ('{Id}', '{Tipo}')";
                    
                    sqlTipo = $@"INSERT INTO Armazem (Nome, Id_Estacao)
                                 VALUES ('{Nome}', '{Id}')";
                }
            }
            else
            {
                if (!Existe(Nome))
                {
                    sqlEstacao = $@"INSERT INTO EstacaoTrabalho (IdEstacao, Tipo)
                                    VALUES('{Id}', '{Tipo}')";
                    
                    sqlTipo = $@"INSERT INTO UnidadeMaritima (Nome, Id_Estacao) 
                                 VALUES ('{Nome}', '{Id}')";
                }
            }

            var dal = new DAL();

            if (!string.IsNullOrEmpty(sqlEstacao) && !string.IsNullOrEmpty(sqlTipo))
            {
                dal.ExecutarComandoSQL(sqlEstacao);
                dal.ExecutarComandoSQL(sqlTipo);
            }
        }

        private bool Existe(string nome)
        {
            var sql = $@"SELECT *
                         FROM Armazem A,
                              UnidadeMaritima UM
                         WHERE A.Nome = '{nome}'
                           OR UM.Nome = '{nome}'";

            var dal = new DAL();
            var dt = dal.RetDataTable(sql);

            return dt.Rows.Count > 0;
        }

        private int GerarSequencial()
        {
            var estacoes = ListaEstacao();

            var listaID = new List<int>();

            foreach (var item in estacoes)
            {
                listaID.Add(item.Id);
            };

            listaID.Sort();

            var sequencial = listaID[listaID.Count - 1];

            return sequencial + 1;
        }

        public void ExcluirEstacao(int id)
        {
            string sql = $@"DELETE
                            FROM EstacaoTrabalho
                            WHERE IdEstacao = '{id}'";

            var dal = new DAL();
            dal.ExecutarComandoSQL(sql);
        }

        public List<EstacaoModel> ListaEstacao()
        {
            var lista = new List<EstacaoModel>();

            var sqlArmazem = $@"SELECT E.IdEstacao AS ID,
                                       E.Tipo AS TIPO,
                                       A.Nome AS NOME
                                FROM EstacaoTrabalho E,
                                     Armazem A
                                WHERE E.IdEstacao = A.Id_Estacao
                                ORDER BY E.IdEstacao";

            var sqlUnidadeMaritima = $@"SELECT E.IdEstacao AS ID,
                                               E.Tipo AS TIPO,
                                               U.Nome AS NOME
                                        FROM EstacaoTrabalho E,
                                             UnidadeMaritima U
                                        WHERE E.IdEstacao = U.Id_Estacao
                                        ORDER BY E.IdEstacao;";

            var dal = new DAL();

            var dtArmazem = dal.RetDataTable(sqlArmazem);

            for (int i = 0; i < dtArmazem.Rows.Count; i++)
            {
                var item = new EstacaoModel
                {
                    Id = Convert.ToInt32(dtArmazem.Rows[i]["ID"].ToString()),
                    Nome = dtArmazem.Rows[i]["NOME"].ToString(),
                    Tipo = Convert.ToInt32(dtArmazem.Rows[i]["TIPO"].ToString())
                };

                lista.Add(item);
            }

            var dtUnidade = dal.RetDataTable(sqlUnidadeMaritima);

            for (int i = 0; i < dtUnidade.Rows.Count; i++)
            {
                var item = new EstacaoModel
                {
                    Id = Convert.ToInt32(dtUnidade.Rows[i]["ID"].ToString()),
                    Nome = dtUnidade.Rows[i]["NOME"].ToString(),
                    Tipo = Convert.ToInt32(dtUnidade.Rows[i]["TIPO"].ToString())
                };

                lista.Add(item);
            }

            return lista;
        }

        #endregion
    }
}

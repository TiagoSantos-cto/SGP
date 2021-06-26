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
            string sqlEstacao = string.Empty;
            string sqlTipo = string.Empty;


            if (Id == 0)
            {
                Id = GerarSequencial();
            }

            if (Tipo == 0)
            {             
                if (!ExisteArmazem(Id))
                {
                    sqlEstacao = $@"INSERT INTO EstacaoTrabalho (IdEstacao, Tipo)
                                    VALUES ('{Id}', '{Tipo}')";

                    sqlTipo = $@"INSERT INTO Armazem (Nome, Id_Estacao)
                                 VALUES ('{Nome}', '{Id}')";
                }
            }
            else
            {
                if (!ExisteUnidade(Id))
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

        private bool ExisteArmazem(int id)
        {
            var sqlArmazem = $@"SELECT A.IdArmazem
                                  FROM Armazem A,
                                       estacaotrabalho e
                                  WHERE e.IdEstacao = A.Id_Estacao
                                    AND e.IdEstacao = '{id}'";

            var dal = new DAL();
            var dtArmazem = dal.RetDataTable(sqlArmazem);

            return dtArmazem.Rows.Count > 0;
        }

        private bool ExisteUnidade(int id)
        {

            var sqlUnidade = $@"SELECT u.IdUnidade
                                  FROM unidademaritima u,
                                       estacaotrabalho e
                                  WHERE e.IdEstacao = u.Id_Estacao
                                    AND e.IdEstacao = '{id}'";

            var dal = new DAL();
            var dtUnidade = dal.RetDataTable(sqlUnidade);

            return dtUnidade.Rows.Count > 0;
        }

        public void ExcluirEstacao(int id)
        {
            string sqlEstacao = string.Empty;
            string sqlTipo = string.Empty;

            if (ExisteArmazem(id))
            {
                sqlTipo = $@"DELETE FROM Armazem WHERE Id_Estacao ='{id}'";

                sqlEstacao = $@"DELETE
                                    FROM EstacaoTrabalho
                                    WHERE IdEstacao = '{id}'";
            }
            else if (ExisteUnidade(id))
            {
                sqlTipo = $@"DELETE FROM UnidadeMaritima WHERE Id_Estacao = '{id}'";

                sqlEstacao = $@"DELETE
                                    FROM EstacaoTrabalho
                                    WHERE IdEstacao = '{id}'";
            }

            var dal = new DAL();

            if (!string.IsNullOrEmpty(sqlEstacao) && !string.IsNullOrEmpty(sqlTipo))
            {
                dal.ExecutarComandoSQL(sqlTipo);
                dal.ExecutarComandoSQL(sqlEstacao);
            }
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

        public EstacaoModel CarregarRegistro(string id)
        {
            var sql = $@"SELECT IdEstacao,
                            (SELECT CASE e.Tipo
                                        WHEN 0 THEN
                                               (SELECT a.Nome
                                                FROM armazem a
                                                WHERE a.Id_Estacao = '{id}')
                                        WHEN 1 THEN
                                               (SELECT u.Nome
                                                FROM unidademaritima u
                                                WHERE u.Id_Estacao = '{id}')
                                        ELSE ('')
                                    END) Nome,
                                 e.Tipo
                          FROM estacaotrabalho e
                          WHERE e.IdEstacao = '{id}'";

            var dal = new DAL();
            var dt = dal.RetDataTable(sql);

            var entity = new EstacaoModel
            {
                Id = dt.Rows[0]["IdEstacao"] != null ? Convert.ToInt32(dt.Rows[0]["IdEstacao"].ToString()) : 0,
                Nome = dt.Rows[0]["Nome"] != null ? dt.Rows[0]["Nome"].ToString() : string.Empty,
                Tipo = dt.Rows[0]["Tipo"] != null ? Convert.ToInt32(dt.Rows[0]["Tipo"].ToString()) : 0

            };

            return entity;
        }


        private int GerarSequencial()
        {
            var sequencial = 0;

            var entacoes = ListaEstacao();

            var listaID = new List<int>();

            if (entacoes.Count > 0)
            {
                foreach (var item in entacoes)
                {
                    listaID.Add(item.Id);
                };

                listaID.Sort();

                sequencial = listaID[listaID.Count - 1];
            }

            return sequencial + 1;
        }

    

        #endregion
    }
}

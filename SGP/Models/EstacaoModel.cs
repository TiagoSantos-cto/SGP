using Microsoft.AspNetCore.Http;
using SGP.Util;
using System;
using System.Collections.Generic;

namespace SGP.Models
{
    public class EstacaoModel
    {
        public int  Id { get; set; }
        public string Nome { get; set; }
        public int Tipo { get; set; }
 
        public IHttpContextAccessor HttpContextAccessor { get; set; }

        public EstacaoModel(){}

        public EstacaoModel(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        public void GravarEstacao()
        {
            Id = GerarSequencial(); 
            
            string sqlEstacao = $"insert into EstacaoTrabalho (IdEstacao, Tipo) values ('{Id}', '{Tipo}')";
           
            string sqlTipo;
            
            if (Tipo == 0)
            {
                sqlTipo = $"INSERT INTO Armazem (Nome, Id_Estacao) VALUES ('{Nome}', '{Id}')";
            }
            else
            {
                sqlTipo = $"INSERT INTO UnidadeMaritima (Nome, Id_Estacao) VALUES ('{Nome}', '{Id}')";
            }
 
            var dal = new DAL();
            dal.ExecutarComandoSQL(sqlEstacao);
            dal.ExecutarComandoSQL(sqlTipo);
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

            return sequencial+1;                 
        }

        public void ExcluirEstacao(int id)
        {
            string sql = $"delete from EstacaoTrabalho where IdEstacao = {id}";
            var dal = new DAL();
            dal.ExecutarComandoSQL(sql);
        }

        public List<EstacaoModel> ListaEstacao()
        {
            var lista = new List<EstacaoModel>();

            var sqlArmazem = $"select E.IdEstacao as ID, E.Tipo as TIPO, A.Nome as NOME from EstacaoTrabalho E, Armazem A where E.IdEstacao = A.Id_Estacao order by E.IdEstacao";
            var sqlUnidadeMaritima = $"select E.IdEstacao as ID, E.Tipo as TIPO, U.Nome as NOME from EstacaoTrabalho E, UnidadeMaritima U where E.IdEstacao = U.Id_Estacao order by E.IdEstacao;";
            
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
    }
}

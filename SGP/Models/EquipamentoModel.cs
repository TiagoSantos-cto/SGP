using Microsoft.AspNetCore.Http;
using SGP.Util;
using System;
using System.Collections.Generic;

namespace SGP.Models
{
    public class EquipamentoModel
    {
        #region PROPRIEDADES

        public string  Id { get; set; }
        public string Descricao { get; set; }
        public string Status { get; set; }
        public int Estacao { get; set; }
        public string NomeEstacao { get; set; }
        public int Quantidade { get; set; }
        public IFormFile Imagem { get; set; }
        public string ImagemPath { get; set; }
        public IHttpContextAccessor HttpContextAccessor { get; set; }

        #endregion

        #region CONTRUTORES
        public EquipamentoModel() { }

        public EquipamentoModel(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }
        #endregion

        #region MÉTODOS

        public void GravarEquipamento()
        {
            string sql;

            if (!Existe(Id))
            {
                sql = $@"INSERT INTO Equipamento (IdEquipamento, Tipo, Descricao, Preco, Quantidade, Status, Estacao, ImagemPath)
                         VALUES ('{Id}','DEFAULT', '{Descricao}', '0.00', '{Quantidade}', '{Status}', '{Estacao}', '{ImagemPath}')";
            }
            else
            {
                UpdateImagemPath(Id);

                sql = $@"UPDATE Equipamento
                         SET Descricao = '{Descricao}',
                             Status = '{Status}',
                             Estacao = '{Estacao}',
                             Quantidade = '{Quantidade}',
                             ImagemPath = '{ImagemPath}'
                         WHERE IdEquipamento = '{Id}'";
            }

            var dal = new DAL();
            dal.ExecutarComandoSQL(sql);
        }

        private void UpdateImagemPath(string id)
        {   
            if (string.IsNullOrEmpty(ImagemPath))
            {
                var sql = @$"SELECT ImagemPath
                             FROM Equipamento  
                        WHERE IdEquipamento = '{id}'";

                var dal = new DAL();
                var dt = dal.RetDataTable(sql);

                ImagemPath = dt.Rows[0]["ImagemPath"] != null ? dt.Rows[0]["ImagemPath"].ToString() : string.Empty;
            }
        }

        public void ExcluirEquipamento(int id)
        {
            string sql = $@"DELETE
                            FROM Equipamento
                            WHERE IdEquipamento = '{id}'";

            var dal = new DAL();
            dal.ExecutarComandoSQL(sql);
        }

        public EquipamentoModel CarregarRegistro(string id)
        {
            var sql = $@"SELECT IdEquipamento,
                                Descricao,
                                Status,
                                Quantidade,
                                Estacao,
                           (SELECT CASE e.Tipo
                                       WHEN 0 THEN
                                              (SELECT a.Nome
                                               FROM armazem a
                                               WHERE a.Id_Estacao = Estacao)
                                       WHEN 1 THEN
                                              (SELECT u.Nome
                                               FROM unidademaritima u
                                               WHERE u.Id_Estacao = Estacao)
                                       ELSE ('')
                                   END
                            FROM estacaotrabalho e
                            WHERE e.IdEstacao = Estacao) AS Nome_Estacao,
                                ImagemPath
                         FROM Equipamento
                         WHERE IdEquipamento = '{id}'";
                                     
            var dal = new DAL();
            var dt = dal.RetDataTable(sql);

            var entity = new EquipamentoModel
            {
                Id = dt.Rows[0]["IdEquipamento"] != null ? dt.Rows[0]["IdEquipamento"].ToString() : string.Empty,
                Descricao = dt.Rows[0]["Descricao"] != null ? dt.Rows[0]["Descricao"].ToString() : string.Empty,
                Estacao = dt.Rows[0]["Estacao"] != null ? Convert.ToInt32(dt.Rows[0]["Estacao"].ToString()) : 0,
                NomeEstacao = dt.Rows[0]["Nome_Estacao"] != null ? dt.Rows[0]["Nome_Estacao"].ToString() : string.Empty,
                Status = dt.Rows[0]["Status"] != null ? dt.Rows[0]["Status"].ToString() : string.Empty,
                Quantidade = dt.Rows[0]["Quantidade"] != null ? Convert.ToInt32(dt.Rows[0]["Quantidade"].ToString()) : 0,
                ImagemPath = dt.Rows[0]["ImagemPath"] != null ? dt.Rows[0]["ImagemPath"].ToString() : string.Empty       
        };

            return entity;
        }

        public List<EquipamentoModel> ListaEquipamento()
        {
            var lista = new List<EquipamentoModel>();

            var sql = $@"SELECT IdEquipamento,
                                Descricao,
                                Status,
                                Quantidade,
                                Estacao,
                                ImagemPath
                         FROM Equipamento";
           
            var dal = new DAL();
            var dt = dal.RetDataTable(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var item = new EquipamentoModel
                {
                    Id = dt.Rows[i]["IdEquipamento"].ToString(),
                    Descricao = dt.Rows[i]["Descricao"].ToString(),
                    Status = dt.Rows[i]["Status"].ToString(),
                    Estacao = Convert.ToInt32(dt.Rows[i]["Estacao"].ToString()),
                    Quantidade = Convert.ToInt32(dt.Rows[i]["Quantidade"].ToString()),
                    ImagemPath = dt.Rows[i]["ImagemPath"].ToString()
                };

                lista.Add(item);
            }
            return lista;
        }

        public bool Existe(string id)
        {
            var sql = $@"SELECT IdEquipamento
                          FROM Equipamento
                          WHERE IdEquipamento = '{id}'";

            var dal = new DAL();
            var dt = dal.RetDataTable(sql);

            return dt.Rows.Count > 0;
        }

        #endregion
    }
}

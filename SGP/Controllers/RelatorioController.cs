using Microsoft.AspNetCore.Mvc;
using PdfSharpCore.Drawing;
using SGP.Models;
using System.IO;

namespace SGP.Controllers
{
    public class RelatorioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Historico()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Movimentacoes()
        {
            return View();
        }

        public IActionResult ImprimirMovimentacoes(RequisicaoModel entity)
        {
            using (var doc = new PdfSharpCore.Pdf.PdfDocument())
            {
                var page = doc.AddPage();
                page.Size = PdfSharpCore.PageSize.Crown;
                page.Orientation = PdfSharpCore.PageOrientation.Portrait;
                var graphics = XGraphics.FromPdfPage(page);
                var corFonte = XBrushes.Black;

                var textFormatter = new PdfSharpCore.Drawing.Layout.XTextFormatter(graphics);
                var fonteOrganzacao = new XFont("Arial", 16);
                var fonteDescricao = new XFont("Arial", 16, XFontStyle.BoldItalic);
                var titulodetalhes = new XFont("Arial", 28, XFontStyle.Bold);
                var fonteDetalhesDescricao = new XFont("Arial", 14);

                var qtdPaginas = doc.PageCount;
                textFormatter.DrawString(qtdPaginas.ToString(), new XFont("Arial", 10), corFonte, new PdfSharpCore.Drawing.XRect(578, 825, page.Width, page.Height));


                var fonteTitulo = new XFont("Arial", 20, XFontStyle.Bold);

                // CABEÇALHO
                textFormatter.DrawString("MOVIMENTAÇÕES DE REQUISIÇÕES", fonteTitulo, corFonte, new XRect(20, 30, page.Width, page.Height));

                textFormatter.DrawString("Status: ", fonteDescricao, corFonte, new XRect(20, 70, page.Width, page.Height));
                textFormatter.DrawString(entity.Status, fonteOrganzacao, corFonte, new XRect(85, 69, page.Width, page.Height));

                textFormatter.DrawString("Período: ", fonteDescricao, corFonte, new XRect(20, 90, page.Width, page.Height));
                textFormatter.DrawString(entity.Data +" até "+ entity.DataFinal , fonteOrganzacao, corFonte, new XRect(105, 89, page.Width, page.Height));

                // DETALHE
                var tituloDetalhes = new PdfSharpCore.Drawing.Layout.XTextFormatter(graphics);
                tituloDetalhes.Alignment = PdfSharpCore.Drawing.Layout.XParagraphAlignment.Center;

                // COLUNAS
                var alturaTituloDetalhesY = 140;
                var detalhes = new PdfSharpCore.Drawing.Layout.XTextFormatter(graphics);

                detalhes.DrawString("Número", fonteDescricao, corFonte, new XRect(20, alturaTituloDetalhesY, page.Width, page.Height));
                
                detalhes.DrawString("Descrição", fonteDescricao, corFonte, new XRect(100, alturaTituloDetalhesY, page.Width, page.Height));

                detalhes.DrawString("Tipo", fonteDescricao, corFonte, new XRect(630, alturaTituloDetalhesY, page.Width, page.Height));

                detalhes.DrawString("Origem", fonteDescricao, corFonte, new XRect(710, alturaTituloDetalhesY, page.Width, page.Height));

                detalhes.DrawString("Destino", fonteDescricao, corFonte, new XRect(850, alturaTituloDetalhesY, page.Width, page.Height));

                detalhes.DrawString("Status", fonteDescricao, corFonte, new XRect(1020, alturaTituloDetalhesY, page.Width, page.Height));

                detalhes.DrawString("Usuário", fonteDescricao, corFonte, new XRect(1200, alturaTituloDetalhesY, page.Width, page.Height));

                var dados = entity.ListaRequisicao();

                // DADOS
                var alturaDetalhesItens = 160;
                foreach (var item in dados)
                {
                    textFormatter.DrawString(item.Id.ToString(), fonteDetalhesDescricao, corFonte, new XRect(21, alturaDetalhesItens, page.Width, page.Height));
                    textFormatter.DrawString(item.Descricao, fonteDetalhesDescricao, corFonte, new XRect(101, alturaDetalhesItens, page.Width, page.Height));
                    textFormatter.DrawString(item.Tipo == "T" ? "Transbordo" : "Backload", fonteDetalhesDescricao, corFonte, new XRect(631, alturaDetalhesItens, page.Width, page.Height));
                    textFormatter.DrawString(item.NomeEstacaoOrigem, fonteDetalhesDescricao, corFonte, new XRect(710, alturaDetalhesItens, page.Width, page.Height));
                    textFormatter.DrawString(item.NomeEstacaoDestino, fonteDetalhesDescricao, corFonte, new XRect(850, alturaDetalhesItens, page.Width, page.Height));
                    textFormatter.DrawString(item.Status, fonteDetalhesDescricao, corFonte, new XRect(1021, alturaDetalhesItens, page.Width, page.Height));
                    textFormatter.DrawString(item.NomeUsuarioAtual, fonteDetalhesDescricao, corFonte, new XRect(1200, alturaDetalhesItens, page.Width, page.Height));

                    alturaDetalhesItens += 20;
                }

                //DOWNLOAD
                using (MemoryStream stream = new MemoryStream())
                {
                    var contantType = "application/pdf";
                    doc.Save(stream, false);

                    var nomeArquivo = "Movimentacoes.pdf";

                    return File(stream.ToArray(), contantType, nomeArquivo);

                }
            }         
        }

        public IActionResult ImprimirHistorico(RequisicaoModel entity)
        {
       
            using (var doc = new PdfSharpCore.Pdf.PdfDocument())
            {
                var page = doc.AddPage();
                page.Size = PdfSharpCore.PageSize.A4;
                page.Orientation = PdfSharpCore.PageOrientation.Portrait;
                var graphics = XGraphics.FromPdfPage(page);
                var corFonte = XBrushes.Black;

                var textFormatter = new PdfSharpCore.Drawing.Layout.XTextFormatter(graphics);
                var fonteOrganzacao = new XFont("Arial", 10);
                var fonteDescricao = new XFont("Arial", 8, XFontStyle.BoldItalic);
                var titulodetalhes = new XFont("Arial", 14, XFontStyle.Bold);
                var fonteDetalhesDescricao = new XFont("Arial", 7);

                var qtdPaginas = doc.PageCount;
                textFormatter.DrawString(qtdPaginas.ToString(), new XFont("Arial", 10), corFonte, new PdfSharpCore.Drawing.XRect(578, 825, page.Width, page.Height));


                var cabecalho = entity.CarregarRegistro(entity.Id);

                var fonteTitulo = new XFont("Arial", 20, XFontStyle.Bold);

                // CABEÇALHO
                textFormatter.DrawString("HISTÓRICO DE REQUISIÇÃO", fonteTitulo, corFonte, new XRect(20, 30, page.Width, page.Height));           
                                                                                              // horizontal, vertical 
                textFormatter.DrawString("Código requisição: ", fonteDescricao, corFonte, new XRect(20, 70, page.Width, page.Height));
                textFormatter.DrawString(cabecalho.Id.ToString(), fonteOrganzacao, corFonte, new XRect(95, 69, page.Width, page.Height));
                
                textFormatter.DrawString("Usuário requisitante: ", fonteDescricao, corFonte, new XRect(20, 90, page.Width, page.Height));
                textFormatter.DrawString(cabecalho.NomeUsuarioResponsavel, fonteOrganzacao, corFonte, new XRect(105, 89, page.Width, page.Height));

                textFormatter.DrawString("Data de abertura: ", fonteDescricao, corFonte, new XRect(20, 110, page.Width, page.Height));
                textFormatter.DrawString(cabecalho.Data.ToString(), fonteOrganzacao, corFonte, new XRect(95, 109, page.Width, page.Height));

                // DETALHE
                var tituloDetalhes = new PdfSharpCore.Drawing.Layout.XTextFormatter(graphics);
                tituloDetalhes.Alignment = PdfSharpCore.Drawing.Layout.XParagraphAlignment.Center;

                // COLUNAS
                var alturaTituloDetalhesY = 140;
                var detalhes = new PdfSharpCore.Drawing.Layout.XTextFormatter(graphics);

                detalhes.DrawString("Data de alteração", fonteDescricao, corFonte, new XRect(20, alturaTituloDetalhesY, page.Width, page.Height));

                detalhes.DrawString("Descrição", fonteDescricao, corFonte, new XRect(200, alturaTituloDetalhesY, page.Width, page.Height));
            
                detalhes.DrawString("Usuário", fonteDescricao, corFonte, new XRect(500, alturaTituloDetalhesY, page.Width, page.Height));

                var dados = entity.ObterHistorico(entity.Id);

                //DADOS
                var alturaDetalhesItens = 160;
                foreach (var item in dados)
                {
                    textFormatter.DrawString(item.DataAlteracao.ToString(), fonteDetalhesDescricao, corFonte, new XRect(21, alturaDetalhesItens, page.Width, page.Height));
                    textFormatter.DrawString(item.Descricao, fonteDetalhesDescricao, corFonte, new XRect(201, alturaDetalhesItens, page.Width, page.Height));
                    textFormatter.DrawString(item.NomeUsuario, fonteDetalhesDescricao, corFonte, new XRect(501, alturaDetalhesItens, page.Width, page.Height));

                    alturaDetalhesItens += 20;
                }

                //DOWNLOAD
                using (MemoryStream stream = new MemoryStream())
                {
                    var contantType = "application/pdf";
                    doc.Save(stream, false);

                    var nomeArquivo = "Historico.pdf";

                    return File(stream.ToArray(), contantType, nomeArquivo);

                }
            }
        }
    }
}

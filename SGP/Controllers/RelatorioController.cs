using Microsoft.AspNetCore.Mvc;
using PdfSharpCore.Drawing;
using SGP.Models;
using SGP.Util;
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
        
        public IActionResult ImprimirMovimentacoes(RequisicaoModel entity)
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

                // LOGO EMPRESA
                //var logo = @"..\wwwroot\upload\porto-do-acu-logo-relatorio.png";
                //XImage imagem = XImage.FromFile(logo);
                //graphics.DrawImage(imagem, 20, 5, 70, 50);
             
                // CABEÇALHO
                textFormatter.DrawString("Status: ", fonteDescricao, corFonte, new XRect(20, 75, page.Width, page.Height));
                textFormatter.DrawString(entity.Status, fonteOrganzacao, corFonte, new XRect(80, 75, page.Width, page.Height));

                textFormatter.DrawString("Período: ", fonteDescricao, corFonte, new XRect(20, 115, page.Width, page.Height));
                textFormatter.DrawString(entity.Data +" até "+ entity.DataFinal , fonteOrganzacao, corFonte, new XRect(80, 115, page.Width, page.Height));

                // DETALHE
                var tituloDetalhes = new PdfSharpCore.Drawing.Layout.XTextFormatter(graphics);
                tituloDetalhes.Alignment = PdfSharpCore.Drawing.Layout.XParagraphAlignment.Center;

                // COLUNAS
                var alturaTituloDetalhesY = 140;
                var detalhes = new PdfSharpCore.Drawing.Layout.XTextFormatter(graphics);

                detalhes.DrawString("Código", fonteDescricao, corFonte, new XRect(20, alturaTituloDetalhesY, page.Width, page.Height));
                
                detalhes.DrawString("Descrição", fonteDescricao, corFonte, new XRect(60, alturaTituloDetalhesY, page.Width, page.Height));

                detalhes.DrawString("Tipo", fonteDescricao, corFonte, new XRect(230, alturaTituloDetalhesY, page.Width, page.Height));

                detalhes.DrawString("Origem", fonteDescricao, corFonte, new XRect(280, alturaTituloDetalhesY, page.Width, page.Height));

                detalhes.DrawString("Destino", fonteDescricao, corFonte, new XRect(380, alturaTituloDetalhesY, page.Width, page.Height));

                detalhes.DrawString("Status", fonteDescricao, corFonte, new XRect(470, alturaTituloDetalhesY, page.Width, page.Height));

                detalhes.DrawString("Usuário", fonteDescricao, corFonte, new XRect(540, alturaTituloDetalhesY, page.Width, page.Height));

                var dados = entity.ListaRequisicao();

                // DADOS TESTES
                var alturaDetalhesItens = 160;
                foreach (var item in dados)
                {
                    textFormatter.DrawString(item.Id.ToString(), fonteDetalhesDescricao, corFonte, new XRect(21, alturaDetalhesItens, page.Width, page.Height));
                    textFormatter.DrawString(item.Descricao, fonteDetalhesDescricao, corFonte, new XRect(61, alturaDetalhesItens, page.Width, page.Height));
                    textFormatter.DrawString(item.Tipo == "T" ? "Transbordo" : "Backload", fonteDetalhesDescricao, corFonte, new XRect(231, alturaDetalhesItens, page.Width, page.Height));
                    textFormatter.DrawString(item.Origem, fonteDetalhesDescricao, corFonte, new XRect(281, alturaDetalhesItens, page.Width, page.Height));
                    textFormatter.DrawString(item.Destino, fonteDetalhesDescricao, corFonte, new XRect(381, alturaDetalhesItens, page.Width, page.Height));
                    textFormatter.DrawString(item.Status, fonteDetalhesDescricao, corFonte, new XRect(471, alturaDetalhesItens, page.Width, page.Height));
                    textFormatter.DrawString(item.NomeUsuarioAtual, fonteDetalhesDescricao, corFonte, new XRect(541, alturaDetalhesItens, page.Width, page.Height));

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

                // LOGO EMPRESA
                //var logo = @"..\wwwroot\upload\porto-do-acu-logo-relatorio.png";
                //XImage imagem = XImage.FromFile(logo);
                //graphics.DrawImage(imagem, 20, 5, 70, 50);

                var cabecalho = entity.CarregarRegistro(entity.Id);

                // CABEÇALHO                                                        // horizontal, vertical 
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
            
                detalhes.DrawString("Usuário", fonteDescricao, corFonte, new XRect(300, alturaTituloDetalhesY, page.Width, page.Height));

                var dados = entity.ObterHistorico(entity.Id);

                // DADOS
                var alturaDetalhesItens = 160;
                foreach (var item in dados)
                {
                    textFormatter.DrawString(item.DataAlteracao.ToString(), fonteDetalhesDescricao, corFonte, new XRect(21, alturaDetalhesItens, page.Width, page.Height));
                    textFormatter.DrawString(item.Descricao, fonteDetalhesDescricao, corFonte, new XRect(201, alturaDetalhesItens, page.Width, page.Height));
                    textFormatter.DrawString(item.NomeUsuario, fonteDetalhesDescricao, corFonte, new XRect(301, alturaDetalhesItens, page.Width, page.Height));

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

#pragma checksum "C:\Users\Tiago\source\repos\TiagoSantos-cto\SGP\SGP\Views\Requisicao\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "cdf392aa3446b73a88c8ea4e055c7c8a6b5cb23d"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Requisicao_Index), @"mvc.1.0.view", @"/Views/Requisicao/Index.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\Tiago\source\repos\TiagoSantos-cto\SGP\SGP\Views\_ViewImports.cshtml"
using SGP;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\Tiago\source\repos\TiagoSantos-cto\SGP\SGP\Views\_ViewImports.cshtml"
using SGP.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"cdf392aa3446b73a88c8ea4e055c7c8a6b5cb23d", @"/Views/Requisicao/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"1c1faec8a5df0c1aa8bee470858d5033e2222604", @"/Views/_ViewImports.cshtml")]
    public class Views_Requisicao_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<RequisicaoModel>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "C:\Users\Tiago\source\repos\TiagoSantos-cto\SGP\SGP\Views\Requisicao\Index.cshtml"
  
    ViewData["Title"] = "Requisição";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n");
#nullable restore
#line 8 "C:\Users\Tiago\source\repos\TiagoSantos-cto\SGP\SGP\Views\Requisicao\Index.cshtml"
  
    var vId = 0;

    try
    {
        vId = Convert.ToInt32(@ViewBag.Registro.Id);
    }
    catch { }

#line default
#line hidden
#nullable disable
            WriteLiteral(@"

<div class=""alert-success container body-content text-center"" style=""padding-top: 100px; bottom:auto; border-radius: 10px; text-align: center; padding: 20px; width: 500px; font-family: Calibri "">
    <h3><strong>Operação realizada com sucesso. Número: ");
#nullable restore
#line 20 "C:\Users\Tiago\source\repos\TiagoSantos-cto\SGP\SGP\Views\Requisicao\Index.cshtml"
                                                   Write(vId);

#line default
#line hidden
#nullable disable
            WriteLiteral("  </strong> </h3>\r\n    <h6><a href=\"..\\..\\Requisicao\\Informacao\">Clique aqui</a> para voltar ou retorne para o <a href=\"..\\..\\Home\\Index\"> Menu inicial</a>.</h6>\r\n</div>\r\n\r\n\r\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<RequisicaoModel> Html { get; private set; }
    }
}
#pragma warning restore 1591

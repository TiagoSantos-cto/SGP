#pragma checksum "C:\Users\Tiago Santos\SGP\SGP\Views\Embarcacao\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "a967802c31c0c386b2096603c1abafc70e3a523b"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Embarcacao_Index), @"mvc.1.0.view", @"/Views/Embarcacao/Index.cshtml")]
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
#line 1 "C:\Users\Tiago Santos\SGP\SGP\Views\_ViewImports.cshtml"
using SGP;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\Tiago Santos\SGP\SGP\Views\_ViewImports.cshtml"
using SGP.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a967802c31c0c386b2096603c1abafc70e3a523b", @"/Views/Embarcacao/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"1c1faec8a5df0c1aa8bee470858d5033e2222604", @"/Views/_ViewImports.cshtml")]
    public class Views_Embarcacao_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/images/navio-icon.png"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("width", new global::Microsoft.AspNetCore.Html.HtmlString("63"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("height", new global::Microsoft.AspNetCore.Html.HtmlString("63"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 1 "C:\Users\Tiago Santos\SGP\SGP\Views\Embarcacao\Index.cshtml"
  
    ViewData["Title"] = "Embarcações de apoio";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<div class=\"row text-left\" style=\"padding-left: 30px;\">\r\n    <div>\r\n        <h2>Embarcações de apoio</h2>\r\n        <h6>Todas os embarcações cadastradas:</h6>\r\n    </div>\r\n    <div style=\"padding-left: 30px;\">\r\n        ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("img", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "a967802c31c0c386b2096603c1abafc70e3a523b4506", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral(@"
    </div>
</div>
<br />

<div style=""background-color: ghostwhite; border-radius: 10px; padding:20px;"">
    <table class=""table table-striped"">
        <thead>
            <tr>
                <th> </th>
                <th>Nome</th>
                <th>Catedoria</th>
                <th>Capacidade máxima (t)</th>
            </tr>
        </thead>

");
#nullable restore
#line 27 "C:\Users\Tiago Santos\SGP\SGP\Views\Embarcacao\Index.cshtml"
          
            foreach (var item in (List<EmbarcacaoModel>)ViewBag.ListaEmbarcacao)
            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                <tbody>\r\n                    <tr>\r\n                        <td><button type=\"button\" class=\" btn btn-primary\"");
            BeginWriteAttribute("onclick", " onclick=\"", 940, "\"", 979, 3);
            WriteAttributeValue("", 950, "Editar(\'", 950, 8, true);
#nullable restore
#line 32 "C:\Users\Tiago Santos\SGP\SGP\Views\Embarcacao\Index.cshtml"
WriteAttributeValue("", 958, item.Id.ToString(), 958, 19, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 977, "\')", 977, 2, true);
            EndWriteAttribute();
            WriteLiteral(">Atualizar</button></td>\r\n                        <td>");
#nullable restore
#line 33 "C:\Users\Tiago Santos\SGP\SGP\Views\Embarcacao\Index.cshtml"
                       Write(item.Nome.ToString());

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                        <td><strong>");
#nullable restore
#line 34 "C:\Users\Tiago Santos\SGP\SGP\Views\Embarcacao\Index.cshtml"
                               Write(item.Categoria.ToString());

#line default
#line hidden
#nullable disable
            WriteLiteral("</strong></td>\r\n                        <td>");
#nullable restore
#line 35 "C:\Users\Tiago Santos\SGP\SGP\Views\Embarcacao\Index.cshtml"
                       Write(item.Capacidade.ToString());

#line default
#line hidden
#nullable disable
            WriteLiteral("t</td>\r\n                    </tr>\r\n                </tbody>\r\n");
#nullable restore
#line 38 "C:\Users\Tiago Santos\SGP\SGP\Views\Embarcacao\Index.cshtml"
            }
        

#line default
#line hidden
#nullable disable
            WriteLiteral(@"    </table>
</div>
<br />

<button type=""button"" class=""btn btn-block btn-success"" onclick=""Registrar()"">Novo equipamento</button>


<script>
    function Registrar() {
        window.location.href = ""../Embarcacao/Registrar"";
    }

    function Editar(id) {
        window.location.href = ""../Embarcacao/Registrar/"" + id;
    }
</script>



");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591

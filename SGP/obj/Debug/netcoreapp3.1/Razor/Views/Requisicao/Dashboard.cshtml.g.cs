#pragma checksum "C:\Users\Tiago\source\repos\TiagoSantos-cto\SGP\SGP\Views\Requisicao\Dashboard.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "1d932f526338eb96a0f53ee6d0a56708c609fea5"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Requisicao_Dashboard), @"mvc.1.0.view", @"/Views/Requisicao/Dashboard.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"1d932f526338eb96a0f53ee6d0a56708c609fea5", @"/Views/Requisicao/Dashboard.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"1c1faec8a5df0c1aa8bee470858d5033e2222604", @"/Views/_ViewImports.cshtml")]
    public class Views_Requisicao_Dashboard : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral(@"
<h2>Dashboard</h2>
<br />

<script src=""https://cdnjs.cloudflare.com/ajax/libs/Chart.js/2.7.2/Chart.bundle.min.js""></script>

<div id=""canvas-holder"" style=""width:80%; background-color: ghostwhite; border-radius: 10px; padding:20px;"">
    <canvas id=""chart-area""></canvas>
</div>

<script>
    var randomScalingFactor = function () {
        return Math.round(Math.random() * 100);
    };

    var config = {
        type: 'pie',
        data: {
            datasets: [{
                data: [
                    ");
#nullable restore
#line 21 "C:\Users\Tiago\source\repos\TiagoSantos-cto\SGP\SGP\Views\Requisicao\Dashboard.cshtml"
               Write(Html.Raw(ViewBag.Valores));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                ],\r\n                backgroundColor: [\r\n                    ");
#nullable restore
#line 24 "C:\Users\Tiago\source\repos\TiagoSantos-cto\SGP\SGP\Views\Requisicao\Dashboard.cshtml"
               Write(Html.Raw(ViewBag.Cores));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                ],\r\n                label: \'Dataset 1\'\r\n            }],\r\n            labels: [\r\n                ");
#nullable restore
#line 29 "C:\Users\Tiago\source\repos\TiagoSantos-cto\SGP\SGP\Views\Requisicao\Dashboard.cshtml"
           Write(Html.Raw(ViewBag.Labels));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
            ]
        },
        options: {
            responsive: true
        }
    };

    window.onload = function () {
        var ctx = document.getElementById('chart-area').getContext('2d');
        window.myPie = new Chart(ctx, config);
    };
</script>");
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

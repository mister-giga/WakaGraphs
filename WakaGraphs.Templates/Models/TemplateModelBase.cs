using Razor.Templating.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WakaGraphs.Templates.Models
{
    public abstract class TemplateModelBase
    {
        public Task<string> GetStringAsync() => RazorTemplateEngine.RenderAsync($"~/Views/{GetType().Name}.cshtml", this);
    }
}

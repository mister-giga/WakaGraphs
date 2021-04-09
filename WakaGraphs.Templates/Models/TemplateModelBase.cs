using Razor.Templating.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WakaGraphs.Templates.Models
{
    public abstract class TemplateModelBase
    {
        public async Task GenerateAsync(string dir)
        {
            var name = GetType().Name;

            var content = await RazorTemplateEngine.RenderAsync($"~/Views/{name}.cshtml", this);
            await File.WriteAllTextAsync(Path.Combine(dir, $"{name}.svg"), content);
        }
    }
}

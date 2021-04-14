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
        readonly string name;
        public TemplateModelBase()
        {
            name = GetType().Name;
        }
        public async Task GenerateAsync(string dir)
        {
            var content = await RazorTemplateEngine.RenderAsync($"~/Views/{name}.cshtml", this);
            await File.WriteAllTextAsync(Path.Combine(dir, $"{name}.svg"), content);
        }

        public string GetUrl(string username, string repo, string branch, string dir) => $"https://raw.githubusercontent.com/{username}/{repo}/{branch}/{dir}/{name}.svg";

        public string GetSample(string username, string repo, string branch, string dir)
        {
            var url = GetUrl(username, repo, branch, dir);
         return @$"<img src='{url}'/>

``` HTML
<img src='{url}'/>
```";
        }
    }
}

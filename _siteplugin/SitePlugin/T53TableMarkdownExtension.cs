//
// BSATroop53 Website Plugin - Extensions to Pretzel.
// Copyright (C) 2024 Seth Hendrick
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published
// by the Free Software Foundation, either version 3 of the License, or
// any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
// 
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using KristofferStrube.ActivityStreams;
using Markdig;
using Markdig.Extensions.Tables;
using Markdig.Renderers;
using Markdig.Renderers.Html;

namespace SitePlugin
{
    internal sealed class T53TableMarkdownExtension : IMarkdownExtension
    {
        public void Setup( MarkdownPipelineBuilder pipeline )
        {
        }

        public void Setup( MarkdownPipeline pipeline, IMarkdownRenderer renderer )
        {
            var htmlRender = renderer as HtmlRenderer;
            if( htmlRender is null )
            {
                return;
            }

            var tableRenderer = renderer.ObjectRenderers.FindExact<HtmlTableRenderer>();
            if( tableRenderer is null )
            {
                return;
            }

            tableRenderer.TryWriters.Remove( TryTableInlineRenderer );
            tableRenderer.TryWriters.Add( TryTableInlineRenderer );
        }

        private bool TryTableInlineRenderer( HtmlRenderer renderer, Table table )
        {
            table.SetAttributes(
                new HtmlAttributes
                {
                    Classes = new List<string>
                    {
                        "table",
                        "table-striped",
                        "table-bordered",
                        "table-hover"
                    }
                }
            );

            return false;
        }
    }
}

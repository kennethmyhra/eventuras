using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.NodeServices;

namespace losol.EventManagement.Services.Pdf
{
    [Obsolete("Use Converto instead")]
    public class NodePdfRenderService : IPdfRenderService
    {
        private const string Script = "./Node/writeToPdf";

        private readonly INodeServices _nodeServices;

        public NodePdfRenderService(INodeServices nodeServices)
        {
            this._nodeServices = nodeServices;
        }

        public async Task<Stream> RenderHtmlAsync(string html, PdfRenderOptions pdfRenderOptions)
        {
            return await _nodeServices.InvokeAsync<Stream>(
                Script,
                html,
                new
                {
                    format = pdfRenderOptions.Format,
                    timeout = 50_000,
                    zoomFactor = pdfRenderOptions.Scale?.ToString()
                }
            );
        }
    }
}

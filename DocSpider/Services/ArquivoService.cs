using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DocSpider.Services
{
    public class ArquivoService : IArquivoService
    {
        private readonly string _uploadsPath;

        public ArquivoService()
        {
            _uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

            if (!Directory.Exists(_uploadsPath))
                Directory.CreateDirectory(_uploadsPath);
        }

        public async Task<(string caminhoVirtual, string nomeOriginal)> SalvarArquivoAsync(IFormFile arquivo)
        {
            var extensao = Path.GetExtension(arquivo.FileName);
            var nomeUnico = Guid.NewGuid().ToString() + extensao;
            var caminhoCompleto = Path.Combine(_uploadsPath, nomeUnico);

            using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
            {
                await arquivo.CopyToAsync(stream);
            }

            return ($"uploads/{nomeUnico}", Path.GetFileName(arquivo.FileName));
        }

        public async Task<FileStreamResult> BaixarArquivoAsync(string caminho, string nomeExibido)
        {
            var caminhoCompleto = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", caminho);
            if (!File.Exists(caminhoCompleto))
                throw new FileNotFoundException("Arquivo não encontrado.");

            var stream = new FileStream(caminhoCompleto, FileMode.Open, FileAccess.Read);
            var contentType = "application/octet-stream";

            return new FileStreamResult(stream, contentType)
            {
                FileDownloadName = nomeExibido ?? Path.GetFileName(caminhoCompleto)
            };
        }
    }
}

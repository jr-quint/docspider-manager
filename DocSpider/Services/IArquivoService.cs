using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DocSpider.Services
{
    public interface IArquivoService
    {
        Task<(string caminhoVirtual, string nomeOriginal)> SalvarArquivoAsync(IFormFile arquivo);
        Task<FileStreamResult> BaixarArquivoAsync(string caminho, string nomeExibido);
    }
}

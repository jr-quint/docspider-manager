using System.Collections.Generic;
using System.Threading.Tasks;
using DocSpider.Models;

namespace DocSpider.Services
{
    public interface IDocumentosService
    {
        Task<List<DocumentosModel>> ListarAtivosAsync();
        Task<DocumentosModel> ObterPorIdAsync(int id);
        Task<bool> TituloExisteAsync(string titulo);
        Task CriarAsync(DocumentosModel model);
        Task AtualizarAsync(DocumentosModel model);
        Task ExcluirLogicamenteAsync(int id);
    }
}

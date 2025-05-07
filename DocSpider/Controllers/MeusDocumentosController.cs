using System.Diagnostics;
using System.Threading.Tasks;
using DocSpider.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DocSpider.Services;
using System.IO;
using System;
using System.Linq;
using Microsoft.AspNetCore.Http.Extensions;

namespace DocSpider.Controllers
{
    public class MeusDocumentosController : Controller
    {
        private readonly ILogger<MeusDocumentosController> _logger;
        private readonly IDocumentosService _documentosService;
        private readonly IArquivoService _arquivoService;

        public MeusDocumentosController(
            ILogger<MeusDocumentosController> logger,
            IDocumentosService documentosService,
            IArquivoService arquivoService)
        {
            _logger = logger;
            _documentosService = documentosService;
            _arquivoService = arquivoService;
        }

        #region Listagem
        public async Task<IActionResult> Index(string titulo = null, string descricao = null, string nomeArquivo = null, DateTime? dataCriacaoInicio = null, DateTime? dataCriacaoFim = null)
        {
            var documentos = await _documentosService.ListarAtivosAsync();

            if (!string.IsNullOrWhiteSpace(titulo))
                documentos = documentos.Where(d => !string.IsNullOrEmpty(d.Titulo) && d.Titulo.Contains(titulo, StringComparison.OrdinalIgnoreCase)).ToList();

            if (!string.IsNullOrWhiteSpace(descricao))
                documentos = documentos.Where(d => !string.IsNullOrEmpty(d.Descricao) && d.Descricao.Contains(descricao, StringComparison.OrdinalIgnoreCase)).ToList();

            if (!string.IsNullOrWhiteSpace(nomeArquivo))
                documentos = documentos.Where(d => !string.IsNullOrEmpty(d.NomeDoArquivo) && d.NomeDoArquivo.Contains(nomeArquivo, StringComparison.OrdinalIgnoreCase)).ToList();

            if (dataCriacaoInicio.HasValue)
                documentos = documentos.Where(d => d.DataCriacao.Date >= dataCriacaoInicio.Value).ToList();

            if (dataCriacaoFim.HasValue)
                documentos = documentos.Where(d => d.DataCriacao.Date <= dataCriacaoFim.Value).ToList();

            return View(documentos);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var documento = await _documentosService.ObterPorIdAsync(id.Value);
            if (documento == null)
                return NotFound();

            return View(documento);
        }
        #endregion

        #region Criação
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DocumentosModel model)
        {
            if (await _documentosService.TituloExisteAsync(model.Titulo))
                ModelState.AddModelError("Titulo", "Já existe um documento com este título.");

            if (model.Upload != null)
            {
                if (model.Upload.Length > 10 * 1024 * 1024)
                {
                    ModelState.AddModelError("Upload", "O arquivo deve ter no máximo 10MB.");
                }
                else
                {
                    var extensao = Path.GetExtension(model.Upload.FileName).ToLowerInvariant();
                    if (new[] { ".exe", ".zip", ".bat" }.Contains(extensao))
                    {
                        ModelState.AddModelError("Upload", "Tipo de arquivo não permitido.");
                    }
                    else
                    {
                        var (caminhoVirtual, nomeOriginal) = await _arquivoService.SalvarArquivoAsync(model.Upload);
                        var nomeDoArquivo = model.NomeDoArquivo ?? nomeOriginal;

                        model.Arquivo = caminhoVirtual;
                        model.NomeDoArquivo = nomeDoArquivo;
                    }
                }
            }

            if (!ModelState.IsValid)
                return View(model);

            await _documentosService.CriarAsync(model);
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Edição
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null)
                return NotFound();

            var documento = await _documentosService.ObterPorIdAsync(id.Value);
            if (documento == null)
                return NotFound();

            return View(documento);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DocumentosModel model)
        {
            if (id != model.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(model);

            if (model.Upload != null && model.Upload.Length > 0)
            {
                if (!FileValidationHelper.IsFileTypeAllowed(model.Upload.FileName))
                {
                    ModelState.AddModelError("Upload", "Tipo de arquivo não permitido. Arquivos .exe, .zip e .bat são proibidos.");
                    return View(model);
                }
                else
                {
                    var (caminhoVirtual, nomeOriginal) = await _arquivoService.SalvarArquivoAsync(model.Upload);
                    var nomeDoArquivo = model.NomeDoArquivo ?? nomeOriginal;

                    model.Arquivo = caminhoVirtual;
                    model.NomeDoArquivo = nomeDoArquivo;
                }
            }

            try
            {
                await _documentosService.AtualizarAsync(model);
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Message.Contains("título", StringComparison.OrdinalIgnoreCase))
                    ModelState.AddModelError("Titulo", ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "Erro ao atualizar o documento.");

                return View(model);
            }
        }
        #endregion

        #region Exclusão
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var documento = await _documentosService.ObterPorIdAsync(id.Value);
            if (documento == null)
                return NotFound();

            return View(documento);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _documentosService.ExcluirLogicamenteAsync(id);
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Arquivo
        public async Task<IActionResult> Download(int id)
        {
            var documento = await _documentosService.ObterPorIdAsync(id);
            if (documento == null)
                return NotFound();

            try
            {
                return await _arquivoService.BaixarArquivoAsync(documento.Arquivo, documento.NomeDoArquivo);
            }
            catch (FileNotFoundException)
            {
                return View("ArquivoNaoEncontrado", documento);                
            }
        }
        #endregion

        #region Outros
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #endregion
    }
}
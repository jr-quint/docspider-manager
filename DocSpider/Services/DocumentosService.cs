using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocSpider.Data;
using DocSpider.Models;
using Microsoft.EntityFrameworkCore;

namespace DocSpider.Services
{
    public class DocumentosService : IDocumentosService
    {
        private readonly ApplicationDbContext _context;

        public DocumentosService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Listar
        public async Task<List<DocumentosModel>> ListarAtivosAsync()
        {
            return await _context.Documentos
                .Where(d => d.Flag == 1)
                .ToListAsync();
        }

        // Por ID
        public async Task<DocumentosModel> ObterPorIdAsync(int id)
        {
            return await _context.Documentos
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.Id == id && d.Flag == 1);
        }

        // Validação Título
        public async Task<bool> TituloExisteAsync(string titulo)
        {
            return await _context.Documentos
                .AnyAsync(d => d.Titulo == titulo && d.Flag == 1);
        }

        // Cadastrar
        public async Task CriarAsync(DocumentosModel model)
        {
            model.DataCriacao = DateTime.Now;
            model.Flag = 1;

            _context.Documentos.Add(model);
            await _context.SaveChangesAsync();
        }

        // Editar
        public async Task AtualizarAsync(DocumentosModel model)
        {
            var original = await _context.Documentos.AsNoTracking()
                .FirstOrDefaultAsync(d => d.Id == model.Id && d.Flag == 1);

            if (original == null)
                throw new InvalidOperationException("Documento não encontrado.");

            var tituloDuplicado = await _context.Documentos
                .AnyAsync(d => d.Titulo == model.Titulo && d.Id != model.Id && d.Flag == 1);

            if (tituloDuplicado)
                throw new InvalidOperationException("Já existe outro documento com este título.");

            model.DataCriacao = original.DataCriacao;
            model.Flag = original.Flag;

            if (string.IsNullOrEmpty(model.Arquivo))
                model.Arquivo = original.Arquivo;

            if (string.IsNullOrEmpty(model.NomeDoArquivo))
                model.NomeDoArquivo = original.NomeDoArquivo;

            _context.Documentos.Update(model);
            await _context.SaveChangesAsync();
        }

        // Exclusão Lógica
        public async Task ExcluirLogicamenteAsync(int id)
        {
            var documento = await _context.Documentos.FindAsync(id);

            if (documento == null)
                throw new InvalidOperationException("Documento não encontrado.");

            documento.Flag = 0;
            _context.Documentos.Update(documento);
            await _context.SaveChangesAsync();
        }
    }
}

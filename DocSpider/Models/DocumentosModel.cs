using System;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DocSpider.Models
{
    public class DocumentosModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "O título deve ter no máximo 100 caracteres.")]
        public string Titulo { get; set; }

        [StringLength(2000, ErrorMessage = "A descrição deve ter no máximo 2000 caracteres.")]
        public string Descricao { get; set; }

        public string Arquivo { get; set; }

        public string NomeDoArquivo { get; set; }

        [NotMapped]
        //[Required(ErrorMessage = "O arquivo é obrigatório.")]
        [MaxFileSize(10 * 1024 * 1024)] // 10 MB
        public IFormFile Upload { get; set; }

        public DateTime DataCriacao { get; set; }

        public int Flag { get; set; }

        public DocumentosModel()
        {
            DataCriacao = DateTime.Now;
            Flag = 1;
        }

    }
}

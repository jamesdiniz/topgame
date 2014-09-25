using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TopGame.Web.Framework.Validator;

namespace TopGame.Web.Models
{
    public class JogadorViewModel
    {
        [Required(ErrorMessage = "O campo nome precisa ser informado.")]
        [StringLength(80, ErrorMessage = "Tamanho máximo permitido é de 80 caracteres.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo e-mail precisa ser informado.")]
        [StringLength(200, ErrorMessage = "Tamanho máximo permitido é de 200 caracteres.")]
        [DisplayName("E-mail")]
        [EmailAddress(ErrorMessage = "O email informado não é válido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo documento precisa ser informado.")]
        [StringLength(14, ErrorMessage = "Tamanho máximo permitido é de 14 caracteres.")]
        [CustomValidationCpf(ErrorMessage = "CPF informado não é válido.")]
        [DisplayName("CPF")]
        public string Documento { get; set; }

        [Required(ErrorMessage = "É necessário capturar uma foto.")]
        public string Foto { get; set; }
    }
}
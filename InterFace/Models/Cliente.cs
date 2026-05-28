using System.ComponentModel.DataAnnotations;

namespace InterFace.Models;

public class Cliente
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Informe o nome.")]
    [StringLength(120, ErrorMessage = "Use no maximo 120 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "Informe o e-mail.")]
    [EmailAddress(ErrorMessage = "Informe um e-mail valido.")]
    [StringLength(160, ErrorMessage = "Use no maximo 160 caracteres.")]
    public string Email { get; set; } = string.Empty;

    [StringLength(30, ErrorMessage = "Use no maximo 30 caracteres.")]
    public string? Telefone { get; set; }

    [StringLength(80, ErrorMessage = "Use no maximo 80 caracteres.")]
    public string? Cidade { get; set; }

    public DateTime CriadoEm { get; set; } = DateTime.Now;
}

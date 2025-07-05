using System.ComponentModel.DataAnnotations;

namespace SPbSTU.OPD.ASAP.API.Dto;

public record UserRegisterDto(
    [Required] string Name,
    [Required] string Login,
    [Required] string Password,
    [Required] string Email,
    [Required] string Role,
    [Required] string GithubLink);
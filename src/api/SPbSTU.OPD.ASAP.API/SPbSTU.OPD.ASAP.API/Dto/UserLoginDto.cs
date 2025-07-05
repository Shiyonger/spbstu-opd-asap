using System.ComponentModel.DataAnnotations;

namespace SPbSTU.OPD.ASAP.API.Dto;

public record UserLoginDto(
    [Required] string Login,
    [Required] string Password);
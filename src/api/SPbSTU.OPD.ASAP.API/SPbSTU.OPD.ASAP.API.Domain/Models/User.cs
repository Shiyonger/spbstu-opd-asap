using SPbSTU.OPD.ASAP.API.Domain.Enums;

namespace SPbSTU.OPD.ASAP.API.Domain.Models;

public record User(long Id, string Name, string Login, string Password, string Email, Role Role, string GithubUsername);
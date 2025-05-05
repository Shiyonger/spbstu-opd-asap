using SPbSTU.OPD.ASAP.Core.Domain.Enums;

namespace SPbSTU.OPD.ASAP.Core.Domain.Models;

public record User(long Id, string Name, string Login, string Password, string Email, Role Role, string GithubUsername);
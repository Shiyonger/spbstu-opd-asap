using FluentMigrator;
using SPbSTU.OPD.ASAP.Core.Persistence.Common;

namespace SPbSTU.OPD.ASAP.Core.Persistence.Migrations;

[Migration(202503241050, "Seed data migration")]
public class SeedData : SqlMigration
{
    protected override string GetUpSql(IServiceProvider services) =>
        """
        insert into actions (id, name)
        values (0, 'Create')
             , (1, 'Update')
             , (2, 'Delete');
        """;

    protected override string GetDownSql(IServiceProvider services) =>
        """
        delete from actions
        """;
}
using FluentMigrator;
using SPbSTU.OPD.ASAP.Core.Persistence.Common;

namespace SPbSTU.OPD.ASAP.Core.Persistence.Migrations;

[Migration(20250919123000, "Add google entity v1 migration")]
public class AddGoogleEntityV1 : SqlMigration {
    protected override string GetUpSql(IServiceProvider services) =>
        """
        DO $$
            BEGIN
                IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'google_entity_v1') THEN
                    CREATE TYPE google_entity_v1 as
                    (
                          id bigint
                        , student_id bigint
                        , course_id bigint
                        , assignment_id bigint
                        , assignment_position_id bigint
                        , student_position_id bigint
                    );
                END IF;
            END
        $$;
        """;

    protected override string GetDownSql(IServiceProvider services) =>
        """
        DO $$
            BEGIN
                DROP TYPE IF EXISTS google_entity_v1;
            END
        $$;
        """;
}
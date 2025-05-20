using FluentMigrator;
using SPbSTU.OPD.ASAP.Core.Persistence.Common;

namespace SPbSTU.OPD.ASAP.Core.Persistence.Migrations;

[Migration(20250324105500, "Add outbox points entity v1 migration")]
public class AddOutboxPointsEntityV1 : SqlMigration {
    protected override string GetUpSql(IServiceProvider services) =>
        """
        DO $$
            BEGIN
                IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'outbox_points_v1') THEN
                    CREATE TYPE outbox_points_v1 as
                    (
                          id bigint
                        , points integer
                        , date timestamp with time zone
                        , course_id bigint
                        , student_position_cell varchar
                        , student_position_spreadsheet_id varchar
                        , assignment_position_cell varchar
                        , assignment_position_spreadsheet_id varchar
                        , is_sent boolean
                    );
                END IF;
            END
        $$;
        """;

    protected override string GetDownSql(IServiceProvider services) =>
        """
        DO $$
            BEGIN
                DROP TYPE IF EXISTS outbox_points_v1;
            END
        $$;
        """;
}
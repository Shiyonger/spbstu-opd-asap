using FluentMigrator;
using SPbSTU.OPD.ASAP.Core.Infrastructure.Common;

namespace SPbSTU.OPD.ASAP.Core.Infrastructure.Migrations;

[Migration(20250324105500, TransactionBehavior.None)]
public class AddOutboxPointsEntityV1 : SqlMigration {
    protected override string GetUpSql(IServiceProvider services) =>
        """
        DO $$
            BEGIN
                IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typename = 'outbox_points_v1') THEN
                    CREATE TYPE orders_v1 as
                    (
                          points integer
                        , date timestamp with time zone
                        , course_id bigint
                        , student_position varchar
                        , assignment_position varchar
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
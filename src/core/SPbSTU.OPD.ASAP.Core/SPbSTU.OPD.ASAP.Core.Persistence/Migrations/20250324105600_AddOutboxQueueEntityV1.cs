using FluentMigrator;
using SPbSTU.OPD.ASAP.Core.Persistence.Common;

namespace SPbSTU.OPD.ASAP.Core.Persistence.Migrations;

[Migration(20250324105600, "Add outbox queue entity v1 migration")]
public class AddOutboxQueueEntityV1 : SqlMigration {
    protected override string GetUpSql(IServiceProvider services) =>
        """
        DO $$
            BEGIN
                IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'outbox_queue_v1') THEN
                    CREATE TYPE outbox_queue_v1 as
                    (
                          id bigint
                        , link varchar
                        , mentor_id bigint
                        , assignment_id bigint
                        , submission_id bigint
                        , action integer
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
                DROP TYPE IF EXISTS outbox_queue_v1;
            END
        $$;
        """;
}
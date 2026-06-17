#!/bin/bash
# Wait for SQL Server to start
echo "Waiting for SQL Server to start..."
SQLCMD=""
for cmd in "/opt/mssql-tools/bin/sqlcmd" "/opt/mssql-tools18/bin/sqlcmd" "sqlcmd"; do
    if [ -f "$cmd" ] || command -v "$cmd" &> /dev/null; then
        SQLCMD="$cmd"
        break
    fi
done

if [ -z "$SQLCMD" ]; then
    echo "Error: sqlcmd not found!"
    exit 1
fi
echo "Using sqlcmd at: $SQLCMD"

# Determine options (tools18 needs -C)
SQLCMD_OPTS="-S database -U sa -P $MSSQL_SA_PASSWORD"
if [[ "$SQLCMD" == *"tools18"* ]]; then
    SQLCMD_OPTS="$SQLCMD_OPTS -C"
fi

for i in {1..50}; do
    $SQLCMD $SQLCMD_OPTS -Q "SELECT 1" &> /dev/null
    if [ $? -eq 0 ]; then
        echo "SQL Server is ready."
        break
    fi
    echo "Still waiting for database container..."
    sleep 2
done

# Check if database JSeeker exists
DB_EXISTS=$($SQLCMD $SQLCMD_OPTS -Q "SET NOCOUNT ON; SELECT DB_ID('JSeeker')" -h -1 | tr -d '[:space:]')
if [ "$DB_EXISTS" = "NULL" ] || [ -z "$DB_EXISTS" ]; then
    echo "Database JSeeker does not exist. Restoring from backup..."
    $SQLCMD $SQLCMD_OPTS -Q "RESTORE DATABASE [JSeeker] FROM DISK = '/var/opt/mssql/backup/JSeeker.bak' WITH MOVE 'JSeeker' TO '/var/opt/mssql/data/JSeeker.mdf', MOVE 'JSeeker_log' TO '/var/opt/mssql/data/JSeeker_log.ldf', REPLACE"
    echo "Database JSeeker restored successfully!"
else
    echo "Database JSeeker already exists. Skipping restore."
fi

echo "Fixing database authorization..."
$SQLCMD $SQLCMD_OPTS -Q "ALTER AUTHORIZATION ON DATABASE::[JSeeker] TO sa"
echo "Database authorization fixed!"

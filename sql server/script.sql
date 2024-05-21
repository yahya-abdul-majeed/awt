RESTORE DATABASE ibex FROM DISK = 'ibex.bak' with MOVE 'ibex' TO '/var/opt/mssql/data/ibex.mdf',MOVE 'ibex_log' TO '/var/opt/mssql/data/ibex_log.ldf';
RESTORE DATABASE kino FROM DISK = 'kino.bak' with MOVE 'kino' TO '/var/opt/mssql/data/kino.mdf',MOVE 'kino_log' TO '/var/opt/mssql/data/kino_log.ldf';


  IF OBJECT_ID(N'tempdb..##BotActions') IS NOT NULL
    DROP TABLE ##BotActions
CREATE table ##BotActions
(
    Entity      varchar(32),
    Command     varchar(32),
    Placeholder varchar(128),
    Description varchar(256)  null,
    Payload     varchar(2048) null
)
SELECT *
FROM ##BotActions

INSERT INTO ##BotActions
VALUES ('BotCommand', '/info', '/{0}', 'Basic DBs information',
        'select CAST(Id as VARCHAR(15)) as Id, Name, Size, DaysAgo, Created as Description
        from ##DataBase
        where Title like '' %{2}%''
        ORDER BY Title OFFSET {0} ROWS
        FETCH NEXT {1} ROWS ONLY');
INSERT INTO ##BotActions
VALUES ('BotCommand', '/list', '/{0}', 'List of data bases with info', '
        SELECT D.database_id                                               as Id,
               D.name                                                      as Name,
               cast(D.create_date as varchar(17))                          as Created,
               CAST(DATEDIFF(Day, D.create_date, GETDATE()) as INT)        as [DaysAgo],
               CONVERT(decimal(10, 2), round(SUM(F.size * 8) / 1024.0, 1)) AS Size
        FROM sys.master_files F
                 INNER JOIN sys.databases D ON D.database_id = F.database_id
        WHERE D.name NOT IN ("model", "tempdb", "msdb", "master")
        GROUP BY create_date, D.name, d.database_id
        ');
INSERT INTO ##BotActions
VALUES ('BotCommand', '/disk', '/{0}', 'Disk space information',
        'SELECT DISTINCT dovs.logical_volume_name                                                                                                      AS Name,
                        dovs.volume_mount_point                                                                                                       AS Drive,
                        CONVERT(NUMERIC(10, 1), ROUND(dovs.available_bytes / 1073741824.0, 1))                                                        AS Free,
                        CONVERT(NUMERIC(10, 1), Round((dovs.total_bytes - dovs.available_bytes) / 1073741824.0, 1))                                   AS Used,
                        CONVERT(NUMERIC(10, 1), round(dovs.total_bytes / 1073741824.0, 1))                                                            AS Total,
                        CONVERT(varchar, (CONVERT(NUMERIC(5, 1), 100.0 - ((dovs.available_bytes / CAST(dovs.total_bytes as real) * 100))))) + '' % '' as Perc
        FROM sys.master_files mf
                 CROSS APPLY sys.dm_os_volume_stats(mf.database_id, mf.FILE_ID) dovs');
INSERT INTO ##BotActions
VALUES ('BotCommand', '/test', '/{0}', 'test Command', '');
INSERT INTO ##BotActions
VALUES ('Bot', 'Search', '', null,
        'select CAST(Id as VARCHAR(15)) as Id, Name, Created, DaysAgo, Size from ##DataBase where Name like ''%{2}%'' ORDER BY Name OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY');

INSERT INTO ##BotActions
VALUES ('DataBase', '
Delete ', '{0}.{1}.{2} ', null, '
select *
from ##Info where Id = {0} ');
INSERT INTO ##BotActions
VALUES (' DataBase ', ' Info ', '{0}.{1}.{2} ', null, 'select *
from ##DataBase where Id = {0} ');
INSERT INTO ##BotActions
VALUES (' DataBase ', ' Details ', '{0}.{1}.{2} ', null, '
select *
from ##Info a
         inner join ##BotActions Commands on Commands.Entity = a.Entity
Where a.Id = {0} ');
INSERT INTO ##BotActions
VALUES (' DataBase ', '
Backup', '{0}.{1}.{2}', null, 'SET @fileName = @Path + @Name + ''_'' + REPLACE(CONVERT(NVARCHAR(20),GETDATE(),108),'':'','''') + ''.BAK'';BACKUP DATABASE @Name TO DISK = @filename ');

SELECT *
INTO ##DataBase
from (select 'DataBase' as EntityName , D.database_id as Id, D.name as Name, cast(D.create_date as varchar(17)) as Created, CAST(DATEDIFF(Day, D.create_date,GETDATE()) as INT) as [DaysAgo], CONVERT(decimal(10,2), round(SUM(F.size*8)/1024.0,1)) AS Size  FROM  sys.master_files F INNER JOIN sys.databases D ON D.database_id = F.database_id WHERE D.name NOT IN ('model','tempdb','msdb','master') GROUP BY create_date, D.name, d.database_id) dbs

USE      DBs

MERGE ##DataBase AS T
USING [DataBases]	AS S
ON S.Id = T.Id
WHEN NOT MATCHED BY Target THEN
    INSERT (EntityName,Id,Name, Created) 
    VALUES ('DataBase',S.Id,S.Title, GetDate());



SELECT * 
INTO ##Info
FROM(
SELECT CAST(database_id as VARCHAR(15)) as Id,
'DataBase' as Entity,
CONVERT(VARCHAR(25), DB.name) AS DBName,
CONVERT(VARCHAR(10), DATABASEPROPERTYEX(name, 'status')) AS [Status],
state_desc as State,
(SELECT COUNT(1) FROM sys.master_files WHERE DB_NAME(database_id) = DB.name AND type_desc = 'rows') AS DataFiles,
(SELECT SUM((size*8)/1024) FROM sys.master_files WHERE DB_NAME(database_id) = DB.name AND type_desc = 'rows') AS DataMB,
(SELECT COUNT(1) FROM sys.master_files WHERE DB_NAME(database_id) = DB.name AND type_desc = 'log') AS LogFiles,
(SELECT SUM((size*8)/1024) FROM sys.master_files WHERE DB_NAME(database_id) = DB.name AND type_desc = 'log') AS LogMB,
recovery_model_desc AS [RecoveryModel],
CONVERT(VARCHAR(20), create_date, 103) + ' ' + CONVERT(VARCHAR(20), create_date, 108) AS [Created],
ISNULL((SELECT TOP 1
CASE TYPE WHEN 'D' THEN 'Full' WHEN 'I' THEN 'Differential' WHEN 'L' THEN 'Transaction log' END + ' – ' +
LTRIM(ISNULL(STR(ABS(DATEDIFF(DAY, GETDATE(),Backup_finish_date))) + ' days ago', 'NEVER')) 

FROM msdb..backupset BK WHERE BK.database_name = DB.name ORDER BY backup_set_id DESC),'-') AS LastBackup,
CASE WHEN is_read_only = 1 THEN 'read only' ELSE '' END AS IsReadOnly
 FROM sys.databases DB
WHERE DB.name NOT IN ('model','tempdb','msdb','master')
AND state = 0 -- database is online
AND is_in_standby = 0 ) Info



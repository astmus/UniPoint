   
--INSERT INTO ##UnitParameters
--VALUES ('Backup', '@path', 'DECLARE @path AS VARCHAR(256)=''{0}'';', 'Basic DBs information');
--INSERT INTO ##UnitParameters
--VALUES ('Backup', '@name', 'DECLARE @name AS VARCHAR(64)=''{0}'';', 'Basic DBs information');


INSERT INTO ##BotUnits
VALUES ('BotCommand.Info','BotCommand', 'Info', '/{0}', 'Basic DBs information',
				'SELECT CAST(Id as VARCHAR(15)) as Id, Name, Size, DaysAgo, Created as Description
				FROM ##DataBase
				WHERE Title like '' %{2}%''
				ORDER BY Title OFFSET {0} ROWS
				FETCH NEXT {1} ROWS ONLY', null);

INSERT INTO ##BotUnits
VALUES ('BotCommand.List','BotCommand', 'List', '/{0}', 'List of data bases with info', '
				SELECT D.database_id                                                    as Id,
				D.name                                                                    as Name,
				cast(D.create_date as varchar(17))                           as Created,
				CAST(DATEDIFF(Day, D.create_date, GETDATE()) as INT)        as [DaysAgo],
				CONVERT(decimal(10, 2), round(SUM(F.size * 8) / 1024.0, 1)) AS Size
				FROM sys.master_files F
                INNER JOIN sys.databases D ON D.database_id = F.database_id
				WHERE D.name NOT IN ("model", "tempdb", "msdb", "master")
				GROUP BY create_date, D.name, d.database_id', null);

INSERT INTO ##BotUnits
VALUES ('BotCommand.Disk','BotCommand', 'Disk', '/{0}', 'Disk space information',
						'SELECT DISTINCT dovs.logical_volume_name                                                                                     AS [Na me],
                        dovs.volume_mount_point                                                                                                                     AS [Dri ve],
                        CONVERT(NUMERIC(10, 1), ROUND(dovs.available_bytes / 1073741824.0, 1))                                     AS Free,
                        CONVERT(NUMERIC(10, 1), Round((dovs.total_bytes - dovs.available_bytes) / 1073741824.0, 1))        AS Used,
                        CONVERT(NUMERIC(10, 1), round(dovs.total_bytes / 1073741824.0, 1))                                               AS Total,
                        CONVERT(varchar, (CONVERT(NUMERIC(5, 1), 100.0 - ((dovs.available_bytes / CAST(dovs.total_bytes as real) * 100)))))  as Perc
						FROM sys.master_files mf
						CROSS APPLY sys.dm_os_volume_stats(mf.database_id, mf.FILE_ID) dovs', null);

INSERT INTO ##BotUnits
VALUES ('BotCommand.Test','BotCommand', 'Test', '/{0}', 'test Command', null,null);
INSERT INTO ##BotUnits
VALUES ('BotCommand.Raw','BotCommand', 'Raw', '/{0}', 'raw request format [/raw] /[request]', null,null);
INSERT INTO ##BotUnits
VALUES ('BotCommand.Add','BotCommand', 'Add', '/{0}', 'add custom request', null,null);
INSERT INTO ##BotUnits
VALUES ('BP','BotUnit', 'Paging', '' , '','OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY',null);
INSERT INTO ##BotUnits
VALUES ('Search.DataBase', 'Search', 'DataBase', '' , '','SELECT * FROM ##DataBase WHERE Name LIKE ''%{0}%'' ORDER BY Name',null);
INSERT INTO ##BotUnits
VALUES ('BD','BotUnit', 'DataBase', '' , '','SELECT Units.* FROM ##BotUnits Units WHERE Unit = ''DataBase''',null);
INSERT INTO ##BotUnits
VALUES ('BR','BotUnit', 'ReadUnit', '' ,  '','WITH u as (SELECT Units.* FROM ##BotUnits Units WHERE Entity = ''{0}'' and Unit = ''BotUnit'')                                            
                                            SELECT u.Unit as ''Entity'', Actions.* FROM u                                            
                                            Inner join ##BotActions Actions
                                            ON Actions.Unit = U.eNTITY order by Actions.Action', null);
INSERT INTO ##BotUnits
VALUES ('BG','BotUnit', 'GenericUnit', 'WITH {0} as ({1}) for json auto, root(''{0}'')' ,  '','', null);
INSERT INTO ##BotUnits
VALUES ('D','BotRepository', 'DataBase', '', 'Id' , 'SELECT ''DataBase.''+CAST(D.database_id as VARCHAR(15)) as Identifier ,''DataBase'' as Unit , D.database_id as Id,
																				D.name as Name,
																				GETDATE()+ (RAND()*100) as Created,
																				CAST(DATEDIFF(Day, D.create_date,GETDATE()) as INT) as [DaysAgo],
																				ROUND(RAND()*100,2) AS Size
																				FROM  sys.master_files F INNER JOIN sys.databases D ON D.database_id = F.database_id 
                                                                                WHERE D.name 
                                                                                NOT IN (''model'',''tempdb'',''msdb'',''master'')
																				GROUP BY create_date, D.name, d.database_id', 'Id');

INSERT INTO ##BotUnits
VALUES ('Q','QueryUnit', 'DataBase', '', 'Id' , 'SELECT * FROM ##DataBase', 'Id');
                                                                                                                                   
INSERT INTO ##BotActions
VALUES ('DataBase.Delete','DataBase', 'Delete', 'SELECT * FROM ##DataBase where Id = @Id', 'Id;Name');
INSERT INTO ##BotActions
VALUES ('DataBase.Info','DataBase', 'Info', 'SELECT * FROM ##Info where Id = @Id', 'Id');
INSERT INTO ##BotActions
VALUES ('DataBase.Restore','DataBase', 'Restore', 'SELECT * FROM ##DataBase where Id = @Id', 'Id');
INSERT INTO ##BotActions
VALUES ('DataBase.Backup','DataBase', 'Backup', 'DECLARE @fileName as VARCHAR(1024) = @path + @name + ''_'' + REPLACE(CONVERT(NVARCHAR(20),GETDATE(),108),'':'','''') + ''.BAK'';BACKUP DATABASE @name TO DISK = @filename ', 'path;name');

IF OBJECT_ID(N'tempdb..##DataBase') IS NOT NULL
    DROP TABLE ##DataBase

SELECT *
INTO ##DataBase
FROM (SELECT 'DataBase.'+CAST(D.database_id as VARCHAR(15)) as Identifier, 'DataBase' as Unit , D.database_id as Id,
			D.name as Name,
			GETDATE()+ (RAND()*100) as Created,
			CAST(DATEDIFF(Day, D.create_date,GETDATE()) as INT) as [DaysAgo],
			ROUND(RAND()*100,2) AS Size
			FROM  sys.master_files F INNER JOIN sys.databases D ON D.database_id = F.database_id WHERE D.name NOT IN ('model','tempdb','msdb','master')
			GROUP BY create_date, D.name, d.database_id) dbs

USE      DBs

MERGE ##DataBase AS T
USING [DataBases]	AS S
ON S.Id = T.Id
WHEN NOT MATCHED BY Target THEN
    INSERT (Identifier, Unit,Id,Name, Created, Size, DaysAgo) 
    VALUES ('DataBase.'+CAST(Id as VARCHAR(15)), 'DataBase',S.Id,S.Title, GETDATE()+ (RAND()*100), ROUND(RAND()*100,2),ROUND(RAND()*100,0));
     

IF OBJECT_ID(N'tempdb..##Info') IS NOT NULL
    DROP TABLE ##Info

SELECT DISTINCT  d.Identifier, Info.* 
INTO ##Info
FROM(
SELECT --d.Id,
'DataBase' as Unit,
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
AND is_in_standby = 0) Info Left JOIN ##DataBase d ON d.Unit = Info.Unit



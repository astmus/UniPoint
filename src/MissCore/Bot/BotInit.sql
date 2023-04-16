
IF OBJECT_ID(N'tempdb..##BotCommands') IS NOT NULL
DROP TABLE ##BotCommands

CREATE table ##BotCommands(Command varchar(32) unique,Description varchar(256))

IF OBJECT_ID(N'tempdb..##BotActionPayloads') IS NOT NULL
DROP TABLE ##BotActionPayloads

CREATE table ##BotActionPayloads(EntityAction varchar(32) ,Payload varchar(1024) )

IF OBJECT_ID(N'tempdb..##BotActions') IS NOT NULL
DROP TABLE ##BotActions
CREATE table ##BotActions(EntityName varchar(32), ActionName varchar(32))


IF OBJECT_ID(N'tempdb..##BotEntityAction') IS NOT NULL
DROP TABLE ##BotEntityAction
CREATE table ##BotEntityAction(Entity varchar(32),Action varchar(32),Placeholder varchar(128) null)

IF OBJECT_ID(N'tempdb..##Info') IS NOT NULL
DROP TABLE ##Info
--CREATE table ##BotEntityInlineActions(varchar(32) EntityId,varchar(32) ActionName)
IF OBJECT_ID(N'tempdb..##DataBase') IS NOT NULL
DROP TABLE ##DataBase

INSERT INTO ##BotCommands VALUES('/info','Basic DBs information')
INSERT INTO ##BotCommands VALUES ('/list', 'List of data bases with infok');
INSERT INTO ##BotCommands VALUES ('/disk','Disk space information');
INSERT INTO ##BotCommands VALUES ('/test','test command');
INSERT INTO ##BotActionPayloads VALUES ('/disk','SELECT DISTINCT dovs.logical_volume_name AS Name,dovs.volume_mount_point AS Drive,CONVERT(NUMERIC(10,1),ROUND(dovs.available_bytes/1073741824.0,1)) AS Free,CONVERT(NUMERIC(10,1),Round((dovs.total_bytes - dovs.available_bytes)/1073741824.0,1)) AS Used,CONVERT(NUMERIC(10,1),round(dovs.total_bytes/1073741824.0,1)) AS Total,CONVERT(varchar,(CONVERT(NUMERIC(5,1),100.0 - ((dovs.available_bytes / CAST(dovs.total_bytes as real) * 100)))))+'' %'' as Perc FROM sys.master_files mf CROSS APPLY sys.dm_os_volume_stats(mf.database_id, mf.FILE_ID) dovs FOR JSON AUTO');
INSERT INTO ##BotActionPayloads VALUES ('/info','select value from OpenJson((select D.database_id as Id, D.name as Name, cast(D.create_date as varchar(17)) as Created, CAST(DATEDIFF(Day, D.create_date,GETDATE()) as INT) as [DaysAgo], CONVERT(decimal(10,2), round(SUM(F.size*8)/1024.0,1)) AS Size  FROM  sys.master_files F INNER JOIN sys.databases D ON D.database_id = F.database_id WHERE D.name NOT IN ("model","tempdb","msdb","master") GROUP BY create_date, D.name, d.database_id FOR JSON AUTO))');
INSERT INTO ##BotActionPayloads VALUES ('/list','select value from OpenJson((select  Id,  Name, Size, DaysAgo, Created as Description from ##DataBase where Title like ''%{2}%'' ORDER BY Title  OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY for JSON AUTO))');

INSERT INTO ##BotEntityAction VALUES ( 'DataBase','Delete','DataBase.Delete.{0}');
INSERT INTO ##BotEntityAction VALUES ( 'DataBase','Info','DataBase.Info.{0}');
INSERT INTO ##BotEntityAction VALUES ( 'DataBase','Restore','DataBase.Restore.{0}');
INSERT INTO ##BotActions VALUES ( 'Search','Query');
INSERT INTO ##BotActions VALUES ( 'Bot','Search');
INSERT INTO ##BotActionPayloads VALUES ('Bot.Search','select value from OpenJson((select  Id, Name, Created, DaysAgo, Size from ##DataBase where Name like ''%{2}%'' ORDER BY Name  OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY for JSON AUTO))');
INSERT INTO ##BotActionPayloads VALUES ('DataBase.Delete','select * from ##Info where Id = {0}');
INSERT INTO ##BotActionPayloads VALUES ('InlineAnswre.Info','select * from ##Info where Id = {0}');
INSERT INTO ##BotActionPayloads VALUES ('DataBase.Details','select value from OpenJson((select DISTINCT i.*, a.ActionName, p.* from ##DataBase d inner join ##Info i on LEN(i.dbName) > 0  INNER JOIN ##BotActions a ON d.EntityName = a.EntityName INNER JOIN ##BotActionPayloads p ON p.EntityAction =d.EntityName+''.''+a.ActionName WHERE d.Id = {0}  FOR JSON PATH))');
INSERT INTO ##BotActionPayloads VALUES ('DataBase.Backup','SET @fileName = @Path + @Name + ''_'' + REPLACE(CONVERT(NVARCHAR(20),GETDATE(),108),'':'','''') + ''.BAK'';BACKUP DATABASE @Name TO DISK = @filename');
--select value from OpenJson((select DISTINCT i.*, a.ActionName, p.* from ##DataBase d inner join ##Info i on LEN(i.dbName) > 0  INNER JOIN ##BotActions a ON d.EntityName = a.EntityName INNER JOIN ##BotActionPayloads p ON p.EntityAction =d.EntityName+'.'+a.ActionName WHERE d.Id =  25523  FOR JSON PATH))
--Update ##BotActioselect DISTINCT i.*, a.ActionName, p.* from ##DataBase d inner join ##Info i on LEN(i.dbName) > 0  INNER JOIN ##BotActions a ON d.EntityName = a.EntityName INNER JOIN ##BotActionPayloads p ON p.EntityAction =d.EntityName+'.'+a.ActionName   FOR JSON PATHnPayloads set Payload = 'select value from OpenJson((select  Id, Name, Created, DaysAgo, Size from ##DataBase where Name like ''%{2}%'' ORDER BY Name  OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY for JSON AUTO))'
--where EntityAction =  'Search'
--UPDATE ##BotActionPayloads Set Payload = 'SELECT DISTINCT dovs.logical_volume_name AS Name,dovs.volume_mount_point AS Drive,CONVERT(NUMERIC(10,1),ROUND(dovs.available_bytes/1073741824.0,1)) AS Free,CONVERT(NUMERIC(10,1),Round((dovs.total_bytes - dovs.available_bytes)/1073741824.0,1)) AS Used,CONVERT(NUMERIC(10,1),round(dovs.total_bytes/1073741824.0,1)) AS Total,CONVERT(varchar,(CONVERT(NUMERIC(5,1),100.0 - ((dovs.available_bytes / CAST(dovs.total_bytes as real) * 100)))))+'' %'' as Perc FROM sys.master_files mf CROSS APPLY sys.dm_os_volume_stats(mf.database_id, mf.FILE_ID) dovs FOR JSON AUTO'
--where EntityAction = '/disk'
--Update  ##BotActionPayloads Set Payload  = 'select value from OpenJson((select DISTINCT i.*, a.ActionName, p.* from ##DataBase d inner join ##Info i on LEN(i.dbName) > 0  INNER JOIN ##BotActions a ON d.EntityName = a.EntityName INNER JOIN ##BotActionPayloads p ON p.EntityAction =d.EntityName+'.'+a.ActionName FOR JSON PATH))'
--where ##BotActionPayloads.EntityAction = 'DataBase.Details'

--select DISTINCT i.*, a.ActionName, p.* from ##DataBase d inner join ##Info i on LEN(i.dbName) > 0  INNER JOIN ##BotActions a ON d.EntityName = a.EntityName INNER JOIN ##BotActionPayloads p ON p.EntityAction =d.EntityName+'.'+a.ActionName WHERE d.Id = 19686  FOR JSON PATH
 
SELECT *
INTO ##DataBase
from (select 'DataBase' as EntityName , D.database_id as Id, D.name as Name, cast(D.create_date as varchar(17)) as Created, CAST(DATEDIFF(Day, D.create_date,GETDATE()) as INT) as [DaysAgo], CONVERT(decimal(10,2), round(SUM(F.size*8)/1024.0,1)) AS Size  FROM  sys.master_files F INNER JOIN sys.databases D ON D.database_id = F.database_id WHERE D.name NOT IN ('model','tempdb','msdb','master') GROUP BY create_date, D.name, d.database_id) dbs


--MERGE ##DataBase AS T
--USING [DataBases]	AS S
--ON S.Id = T.Id
--WHEN NOT MATCHED BY Target THEN
--    INSERT (EntityName,Id,Name, Created) 
--    VALUES ('DataBase',S.Id,S.Title, GetDate());

SELECT * 
INTO ##Info
FROM(
SELECT database_id as Id,
'DataBase' as Entity,
CONVERT(VARCHAR(25), DB.name) AS dbName,
CONVERT(VARCHAR(10), DATABASEPROPERTYEX(name, 'status')) AS [Status],
state_desc as State,
(SELECT COUNT(1) FROM sys.master_files WHERE DB_NAME(database_id) = DB.name AND type_desc = 'rows') AS DataFiles,
(SELECT SUM((size*8)/1024) FROM sys.master_files WHERE DB_NAME(database_id) = DB.name AND type_desc = 'rows') AS DataMB,
(SELECT COUNT(1) FROM sys.master_files WHERE DB_NAME(database_id) = DB.name AND type_desc = 'log') AS LogFiles,
(SELECT SUM((size*8)/1024) FROM sys.master_files WHERE DB_NAME(database_id) = DB.name AND type_desc = 'log') AS LogMB,
recovery_model_desc AS [Recovery model],
CASE compatibility_level
WHEN 60 THEN '60 (SQL Server 6.0)'
WHEN 65 THEN '65 (SQL Server 6.5)'
WHEN 70 THEN '70 (SQL Server 7.0)'
WHEN 80 THEN '80 (SQL Server 2000)'
WHEN 90 THEN '90 (SQL Server 2005)'
WHEN 100 THEN '100 (SQL Server 2008)'
ELSE compatibility_level
END AS [compatibility level],
CONVERT(VARCHAR(20), create_date, 103) + ' ' + CONVERT(VARCHAR(20), create_date, 108) AS [Created],
ISNULL((SELECT TOP 1
CASE TYPE WHEN 'D' THEN 'Full' WHEN 'I' THEN 'Differential' WHEN 'L' THEN 'Transaction log' END + ' – ' +
LTRIM(ISNULL(STR(ABS(DATEDIFF(DAY, GETDATE(),Backup_finish_date))) + ' days ago', 'NEVER')) 

FROM msdb..backupset BK WHERE BK.database_name = DB.name ORDER BY backup_set_id DESC),'-') AS Lastbackup,
CASE WHEN is_read_only = 1 THEN 'read only' ELSE '' END AS [read only]
 FROM sys.databases DB
WHERE DB.name NOT IN ('model','tempdb','msdb','master')
AND state = 0 -- database is online
AND is_in_standby = 0 ) Info

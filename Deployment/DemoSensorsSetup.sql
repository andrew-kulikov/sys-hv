-- values setup script. Execute after Update-Database applied for SysHv.Server
truncate table dbo.SubSensors;
truncate table dbo.ClientSensors;

-- drop constraints

alter table dbo.SubSensors
drop CONSTRAINT [FK_SubSensors_Sensors_SensorId];


alter table dbo.ClientSensors
drop CONSTRAINT [FK_ClientSensors_Clients_ClientId],
     CONSTRAINT [FK_ClientSensors_Sensors_SensorId];

-- truncate referenced table
truncate table dbo.Sensors;

-- restore constraints
alter table dbo.SubSensors
add CONSTRAINT [FK_SubSensors_Sensors_SensorId] FOREIGN KEY ([SensorId]) REFERENCES [dbo].[Sensors] ([Id]) ON DELETE CASCADE;

alter table dbo.ClientSensors
add CONSTRAINT [FK_ClientSensors_Clients_ClientId] FOREIGN KEY ([ClientId]) REFERENCES [dbo].[Clients] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_ClientSensors_Sensors_SensorId] FOREIGN KEY ([SensorId]) REFERENCES [dbo].[Sensors] ([Id]) ON DELETE CASCADE;

-- sensors
insert into dbo.Sensors
values ('CPU total temp', null, 90, 1, 100, 0, 'CPULoadSensorDto', 'CpuTempSensor', 0);

insert into dbo.Sensors
values ('CPU total load', null, 90, 1, 100, 0, 'CPULoadSensorDto', 'CpuLoadSensor', 0);


-- sub sensors
insert into dbo.SubSensors
values ('Core temperature sensor', null, 0, 100, 90, 1, 1);

insert into dbo.SubSensors
values ('Core load sensor', null, 0, 100, 90, 1, 2);


-- client sensors
insert into dbo.ClientSensors
values ('Test Sensor (Cpu temp)', null, 1, 1, 5000);

insert into dbo.ClientSensors
values ('Test Sensor (Cpu load)', null, 1, 2, 10000);
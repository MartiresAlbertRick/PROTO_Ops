use dbbtOSCp1
go

alter table OSC_TeamGroupIds
alter column TeamId bigint not null
go

alter table OSC_Representative
alter column OnShoreRep bit not null
go

alter table OSC_Representative
alter column PhoneRep bit not null
go

alter table OSC_ManualEntry
alter column IsActive bit not null
go

alter table OSC_ImportNPT
alter column IsActive bit not null
go

alter table OSC_TeamGroupIds
add TGIId bigint identity(1,1) not null
go

alter table OSC_ManageGroup
add MGId bigint identity(1,1) not null
go

alter table OSC_ImportNPT
add primary key (NPTReportId)
go

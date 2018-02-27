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

alter table OSC_TeamGroupIds
add primary key (TGIId)
go

alter table OSC_CustomizeScorecard
add CSId bigint identity(1,1) not null
go

alter table OSC_CustomizeScorecard
add primary key (CSId)
go

alter table OSC_ScorecardField
add primary key (FieldId)
go

alter table OSC_TeamNptCategory
add TNCId bigint identity(1,1) not null
go

alter table OSC_TeamNptCategory
add primary key (TNCId)
go

alter table OSC_ManageGroup
add MGId bigint identity(1,1) not null
go

alter table OSC_ManageGroup
add primary key (MGId)
go

alter table OSC_TeamWorkItem
add primary key (WorkItemNo)
go

alter table OSC_ImportNPT
add primary key (NPTReportId)
go

alter table OSC_ImportBIProd
add primary key (BIPReportId)
go

alter table OSC_ImportBIQual
add primary key (BIQReportId)
go

alter table OSC_ImportTA
add primary key (TAReportId)
go

alter table OSC_ImportAIQ
add primary key (AIQReportId)
go

alter table OSC_CustomizeScorecard
add ScorecardType nvarchar(25) null
go

alter table OSC_Representative
add IsVPN bit
go

drop trigger OSC_Representative_After_Update
go

update OSC_Representative set HasPrevious=0
go

update OSC_Representative set PreviousId=0
go

update OSC_Representative set IsCurrent=1
go

update OSC_Representative set IsVPN=0
go

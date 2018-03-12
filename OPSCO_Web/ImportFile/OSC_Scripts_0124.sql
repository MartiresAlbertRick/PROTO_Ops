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

update OSC_CustomizeScorecard set ScorecardType = 'IndividualScorecard'
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
--Changes
alter table OSC_Representative
alter column HasPrevious bit not null
go

alter table OSC_Representative
alter column IsCurrent bit not null
go

alter table OSC_Representative
alter column IsVPN bit not null
go

create table OSC_TeamScorecard_Current
(
	TeamScorecardId bigint identity(1, 1) not null primary key,
	TeamId bigint not null,
	Month int not null,
	Year int not null,
	ProductivityGoal float null,
	QualityGoal float null,
	EfficiencyGoal float null,
	UtilizationGoal float null,
	IndividualSummaryComments nvarchar(1000) null,
	TeamSummaryComments nvarchar(1000) null,
	WorktypeSummaryComments nvarchar(1000) null,
	StatusSummaryComments nvarchar(1000) null,
	IsSignedOff bit not null,
	ManagerSignOff nvarchar(1000) null,
	SignOffBy nvarchar(255) null,
	SignOffDate datetime null
)
go

alter table OSC_Location
add OnShore bit
go

alter table OSC_TeamWorkItem
add IsCompletedItem bit
go

update OSC_TeamWorkItem set IsCompletedItem = 0
go

create table OSC_TeamScorecardAppendix
(
	AppendixId bigint identity(1, 1) not null primary key,
	TeamScorecardId bigint not null,
	Terminology nvarchar(100) not null,
	TermDefinition nvarchar(1000) null
)

create table OSC_CompletedItems
(
	CompletedUnitId bigint identity(1, 1) not null primary key,
	WorkItemNo bigint not null,
	CompletedUnitName nvarchar(100) not null,
	CompletedUnitCount int,
	CompletedUnitStatusChanges int
)
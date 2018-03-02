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
 
insert into OSC_TeamScorecard_Current select TeamId, 1, 2017, 0, 0, 0, 0, null, null, null, null, 0, null, null, null from OSC_Team
go
insert into OSC_TeamScorecard_Current select TeamId, 2, 2017, 0, 0, 0, 0, null, null, null, null, 0, null, null, null from OSC_Team
go
insert into OSC_TeamScorecard_Current select TeamId, 3, 2017, 0, 0, 0, 0, null, null, null, null, 0, null, null, null from OSC_Team
go
insert into OSC_TeamScorecard_Current select TeamId, 4, 2017, 0, 0, 0, 0, null, null, null, null, 0, null, null, null from OSC_Team
go
insert into OSC_TeamScorecard_Current select TeamId, 5, 2017, 0, 0, 0, 0, null, null, null, null, 0, null, null, null from OSC_Team
go
insert into OSC_TeamScorecard_Current select TeamId, 6, 2017, 0, 0, 0, 0, null, null, null, null, 0, null, null, null from OSC_Team
go
insert into OSC_TeamScorecard_Current select TeamId, 7, 2017, 0, 0, 0, 0, null, null, null, null, 0, null, null, null from OSC_Team
go
insert into OSC_TeamScorecard_Current select TeamId, 8, 2017, 0, 0, 0, 0, null, null, null, null, 0, null, null, null from OSC_Team
go
insert into OSC_TeamScorecard_Current select TeamId, 9, 2017, 0, 0, 0, 0, null, null, null, null, 0, null, null, null from OSC_Team
go
insert into OSC_TeamScorecard_Current select TeamId, 10, 2017, 0, 0, 0, 0, null, null, null, null, 0, null, null, null from OSC_Team
go
insert into OSC_TeamScorecard_Current select TeamId, 11, 2017, 0, 0, 0, 0, null, null, null, null, 0, null, null, null from OSC_Team
go
insert into OSC_TeamScorecard_Current select TeamId, 12, 2017, 0, 0, 0, 0, null, null, null, null, 0, null, null, null from OSC_Team
go
insert into OSC_TeamScorecard_Current select TeamId, 1, 2018, 0, 0, 0, 0, null, null, null, null, 0, null, null, null from OSC_Team
go
insert into OSC_TeamScorecard_Current select TeamId, 2, 2018, 0, 0, 0, 0, null, null, null, null, 0, null, null, null from OSC_Team
go
insert into OSC_TeamScorecard_Current select TeamId, 3, 2018, 0, 0, 0, 0, null, null, null, null, 0, null, null, null from OSC_Team
go










--remove
select PRDUserId, Count(*) from OSC_Representative group by PRDUserId having Count(*)>1
select * from OSC_Representative where PRDUserid='kerinsj'
update OSC_Representative set HasPrevious = 1, PreviousId=23 where RepId=24
select * from OSC_Representative where PRDUserid='oxtonsco'
update OSC_Representative set HasPrevious = 1, PreviousId=25 where RepId=4
update OSC_Representative set HasPrevious = 0, PreviousId=0 where RepId=25
update OSC_Representative set IsCurrent = 1 where RepId=4
update OSC_Representative set IsCurrent = 0 where RepId=25
select * from OSC_Team where TeamId=9

select * from OSC_ImportBIQual
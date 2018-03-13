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
	CompletedUnitName nvarchar(100) not null,
	CompletedUnitCount int,
	CompletedUnitStatusChanges int,
	Month int,
	Year int,
)

create table OSC_CompletedWorkItem
(
	CompletedWorkItemId bigint identity(1, 1) not null primary key,
	WorkItemNo bigint not null,
	CompletedUnitId bigint not null,
	Month int not null,
	Year int not null
)

create table OSC_ImportBIQualDetailed_temp
(
	[Group] nvarchar(50) null,
	UserIdName nvarchar(100) null,
	BusinessArea nvarchar(50) null,
	Worktype nvarchar(50) null,
	[Status] nvarchar(50) null,
	Count1 bigint null,
	Count2 bigint null,
	Count3 bigint null,
	Count4 bigint null,
	ErrorPoints float null,
	Rating float null,
	Month int null,
	Year int null,
	DateUploaded datetime null,
	UploadedBy nvarchar(50) null
)
go

create table OSC_ImportBIQualDetailed
(
	BIQReportId bigint identity(1, 1) not null,
	RepId bigint null,
	[Group] nvarchar(50) null,
	UserIdName nvarchar(100) null,
	BusinessArea nvarchar(50) null,
	Worktype nvarchar(50) null,
	[Status] nvarchar(50) null,
	Count1 bigint null,
	Count2 bigint null,
	Count3 bigint null,
	Count4 bigint null,
	ErrorPoints float null,
	Rating float null,
	TeamId bigint null,
	Month int null,
	Year int null,
	DateUploaded datetime null,
	UploadedBy nvarchar(50) null
)
go

  CREATE procedure [dbo].[OSC_ImportBIQualDetailed_Transfer]
(
	@Month			int,
	@Year			int,
	@DateUploaded	datetime,
	@UploadedBy		nvarchar(20)
)
as
begin
	set nocount on
	declare @RepId bigint
	declare @Group nvarchar(50)
	declare @UserIdName nvarchar(100)
	declare @BusinessArea nvarchar(50)
	declare @Worktype nvarchar(50)
	declare @Status nvarchar(50)
	declare @Count1 bigint
	declare @Count2 bigint
	declare @Count3 bigint
	declare @Count4 bigint
	declare @ErrorPoints float
	declare @Rating float
	declare @TeamId bigint
	declare @mm int
	declare @yy int
	declare @date datetime
	declare @upBy nvarchar(50)

	delete from 
		OSC_ImportBIQualDetailed
	where
		[Month]=@Month
	and
		[Year]=@Year

	declare csr CURSOR FOR
		select
			(select Max(RepId) 
					from OSC_Representative r 
					where r.BIUserId = [UserIdName] 
					),
			[Group],
			[UserIdName],
			[BusinessArea],
			[Worktype],
			[Status],
			[Count1],
			[Count2],
			[Count3],
			[Count4],
			[ErrorPoints],
			[Rating],
			(select TeamId from OSC_TeamGroupIds otg where otg.GroupId= [Group] and otg.GroupType = 'BI'),
			[Month],
			[Year],
			[DateUploaded],
			[UploadedBy]
		from
			OSC_ImportBIQualDetailed_temp
		where
			[UploadedBy]=@UploadedBy
		and
			[Month]=@Month
		and
			[Year]=@Year
		and
			[UserIdName] is not null;
	
	OPEN csr;

	FETCH NEXT FROM csr
	INTO  @RepId, @Group, @UserIdName, @BusinessArea, @Worktype, @Status,
		  @Count1, @Count2, @Count3, @Count4, @ErrorPoints, @Rating,
		  @TeamId, @mm, @yy, @date, @upBy
	;

	declare @sGroup nvarchar(50) --Same Group

	while @@FETCH_STATUS = 0 
		BEGIN
			if (select count(*) 
							from OSC_ImportBIQualDetailed
							where	[UserIdName]	= @UserIdName
								and	[BusinessArea]	= @BusinessArea
								and	[Worktype]		= @Worktype
								and	[Status]		= @Status
								and	[Count1]		= @Count1
								and	[Count2]		= @Count2
								and	[Count3]		= @Count3
								and	[Count3]		= @Count3
								and	[Count4]		= @Count4
								and [ErrorPoints]	= @ErrorPoints
								and	[Rating]		= @Rating
								and [Month]			= @mm
								and [Year]			= @yy
							)<1
			BEGIN
				/*IF @Group is null or @Group='' --if group is null follow the group from sGroup (Same Group) on first record of group
					BEGIN
						set @Group = @sGroup
					END;
				ELSE --if not null it is the first record of the group then assign value to sGroup (Same Group)
					BEGIN
						set @sGroup = @Group	
					END;*/

				insert into 
					OSC_ImportBIQualDetailed
					(
						[RepId],
						[Group],
						[UserIdName],
						[BusinessArea],
						[Worktype],
						[Status],
						[Count1],
						[Count2],
						[Count3],
						[Count4],
						[ErrorPoints],
						[Rating],
						[TeamId],
						[Month],
						[Year],
						[DateUploaded],
						[UploadedBy]
					)
				values 
					(
						@RepId, @Group, @UserIdName, @BusinessArea, @Worktype, @Status,
						@Count1, @Count2, @Count3, @Count4, @ErrorPoints, @Rating,
						@TeamId, @mm, @yy, @date, @upBy
					)
			END;

			FETCH NEXT FROM csr
			INTO  @RepId, @Group, @UserIdName, @BusinessArea, @Worktype, @Status,
				  @Count1, @Count2, @Count3, @Count4, @ErrorPoints, @Rating,
				  @TeamId, @mm, @yy, @date, @upBy;
		END;

	CLOSE csr;
	DEALLOCATE csr;

	delete from 
		OSC_ImportBIQualDetailed_temp
	where
		[UploadedBy]=@UploadedBy
	and
		[Month]=@Month
	and
		[Year]=@Year
end
go

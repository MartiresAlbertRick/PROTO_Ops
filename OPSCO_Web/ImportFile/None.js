/*
    To fill file in Import folder
*/
//my vdi
<connectionStrings>
<add name="OSCEntities" connectionString="metadata=res://*/Models.OSCModel.csdl|res://*/Models.OSCModel.ssdl|res://*/Models.OSCModel.msl;provider=System.Data.SqlClient;provider connection string=&quot;data source=JHJHST59\JHS1D;initial catalog=dbbtOSCp1;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework&quot;" providerName="System.Data.EntityClient" />
<add name="oscdb" connectionString="Data Source=JHJHST59\JHS1D;Initial Catalog=dbbtOSCp1;Trusted_Connection=YES;" providerName="System.Data.SqlClient" />
</connectionStrings>
//my laptop
<connectionStrings>
<add name="OSCEntities" connectionString="metadata=res://*/Models.OSCModel.csdl|res://*/Models.OSCModel.ssdl|res://*/Models.OSCModel.msl;provider=System.Data.SqlClient;provider connection string=&quot;data source=DESKTOP-BAP6R0C\ALBERT;initial catalog=dbbtOSCp1;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework&quot;" providerName="System.Data.EntityClient" />
<add name="oscdb" connectionString="Data Source=DESKTOP-BAP6R0C\ALBERT;Initial Catalog=dbbtOSCp1;Trusted_Connection=YES;" providerName="System.Data.SqlClient" />
</connectionStrings>


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
select PRDUserId, Count(*) from OSC_Representative group by PRDUserId having Count(*)> 1
select * from OSC_Representative where PRDUserid= 'kerinsj'
update OSC_Representative set HasPrevious = 1, PreviousId = 23 where RepId= 24
select * from OSC_Representative where PRDUserid= 'oxtonsco'
update OSC_Representative set HasPrevious = 1, PreviousId = 25 where RepId= 4
update OSC_Representative set HasPrevious = 0, PreviousId = 0 where RepId= 25
update OSC_Representative set IsCurrent = 1 where RepId= 4
update OSC_Representative set IsCurrent = 0 where RepId= 25
select * from OSC_Team where TeamId= 9

select * from OSC_ImportBIQual
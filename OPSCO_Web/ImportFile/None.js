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

//resources
//HTML to PDF
https://code.msdn.microsoft.com/Free-Html-To-Pdf-Converter-04e4438e
//ClosedXML for Excel
https://www.aspsnippets.com/Articles/ClosedXML-MVC-Example-Export-to-Excel-using-ClosedXML-in-ASPNet-MVC.aspx?_sm_nck=1
//ChartJS
https://code.msdn.microsoft.com/Chart-in-MVC-using-Chartjs-5806c814




--8 Support Line Utilization
	case when[Month] <=@Month then
round(isnull(((isnull([ACDTalkTime], 0) / 3600) + (isnull([IntervalIdleDur], 0) / 3600) + (isnull([ACDWrapUpTime], 0) / 3600) / nullif((isnull([HoursWorked], 0) - isnull(([ProcessingHours] / 60), 0) - isnull([NPTHours], 0)), 0)) * 100, 0), 2)
	else 0 end as [Support Line Utilization],

    --18 Call Management Score
	case when[Month] <=@Month then
isnull([CallManagementScore], 0) 
	else 0 end as [Call Management Score],
    --20 Total Utilization
	case when[Month] <=@Month then
round(isnull(((IsNull(([ProcessingHours] / 60), 0) + IsNull([NPTHours], 0) + (IsNull([ACDTalkTime], 0) / 3600)) / nullif([HoursWorked], 0)) * 100, 0), 2)
	else 0 end as [Total Utilization],
    --21 Efficiency
	case when[Month] <=@Month then
round(
    isnull((IsNull(([ProcessingHours] / 60), 0) /
        nullif((IsNull([HoursWorked], 0) - IsNull([NPTHours], 0) -
            (IsNull([ACDTalkTime], 0) / 3600)
        ), 0)) * 100, 0), 2)
	else 0 end as [Efficiency],

    --22 Gain/ Loss Occurances
	case when[Month] <=@Month then
isnull([GainLossOccurances], 0) 
	else 0 end as [Gain/Loss Occurances],
--23 Gain/ Loss Amount
	case when[Month] <=@Month then
isnull([GainLossAmount], 0) 
	else 0 end as [Gain/Loss Amount],

--26 Average Handle Time
	case when[Month] <=@Month then
[dbo].[fnGetHoursFormat](round(IsNull((IsNull([ACDTalkTime], 0) + IsNull([ACDWrapUpTime], 0) + (IsNull([AvgHoldDur], 0) * IsNull([HeldContacts], 0))) / NullIf([TotalACDCalls], 0), 0), 2))
	else '00:00:00' end as [Average Handle Time],
    --27 Schedule Adherence
	case when[Month] <=@Month then
isnull([ScheduleAdherence], 0)
	else 0 end as [Schedule Adherence],
    --28 Compliance
	case when[Month] <=@Month then
isnull([Compliance], 0)
	else 0 end as [Compliance],
    --29 Product Accuracy
	case when[Month] <=@Month then
isnull([ProductAccuracy], 0)
	else 0 end as [Product Accuracy],
    --30 Commitment
	case when[Month] <=@Month then
isnull([Commitment], 0)
	else 0 end as [Commitment],
    --31 JH Values
	case when[Month] <=@Month then
isnull([JHValues], 0)
	else 0 end as [JH Values],
    --32 Call Efficiency
	case when[Month] <=@Month then
isnull([CallEfficiency], 0)
	else 0 end as [Call Efficiency],
    --33 Engagement
	case when[Month] <=@Month then
isnull([Engagement], 0)
	else 0 end as [Engagement],
    --34 Administrative Procedures
	case when[Month] <=@Month then
isnull([AdministrativeProcedures], 0)
	else 0 end as [Administrative Procedures],


﻿/*
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




//8 Support Line Utilization
	
(([ACDTalkTime] / 3600) + ([IntervalIdleDur] / 3600) + ([ACDWrapUpTime] / 3600)
        / (([HoursWorked] - [ProcessingHours]) - [NPTHours]) * 100
	

    //20 Total Utilization
	
[ProcessingHours] +
    [NPTHours] + ([ACDTalkTime] / 3600) / [HoursWorked] * 100
	

        //21 Efficiency

    ([ProcessingHours] /
        ([HoursWorked] - [NPTHours] - ([ACDTalkTime] / 3600))) * 100

//26 Average Handle Time
    ([ACDTalkTime] + [ACDWrapUpTime] + ([AvgHoldDur] * [HeldContacts]))
            /[TotalACDCalls]



 if (role != "Admin") {
    npts = npts.Where(n => (searchByCategory != "" &&
        n.TypeOfActivity.StartsWith(searchByCategory)) ||
        (TeamIdResult != null &&
            TeamIdResult.Contains((long)n.TeamId)) &&
        n.IsActive);
}
else {
    npts = npts.Where(n => (searchByCategory != "" &&
        n.TypeOfActivity.StartsWith(searchByCategory)) ||
        (TeamIdResult != null &&
            TeamIdResult.Contains((long)n.TeamId)));
}
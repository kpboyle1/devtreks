AAppendix B. ReadMe.txt
Version: 2.0.0, July 08, 2016

Introduction
DevTreks is a multitier ASP.NET Core 1 database 
application. The web project, DevTreks, uses an 
MVC pattern. The data layer, DevTreks.Data, uses 
an EF Core 1 data repository pattern. EF data models 
are stored in the DevTreks.Models project. ASPNET 
Identity models are stored in the DevTreks web 
Project’s Data folder. Localization strings are stored in 
the DevTreks.Exceptions and DevTreks.Resources 
projects. The DevTreks.Extensions folder holds 
projects that use a Managed Extensibility Framework 
pattern. Each project holds a separate group of 
calculators and analyzers. 

Always visit the What's New link on the home site 
for the latest news. The What's New text file lists 
tutorials that have been upgraded recently. Those 
tutorials are usually associated with the current 
release. The Deployment tutorial explains how the 
source code works. The Calculators and Analyzers 
tutorials explains how calculators and analyzers 
work. The Social Budgeting tutorial explains how 
to manage networks, clubs, and members to 
deliver social budgeting data services.

home site
https://www.devtreks.org

source code site 
https://github.com/kpboyle1/devtreks

database.zip site
https://devtreks.codeplex.com/

What's New in Version 2.0.0
1.	This release refactors DevTreks to Microsoft’s 
ASP.NET and Entity Framework Core 1.0 technologies. 

2.	The development database server uses Sql Server 
2016 Express RTM. The Azure database server uses RTM 12. 
The connection properties can be found in this file’s 
Database Connection section.

3.	This release has been deployed to localhost 
and to an Azure App Service web app. Appendixes C and D 
in the Source Code reference explains deployment further.

Database Connections
Server version: Sql Server 2016 Express, RTM

connection string
Server=localhost\SQLEXPRESS;Database=DevTreksDesk;Trusted_Connection=True;

system administrator
July 07, 2016 update: SqlExpress 2016 databases can be accessed using a Windows OS logged in user –these haven’t been tested with the new db server and aren’t critical for accessing the db in SSMS
User: devtreks01_sa or sa
Pwd: public

DevTreks default member login
Name: kpboyle1@comcast.net
Pwd: public2A@


Appendix B. ReadMe.txt
Version: 2.0.0.beta2, June 10, 2016
(updated for azure deployment on 
June 24, 2016)

Introduction
DevTreks is a multitier ASP.NET Core 1 database 
application. The web project, DevTreks, uses an 
MVC pattern. The data layer, DevTreks.Data, uses 
an EF Core 1 data repository pattern. EF data models 
are stored in the DevTreks.Models project. 
Localization strings are stored in the 
DevTreks.Exceptions and DevTreks.Resources projects. 
The DevTreks.Extensions folder holds projects 
that use a Managed Extensibility Framework pattern. 
Each project holds a separate group of calculators 
and analyzers. 

Always visit the What's New link on the home site 
for the latest news. The What's New text file lists 
tutorials that have been upgraded recently. Those 
tutorials are usually associated with the current 
release. The Deployment tutorial explains how the 
source code works. The Calculators and Analyzers 
tutorials explains how calculators and analyzers 
work. The Social Budgeting tutorial explains how 
to manage networks, clubs, and members for 
delivering social budgeting data services.

home site
https://www.devtreks.org

source code site 
https://github.com/kpboyle1/devtreks

database.zip site
https://devtreks.codeplex.com/

What's New in Version 2.0.0.beta2
1.	This release refactors DevTreks to Release 
Candidate 2, or rc2, versions of Microsoft’s ASPNET 
Core 1.0 technologies. DevTreks will continue being 
refactored to the Core 1 technologies until those 
technologies are officially released as RTM.

2.	The database server has been upgraded to 
Sql Server 2016 Express. That platform was released 
June 1, 2016. It is an official RTM version (not 
a release candidate). The connection properties can 
be found in this file’s Database Connection section.

3.	This release has been deployed to the new Azure 
web site successfully. Deploying on Azure required: 
1) removing the following reference from the web 
project’s project.json file:  
"System.Text.Encodings.Web": "4.0.0-rc2-24027". 
2) In addition, each project.json frameworks section 
was changed from “net451” to “net461”.


Database Connections
Server version: Sql Server 2016 Express, RTM

connection string
Server=localhost\SQLEXPRESS;Database=DevTreksDesk;Trusted_Connection=True;

system administrator
User: sa
Pwd: public

DevTreks default member login
Name: kpboyle1@comcast.net
Pwd: public2A@
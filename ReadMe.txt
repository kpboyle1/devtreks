Appendix B. ReadMe.txt

Version: 2.0.0.beta2, June 10, 2016

Introduction

DevTreks is a multitier ASP.NET Core 1 database 
application. The web project, DevTreks, uses an 
MVC pattern. The data layer, DevTreks.Data, uses 
an EF 7 data repository pattern. EF 7 data models 
are stored in the DevTreks.Models folder. 
Localization strings are stored in the 
DevTreks.Exceptions and DevTreks.Resources projects. 
The DevTreks.Extensions folder holds subfolders 
that use a Managed Extensibility Framework pattern. 
Each folder holds a separate group of calculators 
and analyzers. 

Always visit the What's New link on the home site 
for the latest news. The What's New text file lists 
tutorials that have been upgraded recently. Those 
tutorials are usually associated with the current 
release. The Deployment tutorial explains how the 
source code works. The Calculators and Analyzers 
tutorials explains how calculators and analyzers 
work. The Club Administration tutorial explains how 
to manage networks, clubs, and members for social 
budgeting.


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
be found in the Database Connection section.

3.	This release has not been deployed to the 
production Azure site. Although Version 2.0.0.beta1 
was debugged using the Azure storage emulator, the 
current version could not be debugged because the 
old storage emulator stopped working and the new 
version won’t install correctly. The new database 
server might have something to do with it 
–previous Sql servers se fue. Testing on Azure is 
still likely. 


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

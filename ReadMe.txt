Appendix B. ReadMe.txt
Version: 2.0.8, May 10, 2017
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
release. The Source Code tutorial explains how the 
source code works. The Social Budgeting tutorial 
explains how to manage networks, clubs, and 
members to deliver social budgeting data services. 
The Calculators and Analyzers tutorial explains 
how calculators and analyzers work. 

home site
https://www.devtreks.org

source code sites
https://github.com/kpboyle1/devtreks
https://github.com/kpboyle1/devtreksapi1

database.zip site
https://devtreks.codeplex.com/

What's New in Version 2.0.8
1.	Social Performance Analysis Tools: This release includes 4 new algorithms designed to measure Social Performance. A new reference, Social Performance Analysis, documents how these algorithms work. The reference can be found in the Performance Analysis tutorial. 
2. 	Tutorials: Several references in several tutorials were updated to address the changes associated with the new tools. The Life Cycle Calculation reference has changed in order to comply with one of the new algorithms.
Database Connections
Server version: Sql Server 2016 Express, RTM

connection string
Server=localhost\SQLEXPRESS;Database=DevTreksDesk;Trusted_Connection=True;

DevTreks default member login
Name: kpboyle1@comcast.net
Pwd: public2A@

system administrator
SqlExpress 2016 databases can be accessed using a Windows OS logged in user –these haven’t been tested with the new db server and aren’t critical for accessing the db in SSMS
User: devtreks01_sa or sa
Pwd: public



 



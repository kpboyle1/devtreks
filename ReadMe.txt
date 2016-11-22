Appendix B. ReadMe.txt
Version: 2.0.6, November 22, 2016
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

What's New in Version 2.0.6
1.	Monitoring and Evaluation (M&E) Tools: The Version 2.0.4 M&E upgrades were tested and improved to work with all of the Conservation Technology Assessment (CTA) algorithms. The CTA references were updated with M&E examples.
2.	Conservation Technology Assessments (CTAs): Several algorithms in the Technology Assessment tutorials were upgraded to work with M&E calculations and because the additional tests revealed some slight improvements with some of those calculations. Major changes were not made to these algorithms because this release focused on the M&E tools.
3.	Tutorials: Several references in several tutorials were updated to address the changes in the M&E and CTA tools. 

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



 



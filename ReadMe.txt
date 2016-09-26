Appendix B. ReadMe.txt
Version: 2.0.2 Preview, September 26, 2016

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
source code works. The Social Budgeting tutorial 
explains how to manage networks, clubs, and 
members to deliver social budgeting data services. 
The Calculators and Analyzers tutorials explains 
how calculators and analyzers work. 

home site
https://www.devtreks.org

source code sites
https://github.com/kpboyle1/devtreks
https://github.com/kpboyle1/devtreksapi1

database.zip site
https://devtreks.codeplex.com/

What's New in Version 2.0.2
1.	CTAs (Conservation Technology Assessments): R, Python, Statistical Virtual Machine, and AML: The Technology Assessment 01 tutorial is being upgraded to explain the changes being made for Version 2.0.2, including the use of Anaconda 4 with Python 3.5.2, Microsoft R Open and Intel Math Kernel with R 3.3.0, Statistical Virtual Machines with various statistical packages, and Azure Machine Learning (AML) web services. 
2.	DevTreks WebApi: A new ASP.NET Core 1 WebApi app exposes a REST interface that accepts POST http commands that contain a JSON string in the request’s body (i.e. http://locahost:5000/api/statscript). The object’s properties include a data URL and a statistical script URL. The host runs the statistical script against the data and returns the JSON string that holds the statistical results. This WebApi app is deployed to the Statistical Virtual Machine mentioned in Item 3. The source code has been added to the devtrekapi1 github repository. 
3.	CTA-Prevention (Climate Change): The Technology Assessment 02 tutorial is being upgraded and further proofed.

Database Connections
Server version: Sql Server 2016 Express, RTM

connection string
Server=localhost\SQLEXPRESS;Database=DevTreksDesk;Trusted_Connection=True;

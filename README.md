#  Talent Academy Core Project For EmakinaTR

This project is the base / core application of Talent Academy For **Emakina.TR**



##  Umbraco CMS

This project is being developed on top of Umbraco `v7.10.4`. 

Umbraco is a free open source Content Management System (CMS) built on the ASP.NET platform. 
[https://umbraco.com](https://umbraco.com).



## What does it include

This project includes 
- organizational structure of the company, 
- position management,
- employee and HR management,
- resources management,
- inventory tracking etc.



## Installation


### Prerequisites

To run this project on your local machine, 
- you need to have microsoft IIS or IIS Express installed on your machine.
- You can only access the project if you have the ip address of the Emakina.TR organisation

> **Note:** Visual Studio has built-in IIS Express so if you have visual studio
installed on your system you don't need to install IIS or IIS express. 

To install this project on your local machine, go to directory that you want the project to be install in,

download files and put files to that folder or clone the project to that directory using 

`git clone "https://git.emakina.net/scm/emktr/base.git"`

> **Important:** Normally Visual Studio restore packages located at packages.config and packages.json when building solution, 

somehow if it doesn't restore packages then you'll need to restore with commands below.

- Install packages located at packages.config (from VS package manager console)

`Install-Package NuGet.CommandLine //If you installed nuget-cli before you can skip this command`

`Nuget restore //restoring dependencies from packages.config`


- Install packages located at package.json (from terminal)

`npm install //restoring dependencies from package.json`

> **Note:** If you don't have a user for backoffice You can only see front side of the project, 
for a backoffice user contact the admin of the project

Now open the project's solution in the visual studio, build and start the project.



##  Webpack


We are using webpack `v4.8.3` with npm to manage our frontend assets. 
With webpack and its dependencies we are able to


- Transform sass/scss files to css files
- Transform css files to string
- Bundle and minimize our asset files



You can apply above actions to your asset files using this command :



`npm run webpack-dev`

> **Note:** Above command, by default, will use 'src/master/index.js' as entry point and 'dist/' as output folder.   
If you want to change these configurations you need to edit the webpack.config.js file

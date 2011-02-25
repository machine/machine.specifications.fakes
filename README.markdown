# What is Machine.Fakes ?

Machine.Fakes is little framework build on top of Machine.Specifications that can best be described as an extended integration layer between Machine.Specifications and different Mock/Fake/Substitute/"However you call them now" frameworks. At the moment of writing I prever the term "Fake". (What a surprise judging from the name of this project ;-))

Machine.Fakes attempts to simplify the usage of such fake frameworks on top of MSpec by helping to reduce a lot of the typical fake framwork related clutter code in specifications. 

If you choose so, Machine.Fakes even helps you to stay mostly independant of a concrete fake framework by providing a little wrapper API and a provider model for fake frameworks. 

## Main Features

* Shortcuts for creating fakes or sets of fakes
* An integrated AutoMockingContainer
* A fake framework independant API for setting up and verifying behaviors on fakes. Adapters for RhinoMocks, MOQ and FakeItEasy available out-of-the-box. 
* Modularization API for using configuration building blocks for fakes inside specifications

## How to contribute code

* Login in github (you need an account)
* Fork the main repository from [Github](https://github.com/BjRo/Machine.Fakes)
* Push your changes to your fork
* Send me a pull request

# How to get startet

## Building Machine.Fakes from source

Just download the repository from github and run the build.cmd. The build of Machine.Fakes only requires the .NET Framework 4.0 to be installed on your machine. Everything else should work out-of-the-box. If not, please take the time to add an issue to this project. After a succesfulbuild you find all the assemblies in a zip file under the "Release" folder.

## Getting Machine.Fakes via the NuGet package manager

If you've got NuGet installed on your machine it gets even easier. Currently there're 4 Packages available on NuGet. These are the core framework and the different flavors for Rhino.Mocks, MOQ and FakeIteasy. If you want to use Machine.Fakes for example with FakeItEasy just go ahead and type 

install-package Machine.Fakes.FakeItEasy

into the package management console and all necessary depedencies including FakeItEasy itself will be downloaded to your machine. The othe alternatives are

install-package Machine.Fakes.RhinoMocks
install-package Machine.Fakes.Moq
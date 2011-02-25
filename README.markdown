# What is Machine.Fakes ?

Machine.Fakes is little framework build on top of Machine.Specifications that can best be described as an extended integration layer between Machine.Specifications and different Mock/Fake/Substitute/"However you call them now" frameworks. At the moment of writing I prever the term "Fake". (What a surprise judging from the name of this project ;-))

Machine.Fakes attempts to simplify the usage of such fake frameworks on top of MSpec by helping to reduce a lot of the typical fake framwork related clutter code in specifications. 

If you choose so, Machine.Fakes even helps you to stay mostly independent of a concrete fake framework by providing a little wrapper API and a provider model for fake frameworks. 

Many credits for the ideas incorporated in this framework go to Jean Paul Boodhoo (http://blog.jpboodhoo.com/) who introduced me to a completely different way of handling Mocks/Fakes/etc during his NothingButDotnet bootcamp. Maybe this a way for me to give something back ...

## How to contribute code

* Login in github (you need an account)
* Fork the main repository from [Github](https://github.com/BjRo/Machine.Fakes)
* Push your changes to your fork
* Send me a pull request

# How to get startet

## Building Machine.Fakes from source

Just download the repository from github and run the build.cmd. The build of Machine.Fakes only requires the .NET Framework 4.0 to be installed on your machine. Everything else should work out-of-the-box. If not, please take the time to add an issue to this project. After a succesful build you find all the assemblies in a zip file under the "Release" folder.

## Getting Machine.Fakes via the NuGet package manager

If you've got NuGet installed on your machine it gets even easier. Currently there're 4 packages available on NuGet. These are the core framework and the different adapters for Rhino.Mocks, MOQ and FakeIteasy. If you want to use Machine.Fakes for example with FakeItEasy just go ahead and type 

        install-package Machine.Fakes.FakeItEasy

into the package management console and all necessary depedencies including FakeItEasy itself will be downloaded to your machine. The othe alternatives are

         install-package Machine.Fakes.RhinoMocks
         install-package Machine.Fakes.Moq

## How to use it

The core part of Machine.Fakes only consists of two classes: WithFakes and WithSubject<TSubject>. 

### WithFakes

Let's take a look at the simpler one first (WithFakes).By deriving from this class you can use the An<TFake>() and Some<TFake>() (*) methods for creating fakes as well as the Extensionmethods based API for setting up the behavior (**). The WithFakes class only provides the basic fake framework abstraction.


    public class Given_the_current_day_is_monday_when_identifying_my_mood : WithFakes
    {
        static MoodIdentifier _subject;
        static string _mood;

        Establish context = () =>
        {
            var monday = new DateTime(2011, 2, 14);
            var systemClock = An<ISystemClock>(); (*)
            
            systemClock
                .WhenToldTo(x => x.CurrentTime) (**)
                .Return(monday);

            _subject = new MoodIdentifier(systemClock);
        };

        Because of = () => _mood = _subject.IdentifyMood();

        It should_be_pretty_bad = () => _mood.ShouldEqual("Pretty bad");
    }

### With Subject<TSubject>

Do we really need to create the subject of the specification by hand? Can we make it even more simpler? Yes, by introducing the concept of an AutoMockingContainer to the specification. That's exactly what WithSubject<TSubject> does. Here's the same example using WithSubject.

    public class Given_the_current_day_is_monday_when_identifying_my_mood : WithSubject<MoodIdentifier> (*)
    {
        static string _mood;

        Establish context = () =>
        {
            var monday = new DateTime(2011, 2, 14);

            The<ISystemClock>() (**)
                .WhenToldTo(x => x.CurrentTime)
                .Return(monday);
        };

        Because of = () => _mood = Subject.IdentifyMood(); (***)

        It should_be_pretty_bad = () => _mood.ShouldEqual("Pretty bad");
    }

The generic type parameter (*) tells Machine.Fakes what type to create for the specification. Each interface or abstract base class dependency in the constructor of the type specified there will be filled automatically by the configured fake framework. Dependency Injection with fakes so to speak. 

You can access the created instance through the lazy "Subject" property (***). The actual subject is created on the first read access to this property. If you want't to modify the subject when the context is established, go ahead, you can do so. You can even replace the subject by hand if case the automocking approach falls short by setting the property by yourself.

Having the subject created for us is a good thing but how do we access the injected fake without having a reference to it? Thats exactly the purpose of the The<TFake>() method (**) which gives access to the injected dependency.
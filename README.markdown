For latest changes see [the changelog](https://github.com/machine/machine.fakes/blob/master/changelog.markdown).
# What is Machine.Fakes ? #

Machine.Fakes is a little framework built on top of Machine.Specifications that can best be described as an extended integration layer between Machine.Specifications and different mock/fake/substitute/"whatever you call them now" frameworks. At the moment of writing I prefer the term "fake". (What a surprise judging from the name of this project ;-))

Machine.Fakes attempts to simplify the usage of such fake frameworks on top of MSpec by helping to reduce a lot of the typical fake framework related clutter code in specifications.

If you choose so, Machine.Fakes even helps you to stay mostly independent of a concrete fake framework by providing a little wrapper API and a provider model for fake frameworks.

Many credits for the ideas incorporated in this framework go to Jean Paul Boodhoo (http://blog.jpboodhoo.com/) who introduced me to a completely different way of handling mocks/fakes/etc during his NothingButDotnet bootcamp. Maybe this is a way for me to give something back ...

## How to contribute code ##

* Login to github (you need an account)
* Fork the main repository from [Github](https://github.com/machine/machine.fakes)
* Please contribute only on the dev branch. (We don't develop on the master branch. Only releases are built from it.)
* Push your changes to your GitHub repository.
* Send us a pull request

# How to get started #

## Building Machine.Fakes from source ##

Just download the repository from github and run `build.ps1`. The build of Machine.Fakes requires .NET Core SDK 2.0.0 to be installed on your machine. Everything else should work out-of-the-box. If not, please take the time to add an issue to this project. After a successful build you will find the nupkgs in the "Build" folder under their respective project (e.g. `Source\Machine.Fakes\Build`).

## Getting Machine.Fakes via the NuGet package manager

If you've got NuGet installed on your machine it gets even easier. Currently there are 5 packages available on NuGet. These are the core framework and the different adapters for Rhino.Mocks, Moq, NSubstitute and FakeItEasy. If you want to use Machine.Fakes with FakeItEasy for example, just go ahead and type

         install-package Machine.Fakes.FakeItEasy

in the package management console and all necessary dependencies including FakeItEasy itself will be downloaded to your machine. The other alternatives are

         install-package Machine.Fakes.RhinoMocks
         install-package Machine.Fakes.NSubstitute
         install-package Machine.Fakes.Moq

## How to use it ##

The core part of Machine.Fakes consists of only two classes: `WithFakes` and `WithSubject<TSubject>`.

### WithFakes ###

Let's take a look at the simpler one first. By deriving from this class you can use the `An<TFake>()`, `Some<TFake>()` and `The<TFake>()` (\*) methods for creating fakes as well as the extension methods based API for setting up the behavior (\*\*). The `WithFakes` class only provides the basic fake framework abstraction.


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

`WhenToldTo` is used for setting up a behavior but Machine.Fakes also supports behavior verification on fakes with the `WasToldTo` and `WasNotToldTo` extension methods.

    public class When_a_method_is_called_on_a_fake : WithFakes
    {
        static IServiceContainer_subject;

        Establish context = () =>
        {
            _subject = An<IServiceContainer>();
        };

        Because of = () => _subject.GetService(null);

        It should_be_able_to_verify_that = () => _subject.WasToldTo(s => s.GetService(null));
    }

Machine.Fakes even provides an abstraction for the various kinds of inline constraints used in the different fake framework flavors using the `Param` and `Param<T>` classes.

    public class A_person_with_nick_ScottGu : WithFakes
    {
        static VIPChecker _vipChecker;
        static bool _isVip;

        Establish context = () =>
        {
            var specification = An<ISpecification<Person>>();
            _vipChecker = new VIPChecker(specification);

            specification
                .WhenToldTo(x => x.IsSatisfiedBy(Param<Person>.Matches(p => p.NickName == "ScottGu")))
                .Return(true);
        };

        Because of = () => { _isVip = _vipChecker.IsVip(new Person {NickName = "ScottGu"}); };

        It should_be_vip = () => _isVip.ShouldBeTrue();
    }

These constraints get translated to the inline constraints of the target framework when Machine.Fakes executes. With this Machine.Fakes even preserves the nice verification error messages from the target frameworks and still allows you to be fake framework independent.

*Isn't that cool?*


### WithSubject&lt;TSubject&gt; ###

Back to our example with the `MoodIdentifier`. Do we really need to create the subject of the specification by hand? Can we make it even simpler? Yes, by introducing the concept of an AutoMockingContainer to the specification. That's exactly what WithSubject<TSubject> does. Here's a modified example.

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

The generic type parameter (*) tells Machine.Fakes what type to create for the specification. Each interface or abstract base class dependency in the constructor of the type will be filled automatically by the configured fake framework. Dependency Injection with fakes so to speak.

You can access the created instance through the lazy `Subject` property (***). The actual subject is created on the first read access to this property. If you want to modify the subject when the context is established, go ahead, you can do so. You can even replace the subject by hand in case the automocking approach falls short.

Having the subject created for us is a good thing but how do we access the injected fake without having a reference to it? That's exactly the purpose of the `The<TFake>()` method (**) which gives access to the injected dependency.

### Behavior Configurations ###

Re-use in context / specification is an interesting topic. In case you've already used a test case class per fixture setup (like Machine.Specifications does) then I'm pretty sure you've stumbled upon this, too.

Very often we try to accomplish re-use in classes by using inheritance and of course you can do so with Machine.Fakes. However in .NET you can only inherit once and inheritance may not be the weapon of choice for more cross cutting aspects like for instance time (the ISystemClock used in the example above). Machine.Fakes also offers a composition model for specifications: behavior configurations.

    public class BehaviorConfig
    {
        OnEstablish context = fakeAccessor => {};
        OnCleanUp subject = subject => {};
    }

`BehaviorConfig`s mimic the setup and teardown phases of the context / specification. They offer access to all the fakes in a specification and can clean up the subject after a specification. You only have to implement the relevant delegate. Machine.Fakes also ignores not initialized delegates. An example for a `BehaviorConfig` in the context of time looks like this:

    public class CurrentTime
    {
        public static DateTime Time { get; set; }

        public CurrentTime(DateTime time) { Time = time; }

        OnEstablish context = fakeAccessor =>
        {
            fakeAccessor.The<ISystemClock>()
                .WhenToldTo(x => x.CurrentTime)
                .Return(Time);
        };
    }

This is the "Mood" example with a behavior configuration instead of configuring the fake itself:

    public class Given_the_current_day_is_monday_when_identifying_my_mood : WithSubject<MoodIdentifier>
    {
        static string _mood;

        Establish context = () => With(new CurrentTime(new DateTime(2011, 2, 14)));

        Because of = () => _mood = Subject.IdentifyMood();

        It should_be_pretty_bad = () => _mood.ShouldEqual("Pretty bad");
    }

### Faking properties
Behaviours can be set up for fake properties like this:

    The<InterfaceWithProperty>().WhenToldTo(x => x.Property).Return(propertyValue);

But property values are also automatically tracked. So you might as well just set the value that should be returned:

    The<InterfaceWithProperty>().Property = propertyValue;

This way you can also check whether a setter has been called:

    The<InterfaceWithProperty>().Property.ShouldEqual(propertyValue);

The Rhino Mocks adapter is a bit special here: a fake will stop tracking its properties as soon as you set a behavior on one of them.

### Faking out and ref parameters
The FakeItEasy and NSubstitute adapters support setting up values for out and ref parameters on faked method calls. For example, a method

    public interface IReturnOutAndRef
    {
		void Invoke(string input, out string output, ref object additional);
    }

can be faked this way:

	string output;
	object additional;
    The<IReturnOutAndRef>().WhenToldTo(x => x.Invoke("a", out output, ref additional))
            .AssignOutAndRefParameters("b", new object());

`output` will be set to `"b"` and `additional` to a new `object` instance.

`AssignOutAndRefParameters` will use the given values to set up all out and ref parameters in the order they appear in the method signature. Any non out and ref parameters are ignored.

![OhLoh](http://www.ohloh.net/p/machinefakes/widgets/project_thin_badge.gif)

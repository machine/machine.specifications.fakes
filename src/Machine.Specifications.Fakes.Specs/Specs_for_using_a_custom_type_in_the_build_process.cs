using System.Linq;
using Machine.Specifications.Fakes.Sdk;
using Machine.Specifications.Fakes.Specs.TestClasses;

namespace Machine.Specifications.Fakes.Specs
{
    public class CarFromFakeFramework : ICar 
    {
        public void Honk()
        {
        }
    }

    public class LittleKingdom
    {
        public LittleKingdom(ICar car, IGarage garage)
        {
            Car = car;
            Garage = garage;
        }

        public IGarage Garage { get; private set; }
        public ICar Car { get; private set; }
    }

    [Subject(typeof(SpecificationController<>))]
    public class When_using_a_custom_type_in_the_build_process_of_a_subject
    {
        Establish context = () =>
        {
            var dummyFakeEngine = new DummyFakeEngine { CreatedFake = new CarFromFakeFramework() };

            _specController = new SpecificationController<LittleKingdom>(dummyFakeEngine);
            _specController.Configure<IGarage, Garage>();
        };

        Because of = () => _subject = _specController.Subject;

        It should_use_the_configured_type_in_the_build_process = 
            () => _subject.Garage.ShouldBeOfExactType<Garage>();

        It should_inject_fakes_into_the_configured_type = 
            () => _subject.Garage.First().ShouldBeOfExactType<CarFromFakeFramework>();

        static SpecificationController<LittleKingdom> _specController;
        static LittleKingdom _subject;
    }

    [Subject(typeof(SpecificationController<>))]
    public class When_completely_building_the_subject_from_custom_types
    {
        Establish context = () =>
        {
            _specController = new SpecificationController<LittleKingdom>(new DummyFakeEngine());
            _specController.Configure<IGarage, Garage>();
            _specController.Configure<ICar, Car>();
        };

        Because of = () => _subject = _specController.Subject;

        It should_use_the_first_configured_type_in_the_build_process =
            () => _subject.Garage.ShouldBeOfExactType<Garage>();

        It should_also_use_the_second_configured_type_in_the_build_process =
            () => _subject.Garage.First().ShouldBeOfExactType<Car>();

        static SpecificationController<LittleKingdom> _specController;
        static LittleKingdom _subject;
    }
}

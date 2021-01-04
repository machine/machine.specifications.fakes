using System;
using Machine.Fakes.Examples.SampleCode;
using Machine.Specifications;

namespace Machine.Fakes.Examples.HandlingExceptions
{
    [Subject(typeof(Account)), Tags("Examples")]
    public class When_trying_to_transfer_an_amount_larger_than_the_balance_of_the_souce_account
    {
        static Exception _exception;
        static Account _source;
        static Account _target;
        
        Establish context = () =>
        {
            _source = new Account(1m);
            _target = new Account(1m);
        };

        Because of = () =>
        {
            _exception = Catch.Exception(() => _source.Transfer(2m, _target));
        };

        It Should_not_allow_the_transfer = () => _exception.ShouldNotBeNull();
    }
}
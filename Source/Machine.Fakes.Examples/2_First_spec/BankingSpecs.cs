using Machine.Fakes.Examples.SampleCode;
using Machine.Specifications;

namespace Machine.Fakes.Examples.FirstSpec
{
    [Subject(typeof (Account)), Tags("Examples")]
    public class Given_two_accounts_when_transferring_money_between_them
    {
        static Account _source;
        static Account _target;

        Establish context = () =>
        {
            _source = new Account(1m);
            _target = new Account(1m);
        };

        Because of = () => _source.Transfer(1m, _target);

        It should_credit_the_target_account_by_the_amount_transferred = () => _target.Balance.ShouldEqual(2m);

        It should_debit_the_source_account_by_the_amount_transferred = () => _source.Balance.ShouldEqual(0m);
    }
}
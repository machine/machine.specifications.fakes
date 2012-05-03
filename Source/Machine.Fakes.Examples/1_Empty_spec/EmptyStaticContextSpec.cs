using Machine.Fakes.Examples.SampleCode;
using Machine.Specifications;

namespace Machine.Fakes.Examples.EmptySpec
{
    [Subject(typeof(Account)), Tags("Examples")]
    public class When_a_customer_first_views_the_account_summary_page
    {
        Establish context = () => { };

        Because of = () => { };

        It Should_display_all_account_transactions_in_the_last_30_days = () => { };

        It Should_greet_him_with_the_date_of_the_previous_visit = () => { };
    }
}
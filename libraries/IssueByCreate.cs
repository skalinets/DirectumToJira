using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectumToJira.libraries
{
    class IssueByCreate : IssueByCreateBase
    {

        public string Estimate { get; set; }
        public string SourceSystem { get; set; }
        public string PriorityId { get; set; }
        public string SourceName { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }
    }

    class IssueByCreateBase
    {
        public string Project { get; set; }
        public string IssueType { get; set; }
        public string Assignee { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string SourceNumber { get; set; }

        //public class Validator: AbstractValidator<Customer> {
        //    public CustomerValidator() {
        //        RuleFor(x => x.Surname).NotEmpty();
        //        RuleFor(x => x.Forename).NotEmpty().WithMessage("Please specify a first name");
        //        RuleFor(x => x.Discount).NotEqual(0).When(x => x.HasDiscount);
        //        RuleFor(x => x.Address).Length(20, 250);
        //        RuleFor(x => x.Postcode).Must(BeAValidPostcode).WithMessage("Please specify a valid postcode");
        //    }

            //private bool BeAValidPostcode(string postcode) {
            //    // custom postcode validating logic goes here
            //}
        }
    }
}

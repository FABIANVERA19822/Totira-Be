using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Totira.Bussiness.PropertiesService.Commands;
using Totira.Support.Application.Messages;

namespace Totira.Bussiness.PropertiesService.Validators
{
    public class ImportPropertyImagesToS3CommandValidator : IMessageValidator<ImportPropertyImagesToS3Command>
    {
       

        public ValidationResult Validate(ImportPropertyImagesToS3Command message)
        {
            List<string> errors = new List<string>();
            return new ValidationResult(errors);
        }
    }
}

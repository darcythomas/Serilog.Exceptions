using System;
using System.Collections.Generic;
using Serilog.Exceptions.Core;
using Serilog.Exceptions.Destructurers;
using System.Data.Entity.Validation;

namespace DataInsight.Web.Utility.Logging
{
    public class DbEntityValidationExceptionDestructurer : ExceptionDestructurer
    {
        /// <inheritdoc cref="IExceptionDestructurer.TargetTypes"/>
        public override Type[] TargetTypes => new[] { typeof(System.Data.Entity.Validation.DbEntityValidationException) };

        /// <inheritdoc cref="IExceptionDestructurer.Destructure"/>
        public override void Destructure(
            Exception exception,
            IExceptionPropertiesBag propertiesBag,
            Func<Exception, IReadOnlyDictionary<string, object>> destructureException)
        {
            base.Destructure(exception, propertiesBag, destructureException);

            DbEntityValidationException validationException = exception as DbEntityValidationException;
            if (validationException is null) { return; }

            foreach (var error in validationException.EntityValidationErrors)
            {
                foreach (var ve in error.ValidationErrors)
                {
                    propertiesBag.AddProperty(ve.PropertyName, ve.ErrorMessage);
                }
            }
        }
    }
}

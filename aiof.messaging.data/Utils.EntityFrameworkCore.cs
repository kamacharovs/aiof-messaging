using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using JetBrains.Annotations;

namespace aiof.messaging.data
{
    public static partial class Utils
    {
        public static PropertyBuilder HasSnakeCaseColumnName(
            [NotNull] this PropertyBuilder propertyBuilder)
        {
            propertyBuilder.Metadata.SetColumnName(
                propertyBuilder
                    .Metadata
                    .Name
                    .ToSnakeCase());

            return propertyBuilder;
        }
    }
}

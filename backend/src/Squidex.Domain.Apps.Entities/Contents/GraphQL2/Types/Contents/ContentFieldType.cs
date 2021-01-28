﻿// ==========================================================================
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) Squidex UG (haftungsbeschraenkt)
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

using HotChocolate.Types;
using Squidex.Domain.Apps.Core.Schemas;
using Squidex.Domain.Apps.Entities.Schemas;

namespace Squidex.Domain.Apps.Entities.Contents.GraphQL2.Types.Contents
{
    internal sealed class ContentFieldType : ObjectType
    {
        private readonly GraphQLSchemaBuilder builder;
        private readonly SchemaType schemaType;
        private readonly FieldType fieldType;

        public ContentFieldType(GraphQLSchemaBuilder builder, SchemaType schemaType, FieldType fieldType)
        {
            this.builder = builder;
            this.schemaType = schemaType;
            this.fieldType = fieldType;
        }

        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Name(fieldType.LocalizedType)
                .Description($"The structure of the {schemaType.DisplayName}/{fieldType} field");

            var partition = builder.ResolvePartition(((IRootField)fieldType.Field).Partitioning);

            foreach (var key in partition.AllKeys)
            {
                var field =
                    descriptor.Field(key.EscapePartition())
                        .WithSourceName(key)
                        .Description(fieldType.Field.RawProperties.Hints);

                builder.FieldBuilder.Build(field, fieldType);
            }
        }
    }
}
// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Ignitor.RenderTree
{
    /// <summary>
    /// Describes the type of a <see cref="RenderTreeFrame"/>.
    /// </summary>
    public enum RenderTreeFrameType: int
    {
        /// <summary>
        /// Used only for unintialized frames.
        /// </summary>
        None = 0,

        /// <summary>
        /// Represents a container for other frames.
        /// </summary>
        Element = 1,

        /// <summary>
        /// Represents text content.
        /// </summary>
        Text = 2,

        /// <summary>
        /// Represents a key-value pair associated with another <see cref="RenderTreeFrame"/>.
        /// </summary>
        Attribute = 3,

        /// <summary>
        /// Represents a child component.
        /// </summary>
        Component = 4,

        /// <summary>
        /// Defines the boundary around range of sibling frames that should be treated as an
        /// unsplittable group for the purposes of diffing. This is typically used when appending
        /// a tree fragment generated by external code, because the sequence numbers in that tree
        /// fragment are not comparable to sequence numbers outside it.
        /// </summary>
        Region = 5,

        /// <summary>
        /// Represents an instruction to capture or update a reference to the parent element.
        /// </summary>
        ElementReferenceCapture = 6,

        /// <summary>
        /// Represents an instruction to capture or update a reference to the parent component.
        /// </summary>
        ComponentReferenceCapture = 7,

        /// <summary>
        /// Represents a block of markup content.
        /// </summary>
        Markup = 8,
    }
}
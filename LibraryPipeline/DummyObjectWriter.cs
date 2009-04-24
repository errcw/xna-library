using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

namespace LibraryPipeline
{
    /// <summary>
    /// A dummy object for processors that produce no real output.
    /// </summary>
    public class DummyObject
    {
    }

    /// <summary>
    /// Writes a dummy object.
    /// </summary>
    [ContentTypeWriter]
    public class DummyObjectWriter : ContentTypeWriter<DummyObject>
    {
        protected override void Write(ContentWriter output, DummyObject value)
        {
            // write nothing
        }

        public override string GetRuntimeType(TargetPlatform targetPlatform)
        {
            return "Library.DummyObjectReader, " +
                   "Library, Version=1.0.0.0, Culture=neutral";
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return "Library.DummyObjectReader, " +
                   "Library, Version=1.0.0.0, Culture=neutral";
        }
    }
}

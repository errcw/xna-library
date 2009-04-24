using System;
using System.IO;
using System.Reflection;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

namespace LibraryPipeline.Extensions
{
    /// <summary>
    /// Extensions to ContentProcessorContext.
    /// </summary>
    public static class ContentProcessorContextExtensions
    {
        /// <summary>
        /// Writes an asset in compiled binary format.
        /// </summary>
        /// <typeparam name="T">The type of asset to write.</typeparam>
        /// <param name="asset">The asset object to write.</param>
        /// <param name="assetName">Name of the final compiled content.</param>
        /// <returns>Reference to the final compiled content.</returns>
        /// <remarks>Adapted from http://forums.xna.com/forums/p/23919/129260.aspx#129260</remarks>
        public static ExternalReference<T> WriteAsset<T>(this ContentProcessorContext context, T asset, string assetName)
        {
            var constructors = typeof(ContentCompiler).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance);
            var compileMethod = typeof(ContentCompiler).GetMethod("Compile", BindingFlags.NonPublic | BindingFlags.Instance);

            var compiler = constructors[0].Invoke(null) as ContentCompiler;

            string outputFile = context.OutputDirectory + assetName + ".xnb";
            context.AddOutputFile(outputFile);

            using (FileStream stream = new FileStream(outputFile, FileMode.Create))
            {
                object[] parameters = new object[]
                    {
                        stream,
                        asset,
                        context.TargetPlatform,
                        true,
                        context.OutputDirectory,
                        context.OutputDirectory
                    };
                compileMethod.Invoke(compiler, parameters);
            }

            return new ExternalReference<T>(outputFile);
        }
    }
}

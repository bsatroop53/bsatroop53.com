//
// BSATroop53 Website DevOps - Build Tools.
// Copyright (C) 2024 Seth Hendrick
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published
// by the Free Software Foundation, either version 3 of the License, or
// any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
// 
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Cake.Common;
using Cake.Common.Diagnostics;
using Cake.Common.IO;
using Cake.Core;
using Cake.Core.IO;
using Cake.Frosting;

namespace DevOps.Tasks
{
    [TaskName( "extract" )]
    [TaskDescription( $"Extracts .bsat53 files.  Use the required '{argument}' argument for file location." )]
    public sealed class ExtractTask : FrostingTask<BuildContext>
    {
        // ---------------- Fields ----------------

        private const string argument = "file_path";

        private const string expectedFileFormat = ".bsat53";

        // ---------------- Functions ----------------

        public override bool ShouldRun( BuildContext context )
        {
            bool hasArgument = context.HasArgument( argument );

            if( hasArgument == false )
            {
                context.Error( $"Required argument not specified: {argument}." );
            }

            return hasArgument;
        }

        public override void Run( BuildContext context )
        {
            string? filePathStr = context.Arguments.GetArguments( argument ).FirstOrDefault();
            if( string.IsNullOrWhiteSpace( filePathStr ) )
            {
                throw new CakeException( $"Could not get value from argument '{argument}'." );
            }

            var filePath = new FilePath( filePathStr );
            if( context.FileExists( filePath ) == false )
            {
                throw new FileNotFoundException(
                    $"Can not find file specified in {argument} argument.",
                    filePath.FullPath
                );
            }

            if( filePath.HasExtension == false )
            {
                throw new ArgumentException(
                    $"Specified file in {argument} has no extension.",
                    argument
                );
            }

            string extension = filePath.GetExtension();
            if( extension != expectedFileFormat )
            {
                throw new ArgumentException(
                    $"Specified file in {argument} has the wrong extension.  Expected: {expectedFileFormat}, got: {extension}.",
                    argument
                );
            }
            string subExtension = filePath.GetFilenameWithoutExtension().GetExtension();

            if( subExtension == ".md" )
            {
                HandleMarkdownFile( context, filePath );
            }
            else if( subExtension == ".zip" )
            {
                HandlePostFile( context, filePath );
            }
            else
            {
                throw new ArgumentException(
                    $"Specified file in {argument} has an unsupported sub-extension.",
                    argument
                );
            }
        }

        private static void HandleMarkdownFile( BuildContext context, FilePath bsat53File )
        {
            FilePath targetPath = context.PostsDir.CombineWithFilePath(
                bsat53File.GetFilenameWithoutExtension()
            );

            context.Information(
                $"Markdown file detected.  Moving '{bsat53File.FullPath}' to '{targetPath.FullPath}'."
            );
            context.MoveFile(
                bsat53File,
                targetPath
            );
        }

        private static void HandlePostFile( BuildContext context, FilePath bsat53File )
        {
            context.Information(
                $"Extracting zip file '{bsat53File.FullPath}' to '{context.RepoRoot}'."
            );

            context.Unzip(
                bsat53File,
                context.RepoRoot,
                false
            );

            context.DeleteFile( bsat53File.FullPath );
        }
    }
}

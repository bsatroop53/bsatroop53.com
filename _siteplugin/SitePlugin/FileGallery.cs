//
// BSATroop53 Website Plugin - Extensions to Pretzel.
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

using System.Collections.ObjectModel;
using System.Xml.Linq;
using Pretzel.Logic.Extensibility;
using Pretzel.Logic.Templating.Context;

namespace SitePlugin
{
    //[Export( typeof( IBeforeProcessingTransform ) )]
    public class FileGallery : IBeforeProcessingTransform
    {
        // ---------------- Fields ----------------

        // ---------------- Constructor ----------------

        public FileGallery()
        {
        }

        static FileGallery()
        {
            var emptyDict = new Dictionary<string, IReadOnlyList<T53File>>();
            FileData = new ReadOnlyDictionary<string, IReadOnlyList<T53File>>( emptyDict );
        }

        // ---------------- Properties ----------------

        public static IReadOnlyDictionary<string, IReadOnlyList<T53File>> FileData { get; private set; }

        // ---------------- Functions ----------------

        public void Transform( SiteContext context )
        {
            Dictionary<string, string> ipfsData = ParseIpfsFile( context );
            IReadOnlyDictionary<string, IReadOnlyList<T53File>> fileData = ParseFileInfo( context, ipfsData );
            FileData = fileData;
        }

        private static IReadOnlyDictionary<string, IReadOnlyList<T53File>> ParseFileInfo(
            SiteContext context,
            Dictionary<string, string> ipfsInfo
        )
        {
            var dict = new Dictionary<string, List<T53File>>();

            var fileInfoFile = new FileInfo( Path.Combine( context.SourceFolder, "fileinfo", "fileinfo.xml" ) );

            XDocument fileInfoDoc = XDocument.Load( fileInfoFile.FullName );

            XElement? root = fileInfoDoc.Root;
            if( root is null )
            {
                Console.WriteLine( "WARNING!  Can not read file info file, this page will be empty." );
            }
            else
            {

            }

            return new ReadOnlyDictionary<string, IReadOnlyList<T53File>>(
                new Dictionary<string, IReadOnlyList<T53File>>( dict.Select( d => new KeyValuePair<string, IReadOnlyList<T53File>>( d.Key, d.Value ) ) )
            );
        }

        private static Dictionary<string, string> ParseIpfsFile( SiteContext context )
        {
            var dict = new Dictionary<string, string>();
            
            var ipfsInfoFile = new FileInfo( Path.Combine( context.SourceFolder, "fileinfo", "ipfs.xml" ) );
            if( ipfsInfoFile.Exists == false )
            {
                return dict;
            }

            XDocument ipfsDoc = XDocument.Load( ipfsInfoFile.FullName );

            XElement? root = ipfsDoc.Root;
            if( root is null )
            {
                Console.WriteLine( "WARNING!  Can not read IPFS file, no IPFS CIDs will be included." );
                return dict;
            }

            foreach( XElement ipfsNode in root.Elements() )
            {
                if( ipfsNode.Name is null )
                {
                    continue;
                }
                else if( "File".Equals( ipfsNode.Name.LocalName ) == false )
                {
                    continue;
                }

                string? fileId = null;
                string? cid = null;

                foreach( XAttribute attr in ipfsNode.Attributes() )
                {
                    if( "name".Equals( attr.Name ) == false )
                    {
                        continue;
                    }
                    fileId = attr.Value;
                }

                if( fileId is null )
                {
                    continue;
                }

                foreach( XElement fileNode in ipfsNode.Elements() )
                {
                    if( ipfsNode.Name is null )
                    {
                        continue;
                    }
                    else if( "IpfsHash".Equals( ipfsNode.Name.LocalName ) == false )
                    {
                        continue;
                    }

                    cid = fileNode.Value;
                }

                if( cid is null )
                {
                    continue;
                }

                dict[fileId] = cid;
            }

            return dict;
        }
    }
}
